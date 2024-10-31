using MeshFiller.Classes;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace MeshFiller
{
    public partial class MainWindow : Form
    {
        public Vector3[,] surface;
        public List<Triangle> mesh = new();

        public float alpha = 0;
        public float beta = 0;
        public int resolution = 10;

        public const int vertexRadius = 10;

        public MainWindow()
        {
            InitializeComponent();

            AlphaSlider_Scroll(null, null);
            BetaSlider_Scroll(null, null);
            ResolutionSlider_Scroll(null, null);    
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
            if (surface == null)
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

                    float x = float.Parse(tokens[0]);
                    float y = float.Parse(tokens[1]);
                    float z = float.Parse(tokens[2]);

                    surface[i / 4, i % 4] = new(x, y, z);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading surface: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            mesh.Clear();
        }

        private void AlphaSlider_Scroll(object sender, EventArgs e)
        {
            alpha = (float)alphaSlider.Value / 10;
            alphaLabel.Text = Math.Round(alpha, 2).ToString() + '°';
            canvas.Invalidate();
        }

        private void BetaSlider_Scroll(object sender, EventArgs e)
        {
            beta = (float)betaSlider.Value / 10;
            betaLabel.Text = Math.Round(beta, 2).ToString() + '°';
            canvas.Invalidate();
        }
        private void ResolutionSlider_Scroll(object sender, EventArgs e)
        {
            resolution = resolutionSlider.Value;
            resolutionLabel.Text = resolution.ToString();
            canvas.Invalidate();
        }
    }
}
