﻿using System.Numerics;

namespace MeshFiller.Classes
{
    public class Renderer
    {
        public float kd { get; set; }
        public float ks { get; set; }
        public float m { get; set; }

        public bool UseTexture { get; set; }
        public Color[,]? Texture { get; set; }

        public bool UseNormalMap { get; set; }
        public Color[,]? NormalMap { get; set; }

        public Vector3 LightColor { get; set; } = new(1, 1, 1);
        public Vector3 ObjectColor { get; set; } = new(1, 0, 0);

        public Vector3 LightPosition = new(130, 0, 400);

        private float[,] zBuffer;
        public int ChangeX { get; set; }
        public int ChangeY { get; set; }
        public bool UseZBuffer { get; set; }

        private Vector3 V = new(0, 0, 1);

        private class Edge
        {
            public int end;
            public float XCurrent;
            public float InvSlope;
        }
        private class YEvent
        {
            public int Y { get; set; }
            public List<int> VertexIndices { get; set; } = new();
        }

        // Update AET with new edges, start.Y <= end.Y
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

        public void FillPolygon(DirectBitmap bitmap, List<Vertex> vertices, Triangle? triangle)
        {
            Dictionary<int, YEvent> yEvents = [];

            // For each vertex, create or update events for its y-coordinate
            for (int i = 0; i < vertices.Count; i++)
            {
                int y = (int)Math.Round(vertices[i].Y);
                if (!yEvents.ContainsKey(y))
                {
                    yEvents[y] = new YEvent { Y = y };
                }
                yEvents[y].VertexIndices.Add(i);
            }

            // Convert to sorted list
            List<YEvent> sortedEvents = yEvents.Values.OrderBy(e => e.Y).ToList();

            int minY = sortedEvents[0].Y;
            int maxY = sortedEvents[^1].Y;
            List<Edge> aet = [];

            int currentEventIndex = 0;
            YEvent? currentEvent = sortedEvents[0];

            for (int y = minY; y <= maxY; y++)
            {
                // Process vertices on current scanline
                if (currentEvent != null && currentEvent.Y == y - 1)
                {
                    foreach (int i in currentEvent.VertexIndices)
                    {
                        Vertex v = vertices[i];
                        // Find the previous vertex Pi-1
                        Vertex prev = vertices[(i - 1 + vertices.Count) % vertices.Count];
                        UpdateAET(aet, v, prev, y);
                        // Find the next vertex Pi+1
                        Vertex next = vertices[(i + 1) % vertices.Count];
                        UpdateAET(aet, v, next, y);
                    }

                    // Move to next event
                    currentEventIndex++;
                    currentEvent = currentEventIndex < sortedEvents.Count ? sortedEvents[currentEventIndex] : null;
                }

                // Remove ending edges
                aet.RemoveAll(e => e.end == y - 1);

                // Update AET:
                // Sort ascending x
                aet = aet.OrderBy(e => e.XCurrent).ToList();

                int bitmapY = ChangeY - y;
                if (bitmapY >= 0 && bitmapY < bitmap.Height)
                {
                    // Fill pixels between edges 0-1, 2-3, ..
                    for (int i = 0; i < aet.Count; i += 2)
                    {
                        int x1 = (int)Math.Round(aet[i].XCurrent);
                        int x2 = (int)Math.Round(aet[i + 1].XCurrent);

                        for (int x = x1; x < x2; x++)
                        {
                            if (triangle != null)
                                FillPixel(bitmap, triangle, x, y);
                            // Multiple-vertices polygon
                            else
                                bitmap.SetPixel(x + ChangeX, bitmapY, Color.Red);
                        }
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
            int bitmapX = x + ChangeX;
            int bitmapY = ChangeY - y;

            if (bitmapX < 0 || bitmapX >= bitmap.Width)
                return;

            (float a, float b, float c) = Barycentric2(new Vector2(x, y), t);

            if (a == -1 && b == -1 && c == -1)
            {
                // Degenerate triangle
                //bitmap.SetPixel(bitmapX, bitmapY, Color.Blue);
                return;
            }

            // Interpolate z, u, v, normal
            float z = a * t.V1.Z + b * t.V2.Z + c * t.V3.Z;

            if (zBuffer[bitmapX, bitmapY] < z)
            {
                //bitmap.SetPixel(bitmapX, bitmapY, Color.Cyan);
                if (UseZBuffer)
                    return;
            }

            zBuffer[bitmapX, bitmapY] = z;

            Vector3 P = new(x, y, z);

            (a, b, c) = Barycentric(P, t);

            float u = Math.Clamp(a * t.V1.u + b * t.V2.u + c * t.V3.u, 0, 1);
            float v = Math.Clamp(a * t.V1.v + b * t.V2.v + c * t.V3.v, 0, 1);

            Vector3 normal = Vector3.Normalize(a * t.V1.RotN + b * t.V2.RotN + c * t.V3.RotN);
            normal = GetNormalMap(u, v, normal, t, a, b, c);

            DrawPixel(bitmap, bitmapX, bitmapY, P, u, v, normal);
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
            Vector2 P2 = P - new Vector2(t.V1.X, t.V1.Y);

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

            int x = (int)(v * (Texture.GetLength(0) - 1));
            int y = (int)(u * (Texture.GetLength(1) - 1));
            Color color = Texture[x, y];

            return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        }

        // Calculate normal from normal map
        private Vector3 GetNormalMap(float u, float v, Vector3 normal, Triangle t, float a, float b, float c)
        {
            if (!UseNormalMap || NormalMap is null)
                return normal;

            int x = (int)(v * (NormalMap.GetLength(0) - 1));
            int y = (int)(u * (NormalMap.GetLength(1) - 1));
            Color color = NormalMap[x, y];

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
        private void DrawPixel(DirectBitmap bitmap, int x, int y, Vector3 P, float u, float v, Vector3 normal)
        {
            Vector3 L = Vector3.Normalize(LightPosition - P);

            float NdotL = Vector3.Dot(normal, L);
            float cosNL = Math.Max(NdotL, 0); // negative cos = 0

            Vector3 R = Vector3.Normalize(2 * NdotL * normal - L);

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

        public void UpdateZBuffer(int width, int height, int changeX, int changeY)
        {
            ChangeX = changeX;
            ChangeY = changeY;
            zBuffer = new float[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    zBuffer[i, j] = float.MaxValue;
                }
            }
        }
    }
}