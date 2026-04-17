using System.Text;
using System.Text.Json;
using TextureAttacher.Library.Core.Enums;
using TextureAttacher.Library.Core.Model;
using TextureAttacher.Library.Image.OpenCV;
using TextureAttacher.Library.Image.OpenCV.Enums;
using TextureProjectCreator.Enums;

namespace TextureProjectCreator;

public partial class Form1 : Form
{
    OperatorMode mode = OperatorMode.Free;
    EyeRectangleData saveData = new EyeRectangleData()
    {
        Name = "HowlHeart"
    };

    private Point _startPoint;
    private Point _imageOffset { get { return _imageRectangleInControl.Location; } }
    private RectangleF _selectedImageNormalizedRectangle = RectangleF.Empty;
    private Rectangle _imageRectangleInControl = Rectangle.Empty;
    private Rectangle SelectedRectangleInControl
    {
        get
        {
            if (_selectedImageNormalizedRectangle == RectangleF.Empty) return Rectangle.Empty;
            return ImagePositionToControlPosition(_selectedImageNormalizedRectangle);
        }
    }

    private RectangleF[] _NormalizedCountours = [];


    string displayedDebugText = string.Empty;

    private Image currentImage = null;

    public Form1()
    {
        InitializeComponent();
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {

    }

    private void Form1_Load(object sender, EventArgs e)
    {
        toolTip1.SetToolTip(pictureBox1, "");
        this.SelectEyeTextures.Tag = TargetTextureKind.Eye;
        this.SelectEyeBackGround_Textures.Tag = TargetTextureKind.EyeBackGround;
        this.SelectPupilsButton.Tag = TargetTextureKind.EyePupils;

        this.comboBox1.SelectedIndex = 0;
    }

    private void splitContainer1_Panel1_DragEnter(object sender, DragEventArgs e)
    {
        var data = e.Data;
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) goto SetNone;
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files.Length != 1) goto SetNone;
        var file = files[0];
        if (!File.Exists(file) || Path.GetExtension(file) != ".png") goto SetNone;
        e.Effect = DragDropEffects.Copy;
        return;
    SetNone:
        e.Effect = DragDropEffects.None;
        return;
    }

    private void splitContainer1_Panel1_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Effect != DragDropEffects.Copy) return;
        string file = (e.Data.GetData(DataFormats.FileDrop) as string[])[0];
        Image image = Image.FromFile(file);
        currentImage = image;
        this.pictureBox1.Image = image;
        UpdateImageOffsets();
    }

    private bool IsInImage(Point point)
    {
        return _imageRectangleInControl.Contains(point);
    }
    private Size SizeAttachToOtherSize(PictureBox control)
    {
        if (control.Image is null) return Size.Empty;
        float rate = ImageConvertRate(control);
        Size size = control.Image.Size;
        return new Size((int)(size.Width * rate), (int)(size.Width * rate));
    }
    private Rectangle GetCurrentImageRectanle(PictureBox pictureBox)
    {
        var size = pictureBox.Image?.Size;
        if (size == null) return Rectangle.Empty;

        var attachedSize = SizeAttachToOtherSize(pictureBox);
        Rectangle imageRectangle = pictureBox.SizeMode switch
        {
            PictureBoxSizeMode.Zoom => new Rectangle((pictureBox.ClientSize.Width - attachedSize.Width) / 2, (pictureBox.ClientSize.Height - attachedSize.Height) / 2, attachedSize.Width, attachedSize.Height),
            _ => throw new NotImplementedException("SizeMode is implemented only ZOOM")
        };
        return imageRectangle;
    }

    private void SelectTextureSettingButtonClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        if (button?.Tag is not TargetTextureKind loadMode) return;
        if (mode != OperatorMode.Free) return;
        if (_selectedImageNormalizedRectangle == Rectangle.Empty) return;
        _NormalizedCountours = GenSaveInfo();
        var drawTarget = _NormalizedCountours.Select(c => ImagePositionToControlPosition(c)).ToArray();
        var labelText = BuildRectangleText(drawTarget);
        switch (loadMode)
        {
            case TargetTextureKind.Eye:
                this.EyeTexturesLabel.Text = labelText;
                saveData.Eye = _NormalizedCountours;
                break;
            case TargetTextureKind.EyeBackGround:
                this.BackGround_Label.Text = labelText;
                saveData.EyeBackGround = _NormalizedCountours;
                break;
            case TargetTextureKind.EyePupils:
                this.Pupil_Label.Text = labelText;
                saveData.EyePupils = _NormalizedCountours;
                break;
        }

        mode = OperatorMode.Free;
        this.pictureBox1.Invalidate();
    }
    private void pictureBox1_Paint(object sender, PaintEventArgs e)
    {
        Pen pen = new Pen(Color.Black, 2);
        SolidBrush brush = new SolidBrush(Color.FromArgb(128, Color.Gray));

        switch (mode)
        {
            case OperatorMode.Free:
                if (SelectedRectangleInControl != Rectangle.Empty)
                {
                    pen = new Pen(Color.Gray, 2);
                    brush = new SolidBrush(Color.FromArgb(128, Color.Gray));
                    e.Graphics.DrawRectangle(pen, SelectedRectangleInControl);
                    e.Graphics.FillRectangle(brush, SelectedRectangleInControl);
                }
                if (_NormalizedCountours.Length != 0)
                {
                    var countours = _NormalizedCountours.Select(c => ImagePositionToControlPosition(c)).ToArray();
                    pen.Color = Color.Blue;
                    brush.Color = Color.FromArgb(128, Color.Blue);
                    foreach (var contour in countours)
                    {
                        e.Graphics.DrawRectangle(pen, contour);
                        e.Graphics.FillRectangle(brush, contour);
                        var center = new Point(contour.Left + (contour.Width >> 1), contour.Top + (contour.Height >> 1));
                        var radius = 1;
                        // 中心点を赤い円で描画
                        using var centerBrush = new SolidBrush(Color.Red);
                        e.Graphics.FillEllipse(centerBrush, center.X - radius, center.Y - radius, radius * 2, radius * 2);

                    }


                }
                break;
            case OperatorMode.Selecting:

                pen = new Pen(Color.CadetBlue, 2);

                brush = new SolidBrush(Color.FromArgb(128, Color.CadetBlue));
                e.Graphics.DrawRectangle(pen, SelectedRectangleInControl);
                e.Graphics.FillRectangle(brush, SelectedRectangleInControl);
                break;
        }


    }

    private float ImageConvertRate(PictureBox pictureBox)
    {
        Size imageSize = pictureBox.Image.Size;
        Size controlSize = pictureBox.ClientSize;

        bool is_width = Math.Min(controlSize.Width, controlSize.Height) == controlSize.Width;
        float rate = 0.0f;
        if (is_width) rate = (float)controlSize.Width / imageSize.Width;
        else rate = (float)controlSize.Height / imageSize.Height;
        return rate;

    }

    private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        if (sender is not PictureBox target) return;
        if (!IsInImage(e.Location)) return;
        mode = mode switch
        {
            OperatorMode.Free => OperatorMode.Selecting,
            OperatorMode.Selecting => OperatorMode.Free,
            _ => throw new NotImplementedException()
        };

        switch (mode)
        {
            case OperatorMode.Free:
                _selectedImageNormalizedRectangle = RectangleF.Empty;
                _startPoint = Point.Empty;
                break;
            case OperatorMode.Selecting:
                _startPoint = e.Location;
                break;
        }

        target.Invalidate();
    }
    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {

        if (sender is not PictureBox target) return;

        //UpdateToolTip(target, e.Location);

        if ((mode & OperatorMode.Selecting) != OperatorMode.Selecting) return;
        var currentLocation = IsInImage(e.Location) ? e.Location : new Point(
            (e.Location.X < _imageRectangleInControl.Left) ? _imageRectangleInControl.Left : (e.Location.X > _imageRectangleInControl.Right) ? _imageRectangleInControl.Right : e.Location.X,
            (e.Location.Y < _imageRectangleInControl.Top) ? _imageRectangleInControl.Top : (e.Location.Y > _imageRectangleInControl.Bottom) ? _imageRectangleInControl.Bottom : e.Location.Y
           );
        Point selectedScreenPosition = new Point(
            Math.Min(_startPoint.X, currentLocation.X),
            Math.Min(_startPoint.Y, currentLocation.Y)
            );
        Size selectedScreenSize = new Size(Math.Abs(_startPoint.X - currentLocation.X),
            Math.Abs(_startPoint.Y - currentLocation.Y));

        _selectedImageNormalizedRectangle = new RectangleF(
            (float)(selectedScreenPosition.X - _imageRectangleInControl.Left) / _imageRectangleInControl.Width,
            (float)(selectedScreenPosition.Y - _imageRectangleInControl.Top) / _imageRectangleInControl.Height,
            (float)selectedScreenSize.Width / _imageRectangleInControl.Width,
            (float)selectedScreenSize.Height / _imageRectangleInControl.Height
            );

        target.Invalidate();
        return;
    }
    private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
    {
        if (sender is not PictureBox target) return;
        if (mode != OperatorMode.Selecting) return;
        mode = OperatorMode.Free;
        target.Invalidate();
    }
    private Rectangle ImagePositionToControlPosition(RectangleF target)
    {
        var info = ImageRateToContorlLocate([target.X, target.Y]).ToList();
        info.AddRange([(int)(target.Width * _imageRectangleInControl.Width), (int)(target.Height * _imageRectangleInControl.Height)]);
        return new(info[0], info[1], info[2], info[3]);
    }
    private int[] ImageRateToContorlLocate(float[] rates)
    {
        if (rates.Length != 2) throw new ArgumentException("rates must be length of 2.", nameof(rates));
        var currentImageRectangle = _imageRectangleInControl;
        var imagepos = new Point((int)(currentImageRectangle.Left + rates[0] * currentImageRectangle.Width), (int)(currentImageRectangle.Top + rates[1] * currentImageRectangle.Height));
        return [imagepos.X, imagepos.Y];
    }
    private string BuildRectangleText(Rectangle[] rectangle)
    {
        StringBuilder sb = new StringBuilder();
        int count = 0;
        foreach (var rect in rectangle)
        {
            sb.Append($"[{count}]:{{");
            sb.Append($"Center:{{X:{(rect.Left + rect.Right) / 2},Y:{(rect.Top + rect.Bottom) / 2}}}");
            sb.Append($"Rect:{{{rect}}}");
            sb.AppendLine("}");
            count++;
        }

        return sb.ToString();
    }
    private RectangleF[] GenSaveInfo()
    {

        if (pictureBox1.Image is null) return [];
        RectangleF[] rects;
        using (var ms = new MemoryStream())
        {
            pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Position = 0;
            Size imageSize = pictureBox1.Image.Size;
            var imageSelectRectanlge = new Rectangle()
            {
                X = (int)(imageSize.Width * _selectedImageNormalizedRectangle.X),
                Y = (int)(imageSize.Height * _selectedImageNormalizedRectangle.Y),
                Width = (int)(imageSize.Width * _selectedImageNormalizedRectangle.Width),
                Height = (int)(imageSize.Height * _selectedImageNormalizedRectangle.Height)
            };
            ImageProcessor.FindUVFormatContoursWithCulling(ms, imageSelectRectanlge, out var contours, trackBar1.Value, ContourFilterOptions.Without_TouchImageEdge);
            rects = [.. contours.Select(c =>
            {
                var minX = c.Min(p => p.X);
                var maxX = c.Max(p => p.X);
                var minY = c.Min(p => p.Y);
                var maxY = c.Max(p => p.Y);
                return new RectangleF(
                    (float)minX,
                    (float)minY,
                    (float)(maxX - minX),
                    (float)(maxY - minY)
                    );
            })];
        }
        return rects;
    }

    private void trackBar1_ValueChanged(object sender, EventArgs e)
    {
        if (currentImage is null) return;
        timer1.Enabled = true;
        timer1.Stop();
        timer1.Start();

        var ms = new MemoryStream();
        currentImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        ms.Position = 0;
        ImageProcessor.GetThresholdedImage(ref ms, trackBar1.Value);
        if (this.pictureBox1.Image != currentImage) this.pictureBox1.Image?.Dispose();
        this.pictureBox1.Image = Image.FromStream(ms);
        ms.Close();
        ms.Dispose();
    }

    private void pictureBox1_SizeChanged(object sender, EventArgs e)
    {
        if (sender is not PictureBox target) return;
        UpdateImageOffsets();
    }
    private void UpdateImageOffsets()
    {
        _imageRectangleInControl = GetCurrentImageRectanle(this.pictureBox1);
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        this.pictureBox1.Image?.Dispose();
        this.pictureBox1.Image = currentImage;
        this.timer1.Stop();
        this.timer1.Enabled = false;
    }

    private void saveAsProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var targetKind = (EyeTextureType)this.comboBox1.SelectedIndex;
        saveData.Kind = targetKind;
        switch (targetKind)
        {
            case EyeTextureType.INALL:
                if(saveData.Eye is null || saveData.Eye.Length == 0)
                {
                    MessageBox.Show("Please set eye textures.");
                    return;
                }
                break;
            case EyeTextureType.BACKGROUND_AND_PUPIL:
                if ((saveData.EyeBackGround?.Length ?? 0) == 0 && (saveData.EyePupils?.Length ?? 0) == 0)
                {
                    MessageBox.Show("Please set background and pupil textures.");
                    return;
                }
                break;
        }
        var dr = saveFileDialog1.ShowDialog();
        if (dr != DialogResult.OK) return;
        var path = saveFileDialog1.FileName;
        var option = new JsonSerializerOptions() {
            WriteIndented = true,
            IndentCharacter = ' '
        };
        using (var sw = new StreamWriter(path))
        {
            sw.WriteLine(JsonSerializer.Serialize(saveData, option));
        }
    }
}
