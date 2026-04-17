using ImageMagick;
using System.Drawing;
using System.IO.Compression;
using static System.Net.Mime.MediaTypeNames;

namespace TextureAttacher.Library.Image.ImageMagic;

public static class ImageProcessor
{

    public static void PasteImageOnRectangles(MemoryStream pasteMs,ref MemoryStream dstMs,Rectangle[] dst_rects)
    {
        using (var basePic = new MagickImage(dstMs,MagickFormat.Bmp))
        {
            foreach (var rect in dst_rects)
            {
                using (var paste = new MagickImage(pasteMs, MagickFormat.Bmp))
                {
                    paste.Resize((uint)rect.Size.Width, (uint)rect.Size.Height);
                    basePic.Composite(paste, rect.X, rect.Y, CompositeOperator.Over);
                }
            }
            dstMs.Close();
            dstMs = new MemoryStream();
            basePic.Write(dstMs, MagickFormat.Bmp);
        }
    }
    public static void ClipAlpha0(ref MemoryStream ms)
    {
        using (var basePic = new MagickImage(ms, MagickFormat.Bmp))
        {
            basePic.Trim();
            basePic.Write(ms, MagickFormat.Bmp);
        }
    }

}
