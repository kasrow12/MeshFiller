using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace MeshFiller.Classes
{
    public class Scanline
    {
        public static void FillTriangle(Graphics g, List<Vertex> vertices)
        {
            Pen pen = new Pen(Color.FromArgb(new Random().Next(0, 255), new Random().Next(0, 255), new Random().Next(0, 255)));

            // Find triangle bounds
            int minY = (int)vertices.Min(p => p.RotP.Y);
            int maxY = (int)vertices.Max(p => p.RotP.Y);

            // Sort vertices by Y coordinate
            var edges = CreateEdgeTable(vertices);

            // Active edge list
            var activeEdges = new List<Edge>();

            // Scan lines
            for (int y = minY; y <= maxY; y++)
            {
                // Update active edge list
                activeEdges.RemoveAll(e => e.YMax == y);
                activeEdges.AddRange(edges.Where(e => e.YMin == y));

                // Sort active edges by X
                activeEdges.Sort((a, b) => a.XCurrent.CompareTo(b.XCurrent));

                // Fill scan line
                for (int i = 0; i < activeEdges.Count; i += 2)
                {
                    if (i + 1 >= activeEdges.Count) break;

                    int xStart = (int)activeEdges[i].XCurrent;
                    int xEnd = (int)activeEdges[i + 1].XCurrent;

                    for (int x = xStart; x <= xEnd; x++)
                    {
                        // Calculate barycentric coordinates
                        //Vector3 bary = CalculateBarycentricCoordinates(
                        //    new Point(x, y),
                        //    screenPoints[0],
                        //    screenPoints[1],
                        //    screenPoints[2]
                        //);

                        //// Interpolate Z and normal
                        //float z = InterpolateZ(triangle, bary);
                        //Vector3 normal = Vector3.Normalize(InterpolateNormal(triangle, bary));

                        // Calculate lighting
                        //Color pixelColor = CalculatePixelColor(normal, z, lightPos);

                        //using (SolidBrush brush = new SolidBrush(pixelColor))
                        //{
                            g.FillRectangle(pen.Brush, x, y, 1, 1);
                        //}
                    }

                    // Update X coordinates for next scanline
                    foreach (var edge in activeEdges)
                    {
                        edge.XCurrent += edge.Slope;
                    }
                }
            }
        }

        private static List<Edge> CreateEdgeTable(List<Vertex> vertices)
        {
            var edges = new List<Edge>();

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex start = vertices[i];
                Vertex end = vertices[(i + 1) % vertices.Count];

                if (start.RotP.Y != end.RotP.Y) // Skip horizontal edges
                {
                    if (start.RotP.Y > end.RotP.Y)
                    {
                        // Swap points to ensure start.Y < end.Y
                        var temp = start;
                        start = end;
                        end = temp;
                    }

                    edges.Add(new Edge
                    {
                        YMin = (int)start.RotP.Y,
                        YMax = (int)end.RotP.Y,
                        XCurrent = start.RotP.X,
                        Slope = (float)(end.RotP.X - start.RotP.X) / (end.RotP.Y - start.RotP.Y)
                    });
                }
            }

            return edges;
        }

        private class Edge
        {
            public int YMin { get; set; }
            public int YMax { get; set; }
            public float XCurrent { get; set; }
            public float Slope { get; set; }
        }
    }
}