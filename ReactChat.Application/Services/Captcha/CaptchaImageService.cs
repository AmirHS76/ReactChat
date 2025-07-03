using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ReactChat.Application.Services.Captcha
{
    public class CaptchaImageService
    {
        public byte[] GenerateCaptchaImage(string text)
        {
            int width = 180;
            int height = 60;
            var random = new Random();

            using var image = new Image<Rgba32>(width, height);

            image.Mutate(ctx =>
            {
                ctx.BackgroundColor(Color.White);

                var collection = new FontCollection();
                var fontFamily = collection.Add("Fonts/OpenSans-VariableFont_wdth,wght.ttf");
                var font = fontFamily.CreateFont(28, FontStyle.Bold);
                var eachChar = text.ToCharArray();
                for (int i = 0; i < eachChar.Length; i++)
                {
                    float angle = random.Next(-45, 45);
                    using var textImage = new Image<Rgba32>(40, 40);
                    textImage.Mutate(tctx =>
                    {
                        tctx.BackgroundColor(Color.Transparent);

                        tctx.DrawText(eachChar[i].ToString(), font, Color.Black, new PointF(10, 10));
                    });
                    textImage.Mutate(tctx => tctx.Rotate(angle));
                    ctx.DrawImage(textImage, new Point(30 * i, 0), 1f);
                }
                for (int i = 0; i < 5; i++)
                {
                    var penColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                    float x1 = random.Next(0, width);
                    float y1 = random.Next(0, height);
                    float x2 = random.Next(0, width);
                    float y2 = random.Next(0, height);

                    ctx.DrawLine(penColor, 1, new PointF[] { new(x1, y1), new(x2, y2) });
                }

                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(width);
                    int y = random.Next(height);
                    var dotColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                    ctx.Fill(dotColor, new RectangleF(x, y, 2, 2));
                }
            });

            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            return ms.ToArray();
        }
    }
}