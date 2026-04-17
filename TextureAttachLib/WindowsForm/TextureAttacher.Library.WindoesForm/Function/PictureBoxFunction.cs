using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace TextureAttacher.Library.WindoesForm.Function;

public static class PictureBoxFunction
{

    public static Rectangle GetPictureRectangleBasedOnControl(PictureBox pictureBoxControl,Size imageSize)
    {
        Point center = new Point(pictureBoxControl.Width / 2, pictureBoxControl.Height / 2);
        switch (pictureBoxControl.SizeMode)
        {
            case PictureBoxSizeMode.StretchImage:
                return new Rectangle(0,0,pictureBoxControl.Width,pictureBoxControl.Height);
            case PictureBoxSizeMode.CenterImage:
                return new Rectangle(center.X - imageSize.Width / 2, center.Y - imageSize.Height / 2, imageSize.Width, imageSize.Height);
            case PictureBoxSizeMode.Zoom:
                bool is_width = Math.Min(pictureBoxControl.Width, pictureBoxControl.Height) == pictureBoxControl.Width;
                float rate = 0.0f;
                if (is_width) rate = (float)pictureBoxControl.Width / imageSize.Width;
                else rate = (float)pictureBoxControl.Height / imageSize.Height;
                var stretchedSize = new SizeF((float)imageSize.Width*rate,(float)imageSize.Height*rate);
                return new Rectangle((int)(center.X-stretchedSize.Width/ 2), (int)(center.Y-stretchedSize.Height/ 2), (int)stretchedSize.Width, (int)stretchedSize.Height);
            case PictureBoxSizeMode.Normal:
            case PictureBoxSizeMode.AutoSize:
            default:
                return new Rectangle(0, 0, imageSize.Width, imageSize.Height);
        }
    }
    public static Rectangle ImageBaseRectangleToControlRectangle(Rectangle imageBaseRect, Rectangle imageRectOnControl)
    {
        return new Rectangle(
            imageBaseRect.X + imageRectOnControl.X,
            imageBaseRect.Y + imageRectOnControl.Y,
            imageBaseRect.Width,
            imageBaseRect.Height
            );
    }

}
