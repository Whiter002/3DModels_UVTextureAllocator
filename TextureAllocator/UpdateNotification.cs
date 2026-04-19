using Markdig;
using Markdig.Renderers;
using System;
using System.Windows.Forms;

namespace TextureAllocator;

public partial class UpdateNotification : Form
{
    public UpdateNotification()
    {
        InitializeComponent();
        this.ToReleaseNotes.Links.Add(new LinkLabel.Link() { LinkData = "https://github.com/Whiter002/3DModels_UVTextureAllocator/releases" });
    }
    public DialogResult ShowDialog(string releaseNotesMd)
    {
        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer) { EnableHtmlForBlock = false, EnableHtmlForInline = false };
        var pipeline = new MarkdownPipelineBuilder().Build();
        Markdig.Markdown.Convert(releaseNotesMd, renderer, pipeline);

        string plainText = writer.ToString();
        plainText = plainText.Replace("\n", Environment.NewLine);
        // FIXME: plainText を自分自身に連結しているためテキストが指数的に増大する。末尾改行の追加だけが目的なら修正すること。
        for(int i = 0; i < 5; i++)
        {
            plainText += plainText.EndsWith(Environment.NewLine) ? plainText : plainText+Environment.NewLine;
        }
        this.textBox1.Text = plainText;
        return this.ShowDialog();
    }

    private void Button_Click(object sender, EventArgs e)
    {
        if (sender is not Button button) return;
        if (button.Tag is not DialogResult result) return;
        this.DialogResult = result;

    }

    private void UpdateNotification_Load(object sender, EventArgs e)
    {
        this.SkipButton.Tag = DialogResult.Cancel;
        this.UpdateButton.Tag = DialogResult.OK;
    }

    private void ToReleaseNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        if (e.Link?.LinkData is not string urlstr) return;
        if(!Uri.TryCreate(urlstr,UriKind.Absolute,out Uri? url))
        {
            MessageBox.Show("不正なリンクが設定されています。開発者に問い合わせてください。", "エラー", MessageBoxButtons.OK);
            return;
        }
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = urlstr,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show("リンクを開くことができませんでした。エラー: " + ex.Message, "エラー", MessageBoxButtons.OK);
        }
    }
}
