using Markdig;
using Markdig.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TextureAllocator;

public partial class UpdateNotification : Form
{
    public UpdateNotification()
    {
        InitializeComponent();
    }
    public DialogResult ShowDialog(string releaseNotesMd)
    {
        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer) { EnableHtmlForBlock = false, EnableHtmlForInline = false };
        var pipeline = new MarkdownPipelineBuilder().Build();
        Markdig.Markdown.Convert(releaseNotesMd, renderer, pipeline);

        string plainText = writer.ToString();
        plainText = plainText.Replace("\n", Environment.NewLine);
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
}
