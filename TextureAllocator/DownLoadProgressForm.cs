using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TextureAllocator;

public partial class DownLoadProgressForm : Form
{
    public struct DownloadInfo
    {
        public string Url { get; set; }
        public string fileNameCaption { get; set; }
        public string saveTo { get; set; }
    }

    Task? installTask;
    private readonly CancellationTokenSource _cts = new();
    DownloadInfo[] downloadContents = [];
    // FIXME: HttpClient はアプリケーション全体で再利用すべき。インスタンスフィールドとして毎回生成するとソケット枯渇の原因になる。static または DI 経由で共有すること。
    HttpClient httpClient = new HttpClient()
    {
        DefaultRequestHeaders =
        {
            UserAgent = { new System.Net.Http.Headers.ProductInfoHeaderValue("Mozilla", "5.0") }
        },
    };
    IProgress<long> updateProgress;
    IProgress<(long,string)> initializeProgress;
    IProgress<int> CompleteProgress; 
    public DownLoadProgressForm()
    {
        InitializeComponent();
        updateProgress = new Progress<long>(value =>
            {
                long total = this.downloadProgress.Tag is long t ? t : 0;
                if (total == 0) this.downloadProgress.Value = 1000;
                else this.downloadProgress.Value = (int)(1000*(float)value/total);
            });
        initializeProgress = new Progress<(long totalContents, string caption)>(item =>
            {
                this.ProgressLabel.Text = item.caption + "をダウンロード中....";
                this.downloadProgress.Tag = item.totalContents;
                this.downloadProgress.Value = 0;
            });
        CompleteProgress = new Progress<int>(totalContents => Close());
    }

    public DownLoadProgressForm(DownloadInfo[] downloadContents) : this()
    {
        this.downloadContents = downloadContents;
    }
    public DownLoadProgressForm(DownloadInfo[] downloadContents, string caption) : this(downloadContents)
    {
        this.Text = caption;
    }
    public new DialogResult ShowDialog()
    {
        this.StartAsync();
        return base.ShowDialog();

    }
    // FIXME: ConfigureAwait(false) により、ループ完了後に UI スレッド外で実行される。完了時に DialogResult を設定するなど UI 操作が必要な場合はデッドロックや InvalidOperationException の原因になる。
    public async void StartAsync()
    {
        try
        {
            foreach (DownloadInfo downloadInfo in downloadContents)
            {
                _cts.Token.ThrowIfCancellationRequested();
                installTask = DownloadFileinfo(downloadInfo, _cts.Token);
                await installTask.ConfigureAwait(false);
            }
            CompleteProgress.Report(0);

        }
        catch (OperationCanceledException)
        {
            // キャンセルされた場合は何もしない
        }
    }
    public async Task DownloadFileinfo(DownloadInfo downloadInfo, CancellationToken token)
    {
        string url = downloadInfo.Url;
        try
        {
            using var response = await httpClient.GetAsync(url,
                HttpCompletionOption.ResponseHeadersRead, token);
            response.EnsureSuccessStatusCode();

            long? contentLength = response.Content.Headers.ContentLength;
            using var contentStream = await response.Content.ReadAsStreamAsync(token);
            using var fileStream = new FileStream(downloadInfo.saveTo, FileMode.Create, FileAccess.Write, FileShare.None);

            var buffer = new byte[8192];
            long downloaded = 0;
            int bytesRead;

            initializeProgress.Report((contentLength ?? 0, downloadInfo.fileNameCaption));

            while ((bytesRead = await contentStream.ReadAsync(buffer, token)) > 0)
            {
                token.ThrowIfCancellationRequested();
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), token);
                downloaded += bytesRead;
                updateProgress.Report(downloaded);
            }
        }
        catch (OperationCanceledException)
        {
            throw; // 上位に伝播
        }
        catch (Exception ex)
        {
            foreach(var contents in downloadContents)
            {
                if(File.Exists(contents.saveTo)) File.Delete(contents.saveTo);
            }
            MessageBox.Show($"Error downloading file: {ex.Message}");
        }
    }
    public void Stop()
    {
        if (installTask?.IsCompleted ?? true) return;
        _cts.Cancel();
    }
    private void cancelButton_Click(object sender, EventArgs e)
    {
        Stop();
        this.DialogResult = DialogResult.Cancel;
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cts.Dispose();
            httpClient.Dispose();
        }
        components?.Dispose();
        base.Dispose(disposing);
    }

    private void DownLoadProgressForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (installTask?.IsCompleted ?? false) return;
        var dr = MessageBox.Show("ダウンロードが進行中です。キャンセルしますか？", "警告", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
        if (dr == DialogResult.No)
        {
            e.Cancel = true;
            return;
        }
        DialogResult = DialogResult.Cancel;
    }
}
