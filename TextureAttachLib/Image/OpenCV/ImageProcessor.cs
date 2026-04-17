using OpenCvSharp;
using System.Drawing;
using System.Xml.Serialization;
using TextureAttacher.Library.Image.OpenCV.Enums;
using OPoint = OpenCvSharp.Point;
using OPointF = OpenCvSharp.Point2f;
using ORectangle = OpenCvSharp.Rect;
using SPoint = System.Drawing.Point;
using SPointF = System.Drawing.PointF;
using SRectangle = System.Drawing.Rectangle;

namespace TextureAttacher.Library.Image.OpenCV;

public static class ImageProcessor
{

    public static void Culling(ref MemoryStream ms,Rectangle dst_rect)
    {
        Mat src = Cv2.ImDecode(ms.ToArray(),ImreadModes.Grayscale);
        Mat dst = Crop(src,new(dst_rect.X,dst_rect.Y,dst_rect.Width,dst_rect.Height));
        ms.Close();
        Cv2.ImEncode(".bmp", dst, out var buffer);
        ms = new MemoryStream(buffer);
    }

    /// <summary>
    /// 輪郭を検出して、輪郭を切り取る
    /// </summary>
    public static void FindContours(MemoryStream ms,out SPoint[][] contours,float threshold = 126)
    {
        Mat src = Mat.FromStream(ms, ImreadModes.Grayscale);
        contours = FindContours(src,threshold).Select(s => s.Select(p => new SPoint(p.X, p.Y)).ToArray()).ToArray();
    }
    public static void FindUVFormatContours(MemoryStream ms, out SPointF[][] contours, float threshold)
    { 
        Mat src = Mat.FromStream(ms, ImreadModes.Color);
        var srcSize = src.Size();
        contours = FindContours(src, threshold).Select(s => s.Select(p => new SPointF((float)p.X/srcSize.Width,(float)p.Y/srcSize.Height)).ToArray()).ToArray();
    }
    /// <summary>
    /// This method detects contours in the cropped area defined by dst_rect and returns the contours with their coordinates adjusted to the original image. It also updates the MemoryStream to contain the cropped image. 
    /// The threshold parameter is used for binary thresholding before contour detection, and it must be between 0 and 255.
    /// </summary>
    /// <param name="ms"></param>
    /// <param name="dst_rect"></param>
    /// <param name="counters"></param>
    /// <param name="threshold"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void FindContoursWithCulling(MemoryStream ms, Rectangle dst_rect, out SPoint[][] counters, float threshold=126)
    {
        Mat src = Mat.FromStream(ms, ImreadModes.Grayscale);
        Rect dst_r = new Rect(dst_rect.X, dst_rect.Y, dst_rect.Width, dst_rect.Height);
        var intersect = src.BoundingRect().Intersect(dst_r);
        Mat dst = new Mat(src, intersect);
        Mat dummy = new();
        Cv2.CvtColor(dst, dummy, ColorConversionCodes.BGRA2GRAY);
        // 2. 二値化
        var bin = new Mat();
        Cv2.Threshold(dummy, bin, threshold, 255, ThresholdTypes.Binary);
        var open_counters = bin.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);
        counters = open_counters.Select(s => s.Select(p => new SPoint( p.X + dst_rect.X, p.Y + dst_rect.Y)).ToArray()).ToArray();
        ms.Close();
    }
    public static void FindUVFormatContoursWithCulling(MemoryStream ms, Rectangle dst_rect, out SPointF[][] counters, float threshold = 126,ContourFilterOptions options = ContourFilterOptions.None)
    {
        Mat src = Mat.FromStream(ms, ImreadModes.Grayscale);
        Mat dst = Crop(src,new(dst_rect.X,dst_rect.Y,dst_rect.Width,dst_rect.Height));
        var open_counters = FindContours(dst, threshold);

        var edgeMergin = 0;
        switch(options)
        {
            case ContourFilterOptions.Without_TouchImageEdge:
                open_counters = [..open_counters.Where(c =>
                    c.Min(p=>p.X)>edgeMergin && c.Max(p => p.X) < dst.Size().Width - edgeMergin &&
                    c.Min(p=>p.Y)>edgeMergin && c.Max(p => p.Y) < dst.Size().Height - edgeMergin
                    )];
                break;
            default: break;
        }

        var srcSize = src.Size();
        counters = open_counters.Select(
            s => s.Select(p => new SPointF((float)(p.X + dst_rect.X) / srcSize.Width, (float)(p.Y + dst_rect.Y) / srcSize.Height)
            ).ToArray()).ToArray();
        ms.Close();
    }
    public static void GetThresholdedImage(ref MemoryStream ms,int threshold)
    {
        Mat src = Mat.FromStream(ms, ImreadModes.Grayscale);
        Cv2.Threshold(src, src, threshold, 255, ThresholdTypes.Binary);
        Cv2.ImEncode(".bmp", src, out var buffer);
        ms.Close();
        ms = new MemoryStream(buffer);
    }


    private static OPoint[][] FindContours(Mat src, float threshold, bool invert = false)
    {
        if (threshold < 0 || threshold > 255) throw new ArgumentOutOfRangeException(nameof(threshold), "threshold must be between 0 and 255.");

        // 2. 二値化
        var bin = new Mat();
        Cv2.Threshold(src, bin, threshold, 255, invert ? ThresholdTypes.BinaryInv : ThresholdTypes.Binary);
        return bin.FindContoursAsArray(RetrievalModes.List, ContourApproximationModes.ApproxSimple);
    }
    private static Mat Crop(Mat src, ORectangle dst_rect)
    {
        var intersect = src.BoundingRect().Intersect(dst_rect);
        return new Mat(src, intersect);
    }

    

}
