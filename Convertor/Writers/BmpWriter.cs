using System.Drawing;
using Converter.Interfaces;
using Image = Converter.ImageBase.Image;

namespace Converter.Writers
{
    public class BmpWriter : IImageWriter
    {
        public void Write(string path, Image image)
        {
            Bitmap pic = new Bitmap(image.Header.Width, image.Header.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            for (int i = 0; i < image.Header.Height; i++)
            {
                for (int j = 0; j < image.Header.Width; j++)
                {
                    Color c = Color.FromArgb(image.Bitmap[i, j].R, image.Bitmap[i, j].G, image.Bitmap[i, j].B);
                    pic.SetPixel(j, i, c);
                }
            }
            
            pic.Save($"{path}.bmp");
        }
    }
}