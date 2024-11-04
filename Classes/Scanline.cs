using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace MeshFiller.Classes
{
    public class Scanline
    {
        public static void ScanlineFillPolygon(Graphics g, List<Vertex> vertices)
        {
            var sortedVertices = vertices.OrderBy(v => v.RotP.Y).ToList();

            int yMin = (int)sortedVertices.First().RotP.Y;
            int yMax = (int)sortedVertices.Last().RotP.Y;

            List<Edge> aet = [];
            List<Edge> edgeTable = CreateEdgeTable(vertices);
            Pen pen = new Pen(Color.FromArgb(new Random().Next(0, 255), new Random().Next(0, 255), new Random().Next(0, 255)));

            for (int y = yMin; y <= yMax; y++)
            {
                aet.RemoveAll(edge => edge.YMax == y);

                // Add edges starting at the current y
                var edgesToAdd = edgeTable.Where(edge => edge.YMin == y).ToList();
                aet.AddRange(edgesToAdd);

                // Sort AET by current x values
                aet = aet.OrderBy(edge => edge.CurrentX).ToList();

                // Fill pixels between pairs of edges
                for (int i = 0; i < aet.Count - 1; i += 2)
                {
                    int xStart = (int)Math.Round(aet[i].CurrentX);
                    int xEnd = (int)Math.Round(aet[i + 1].CurrentX);
                    g.DrawLine(pen, xStart, y, xEnd, y);
                }

                foreach (var edge in aet)
                {
                    edge.CurrentX += edge.SlopeInverse;
                }

                //Debug.WriteLine($"Y: {y}");
                //Debug.WriteLine($"AET: {string.Join(", ", aet.Select(e => $"({e.CurrentX}, {e.YMin})"))}");
            }
        }

        private static List<Edge> CreateEdgeTable(List<Vertex> vertices)
        {
            List<Edge> edges = [];

            for (int i = 0; i < vertices.Count; i++)
            {
                var start = vertices[i];
                var end = vertices[(i + 1) % vertices.Count];

                // Skip horizontal edges
                if (start.RotP.Y == end.RotP.Y)
                    continue;

                Vertex top = start.RotP.Y < end.RotP.Y ? start : end;
                Vertex bottom = start.RotP.Y < end.RotP.Y ? end : start;

                float slopeInverse = (bottom.RotP.X - top.RotP.X) / (bottom.RotP.Y - top.RotP.Y);

                edges.Add(new Edge
                {
                    YMin = (int)top.RotP.Y,
                    YMax = (int)bottom.RotP.Y,
                    CurrentX = top.RotP.X,
                    SlopeInverse = slopeInverse
                });
            }

            return edges;
        }

        private class Edge
        {
            public int YMin { get; set; }
            public int YMax { get; set; }
            public float CurrentX { get; set; }
            public float SlopeInverse { get; set; }
        }

        //    public static void FillPolygon(Graphics g, List<Vertex> vertices)
        //    {
        //        // Sort vertices by y-coordinate
        //        vertices = vertices.OrderBy(v => v.RotP.Y).ToList();
        //        int ymin = (int)vertices.First().RotP.Y;
        //        int ymax = (int)vertices.Last().RotP.Y;
        //            Pen pen = new Pen(Color.FromArgb(new Random().Next(0, 255), new Random().Next(0, 255), new Random().Next(0, 255)));

        //        // Active Edge Table (AET) to store edges intersecting the scanline
        //        List<Edge> AET = new List<Edge>();

        //        // Loop through each scanline from ymin to ymax
        //        for (int y = ymin; y <= ymax; y++)
        //        {
        //            // Step 1: Update AET for current y by adding/removing edges

        //            for (int i = 0; i < vertices.Count; i++)
        //            {
        //                Vertex current = vertices[i];
        //                Vertex previous = vertices[(i - 1 + vertices.Count) % vertices.Count];
        //                Vertex next = vertices[(i + 1) % vertices.Count];

        //                if ((int)current.RotP.Y == y - 1)
        //                {
        //                    // Previous vertex check
        //                    if (previous.RotP.Y >= current.RotP.Y)
        //                    {
        //                        AddEdgeToAET(AET, current, previous);
        //                    }
        //                    else
        //                    {
        //                        RemoveEdgeFromAET(AET, current, previous);
        //                    }

        //                    // Next vertex check
        //                    if (next.RotP.Y >= current.RotP.Y)
        //                    {
        //                        AddEdgeToAET(AET, current, next);
        //                    }
        //                    else
        //                    {
        //                        RemoveEdgeFromAET(AET, current, next);
        //                    }
        //                }
        //            }

        //            // Step 2: Sort AET by x
        //            AET = AET.OrderBy(edge => edge.x).ToList();

        //            // Step 3: Fill pixels between pairs of x-coordinates
        //            for (int j = 0; j < AET.Count; j += 2)
        //            {
        //                if (j + 1 < AET.Count)
        //                {
        //                    int startX = (int)Math.Round(AET[j].x);
        //                    int endX = (int)Math.Round(AET[j + 1].x);
        //                    g.FillRectangle(pen.Brush, startX, y, endX - startX, 1);
        //                }
        //            }

        //            // Step 4: Update x for edges in AET
        //            foreach (var edge in AET)
        //            {
        //                edge.x += edge.slopeInv;
        //            }

        //            // Remove edges where ymax == y (end of edge)
        //            AET.RemoveAll(edge => edge.ymax == y);
        //        }
        //    }

        //    // Helper method to add edge to AET
        //    private static void AddEdgeToAET(List<Edge> AET, Vertex v1, Vertex v2)
        //    {
        //        float dx = v2.RotP.X - v1.RotP.X;
        //        float dy = v2.RotP.Y - v1.RotP.Y;
        //        if (dy != 0)
        //        {
        //            float slopeInv = dx / dy;
        //            float ymax = Math.Max(v1.RotP.Y, v2.RotP.Y);
        //            float x = (v1.RotP.Y < v2.RotP.Y) ? v1.RotP.X : v2.RotP.X;
        //            AET.Add(new Edge(x, slopeInv, ymax));
        //        }
        //    }

        //    // Helper method to remove edge from AET
        //    private static void RemoveEdgeFromAET(List<Edge> AET, Vertex v1, Vertex v2)
        //    {
        //        AET.RemoveAll(e => (e.x == v1.RotP.X && e.ymax == Math.Max(v1.RotP.Y, v2.RotP.Y)));
        //    }


        //}

        //public class Edge
        //{
        //    public float x;       // Current x intersection
        //    public float slopeInv; // 1 / slope (for x update)
        //    public float ymax;    // Maximum y for the edge

        //    public Edge(float x, float slopeInv, float ymax)
        //    {
        //        this.x = x;
        //        this.slopeInv = slopeInv;
        //        this.ymax = ymax;
        //    }
        //}
    }
    }