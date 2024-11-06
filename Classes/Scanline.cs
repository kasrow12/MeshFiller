using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace MeshFiller.Classes
{
    public class Scanline
    {
        public static void FillPolygon(Graphics g, List<Vertex> vertices)
        {
            List<int> ind = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                ind.Add(i);
            }
            ind.Sort((a, b) => ((int)vertices[a].RotP.Y).CompareTo((int)vertices[b].RotP.Y));
            int minY = (int)vertices[ind[0]].RotP.Y;
            int maxY = (int)vertices[ind[^1]].RotP.Y;

            // Random brush
            Brush brush = new SolidBrush(Color.FromArgb(new Random().Next(256), new Random().Next(256), new Random().Next(256)));

            List<Edge> aet = new List<Edge>();

            for (int y = minY; y <= maxY; y++)
            {
                // For each vertex Pi lying on the previous scanline: P[ind[k]].y = y-1
                for (int k = 0; k < ind.Count; k++)
                {
                    if ((int)vertices[ind[k]].RotP.Y == y - 1)
                    {
                        // Find the previous vertex Pi-1
                        int prevInd = (ind[k] - 1 + vertices.Count) % vertices.Count;
                        if ((int)vertices[prevInd].RotP.Y >= y) //
                        {
                            //Debug.WriteLine("Added edge: " + ind[k] + " " + prevInd);
                            // Add edge Pi-1Pi to AET
                            aet.Add(new Edge
                            {
                                start = ind[k],
                                stop = prevInd,
                                XCurrent = vertices[ind[k]].RotP.X,
                                InvSlope = (vertices[prevInd].RotP.X - vertices[ind[k]].RotP.X) / (vertices[prevInd].RotP.Y - vertices[ind[k]].RotP.Y)

                            });
                        }
                        else if (vertices[prevInd].RotP.Y < y)
                        {
                            //Debug.WriteLine("Removed edge: " + ind[k] + " " + prevInd);
                            // Remove edge Pi-1Pi from AET
                            int index = aet.FindIndex(e => e.start == prevInd && e.stop == ind[k]);
                            if (index != -1)
                            {
                                aet.RemoveAt(index);
                            }
                        }

                        // Find the next vertex Pi+1
                        int nextInd = (ind[k] + 1) % vertices.Count;
                        if (nextInd < vertices.Count && (int)vertices[nextInd].RotP.Y >= y)
                        {
                            //Debug.WriteLine("Added edge: " + ind[k] + " " + nextInd);
                            // Add edge Pi+1Pi to AET
                            aet.Add(new Edge
                            {
                                start = ind[k],
                                stop = nextInd,
                                XCurrent = vertices[ind[k]].RotP.X,
                                InvSlope = (vertices[nextInd].RotP.X - vertices[ind[k]].RotP.X) / (vertices[nextInd].RotP.Y - vertices[ind[k]].RotP.Y)
                            });
                        }
                        else if (nextInd < vertices.Count && (int)vertices[nextInd].RotP.Y < y)
                        {
                            //Debug.WriteLine("Removed edge: " + ind[k] + " " + nextInd);
                            // Remove edge Pi+1Pi from AET
                            int index = aet.FindIndex(e => e.start == nextInd && e.stop == ind[k]);
                            if (index != -1)
                            {
                                aet.RemoveAt(index);
                            }
                        }
                    }
                }

                // Update AET:
                // Sort in ascending order of x
                aet = aet.OrderBy(e => e.XCurrent).ToList();

                // Fill pixels between edges 0-1, 2-3, ..
                for (int i = 0; i < aet.Count; i += 2)
                {
                    int x1 = (int)Math.Round(aet[i].XCurrent);
                    int x2 = (int)Math.Round(aet[i + 1].XCurrent);
                    g.FillRectangle(brush, x1, y, x2 - x1, 1);
                }

                // Update x values for new scanline
                foreach (var edge in aet)
                {
                    edge.XCurrent += edge.InvSlope;
                }
            }
        }

        private class Edge
        {
            public int start;
            public int stop;
            public float XCurrent;
            public float InvSlope;
        }
    }
}
