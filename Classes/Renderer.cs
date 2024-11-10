using System.Numerics;

namespace MeshFiller.Classes
{
    public class Renderer
    {
        public float kd { get; set; }
        public float ks { get; set; }
        public float m { get; set; }

        public bool UseTexture { get; set; }
        public Bitmap? Texture { get; set; }

        public bool UseNormalMap { get; set; }
        public Bitmap? NormalMap { get; set; }

        public Vector3 LightColor { get; set; } = new(1, 1, 1);
        public Vector3 ObjectColor { get; set; } = new(1, 0, 0);
        public Vector3 LightDirection { get; set; } = Vector3.Normalize(new Vector3(0, -1, 1));

        private Vector3 V = new(0, 0, 1);

        private class Edge
        {
            public int end;
            public float XCurrent;
            public float InvSlope;
        }

        // Update AET with new edges, start should be lower than end
        private static void UpdateAET(List<Edge> aet, Vertex start, Vertex end, int y)
        {
            int endY = (int)Math.Round(end.Y);
            if (endY >= y)
            {
                aet.Add(new Edge
                {
                    end = endY,
                    XCurrent = start.X,
                    InvSlope = (end.X - start.X) / (end.Y - start.Y)
                });
            }
        }

        public void FillPolygon(DirectBitmap bitmap, List<Vertex> vertices, Triangle t)
        {
            // Sort vertices by Y
            List<int> ind = Enumerable.Range(0, vertices.Count).ToList();
            ind.Sort((a, b) => (vertices[a].Y).CompareTo(vertices[b].Y));

            int minY = (int)Math.Round(vertices[ind[0]].Y);
            int maxY = (int)Math.Round(vertices[ind[^1]].Y);

            List<Edge> aet = [];

            for (int y = minY; y <= maxY; y++)
            {
                // For each vertex Pi lying on the previous scanline: P[ind[k]].y = y-1
                foreach (int i in ind)
                {
                    Vertex v = vertices[i];
                    if ((int)Math.Round(v.Y) == y - 1)
                    {
                        // Find the previous vertex Pi-1
                        Vertex prev = vertices[(i - 1 + vertices.Count) % vertices.Count];
                        UpdateAET(aet, v, prev, y);

                        // Find the next vertex Pi+1
                        Vertex next = vertices[(i + 1) % vertices.Count];
                        UpdateAET(aet, v, next, y);
                    }
                }

                // Remove ending edges
                aet.RemoveAll(e => e.end == y - 1);

                // Update AET:
                // Sort ascending x
                aet = aet.OrderBy(e => e.XCurrent).ToList();

                // Fill pixels between edges 0-1, 2-3, ..
                for (int i = 0; i < aet.Count; i += 2)
                {
                    int x1 = (int)Math.Round(aet[i].XCurrent);
                    int x2 = (int)Math.Round(aet[i + 1].XCurrent); // potestować z Math.Round/ceiling/floor

                    for (int x = x1; x <= x2; x++)
                    {
                        // for triangles
                        FillPixel(bitmap, t, x, y);
                    }
                }

                // Update x values
                foreach (var edge in aet)
                {
                    edge.XCurrent += edge.InvSlope;
                }
            }
        }

        // Fill pixel based on barycentric coordinates
        private void FillPixel(DirectBitmap bitmap, Triangle t, int x, int y)
        {
            (float a, float b, float c) = Barycentric2(new Vector2(x, y), t);

            if (a == -1)
            {
                // zdegenerowany trójkąt
                return;
            }

            // Interpolate z, u, v, normal
            float z = a * t.V1.Z + b * t.V2.Z + c * t.V3.Z;

            (a, b, c) = Barycentric(new Vector3(x, y, z), t);

            float u = Math.Clamp(a * t.V1.u + b * t.V2.u + c * t.V3.u, 0, 1);
            float v = Math.Clamp(a * t.V1.v + b * t.V2.v + c * t.V3.v, 0, 1);

            Vector3 normal = Vector3.Normalize(a * t.V1.RotN + b * t.V2.RotN + c * t.V3.RotN);
            normal = GetNormalMap(u, v, normal, t, a, b, c);

            DrawPixel(bitmap, x, y, u, v, normal);
        }

        // https://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates
        private static (float, float, float) Barycentric(Vector3 P, Triangle t)
        {
            Vector3 B2 = P - t.V1.RotP;

            float d20 = Vector3.Dot(B2, t.B0);
            float d21 = Vector3.Dot(B2, t.B1);

            if (float.IsInfinity(t.invDenom))
                return (-1, -1, -1);

            float b = (t.d11 * d20 - t.d01 * d21) * t.invDenom;
            float c = (t.d00 * d21 - t.d01 * d20) * t.invDenom;
            float a = 1.0f - b - c;

            return (a, b, c);
        }

        private static (float, float, float) Barycentric2(Vector2 P, Triangle t)
        {
            Vector2 P2 = P - new Vector2(t.V1.RotP.X, t.V1.RotP.Y);

            float p20 = Vector2.Dot(P2, t.P0);
            float p21 = Vector2.Dot(P2, t.P1);

            if (float.IsInfinity(t.pInvDenom))
                return (-1, -1, -1);

            float b = (t.p11 * p20 - t.p01 * p21) * t.pInvDenom;
            float c = (t.p00 * p21 - t.p01 * p20) * t.pInvDenom;
            float a = 1.0f - b - c;

            return (a, b, c);
        }

        // Get object color from texture
        private Vector3 GetObjectColor(float u, float v)
        {
            if (!UseTexture || Texture is null)
                return ObjectColor;

            int x = (int)(v * (Texture.Width - 1));
            int y = (int)(u * (Texture.Height - 1));

            Color color = Texture.GetPixel(x, y);

            return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        }

        // Calculate normal from normal map
        private Vector3 GetNormalMap(float u, float v, Vector3 normal, Triangle t, float a, float b, float c)
        {
            if (!UseNormalMap || NormalMap is null)
                return normal;

            int x = (int)(v * (NormalMap.Width - 1));
            int y = (int)(u * (NormalMap.Height - 1));

            Color color = NormalMap.GetPixel(x, y);

            Vector3 normalMap = new(
                color.R / 255f * 2 - 1,
                color.G / 255f * 2 - 1,
                color.B / 255f * 2 - 1
            );

            Vector3 Pu = Vector3.Normalize(a * t.V1.RotPu + b * t.V2.RotPu + c * t.V3.RotPu);
            Vector3 Pv = Vector3.Normalize(a * t.V1.RotPv + b * t.V2.RotPv + c * t.V3.RotPv);

            // M = [Pu, Pv, N]
            Matrix3x3 M = new(Pu, Pv, normal);

            return Vector3.Normalize(M * normalMap);
        }

        // Draw pixel with Lambert shading
        private void DrawPixel(DirectBitmap bitmap, int x, int y, float u, float v, Vector3 normal)
        {
            float NdotL = Vector3.Dot(normal, LightDirection);
            float cosNL = Math.Max(NdotL, 0); // cos >= 0

            Vector3 R = Vector3.Normalize(2 * NdotL * normal - LightDirection);

            float cosVR = Math.Max(Vector3.Dot(V, R), 0);

            Vector3 objectColor = GetObjectColor(u, v);

            Vector3 diffuse = kd * LightColor * objectColor * cosNL;
            Vector3 specular = ks * LightColor * objectColor * (float)Math.Pow(cosVR, m);

            Vector3 color = Vector3.Clamp(diffuse + specular, Vector3.Zero, Vector3.One);

            // Convert to 0-255 range
            Color finalColor = Color.FromArgb(
                (int)(color.X * 255),
                (int)(color.Y * 255),
                (int)(color.Z * 255)
            );

            bitmap.SetPixel(x, y, finalColor);
        }
    }
}
