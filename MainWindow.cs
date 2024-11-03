using MeshFiller.Classes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Numerics;

namespace MeshFiller
{
    public partial class MainWindow : Form
    {
        public Vector3[,] surface;
        public Vertex[,] vertices;
        public List<Triangle> mesh = [];
        private PolygonRenderer renderer;

        public float alpha;
        public float beta;
        public int resolution = 10;

        public float kd = 0.5f;
        public float ks = 0.5f;
        public float m = 10.0f;

        private bool triangulationVisible = false;
        private Pen trianglePen = Pens.Blue;
        private Vector3 lightColor = new Vector3(1, 1, 1);

        public const int vertexRadius = 10;

        private System.Windows.Forms.Timer animationTimer;
        private bool alphaIncreasing = true;
        private bool betaIncreasing = true;
        private const float ALPHA_SPEED = 0.5f;
        private const float BETA_SPEED = 0.1f;

        public MainWindow()
        {
            InitializeComponent();

            AlphaSlider_Scroll(null, null);
            BetaSlider_Scroll(null, null);
            ResolutionSlider_Scroll(null, null);
            kdSlider_Scroll(null, null);
            ksSlider_Scroll(null, null);
            mSlider_Scroll(null, null);

            renderer = new PolygonRenderer();

            SetupTimer();
        }

        private void SetupTimer()
        {
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 4; // Approximately 60 FPS
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (alphaIncreasing)
            {
                alpha += ALPHA_SPEED;
                if (alpha >= 90.0f)
                {
                    alpha = 90.0f;
                    alphaIncreasing = false;
                }
            }
            else
            {
                alpha -= ALPHA_SPEED;
                if (alpha <= -90.0f)
                {
                    alpha = -90.0f;
                    alphaIncreasing = true;
                }
            }

            if (betaIncreasing)
            {
                beta += BETA_SPEED;
                if (beta >= 60.0f)
                {
                    beta = 60.0f;
                    betaIncreasing = false;
                }
            }
            else
            {
                beta -= BETA_SPEED;
                if (beta <= 50.0f)
                {
                    beta = 50.0f;
                    betaIncreasing = true;
                }
            }

            alphaSlider.Value = (int)(alpha * 10);
            betaSlider.Value = (int)(beta * 10);
            RotateMesh();
        }

        private void ToggleAnimation()
        {
            if (animationTimer.Enabled)
                animationTimer.Stop();
            else
                animationTimer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadBezierSurface(openFileDialog.FileName);
                canvas.Invalidate();
            }
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (surface == null || mesh == null || mesh.Count == 0)
                return;

            Graphics g = e.Graphics;
            g.ScaleTransform(1, -1);
            g.TranslateTransform(canvas.Width / 2, -canvas.Height / 2);

            Pen pen = new(Color.Black);

            Quaternion rotZ = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI * alpha / 180f);
            Quaternion rotX = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * beta / 180f);
            Quaternion rotZX = Quaternion.Concatenate(rotZ, rotX);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Vector3 b = surface[i, j];
                    b = Vector3.Transform(b, rotZX);

                    g.FillEllipse(Brushes.Green, b.X - vertexRadius / 2, b.Y - vertexRadius / 2, vertexRadius, vertexRadius);

                    if (i > 0)
                    {
                        Vector3 a = surface[i - 1, j];
                        a = Vector3.Transform(a, rotZX);
                        g.DrawLine(pen, a.X, a.Y, b.X, b.Y);
                    }
                    if (j > 0)
                    {
                        Vector3 c = surface[i, j - 1];
                        c = Vector3.Transform(c, rotZX);
                        g.DrawLine(pen, c.X, c.Y, b.X, b.Y);
                    }
                }
            }

            if (triangulationVisible)
                DrawTriangulation(g);
            else
            {
                Vector3 lightPosition = new Vector3(0, 0, 1000);

                foreach (Triangle triangle in mesh)
                {
                    // Project 3D points to 2D screen coordinates
                    Point[] screenPoints = new Point[3]
                    {
            Project(triangle.V1.RotP),
            Project(triangle.V2.RotP),
            Project(triangle.V3.RotP)
                    };

                    FillTriangle(g, screenPoints, triangle, lightPosition);
                }

            }
        }

        public void DrawTriangulation(Graphics g)
        {
            foreach (Triangle triangle in mesh)
            {
                Vector3 v1 = triangle.V1.RotP;
                Vector3 v2 = triangle.V2.RotP;
                Vector3 v3 = triangle.V3.RotP;

                g.DrawLine(trianglePen, v1.X, v1.Y, v2.X, v2.Y);
                g.DrawLine(trianglePen, v2.X, v2.Y, v3.X, v3.Y);
                g.DrawLine(trianglePen, v3.X, v3.Y, v1.X, v1.Y);
            }
        }

        private void LoadBezierSurface(string fileName)
        {
            surface = new Vector3[4, 4];
            try
            {
                string[] lines = File.ReadAllLines(fileName);
                if (lines.Length != 16)
                    throw new Exception("File must contain exactly 16 lines.");

                for (int i = 0; i < 16; i++)
                {
                    string[] tokens = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (tokens.Length != 3)
                        throw new Exception("Each line must contain exactly 3 values (x, y, z).");

                    float x = float.Parse(tokens[0], CultureInfo.InvariantCulture);
                    float y = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                    float z = float.Parse(tokens[2], CultureInfo.InvariantCulture);

                    surface[i / 4, i % 4] = new(x, y, z);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading surface: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            GenerateMesh();
        }

        private float Bernstein(int i, float t)
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

        private float BernsteinDerivative(int i, float t)
        {
            return i switch
            {
                0 => -3 * (1 - t) * (1 - t),
                1 => 3 * (1 - t) * (1 - t) - 6 * t * (1 - t),
                2 => 6 * t * (1 - t) - 3 * t * t,
                3 => 3 * t * t,
                _ => 0,
            };
        }

        // Evaluate point on the surface and return Vertex
        private Vertex Evaluate(float u, float v)
        {
            Vector3 position = Vector3.Zero;
            Vector3 tangentU = Vector3.Zero;
            Vector3 tangentV = Vector3.Zero;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float Bu = Bernstein(i, u);
                    float Bv = Bernstein(j, v);
                    float dBu = BernsteinDerivative(i, u);
                    float dBv = BernsteinDerivative(j, v);

                    position += Bu * Bv * surface[i, j];
                    tangentU += dBu * Bv * surface[i, j];
                    tangentV += Bu * dBv * surface[i, j];
                }
            }

            Vector3 normal = Vector3.Cross(tangentU, tangentV);
            normal = Vector3.Normalize(normal);

            return new Vertex
            {
                P = position,
                Pu = tangentU,
                Pv = tangentV,
                N = normal
            };
        }

        // Generate the triangulated mesh
        public void GenerateMesh()
        {
            mesh.Clear();

            vertices = new Vertex[resolution, resolution];
            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    float u = (float)i / (resolution - 1);
                    float v = (float)j / (resolution - 1);
                    vertices[i, j] = Evaluate(u, v);
                }
            }

            for (int i = 0; i < resolution - 1; i++)
            {
                for (int j = 0; j < resolution - 1; j++)
                {
                    // Create two triangles from each square
                    Vertex upLeft = vertices[i, j];
                    Vertex downLeft = vertices[i + 1, j];
                    Vertex upRight = vertices[i, j + 1];
                    Vertex downRight = vertices[i + 1, j + 1];

                    mesh.Add(new Triangle(upLeft, downLeft, upRight));
                    mesh.Add(new Triangle(downLeft, downRight, upRight));
                }
            }

            RotateMesh();
        }

        public void RotateMesh()
        {
            alphaLabel.Text = Math.Round(alpha, 2).ToString() + '°';
            betaLabel.Text = Math.Round(beta, 2).ToString() + '°';

            if (vertices == null)
                return;

            Quaternion rotZ = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI * alpha / 180f);
            Quaternion rotX = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * beta / 180f);
            Quaternion rotZX = Quaternion.Concatenate(rotZ, rotX);

            for (int i = 0; i < vertices.GetLength(0); i++)
            {
                for (int j = 0; j < vertices.GetLength(1); j++)
                {
                    vertices[i, j].RotP = Vector3.Transform(vertices[i, j].P, rotZX);
                    vertices[i, j].RotPu = Vector3.Transform(vertices[i, j].Pu, rotZX);
                    vertices[i, j].RotPv = Vector3.Transform(vertices[i, j].Pv, rotZX);
                    vertices[i, j].RotN = Vector3.Transform(vertices[i, j].N, rotZX);
                }
            }

            canvas.Invalidate();
        }

        private void AlphaSlider_Scroll(object sender, EventArgs e)
        {
            alpha = (float)alphaSlider.Value / 10;
            RotateMesh();
        }

        private void BetaSlider_Scroll(object sender, EventArgs e)
        {
            beta = (float)betaSlider.Value / 10;
            RotateMesh();
        }

        private void ResolutionSlider_Scroll(object sender, EventArgs e)
        {
            resolution = resolutionSlider.Value;
            resolutionLabel.Text = resolution.ToString();

            if (surface != null)
                GenerateMesh();

            canvas.Invalidate();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            canvas.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ToggleAnimation();
        }

        private void triangulationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            triangulationVisible = triangulationCheckbox.Checked;
            canvas.Invalidate();
        }


        private void kdSlider_Scroll(object sender, EventArgs e)
        {
            kd = (float)kdSlider.Value / 100.0f;
            UpdateLighting();
        }

        private void ksSlider_Scroll(object sender, EventArgs e)
        {
            ks = (float)ksSlider.Value / 100.0f;
            UpdateLighting();
        }

        private void mSlider_Scroll(object sender, EventArgs e)
        {
            m = (float)mSlider.Value;
            UpdateLighting();
        }

        public void UpdateLighting()
        {
            kdLabel.Text = Math.Round(kd, 2).ToString();
            ksLabel.Text = Math.Round(ks, 2).ToString();
            mLabel.Text = Math.Round(m, 2).ToString();
            canvas.Invalidate();
        }

        private void FillTriangle(Graphics g, Point[] screenPoints, Triangle triangle, Vector3 lightPos)
        {
            // Find triangle bounds
            int minY = screenPoints.Min(p => p.Y);
            int maxY = screenPoints.Max(p => p.Y);

            // Sort vertices by Y coordinate
            var edges = CreateEdgeTable(screenPoints, triangle);

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
                        Vector3 bary = CalculateBarycentricCoordinates(
                            new Point(x, y),
                            screenPoints[0],
                            screenPoints[1],
                            screenPoints[2]
                        );

                        // Interpolate Z and normal
                        float z = InterpolateZ(triangle, bary);
                        Vector3 normal = Vector3.Normalize(InterpolateNormal(triangle, bary));

                        // Calculate lighting
                        Color pixelColor = CalculatePixelColor(normal, z, lightPos);

                        using (SolidBrush brush = new SolidBrush(pixelColor))
                        {
                            g.FillRectangle(brush, x, y, 1, 1);
                        }
                    }

                    // Update X coordinates for next scanline
                    foreach (var edge in activeEdges)
                    {
                        edge.XCurrent += edge.Slope;
                    }
                }
            }
        }

        private List<Edge> CreateEdgeTable(Point[] points, Triangle triangle)
        {
            var edges = new List<Edge>();

            for (int i = 0; i < 3; i++)
            {
                Point start = points[i];
                Point end = points[(i + 1) % 3];

                if (start.Y != end.Y) // Skip horizontal edges
                {
                    if (start.Y > end.Y)
                    {
                        // Swap points to ensure start.Y < end.Y
                        var temp = start;
                        start = end;
                        end = temp;
                    }

                    edges.Add(new Edge
                    {
                        YMin = start.Y,
                        YMax = end.Y,
                        XCurrent = start.X,
                        Slope = (float)(end.X - start.X) / (end.Y - start.Y)
                    });
                }
            }

            return edges;
        }

        private Color CalculatePixelColor(Vector3 normal, float z, Vector3 lightPos)
        {
            Vector3 viewVector = new Vector3(0, 0, 1);
            Vector3 lightDir = Vector3.Normalize(lightPos - new Vector3(0, 0, z));

            // Calculate reflection vector R = 2(N·L)N - L
            float NdotL = Vector3.Dot(normal, lightDir);
            Vector3 reflection = Vector3.Normalize((2 * NdotL * normal) - lightDir);

            float diffuse = Math.Max(0, NdotL);
            float specular = (float)Math.Pow(Math.Max(0, Vector3.Dot(reflection, viewVector)), m);

            // Get object color (either from texture or solid color)
            Vector3 objectColor = new Vector3(1, 0, 0);

            // Calculate final color components
            float r = Math.Min(1, kd * diffuse * lightColor.X * objectColor.X +
                                 ks * specular * lightColor.X);
            float g = Math.Min(1, kd * diffuse * lightColor.Y * objectColor.Y +
                                 ks * specular * lightColor.Y);
            float b = Math.Min(1, kd * diffuse * lightColor.Z * objectColor.Z +
                                 ks * specular * lightColor.Z);

            // Clamp values between 0 and 255
            int red = Math.Max(0, Math.Min(255, (int)(r * 255)));
            int green = Math.Max(0, Math.Min(255, (int)(g * 255)));
            int blue = Math.Max(0, Math.Min(255, (int)(b * 255)));

            return Color.FromArgb(red, green, blue);
        }

        private Vector3 InterpolateNormal(Triangle triangle, Vector3 bary)
        {
            return new Vector3(
                bary.X * triangle.V1.RotN.X + bary.Y * triangle.V2.RotN.X + bary.Z * triangle.V3.RotN.X,
                bary.X * triangle.V1.RotN.Y + bary.Y * triangle.V2.RotN.Y + bary.Z * triangle.V3.RotN.Y,
                bary.X * triangle.V1.RotN.Z + bary.Y * triangle.V2.RotN.Z + bary.Z * triangle.V3.RotN.Z
            );
        }

        private float InterpolateZ(Triangle triangle, Vector3 bary)
        {
            return bary.X * triangle.V1.RotP.Z +
                   bary.Y * triangle.V2.RotP.Z +
                   bary.Z * triangle.V3.RotP.Z;
        }

        private Vector3 CalculateBarycentricCoordinates(Point p, Point p1, Point p2, Point p3)
        {
            float denominator = ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            float a = ((p2.Y - p3.Y) * (p.X - p3.X) + (p3.X - p2.X) * (p.Y - p3.Y)) / denominator;
            float b = ((p3.Y - p1.Y) * (p.X - p3.X) + (p1.X - p3.X) * (p.Y - p3.Y)) / denominator;
            float c = 1 - a - b;

            return new Vector3(a, b, c);
        }

        private Point Project(Vector3 point)
        {
            return new Point(
                (int)(point.X),
                (int)(point.Y)
            );
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
