using MeshFiller.Classes;
using System.Drawing.Imaging;
using System.Globalization;
using System.Numerics;

namespace MeshFiller
{
    public partial class MainWindow : Form
    {
        public DirectBitmap bitmap;

        public Vector3[,] surface;
        public Vector3[,] rotSurface = new Vector3[4, 4];
        public Vertex[,] vertices;
        public List<Triangle> mesh = [];

        private readonly Renderer renderer = new();

        public float alpha;
        public float beta;
        public int resolution;

        private bool triangulationVisible = false;
        private readonly Pen trianglePen = Pens.Blue;

        public const int vertexRadius = 10;

        private System.Windows.Forms.Timer animationTimer;
        private bool alphaIncreasing = true;
        private bool betaIncreasing = true;
        private const float ALPHA_SPEED = 0.5f;
        private const float BETA_SPEED = 0.1f;

        private const float scale = 1;
        private const float scaleZ = 1;

        public MainWindow()
        {
            InitializeComponent();
            bitmap = new DirectBitmap(canvas.Width, canvas.Height, canvas.Width / 2, canvas.Height / 2);

            AngleSlider_Scroll(null, null);
            ResolutionSlider_Scroll(null, null);
            LightingSlider_Scroll(null, null);

            SetupTimer();
        }

        private void SetupTimer()
        {
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 1;
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // animate renderer.LightDirection ¯ród³o œwiat³a - animacja ruchu po okregu na pewnej p³aszczyŸnie z=const (z - suwak)
            float angle = 0.03f;
            renderer.LightDirection = Vector3.Transform(renderer.LightDirection, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, angle));
            canvas.Invalidate();
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

            if (triangulationVisible)
            {
                Graphics g = e.Graphics;
                g.ScaleTransform(1, -1);
                g.TranslateTransform(canvas.Width / 2, -canvas.Height / 2);

                DrawTriangulation(g);
            }
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

                bitmap.Clear(Color.White);
                Parallel.ForEach(mesh, t =>
                {
                    renderer.FillPolygon(bitmap, [t.V1, t.V2, t.V3], t);
                });

                e.Graphics.DrawImage(bitmap.Bitmap, 0, 0);

                //Fill random 4 points 
                //renderer.FillPolygon(g, [mesh[2].V1, mesh[40].V2, mesh[23].V3, mesh[88].V3, mesh[100].V3]);
            }
        }

        public void DrawTriangulation(Graphics g)
        {
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

                    float x = float.Parse(tokens[0], CultureInfo.InvariantCulture) * scale;
                    float y = float.Parse(tokens[1], CultureInfo.InvariantCulture) * scale;
                    float z = float.Parse(tokens[2], CultureInfo.InvariantCulture) * scaleZ;

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

            foreach (Triangle t in mesh)
            {
                t.Recalculate();
            }

            canvas.Invalidate();
        }

        private void LoadSurfaceButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadBezierSurface(openFileDialog.FileName);
            }
        }

        // ----- UI ----------------------------------------------------------------

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            bitmap?.Dispose();
            bitmap = new DirectBitmap(canvas.Width, canvas.Height, canvas.Width / 2, canvas.Height / 2);
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
            renderer.kd = (float)kdSlider.Value / 100.0f;
            renderer.ks = (float)ksSlider.Value / 100.0f;
            renderer.m = (float)mSlider.Value;
            UpdateLighting();
        }

        public void UpdateLighting()
        {
            kdLabel.Text = Math.Round(renderer.kd, 2).ToString();
            ksLabel.Text = Math.Round(renderer.ks, 2).ToString();
            mLabel.Text = Math.Round(renderer.m, 2).ToString();
            canvas.Invalidate();
        }

        private void TriangulationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            triangulationVisible = triangulationCheckbox.Checked;
            canvas.Invalidate();
        }

        private (Vector3, Color) OpenColorDialog()
        {
            using ColorDialog lightColorDialog = new();
            lightColorDialog.ShowDialog();
            Color color = lightColorDialog.Color;
            Vector3 rgb = new(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
            return (rgb, color);
        }

        private void LightColorSelect_Click(object sender, EventArgs e)
        {
            (Vector3 rgb, Color color) = OpenColorDialog();
            renderer.LightColor = rgb;
            lightColorSelect.BackColor = color;
            canvas.Invalidate();
        }

        private void ObjectColorSelect_Click(object sender, EventArgs e)
        {
            (Vector3 rgb, Color color) = OpenColorDialog();
            renderer.ObjectColor = rgb;
            objectColorSelect.BackColor = color;
            renderer.UseTexture = false;
            solidColorRadio.Checked = true;
            canvas.Invalidate();
        }

        private static Bitmap? LoadImage()
        {
            using OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return null;

            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(openFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (bitmap.Width < 2 || bitmap.Height < 2)
            {
                MessageBox.Show("Image must be at least 2x2 pixels.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return bitmap;
        }

        private bool LoadTexture()
        {
            Bitmap? bitmap = LoadImage();
            if (bitmap == null)
                return false;

            renderer.UseTexture = true;
            renderer.Texture = LoadBitmapToColorArray(bitmap);
            textureSelect.BackgroundImage = bitmap;
            textureRadio.Checked = true;

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

        public static Color[,] LoadBitmapToColorArray(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            Color[,] colorArray = new Color[width, height];

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // Calculate the number of bytes in the bitmap.
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            int byteCount = bmpData.Stride * height;
            byte[] pixelData = new byte[byteCount];

            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelData, 0, byteCount);
            bitmap.UnlockBits(bmpData);

            if (bmpData.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette palette = bitmap.Palette;

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        int pixelIndex = y * bmpData.Stride + x;
                        byte paletteIndex = pixelData[pixelIndex];
                        colorArray[x, y] = palette.Entries[paletteIndex];
                    }
                });
            }
            else
            {
                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        int pixelIndex = y * bmpData.Stride + x * bytesPerPixel;

                        byte b = pixelData[pixelIndex];
                        byte g = pixelData[pixelIndex + 1];
                        byte r = pixelData[pixelIndex + 2];
                        byte a = bytesPerPixel == 4 ? pixelData[pixelIndex + 3] : (byte)255;

                        colorArray[x, y] = Color.FromArgb(a, r, g, b);
                    }
                });
            }

            return colorArray;
        }


        private bool LoadNormalMap()
        {
            Bitmap? bitmap = LoadImage();
            if (bitmap == null)
                return false;

            renderer.UseNormalMap = true;
            renderer.NormalMap = LoadBitmapToColorArray(bitmap);
            normalMapSelect.BackgroundImage = bitmap;
            normalMapCheckbox.Checked = true;

            canvas.Invalidate();
            return true;
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

        private void lightAnimationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ToggleAnimation();
        }
    }
}
