
using OpenCvSharp.ImgHash;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Transactions;
using TextureAllocator.Core;
using TextureAllocator.Enums;
using TextureAllocator.Events;
using TextureAttacher.Library.Core.Enums;
using TextureAttacher.Library.Core.Functions;
using TextureAttacher.Library.Core.Model;
using TextureAttacher.Library.Image.ImageMagic;
using TextureAttacher.Library.WindoesForm.Function;

namespace TextureAllocator;

public partial class MainForm : Form
{
    event TransitionedMainStatusDelegate TransitionedMainStatusEvent;
    ColorPalette palette = new ColorPalette(Color.LightBlue, Color.LightPink, Color.LightGreen);
    Image[] imageSources;
    Image BackGroundImage;
    string output = "";

    private TargetTextureKind _allocateTargetTextureKind = TargetTextureKind.None;
    public MainForm()
    {
        InitializeComponent();
    }
    public MainForm(bool checkUpdate):this()
    {

        if (checkUpdate)
        {//Run Update Check

            var result = UpdateChecker.AutoCheckForUpdate().Result;
            if (result.IsAvailable && !String.IsNullOrEmpty(result.DownloadUrl))
            {
                UpdateNotification notifForm = new UpdateNotification();
                notifForm.ShowDialog(result.ReleaseNotesMarkDown);
            }
        }
    }
    EyeRectangleData Profile;
    MainOperatePhase? _currentPhase = null;
    MainOperatePhase CurrentPhase
    {
        set
        {
            _currentPhase = value;
            TransitionedMainStatusEvent?.Invoke(this, new TransitionedMainStatusEventArgs(value));
        }
    }


    internal void TransitionmainStatus(object sender, TransitionedMainStatusEventArgs e)
    {
        switch (e.Phase)
        {

            case MainOperatePhase.ProfileSelect:
                this.Text = "プロファイルを選択してください。(0.9.2 alpha)";
                PictureBoxSettingAllDisable();
                LabelSettingAllEnable();
                this.label1.Text = $"テクスチャの貼り付けプロファイルを選択してください。({GetMenuBreadcrumb(loadProfilesToolStripMenuItem)})";
                this.label1.AllowDrop = false;
                break;
            case MainOperatePhase.UVSelect:
                this.Text = "素体UVをドラッグアンドドロップしてください。(0.9.2 alpha)";
                PictureBoxSettingAllDisable();
                LabelSettingAllEnable();
                this.label1.Text = $"ここに素体のUVテクスチャをドラッグアンドドロップしてください";
                this.label1.AllowDrop = true;
                imageSources = Profile.Kind == EyeTextureType.INALL ? new Image[1] { null } : new Image[2] { null, null };
                break;
            case MainOperatePhase.EyeTextureSelect:
                this.Text = "0.9.2 alpha";
                LabelSettingAllDisable();
                PictureBoxSettingAllEnable();
                // Handle end phase
                break;
            case MainOperatePhase.Done:
                break;
        }
    }
    private void PictureBoxSettingAllDisable()
    {
        pictureBox1.Enabled = false;
        pictureBox1.Visible = false;
        pictureBox1.Image?.Dispose();
        pictureBox1.Image = null;
    }
    private void PictureBoxSettingAllEnable()
    {
        pictureBox1.Enabled = true;
        pictureBox1.Visible = true;
    }
    private void LabelSettingAllDisable()
    {
        this.label1.Enabled = false;
        this.label1.Visible = false;
        this.AllowDrop = false;
    }
    private void LabelSettingAllEnable()
    {
        this.label1.Enabled = true;
        this.label1.Visible = true;
        this.AllowDrop = true;
    }
    private string GetMenuBreadcrumb(ToolStripMenuItem item)
    {
        List<string> ownerStrings = new List<string>();
        if (item.OwnerItem is not ToolStripMenuItem owner) return item.Text;
        ownerStrings.Add(GetMenuBreadcrumb(owner));
        ownerStrings.Add(item.Text);
        return String.Join(">", ownerStrings);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        this.TransitionedMainStatusEvent += TransitionmainStatus;
        this.CurrentPhase = MainOperatePhase.ProfileSelect;
        this.pictureBox1.AllowDrop = true;
        this.Location = new Point(0, 0);
    }

    private void howlHeartToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (sender is not ToolStripMenuItem item) return;
        var loadedProfile = LoadFromProfile(item);
        if (!loadedProfile.isSuccess) return;
        if (loadedProfile.result is not EyeRectangleData data)
        {
            MessageBox.Show($"プロファイル[{item.Text}]のデータ読み込みに失敗しました。", "データ読み込みエラー", MessageBoxButtons.OK);
            return;
        }
        this.Profile = data;
        this.CurrentPhase = MainOperatePhase.UVSelect;

    }
    private (bool isSuccess, EyeRectangleData result) LoadFromProfile(ToolStripMenuItem profileItem)
    {
        if (profileItem.Tag is not string path || !File.Exists(path))
        {
            MessageBox.Show($"選択されたプロファイル[{profileItem.Text}]が設定されていないか存在していません。", "データ参照エラー", MessageBoxButtons.OK);
            return (false, null);
        }
        return (true, JsonSerializer.Deserialize<EyeRectangleData>(File.ReadAllText(path)));
    }

    private void label1_DragDrop(object sender, DragEventArgs e)
    {

        string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        if (!File.Exists(path)) return;
        using (var img = Image.FromFile(path)) BackGroundImage = ImageFunction.NormalizedPixcelFormat(img);
        this.pictureBox1.Image = BackGroundImage;
        this.CurrentPhase = MainOperatePhase.EyeTextureSelect;

    }

    private void label1_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is string[] pathes && pathes.Length == 1)
        {
            e.Effect = DragDropEffects.Copy;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    private void pictureBox1_Paint(object sender, PaintEventArgs e)
    {

        Pen[] pens = [.. palette.Entries.Select(c => new Pen(c, 3)).ToArray()];
        SolidBrush[] brushes = [.. palette.Entries.Select(c => new SolidBrush(Color.FromArgb(64, c)))];

        if (Profile is null) return;
        Rectangle imageRectangleOnBox = PictureBoxFunction.GetPictureRectangleBasedOnControl(pictureBox1, pictureBox1.Image?.Size ?? Size.Empty);
        var imageSize = new Size(imageRectangleOnBox.Width, imageRectangleOnBox.Height);
        Rectangle[][] targetRectangles = Profile.Kind switch
        {
            EyeTextureType.INALL => [[..Profile.Eye.Select(r =>
            PictureBoxFunction.ImageBaseRectangleToControlRectangle(
                ValueConverter.AttachUVRectangleToSize(r, imageSize), imageRectangleOnBox))
            ]],
            EyeTextureType.BACKGROUND_AND_PUPIL => [[..Profile.EyeBackGround.Select(r=>
            PictureBoxFunction.ImageBaseRectangleToControlRectangle(
                ValueConverter.AttachUVRectangleToSize(r, imageSize), imageRectangleOnBox)
            )],[..Profile.EyePupils.Select(r =>
                PictureBoxFunction.ImageBaseRectangleToControlRectangle(
                ValueConverter.AttachUVRectangleToSize(r, imageSize), imageRectangleOnBox)
            )]]
        };
        for (int i = 0; i < targetRectangles.Length; i++)
        {
            var eyeTextureKind = (TargetTextureKind)(i + Profile.Kind);
            int index = eyeTextureKind == _allocateTargetTextureKind ? 2 : i;
            if (index == 2 && imageSources[i] is not null)
            {
                var pen = pens[index];
                pen.Width = 1;
                e.Graphics.DrawRectangles(pen, targetRectangles[i]);
            }
            else
            {
                e.Graphics.DrawRectangles(pens[index], targetRectangles[i]);
                e.Graphics.FillRectangles(brushes[index], targetRectangles[i]);
            }
        }
    }

    private void pictureBox1_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is not string[] pathes || pathes.Length != 1) goto DisableDragDropEffect;
        List<RectangleF> targetRectangleFs = Profile.Kind switch
        {
            EyeTextureType.INALL => [.. Profile.Eye ?? []],
            EyeTextureType.BACKGROUND_AND_PUPIL => [.. (Profile.EyeBackGround ?? []).Concat(Profile.EyePupils ?? [])],
            _ => new()
        };
        Rectangle imageRectangleOnBox = PictureBoxFunction.GetPictureRectangleBasedOnControl(pictureBox1, pictureBox1.Image?.Size ?? Size.Empty);
        var imageSize = new Size(imageRectangleOnBox.Width, imageRectangleOnBox.Height);
        var targetRectangle = targetRectangleFs.Select(r =>
            PictureBoxFunction.ImageBaseRectangleToControlRectangle(
                ValueConverter.AttachUVRectangleToSize(r, imageSize), imageRectangleOnBox)
        ).ToList();
        Point cursorPosOnBox = pictureBox1.PointToClient(new(e.X, e.Y));
        int rectangleIndex = targetRectangle.FindIndex(r => r.Contains(cursorPosOnBox.X, cursorPosOnBox.Y));
        if (rectangleIndex == -1) goto DisableDragDropEffect;

        e.Effect = DragDropEffects.Copy;
        _allocateTargetTextureKind = Profile.Kind switch
        {
            EyeTextureType.INALL => TargetTextureKind.Eye,
            EyeTextureType.BACKGROUND_AND_PUPIL => rectangleIndex < (Profile.EyeBackGround?.Length ?? 0) ? TargetTextureKind.EyeBackGround : TargetTextureKind.EyePupils,
            _ => TargetTextureKind.None
        };
        this.pictureBox1.Invalidate();
        return;

    DisableDragDropEffect:
        e.Effect = DragDropEffects.None;
        _allocateTargetTextureKind = TargetTextureKind.None;
        this.pictureBox1.Invalidate();
    }
    private void pictureBox1_DragDrop(object sender, DragEventArgs e)
    {

        string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        if (String.IsNullOrEmpty(path) || !File.Exists(path)) return;
        int index = (int)_allocateTargetTextureKind - (int)Profile.Kind;
        using (var img = Image.FromFile(path)) imageSources[index] = ImageFunction.NormalizedPixcelFormat(img);
        imageSources[index] = ClipImage(imageSources[index]);
        RectangleF[][] targetRectangleFs = Profile.Kind switch
        {
            EyeTextureType.INALL => [Profile.Eye ?? []],
            EyeTextureType.BACKGROUND_AND_PUPIL => [Profile.EyeBackGround ?? [], Profile.EyePupils ?? []],
            _ => [[]]
        };
        var targetRectangles = targetRectangleFs.Select(Rectangles => Rectangles.Select(r => ValueConverter.AttachUVRectangleToSize(r, BackGroundImage.Size)).ToArray()).ToArray();
        var ms = new MemoryStream();
        BackGroundImage.Save(ms, ImageFormat.Bmp);
        int rec_index = 0;
        foreach (var _image in imageSources)
        {
            ms.Position = 0;
            var rectangles = targetRectangles[rec_index++];
            if (_image is not Image image) continue;
            using (var useMs = new MemoryStream())
            {
                image.Save(useMs, ImageFormat.Bmp);
                useMs.Position = 0;
                ImageProcessor.PasteImageOnRectangles(useMs, ref ms, rectangles);
            }
        }
        this.pictureBox1.Image = Image.FromStream(ms);

    }
    private Image ClipImage(Image source)
    {
        var ms = new MemoryStream();
        source.Save(ms, ImageFormat.Bmp);
        ms.Position = 0;
        ImageProcessor.ClipAlpha0(ref ms);
        source = Image.FromStream(ms);
        return source;
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {

        if (_currentPhase < MainOperatePhase.EyeTextureSelect) return;
        if (String.IsNullOrEmpty(output) || !File.Exists(output))
        {
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr != DialogResult.OK) return;
            output = saveFileDialog1.FileName;
        }
        Save(output);

    }
    private RectangleF[][] GetRectangles()
    {
        return Profile.Kind switch
        {
            EyeTextureType.INALL => [Profile.Eye ?? []],
            EyeTextureType.BACKGROUND_AND_PUPIL => [Profile.EyeBackGround ?? [], Profile.EyePupils ?? []],
            _ => [[]]
        };
    }
    private void Save(string Output)
    {
        Bitmap bitmap = new Bitmap(BackGroundImage.Width, BackGroundImage.Height, PixelFormat.Format32bppArgb);
        for (int i = 0; i < imageSources.Length; i++)
        {
            var img = imageSources[i];
            if (img is null) continue;
            var Rectangles = GetRectangles().Select(item => item.Select(r => ValueConverter.AttachUVRectangleToSize(r, bitmap.Size)).ToArray()).ToArray();
            using (var g = Graphics.FromImage(bitmap))
            {
                foreach (var rec in Rectangles[i])
                {
                    g.DrawImage(img, rec);
                }
            }
        }
        bitmap.Save(Output, ImageFormat.Png);
    }

    private void saveAsToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
        if (_currentPhase < MainOperatePhase.EyeTextureSelect) return;
        DialogResult dr = saveFileDialog1.ShowDialog();
        if (dr != DialogResult.OK) return;
        output = saveFileDialog1.FileName;

        Save(output);
    }
}
