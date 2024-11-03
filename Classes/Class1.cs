using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MeshFiller.Classes
{
    public class PolygonRenderer
    {
        // Lighting parameters
        private float kd = 0.7f; // diffuse coefficient
        private float ks = 0.3f; // specular coefficient
        private int m = 20; // specular power
        private Vector3 lightColor = new Vector3(1, 1, 1); // IL
        private Vector3 objectColor = new Vector3(1, 0, 0); // IO default red
        private Vector3 viewVector = new Vector3(0, 0, 1); // V constant view vector

        // Light position animation
        private float lightZ = 100f;
        private float lightAnimationAngle = 0f;
        private bool isAnimating = false;
        private const float SPIRAL_A = 100f; // spiral radius
        private const float SPIRAL_B = 10f;  // spiral step

        // Active edges for scanline algorithm
        private class ActiveEdge
        {
            public int yMax;
            public float x;
            public float dX;
            public Triangle triangle;
            public int startVertex;
            public int endVertex;
        }

        public void RenderTriangle(Triangle triangle, Graphics g)
        {
            // Convert triangle vertices to screen coordinates (assuming they're already projected)
            Point[] screenPoints = new Point[3]
            {
            new Point((int)triangle.V1.RotP.X, (int)triangle.V1.RotP.Y),
            new Point((int)triangle.V2.RotP.X, (int)triangle.V2.RotP.Y),
            new Point((int)triangle.V3.RotP.X, (int)triangle.V3.RotP.Y)
            };

            // Find y bounds
            int yMin = screenPoints.Min(p => p.Y);
            int yMax = screenPoints.Max(p => p.Y);

            // Create and sort edges
            List<ActiveEdge> edges = CreateEdgeList(triangle, screenPoints);

            // Scanline algorithm
            List<ActiveEdge> activeEdges = new List<ActiveEdge>();

            for (int y = yMin; y <= yMax; y++)
            {
                // Remove inactive edges
                activeEdges.RemoveAll(e => e.yMax <= y);

                // Add new active edges
                var newEdges = edges.Where(e => screenPoints[e.startVertex].Y == y).ToList();
                activeEdges.AddRange(newEdges);
                edges.RemoveAll(e => screenPoints[e.startVertex].Y == y);

                // Sort active edges by x
                activeEdges = activeEdges.OrderBy(e => e.x).ToList();

                // Fill between pairs of edges
                for (int i = 0; i < activeEdges.Count - 1; i += 2)
                {
                    int xStart = (int)activeEdges[i].x;
                    int xEnd = (int)activeEdges[i + 1].x;

                    for (int x = xStart; x <= xEnd; x++)
                    {
                        // Calculate barycentric coordinates
                        Vector3 bary = CalculateBarycentric(
                            new Vector2(x, y),
                            new Vector2(screenPoints[0].X, screenPoints[0].Y),
                            new Vector2(screenPoints[1].X, screenPoints[1].Y),
                            new Vector2(screenPoints[2].X, screenPoints[2].Y));

                        // Interpolate Z and normal
                        float z = InterpolateZ(triangle, bary);
                        Vector3 normal = Vector3.Normalize(InterpolateNormal(triangle, bary));

                        // Calculate light position
                        Vector3 lightPos = CalculateLightPosition();
                        Vector3 L = Vector3.Normalize(lightPos - new Vector3(x, y, z));

                        // Calculate reflection vector
                        float NdotL = Vector3.Dot(normal, L);
                        Vector3 R = Vector3.Normalize(2 * NdotL * normal - L);

                        // Calculate lighting
                        Vector3 color = CalculatePixelColor(normal, L, R);

                        // Set pixel color
                        //if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                        //{
                        //bitmap.SetPixel(x, y, ColorFromVector(color));
                        Debug.WriteLine(color);
                        Color xd = ColorFromVector(color);
                        if (color.X == float.NaN)
                            ;
                        g.FillRectangle(new SolidBrush(xd), x, y, 1, 1);
                        //}
                    }
                }

                // Update X coordinates
                foreach (var edge in activeEdges)
                {
                    edge.x += edge.dX;
                }
            }
        }

        private List<ActiveEdge> CreateEdgeList(Triangle triangle, Point[] screenPoints)
        {
            List<ActiveEdge> edges = new List<ActiveEdge>();

            for (int i = 0; i < 3; i++)
            {
                int j = (i + 1) % 3;
                Point start = screenPoints[i];
                Point end = screenPoints[j];

                if (start.Y != end.Y) // Skip horizontal edges
                {
                    ActiveEdge edge = new ActiveEdge
                    {
                        yMax = Math.Max(start.Y, end.Y),
                        x = start.Y < end.Y ? start.X : end.X,
                        dX = (float)(end.X - start.X) / (end.Y - start.Y),
                        triangle = triangle,
                        startVertex = start.Y < end.Y ? i : j,
                        endVertex = start.Y < end.Y ? j : i
                    };
                    edges.Add(edge);
                }
            }

            return edges;
        }

        private Vector3 CalculateBarycentric(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 v0 = b - a;
            Vector2 v1 = c - a;
            Vector2 v2 = p - a;

            float d00 = Vector2.Dot(v0, v0);
            float d01 = Vector2.Dot(v0, v1);
            float d11 = Vector2.Dot(v1, v1);
            float d20 = Vector2.Dot(v2, v0);
            float d21 = Vector2.Dot(v2, v1);

            float denom = d00 * d11 - d01 * d01;
            float v = (d11 * d20 - d01 * d21) / denom;
            float w = (d00 * d21 - d01 * d20) / denom;
            float u = 1.0f - v - w;

            return new Vector3(u, v, w);
        }

        private float InterpolateZ(Triangle triangle, Vector3 bary)
        {
            return bary.X * triangle.V1.RotP.Z +
                   bary.Y * triangle.V2.RotP.Z +
                   bary.Z * triangle.V3.RotP.Z;
        }

        private Vector3 InterpolateNormal(Triangle triangle, Vector3 bary)
        {
            return bary.X * triangle.V1.RotN +
                   bary.Y * triangle.V2.RotN +
                   bary.Z * triangle.V3.RotN;
        }

        private Vector3 CalculateLightPosition()
        {
            if (isAnimating)
                lightAnimationAngle += 0.1f;

            float x = SPIRAL_A * (float)Math.Cos(lightAnimationAngle);
            float y = SPIRAL_A * (float)Math.Sin(lightAnimationAngle);
            float z = lightZ + SPIRAL_B * lightAnimationAngle;

            return new Vector3(x, y, z);
        }

        private Vector3 CalculatePixelColor(Vector3 N, Vector3 L, Vector3 R)
        {
            float NdotL = Math.Max(0, Vector3.Dot(N, L));
            float VdotR = Math.Max(0, Vector3.Dot(viewVector, R));

            // Calculate diffuse component
            Vector3 diffuse = kd * lightColor * objectColor * NdotL;

            // Calculate specular component
            Vector3 specular = ks * lightColor * objectColor * (float)Math.Pow(VdotR, m);

            // Combine components
            Vector3 color = diffuse + specular;

            // Clamp values between 0 and 1
            color.X = Math.Min(1, color.X);
            color.Y = Math.Min(1, color.Y);
            color.Z = Math.Min(1, color.Z);

            return color;
        }

        private Color ColorFromVector(Vector3 color)
        {
            return Color.FromArgb(
                (int)(color.X * 255),
                (int)(color.Y * 255),
                (int)(color.Z * 255));
        }

        // Public methods to control rendering parameters
        public void SetLightZ(float z) => lightZ = z;
        public void SetDiffuseCoefficient(float value) => kd = value;
        public void SetSpecularCoefficient(float value) => ks = value;
        public void SetSpecularPower(int value) => m = value;
        public void SetObjectColor(Color color) =>
            objectColor = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        public void SetLightColor(Color color) =>
            lightColor = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        public void ToggleLightAnimation() => isAnimating = !isAnimating;
    }
}
