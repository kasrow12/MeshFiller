using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MeshFiller.Classes
{
    public class Renderer()
    {
        public float kd { get; set; }
        public float ks { get; set; }
        public float m { get; set; }

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
            if ((int)end.Y >= y)
            {
                aet.Add(new Edge
                {
                    end = (int)end.Y,
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

            int minY = (int)vertices[ind[0]].Y;
            int maxY = (int)vertices[ind[^1]].Y;

            List<Edge> aet = [];

            for (int y = minY; y <= maxY; y++)
            {
                // For each vertex Pi lying on the previous scanline: P[ind[k]].y = y-1
                foreach (int i in ind)
                {
                    Vertex v = vertices[i];
                    if ((int)v.Y == y - 1)
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
                    int x1 = (int)aet[i].XCurrent;
                    int x2 = (int)aet[i + 1].XCurrent;

                    for (int x = x1; x < x2; x++)
                    {
                        (float alpha, float beta, float gamma) = Barycentric(vertices[0].RotP, vertices[1].RotP, vertices[2].RotP, new Vector3(x, y, 0));
                        Vector3 interpolatedNormal = InterpolateNormal(vertices[0].RotN, vertices[1].RotN, vertices[2].RotN, alpha, beta, gamma);

                        float u = alpha * vertices[0].u + beta * vertices[1].u + gamma * vertices[2].u;
                        float v = alpha * vertices[0].v + beta * vertices[1].v + gamma * vertices[2].v;

                        DrawPixel(g, x, y, u, v, interpolatedNormal);
                    }
                }

                // Update x values
                foreach (var edge in aet)
                {
                    edge.XCurrent += edge.InvSlope;
                }
            }
        }

        public Vector3 InterpolateNormal(Vector3 N1, Vector3 N2, Vector3 N3, float alpha, float beta, float gamma)
        {
            // Interpolating normals based on barycentric weights
            Vector3 interpolatedNormal = alpha * N1 + beta * N2 + gamma * N3;

            // Normalize the result to maintain unit length
            return Vector3.Normalize(interpolatedNormal);
        }

        // https://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates
        private static (float, float, float) Barycentric(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
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
            float beta = (d11 * d20 - d01 * d21) / denom;
            float gamma = (d00 * d21 - d01 * d20) / denom;
            float alpha = 1.0f - beta - gamma;

            return (alpha, beta, gamma);
        }

        private Vector3 GetObjectColor(float u, float v)
        {
            return ObjectColor;

            // Sample texture
            int x = (int)(u * 100) % 100;
            int y = (int)(v * 100) % 100;

            // Random color
            return new Vector3((float)x / 255, (float)y / 255, 0.5f);
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
}
