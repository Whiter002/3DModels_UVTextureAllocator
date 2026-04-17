using System.Drawing;

namespace TextureAttacher.Library.Core.Functions;

public static class ValueConverter
{

    public static Rectangle AttachUVRectangleToSize(RectangleF uv, Size attachTo)
    {
        return new Rectangle(
            (int)(uv.X * attachTo.Width),
            (int)(uv.Y * attachTo.Height),
            (int)(uv.Width * attachTo.Width),
            (int)(uv.Height * attachTo.Height));
    }

}
