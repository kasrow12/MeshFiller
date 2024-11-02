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

        public float alpha;
        public float beta;
        public int resolution = 10;
        
        private bool triangulationVisible = false;
        private Pen trianglePen = Pens.Blue;

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

        // Evaluate point on the surface and return Vertex
        private Vertex Evaluate(float u, float v)
        {
            Vector3 position = Vector3.Zero;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float Bu = Bernstein(i, u);
                    float Bv = Bernstein(j, v);

                    position += Bu * Bv * surface[i, j];
                }
            }

            return new Vertex
            {
                P = position,
                Pu = Vector3.Zero,
                Pv = Vector3.Zero,
                N = Vector3.Zero
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
    }
}
