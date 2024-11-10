using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

        public void FillPolygon(Graphics g, List<Vertex> vertices)
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

                    Vertex V0 = vertices[0];
                    Vertex V1 = vertices[1];
                    Vertex V2 = vertices[2];

                    for (int x = x1; x <= x2; x++)
                    {
                        (float alpha, float beta, float gamma) = Barycentric(
                            new Vector3(x, y, 0),
                            new Vector3(V0.X, V0.Y, 0),
                            new Vector3(V1.X, V1.Y, 0),
                            new Vector3(V2.X, V2.Y, 0)
                        );

                        if (alpha == -1)
                        {
                            // zdegenerowane
                            //Debug.WriteLine($"x: {x} y: {y}");
                            //g.FillRectangle(Brushes.Green, x, y, 1, 1);
                            continue;
                        }

                        float z = alpha * V0.Z + beta * V1.Z + gamma * V2.Z;

                        (alpha, beta, gamma) = Barycentric(
                            new Vector3(x, y, z),
                            V0.RotP, V1.RotP, V2.RotP
                        );

                        Vector3 interpolatedNormal = alpha * V0.RotN + beta * V1.RotN + gamma * V2.RotN;
                        Vector3.Normalize(interpolatedNormal);

                        float u = Math.Clamp(alpha * V0.u + beta * V1.u + gamma * V2.u, 0, 1);
                        float v = Math.Clamp(alpha * V0.v + beta * V1.v + gamma * V2.v, 0, 1);

                        Vector3 Pu = alpha * V0.RotPu + beta * V1.RotPu + gamma * V2.RotPu;
                        Vector3 Pv = alpha * V0.RotPv + beta * V1.RotPv + gamma * V2.RotPv;
                        Pu = Vector3.Normalize(Pu);
                        Pv = Vector3.Normalize(Pv);

                        Vector3 normal = GetNormal(u, v, interpolatedNormal, Pu, Pv);

                        DrawPixel(g, x, y, u, v, normal);
                    }
                }

                // Update x values
                foreach (var edge in aet)
                {
                    edge.XCurrent += edge.InvSlope;
                }
            }
        }

        // https://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates
        private static (float, float, float) Barycentric(Vector3 P, Vector3 A, Vector3 B, Vector3 C)
        {
            Vector3 V0 = B - A;
            Vector3 V1 = C - A;
            Vector3 V2 = P - A;

            float d00 = Vector3.Dot(V0, V0);
            float d01 = Vector3.Dot(V0, V1);
            float d11 = Vector3.Dot(V1, V1);
            float d20 = Vector3.Dot(V2, V0);
            float d21 = Vector3.Dot(V2, V1);

            float denom = d00 * d11 - d01 * d01;
            if (denom == 0)
                return (-1, -1, -1);
            float beta = (d11 * d20 - d01 * d21) / denom;
            float gamma = (d00 * d21 - d01 * d20) / denom;
            float alpha = 1.0f - beta - gamma;

            return (alpha, beta, gamma);
        }

        private Vector3 GetObjectColor(float u, float v)
        {
            if (!UseTexture)
                return ObjectColor;

            int x = (int)(v * (Texture.Width - 1));
            int y = (int)(u * (Texture.Height - 1));

            Color color = Texture.GetPixel(x, y);

            return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        }

        private Vector3 GetNormal(float u, float v, Vector3 normal, Vector3 tangentU, Vector3 tangentV)
        {
            if (!UseNormalMap)
                return normal;

            int x = (int)(v * (NormalMap.Width - 1));
            int y = (int)(u * (NormalMap.Height - 1));

            Color color = NormalMap.GetPixel(x, y);

            Vector3 normalMap = new(
                color.R / 255f * 2 - 1,
                color.G / 255f * 2 - 1,
                color.B / 255f * 2 - 1
            );

            // M = [Pu, Pv, N]
            Matrix3x3 M = new(tangentU, tangentV, normal);

            return Vector3.Normalize(M * normalMap);
        }

        private void DrawPixel(Graphics g, int x, int y, float u, float v, Vector3 normal)
        {
            float NdotL = Math.Max(Vector3.Dot(normal, LightDirection), 0);

            Vector3 R = 2 * Vector3.Dot(normal, LightDirection) * normal - LightDirection;
            R = Vector3.Normalize(R);

            float VdotR = Math.Max(Vector3.Dot(V, R), 0);

            Vector3 objectColor = GetObjectColor(u, v);
            Vector3 diffuse = kd * LightColor * objectColor * NdotL;
            Vector3 specular = ks * LightColor * objectColor * (float)Math.Pow(VdotR, m);

            Vector3 color = Vector3.Clamp(diffuse + specular, new Vector3(0), new Vector3(1));

            // Convert to 0-255 range
            Color finalColor = Color.FromArgb(
                (int)(color.X * 255),
                (int)(color.Y * 255),
                (int)(color.Z * 255)
            );

            g.FillRectangle(new SolidBrush(finalColor), x, y, 1, 1);
        }

    }

    public struct Matrix3x3
    {
        private Vector3 row1, row2, row3;

        public Matrix3x3(Vector3 col1, Vector3 col2, Vector3 col3)
        {
            row1 = new Vector3(col1.X, col2.X, col3.X);
            row2 = new Vector3(col1.Y, col2.Y, col3.Y);
            row3 = new Vector3(col1.Z, col2.Z, col3.Z);
        }

        public static Vector3 operator *(Matrix3x3 matrix, Vector3 vec)
        {
            return new Vector3(
                Vector3.Dot(matrix.row1, vec),
                Vector3.Dot(matrix.row2, vec),
                Vector3.Dot(matrix.row3, vec)
            );
        }
    }
}
