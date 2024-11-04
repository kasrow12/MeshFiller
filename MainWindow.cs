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
        public Vector3[,] rotSurface = new Vector3[4, 4];
        public Vertex[,] vertices;
        public List<Triangle> mesh = [];

        public float alpha;
        public float beta;
        public int resolution;

        public float kd;
        public float ks;
        public float m;

        private bool triangulationVisible = false;
        private Pen trianglePen = Pens.Blue;
        private Vector3 lightColor = new(1, 1, 1);

        public const int vertexRadius = 10;

        private System.Windows.Forms.Timer animationTimer;
        private bool alphaIncreasing = true;
        private bool betaIncreasing = true;
        private const float ALPHA_SPEED = 0.5f;
        private const float BETA_SPEED = 0.1f;

        public MainWindow()
        {
            InitializeComponent();

            AngleSlider_Scroll(null, null);
            ResolutionSlider_Scroll(null, null);
            LightingSlider_Scroll(null, null);

            SetupTimer();
        }

        private void SetupTimer()
        {
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 4;
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
            UpdateRotation();
        }

        private void ToggleAnimation()
        {
            if (animationTimer.Enabled)
                animationTimer.Stop();
            else
                animationTimer.Start();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (surface == null || mesh == null || mesh.Count == 0)
                return;

            Graphics g = e.Graphics;
            g.ScaleTransform(1, -1);
            g.TranslateTransform(canvas.Width / 2, -canvas.Height / 2);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Vector3 v = rotSurface[i, j];
                    g.FillEllipse(Brushes.Green, v.X - vertexRadius / 2, v.Y - vertexRadius / 2, vertexRadius, vertexRadius);

                    if (i > 0)
                    {
                        Vector3 u = rotSurface[i - 1, j];
                        g.DrawLine(Pens.Black, u.X, u.Y, v.X, v.Y);
                    }
                    if (j > 0)
                    {
                        Vector3 u = rotSurface[i, j - 1];
                        g.DrawLine(Pens.Black, u.X, u.Y, v.X, v.Y);
                    }
                }
            }

            if (triangulationVisible)
                DrawTriangulation(g);
            else
            {
                // DEBUG
                // draw tangents and normals
                //foreach (Vertex vertex in vertices)
                //{
                //    Vector3 p = vertex.RotP;
                //    Vector3 pu = vertex.RotPu;
                //    Vector3 pv = vertex.RotPv;

                //    g.DrawLine(Pens.Red, p.X, p.Y, p.X + pu.X / 4, p.Y + pu.Y / 4);
                //    g.DrawLine(Pens.Green, p.X, p.Y, p.X + pv.X / 4, p.Y + pv.Y / 4);
                //    g.DrawLine(Pens.Blue, p.X, p.Y, p.X + vertex.RotN.X * 20, p.Y + vertex.RotN.Y * 20);
                //}

                foreach (Triangle t in mesh)
                {
                    Scanline.ScanlineFillPolygon(g, [t.V1, t.V2, t.V3]);
                    //Scanline.FillPolygon(g, [t.V1, t.V2, t.V3]);
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

        private float Bernstein(int i, float t) // n variant (n = 3)
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

        private float Bernstein2(int i, float t) // (n - 1) variant
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
        private Vertex Evaluate(float u, float v)
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

            for (int i = 0; i < surface.GetLength(0); i++)
            {
                for (int j = 0; j < surface.GetLength(1); j++)
                {
                    rotSurface[i, j] = Vector3.Transform(surface[i, j], rotZX);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ToggleAnimation();
        }

        private void LoadSurfaceButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadBezierSurface(openFileDialog.FileName);
                canvas.Invalidate();
            }
        }

        // ----- UI ----------------------------------------------------------------

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            canvas.Invalidate();
        }

        private void AngleSlider_Scroll(object sender, EventArgs e)
        {
            alpha = (float)alphaSlider.Value / 10;
            beta = (float)betaSlider.Value / 10;
            UpdateRotation();
        }

        public void UpdateRotation()
        {
            alphaLabel.Text = Math.Round(alpha, 2).ToString() + '°';
            betaLabel.Text = Math.Round(beta, 2).ToString() + '°';

            RotateMesh();
            canvas.Invalidate();
        }

        private void ResolutionSlider_Scroll(object sender, EventArgs e)
        {
            resolution = resolutionSlider.Value;
            resolutionLabel.Text = resolution.ToString();

            GenerateMesh();
            canvas.Invalidate();
        }

        private void LightingSlider_Scroll(object sender, EventArgs e)
        {
            kd = (float)kdSlider.Value / 100.0f;
            ks = (float)ksSlider.Value / 100.0f;
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

        private void triangulationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            triangulationVisible = triangulationCheckbox.Checked;
            canvas.Invalidate();
        }

    }
}
