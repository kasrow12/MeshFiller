using System.Numerics;

namespace MeshFiller.Classes
{
    public class BezierSurface
    {
        public Vertex[,] vertices;
        public Vector3[,] surface;
        public Vector3[,] rotSurface = new Vector3[4, 4];
        public List<Triangle> Mesh { get; private set; } = [];

        public float Alpha { get; set; } = 0;
        public float Beta { get; set; } = 0;
        public int Resolution { get; set; } = 20;
        public float Scale { get; set; } = 1;

        public static float Bernstein(int i, float t) // n variant (n = 3)
        {
            return i switch
            {
                0 => (1 - t) * (1 - t) * (1 - t),
                1 => 3 * t * (1 - t) * (1 - t),
                2 => 3 * t * t * (1 - t),
                3 => t * t * t,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static float Bernstein2(int i, float t) // (n - 1) variant
        {
            return i switch
            {
                0 => (1 - t) * (1 - t),
                1 => 2 * t * (1 - t),
                2 => t * t,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        // Evaluate point on the surface and return Vertex
        public Vertex Evaluate(float u, float v)
        {
            Vector3 position = Vector3.Zero;
            Vector3 tangentU = Vector3.Zero;
            Vector3 tangentV = Vector3.Zero;

            // P(u, v)
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float Bu = Bernstein(i, u);
                    float Bv = Bernstein(j, v);
                    position += surface[i, j] * Bu * Bv;
                }
            }

            // Pu(u, v)
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float Bu = Bernstein2(i, u);
                    float Bv = Bernstein(j, v);
                    tangentU += (surface[i + 1, j] - surface[i, j]) * Bu * Bv;
                }
            }
            tangentU *= 3; // n * ()

            // Pv(u, v)
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float Bu = Bernstein(i, u);
                    float Bv = Bernstein2(j, v);
                    tangentV += (surface[i, j + 1] - surface[i, j]) * Bu * Bv;
                }
            }
            tangentV *= 3; // m * ()

            Vector3 normal = Vector3.Normalize(Vector3.Cross(tangentU, tangentV));

            return new Vertex
            {
                u = u,
                v = v,
                P = position,
                Pu = tangentU,
                Pv = tangentV,
                N = normal
            };
        }

        // Generate the triangulated mesh
        public void GenerateMesh()
        {
            if (surface is null)
                return;

            Mesh.Clear();

            vertices = new Vertex[Resolution, Resolution];
            for (int i = 0; i < Resolution; i++)
            {
                for (int j = 0; j < Resolution; j++)
                {
                    float u = (float)i / (Resolution - 1);
                    float v = (float)j / (Resolution - 1);
                    vertices[i, j] = Evaluate(u, v);
                }
            }

            for (int i = 0; i < Resolution - 1; i++)
            {
                for (int j = 0; j < Resolution - 1; j++)
                {
                    // Create two triangles from each square
                    Vertex upLeft = vertices[i, j];
                    Vertex downLeft = vertices[i + 1, j];
                    Vertex upRight = vertices[i, j + 1];
                    Vertex downRight = vertices[i + 1, j + 1];

                    Mesh.Add(new Triangle(upLeft, downLeft, upRight));
                    Mesh.Add(new Triangle(downLeft, downRight, upRight));
                }
            }

            RotateMesh();
        }


        public void RotateMesh()
        {
            if (vertices == null)
                return;

            Quaternion rotZ = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI * Alpha / 180f);
            Quaternion rotX = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * Beta / 180f);
            Quaternion rotZX = Quaternion.Concatenate(rotZ, rotX);

            Parallel.For(0, vertices.GetLength(0), i =>
            {
                for (int j = 0; j < vertices.GetLength(1); j++)
                {
                    vertices[i, j].RotP = Scale * Vector3.Transform(vertices[i, j].P, rotZX);
                    vertices[i, j].RotPu = Scale * Vector3.Transform(vertices[i, j].Pu, rotZX);
                    vertices[i, j].RotPv = Scale * Vector3.Transform(vertices[i, j].Pv, rotZX);
                    vertices[i, j].RotN = Scale * Vector3.Transform(vertices[i, j].N, rotZX);
                }
            });

            for (int i = 0; i < surface.GetLength(0); i++)
            {
                for (int j = 0; j < surface.GetLength(1); j++)
                {
                    rotSurface[i, j] = Scale * Vector3.Transform(surface[i, j], rotZX);
                }
            }

            Parallel.ForEach(Mesh, t =>
            {
                t.Recalculate();
            });
        }
    }
}
