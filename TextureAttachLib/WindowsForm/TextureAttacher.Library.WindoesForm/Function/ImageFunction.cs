using System;
using System.Drawing.Imaging;

namespace TextureAttacher.Library.WindoesForm.Function;

public static class ImageFunction
{

    public static Image NormalizedPixcelFormat(Image image,PixelFormat format = PixelFormat.Format32bppArgb)
    {
        Bitmap bmp = new Bitmap(image.Width, image.Height,format);
        using(var g = Graphics.FromImage(bmp))
        {
            g.DrawImage(image, 0, 0, image.Width, image.Height);
        }
        return bmp;
    }

}
