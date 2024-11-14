using MeshFiller.Classes;
using System.Globalization;
using System.Numerics;

namespace MeshFiller
{
    public partial class MainWindow : Form
    {
        private DirectBitmap bitmap;
        private readonly BezierSurface bezier;
        private readonly Renderer renderer = new();

        private bool triangulationVisible = false;

        private readonly Pen trianglePen = Pens.Blue;
        private const int VERTEX_RADIUS = 10;

        private System.Windows.Forms.Timer animationTimer;
        private const float ANIMATION_ANGLE = 0.03f;

        public static string ExamplesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Examples");

        public MainWindow()
        {
            InitializeComponent();
            bitmap = new(canvas.Width, canvas.Height);
            bezier = new();

            // Set default values
            AngleSlider_Scroll(null, null);
            ResolutionSlider_Scroll(null, null);
            LightingSlider_Scroll(null, null);
            ScaleSlider_Scroll(null, null);
            zSlider_Scroll(null, null);
            zBufferCheckbox_CheckedChanged(null, null);

            SetupTimer();
        }

        // ----- Drawing ----------------------------------------------------------

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (bezier.surface == null || bezier.Mesh == null || bezier.Mesh.Count == 0)
                return;

            Graphics g = e.Graphics;
            bitmap.Clear(Color.White);
            renderer.UpdateZBuffer(bitmap.Width, bitmap.Height, bitmap.Width / 2, bitmap.Height / 2);

            Parallel.ForEach(bezier.Mesh, t =>
            {
                renderer.FillPolygon(bitmap, [t.V1, t.V2, t.V3], t);
            });

            g.DrawImage(bitmap.Bitmap, 0, 0);

            g.DrawEllipse(Pens.Black, renderer.LightPosition.X - 5 + renderer.ChangeX,
                renderer.ChangeY - renderer.LightPosition.Y + 5, 10, 10);
            g.FillEllipse(Brushes.White, renderer.LightPosition.X - 5 + renderer.ChangeX,
                renderer.ChangeY - renderer.LightPosition.Y + 5, 10, 10);

            if (triangulationVisible)
            {
                g.ScaleTransform(1, -1);
                g.TranslateTransform(canvas.Width / 2, -canvas.Height / 2);
                DrawTriangulation(g);
            }

            //Fill random 4 points 
            //renderer.FillPolygon(g, [mesh[2].V1, mesh[40].V2, mesh[23].V3, mesh[88].V3, mesh[100].V3]);
        }

        public void DrawTriangulation(Graphics g)
        {
            // Lines between control points
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Vector3 v = bezier.rotSurface[i, j];
                    g.FillEllipse(Brushes.Green, v.X - VERTEX_RADIUS / 2, v.Y - VERTEX_RADIUS / 2, VERTEX_RADIUS, VERTEX_RADIUS);

                    if (i > 0)
                    {
                        Vector3 u = bezier.rotSurface[i - 1, j];
                        g.DrawLine(Pens.Black, u.X, u.Y, v.X, v.Y);
                    }
                    if (j > 0)
                    {
                        Vector3 u = bezier.rotSurface[i, j - 1];
                        g.DrawLine(Pens.Black, u.X, u.Y, v.X, v.Y);
                    }
                }
            }

            // Triangles
            foreach (Triangle triangle in bezier.Mesh)
            {
                Vector3 v1 = triangle.V1.RotP;
                Vector3 v2 = triangle.V2.RotP;
                Vector3 v3 = triangle.V3.RotP;

                g.DrawLine(trianglePen, v1.X, v1.Y, v2.X, v2.Y);
                g.DrawLine(trianglePen, v2.X, v2.Y, v3.X, v3.Y);
                g.DrawLine(trianglePen, v3.X, v3.Y, v1.X, v1.Y);
            }
        }

        // ----- Surface ----------------------------------------------------------

        private void LoadBezierSurface(string fileName)
        {
            bezier.surface = new Vector3[4, 4];
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

                    bezier.surface[i / 4, i % 4] = new(x, y, z);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading surface: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            bezier.GenerateMesh();
        }

        // ----- UI ----------------------------------------------------------------

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            bitmap?.Dispose();
            bitmap = new DirectBitmap(canvas.Width, canvas.Height);
            canvas.Invalidate();
        }

        // ----- Surface ----------------------------------------------------------

        private void LoadSurfaceButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = Path.GetFullPath(ExamplesPath);
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadBezierSurface(openFileDialog.FileName);
                canvas.Invalidate();
            }
        }

        // ----- Rotation ----------------------------------------------------------

        private void AngleSlider_Scroll(object? sender, EventArgs? e)
        {
            bezier.Alpha = (float)alphaSlider.Value / 10;
            bezier.Beta = (float)betaSlider.Value / 10;
            UpdateRotation();
        }

        public void UpdateRotation()
        {
            alphaLabel.Text = Math.Round(bezier.Alpha, 2).ToString() + '°';
            betaLabel.Text = Math.Round(bezier.Beta, 2).ToString() + '°';

            bezier.RotateMesh();
            canvas.Invalidate();
        }

        // ----- Triangulation ----------------------------------------------------

        private void ResolutionSlider_Scroll(object? sender, EventArgs? e)
        {
            bezier.Resolution = resolutionSlider.Value;
            resolutionLabel.Text = bezier.Resolution.ToString();

            bezier.GenerateMesh();
            canvas.Invalidate();
        }

        private void TriangulationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            triangulationVisible = triangulationCheckbox.Checked;
            canvas.Invalidate();
        }

        // ----- Lighting ----------------------------------------------------------

        private void LightingSlider_Scroll(object? sender, EventArgs? e)
        {
            renderer.kd = (float)kdSlider.Value / 100.0f;
            renderer.ks = (float)ksSlider.Value / 100.0f;
            renderer.m = (float)mSlider.Value;
            UpdateLighting();
        }

        private void LightColorSelect_Click(object sender, EventArgs e)
        {
            (Vector3 rgb, Color color) = OpenColorDialog();
            renderer.LightColor = rgb;
            lightColorSelect.BackColor = color;
            canvas.Invalidate();
        }

        private void LightAnimationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ToggleAnimation();
        }

        public void UpdateLighting()
        {
            kdLabel.Text = Math.Round(renderer.kd, 2).ToString();
            ksLabel.Text = Math.Round(renderer.ks, 2).ToString();
            mLabel.Text = Math.Round(renderer.m, 2).ToString();
            canvas.Invalidate();
        }

        // ----- Texture -----------------------------------------------------------

        private void ObjectColorSelect_Click(object sender, EventArgs e)
        {
            (Vector3 rgb, Color color) = OpenColorDialog();
            renderer.ObjectColor = rgb;
            objectColorSelect.BackColor = color;
            renderer.UseTexture = false;
            solidColorRadio.Checked = true;
            canvas.Invalidate();
        }

        private bool LoadTexture()
        {
            Bitmap? bitmap = BitmapLoader.Load();
            if (bitmap == null)
                return false;

            renderer.UseTexture = true;
            renderer.Texture = BitmapLoader.ToColorArray(bitmap);

            textureSelect.BackgroundImage?.Dispose();
            textureSelect.BackgroundImage = bitmap;
            textureRadio.Checked = true;

            canvas.Invalidate();
            return true;
        }

        private bool LoadNormalMap()
        {
            Bitmap? bitmap = BitmapLoader.Load();
            if (bitmap == null)
                return false;

            renderer.UseNormalMap = true;
            renderer.NormalMap = BitmapLoader.ToColorArray(bitmap);

            normalMapSelect.BackgroundImage?.Dispose();
            normalMapSelect.BackgroundImage = bitmap;
            normalMapCheckbox.Checked = true;

            canvas.Invalidate();
            return true;
        }

        private void UseTextureRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (textureRadio.Checked && renderer.Texture == null)
            {
                if (!LoadTexture())
                {
                    solidColorRadio.Checked = true;
                    return;
                }
            }

            renderer.UseTexture = !solidColorRadio.Checked;
            canvas.Invalidate();
        }

        private void NormalMapCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (normalMapCheckbox.Checked && renderer.NormalMap == null)
            {
                if (!LoadNormalMap())
                {
                    normalMapCheckbox.Checked = false;
                    return;
                }
            }

            renderer.UseNormalMap = normalMapCheckbox.Checked;
            canvas.Invalidate();
        }

        private void TextureSelect_Click(object sender, EventArgs e)
        {
            LoadTexture();
        }

        private void NormalMapSelect_Click(object sender, EventArgs e)
        {
            LoadNormalMap();
        }

        // ----- Animation ---------------------------------------------------------

        private void SetupTimer()
        {
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 1;
            animationTimer.Tick += AnimationTimer_Tick;
        }

        // Rotate light source around the z-axis
        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            renderer.LightPosition = Vector3.Transform(
                renderer.LightPosition,
                Quaternion.CreateFromAxisAngle(Vector3.UnitZ, ANIMATION_ANGLE)
                );

            canvas.Invalidate();
        }

        private void ToggleAnimation()
        {
            if (animationTimer.Enabled)
                animationTimer.Stop();
            else
                animationTimer.Start();
        }

        // ----- Helper ------------------------------------------------------------

        private static (Vector3, Color) OpenColorDialog()
        {
            using ColorDialog lightColorDialog = new();
            lightColorDialog.ShowDialog();
            Color color = lightColorDialog.Color;
            Vector3 rgb = new(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
            return (rgb, color);
        }

        // ----- View -------------------------------------------------------------

        private void ScaleSlider_Scroll(object? sender, EventArgs? e)
        {
            bezier.Scale = scaleSlider.Value / 100.0f;
            scaleLabel.Text = bezier.Scale.ToString();
            bezier.RotateMesh();
            canvas.Invalidate();
        }

        private void zSlider_Scroll(object? sender, EventArgs? e)
        {
            renderer.LightPosition.Z = zSlider.Value;
            zLabel.Text = renderer.LightPosition.Z.ToString();
            canvas.Invalidate();
        }

        private void zBufferCheckbox_CheckedChanged(object? sender, EventArgs? e)
        {
            renderer.UseZBuffer = zBufferCheckbox.Checked;
            canvas.Invalidate();
        }
    }
}
