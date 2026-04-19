namespace TextureAllocator;

partial class DownLoadProgressForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;


    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tableLayoutPanel1 = new TableLayoutPanel();
        downloadProgress = new ProgressBar();
        ProgressLabel = new Label();
        panel1 = new Panel();
        cancelButton = new Button();
        tableLayoutPanel1.SuspendLayout();
        panel1.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(downloadProgress, 0, 1);
        tableLayoutPanel1.Controls.Add(ProgressLabel, 0, 0);
        tableLayoutPanel1.Controls.Add(panel1, 0, 2);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
        tableLayoutPanel1.Size = new Size(302, 81);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // downloadProgress
        // 
        downloadProgress.Dock = DockStyle.Fill;
        downloadProgress.Location = new Point(3, 26);
        downloadProgress.MarqueeAnimationSpeed = 1000;
        downloadProgress.Maximum = 1000;
        downloadProgress.Name = "downloadProgress";
        downloadProgress.Size = new Size(296, 21);
        downloadProgress.TabIndex = 1;
        // 
        // ProgressLabel
        // 
        ProgressLabel.Dock = DockStyle.Fill;
        ProgressLabel.Location = new Point(3, 0);
        ProgressLabel.Name = "ProgressLabel";
        ProgressLabel.Size = new Size(296, 23);
        ProgressLabel.TabIndex = 2;
        ProgressLabel.Text = "label1";
        ProgressLabel.TextAlign = ContentAlignment.BottomLeft;
        // 
        // panel1
        // 
        panel1.Controls.Add(cancelButton);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(3, 53);
        panel1.Name = "panel1";
        panel1.Size = new Size(296, 25);
        panel1.TabIndex = 3;
        // 
        // cancelButton
        // 
        cancelButton.Dock = DockStyle.Right;
        cancelButton.Location = new Point(221, 0);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(75, 25);
        cancelButton.TabIndex = 0;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        // 
        // DownLoadProgressForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(302, 81);
        Controls.Add(tableLayoutPanel1);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        MinimumSize = new Size(318, 120);
        Name = "DownLoadProgressForm";
        Text = "DownLoadProgressForm";
        FormClosing += DownLoadProgressForm_FormClosing;
        tableLayoutPanel1.ResumeLayout(false);
        panel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private ProgressBar downloadProgress;
    private Label ProgressLabel;
    private Panel panel1;
    private Button cancelButton;
}