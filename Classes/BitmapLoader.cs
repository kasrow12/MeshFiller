using System.Drawing.Imaging;

namespace MeshFiller.Classes
{
    public static class BitmapLoader
    {
        public static Bitmap? Load()
        {
            using OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = Path.GetFullPath(MainWindow.ExamplesPath);
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

        public static Color[,] ToColorArray(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            Color[,] colorArray = new Color[width, height];

            BitmapData? bmpData = null;

            try
            {
                bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

                // Calculate the number of bytes in the bitmap.
                int byteCount = bmpData.Stride * height;
                byte[] pixelData = new byte[byteCount];

                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelData, 0, byteCount);


                int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;

                // handles 8bpp indexed images (normal_brick.png)
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
                    // Assume 24bpp or 32bpp
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
            }
            finally
            {
                if (bmpData != null)
                    bitmap.UnlockBits(bmpData);
            }

            return colorArray;
        }
    }
}
