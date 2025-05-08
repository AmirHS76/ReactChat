using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace ReactChat.Application.Services.Captcha
{
    public class CaptchaImageService
    {
        [SupportedOSPlatform("windows")]
        public byte[] GenerateCaptchaImage(string text)
        {
            int width = 180;
            int height = 60;
            var random = new Random();

            using var baseBitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(baseBitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);

            using var font = new Font("Arial", 28, FontStyle.Bold);
            using var textBrush = new SolidBrush(Color.Black);

            // Draw the full text to a temp bitmap first
            var tempText = new Bitmap(width, height);
            using (var g = Graphics.FromImage(tempText))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                g.DrawString(text, font, textBrush, new PointF(10, 10));
            }

            // Create a new bitmap with warped text
            var warpedBitmap = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Wave distortion formula for X and Y
                    int dx = (int)(5 * Math.Sin(2 * Math.PI * y / random.Next(55, 60)));
                    int dy = (int)(5 * Math.Cos(2 * Math.PI * x / random.Next(50, 55)));

                    int srcX = x + dx;
                    int srcY = y + dy;

                    if (srcX >= 0 && srcX < width && srcY >= 0 && srcY < height)
                    {
                        warpedBitmap.SetPixel(x, y, tempText.GetPixel(srcX, srcY));
                    }
                }
            }

            // Draw background noise
            for (int i = 0; i < 10; i++)
            {
                var pen = new Pen(Color.FromArgb(random.Next(100, 200), random.Next(256), random.Next(256)), 1);
                graphics.DrawLine(pen, random.Next(width), random.Next(height), random.Next(width), random.Next(height));
            }

            // Draw noise dots
            for (int i = 0; i < 150; i++)
            {
                baseBitmap.SetPixel(random.Next(width), random.Next(height),
                    Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));
            }

            // Overlay the warped text onto the base bitmap
            graphics.DrawImage(warpedBitmap, 0, 0);

            using var ms = new MemoryStream();
            baseBitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
