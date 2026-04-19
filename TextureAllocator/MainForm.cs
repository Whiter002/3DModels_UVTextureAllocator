using OpenCvSharp.Aruco;
using OpenCvSharp.ImgHash;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Transactions;
using TextureAllocator.Core;
using TextureAllocator.Enums;
using TextureAllocator.Events;
using TextureAllocator.Properties;
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
    // FIXME: フォーム破棄時に imageSources 内の各 Image および BackGroundImage が Dispose されない。Dispose(bool) をオーバーライドして破棄すること。
    Image[] imageSources;
    Image BackGroundImage;
    string output = "";

    private TargetTextureKind _allocateTargetTextureKind = TargetTextureKind.None;
    public MainForm()
    {
        InitializeComponent();

        var path = ExtractTempPathes(Settings.Default.TempPath);
        if (!Path.Exists(path)) Directory.CreateDirectory(path);
        Settings.Default.TempPath = path;

        path = ExtractTempPathes(Settings.Default.UpdateFilePath);
        if(!Path.Exists(path)) Directory.CreateDirectory(path);
        Settings.Default.UpdateFilePath = path;

        Settings.Default.Save();
    }
    public MainForm(bool checkUpdate) : this()
    {

        if (checkUpdate) CheckUpdate(true);

    }

    public string ExtractTempPathes(string path)
    {
        return Environment.ExpandEnvironmentVariables(path).Replace("$(AssemblyName)", Assembly.GetExecutingAssembly().GetName().Name);
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
    // FIXME: JsonSerializer.Deserialize が null や不正 JSON で例外をスローする可能性がある。try-catch を追加すること。
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

        // FIXME: e.Data や GetData が null の場合 NullReferenceException が発生する。null チェックを追加すること。
        string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        if (!File.Exists(path)) return;
        // FIXME: 前の BackGroundImage を Dispose していないためリソースリークする。
        using (var img = Image.FromFile(path)) BackGroundImage = ImageFunction.NormalizedPixcelFormat(img);
        this.pictureBox1.Image = BackGroundImage;
        this.CurrentPhase = MainOperatePhase.EyeTextureSelect;

    }

    private void label1_DragEnter(object sender, DragEventArgs e)
    {
        // FIXME: e.Data が null の場合 NullReferenceException が発生する。null チェックを追加すること。
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

        // FIXME: Pen / SolidBrush が Dispose されておらず GDI リソースリークの原因になる。using または finally で破棄すること。
        Pen[] pens = [.. palette.Entries.Select(c => new Pen(c, 3)).ToArray()];
        SolidBrush[] brushes = [.. palette.Entries.Select(c => new SolidBrush(Color.FromArgb(64, c)))];

        // FIXME: Profile is null で早期 return した場合、上で生成した pens / brushes が Dispose されない。生成位置を null チェックの後に移動すること。
        if (Profile is null) return;
        Rectangle imageRectangleOnBox = PictureBoxFunction.GetPictureRectangleBasedOnControl(pictureBox1, pictureBox1.Image?.Size ?? Size.Empty);
        var imageSize = new Size(imageRectangleOnBox.Width, imageRectangleOnBox.Height);
        // FIXME: default ケースがないため、想定外の Profile.Kind で MatchFailureException が発生する。
        Rectangle[][] targetRectangles = Profile.Kind switch
        {
            // FIXME: Profile.Eye / Profile.EyeBackGround / Profile.EyePupils が null の場合 NullReferenceException になる。null チェックを追加すること。
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
        // FIXME: e.Data が null の場合 NullReferenceException が発生する。
        if (e.Data.GetData(DataFormats.FileDrop) is not string[] pathes || pathes.Length != 1) goto DisableDragDropEffect;
        // FIXME: 矩形取得ロジックが複数箇所に重複している。GetRectangles() に統一すること。
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

        // FIXME: e.Data や GetData が null の場合 NullReferenceException が発生する。null チェックを追加すること。
        string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        if (String.IsNullOrEmpty(path) || !File.Exists(path)) return;
        // FIXME: index が imageSources の範囲外になると IndexOutOfRangeException が発生する可能性がある。
        int index = (int)_allocateTargetTextureKind - (int)Profile.Kind;
        // FIXME: ClipImage に渡す前の古い imageSources[index] を Dispose していないためリソースリークする。
        using (var img = Image.FromFile(path)) imageSources[index] = ImageFunction.NormalizedPixcelFormat(img);
        imageSources[index] = ClipImage(imageSources[index]);
        // FIXME: 矩形取得ロジックが複数箇所に重複している。GetRectangles() に統一すること。
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
        // FIXME: 前の pictureBox1.Image を Dispose していないため画像リソースがリークする。
        this.pictureBox1.Image = Image.FromStream(ms);

    }
    // FIXME: 引数の source（元画像）を Dispose していないためリソースリークする。
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
        // FIXME: Bitmap が Dispose されていないためリソースリークする。using を使うこと。
        Bitmap bitmap = new Bitmap(BackGroundImage.Width, BackGroundImage.Height, PixelFormat.Format32bppArgb);
        for (int i = 0; i < imageSources.Length; i++)
        {
            var img = imageSources[i];
            if (img is null) continue;
            // FIXME: GetRectangles() をループ内で毎回呼んでいるが結果は不変。ループ外に移動すること。
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

    private void checkUpdateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CheckUpdate(false);
    }
    // FIXME: UIスレッドで .Result を呼んでおりデッドロックの可能性がある。async/await に変更し、try-catch で例外をハンドリングすること。
    private void CheckUpdate(bool is_auto)
    {
        var result = is_auto? UpdateChecker.AutoCheckForUpdate().Result : UpdateChecker.CheckForUpdates().Result;
        if (result.IsAvailable && !String.IsNullOrEmpty(result.DownloadUrl))
        {
            // FIXME: UpdateNotification を Dispose していない。using を使うこと。
            UpdateNotification notifForm = new UpdateNotification();
            DialogResult dr = notifForm.ShowDialog(result.ReleaseNotesMarkDown);
            if(dr != DialogResult.OK) return;
            var save = Path.Combine(Settings.Default.UpdateFilePath,$"{result.TargetVersion}.zip");
            if (!Path.Exists(Path.GetDirectoryName(save)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(save));
            }
            DownLoadProgressForm.DownloadInfo downloadInfo = new DownLoadProgressForm.DownloadInfo()
            {
                Url = result.DownloadUrl,
                fileNameCaption = "release.zip",
                saveTo = save
            };
            if (!File.Exists(save))
            {
                foreach(string path in Directory.GetFiles(Path.GetDirectoryName(save)))
                {
                    File.Delete(path);
                }
                DownLoadProgressForm progressForm = new DownLoadProgressForm([downloadInfo]);
                DialogResult progressResult = progressForm.ShowDialog();
                if (progressResult != DialogResult.OK) return;
            }
            ZipFile.ExtractToDirectory(save, Settings.Default.UpdateFilePath);
            var pid = Process.GetCurrentProcess().Id;
            var exePath = Path.Combine(Settings.Default.TempPath, "release","net10.0-windows","Update.exe");
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -Command \"Wait-Process -Id {pid} -ErrorAction SilentlyContinue; Start-Process '{exePath}' -ArgumentList 'true'\"",
                UseShellExecute = true,
                CreateNoWindow = true
            });
            Application.Exit();
        }else if(!is_auto) MessageBox.Show("現在、利用可能なアップデートはありません。", "お知らせ", MessageBoxButtons.OK);
    }
}
