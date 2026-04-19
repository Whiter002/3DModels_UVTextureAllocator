namespace TextureAllocator;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        pictureBox1 = new PictureBox();
        menuStrip1 = new MenuStrip();
        filesToolStripMenuItem = new ToolStripMenuItem();
        loadProfilesToolStripMenuItem = new ToolStripMenuItem();
        howlHeartToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        saveToolStripMenuItem = new ToolStripMenuItem();
        saveAsToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator2 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        helpToolStripMenuItem = new ToolStripMenuItem();
        howToUseToolStripMenuItem = new ToolStripMenuItem();
        versionsToolStripMenuItem = new ToolStripMenuItem();
        label1 = new Label();
        saveFileDialog1 = new SaveFileDialog();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // pictureBox1
        // 
        pictureBox1.AllowDrop = true;
        pictureBox1.Dock = DockStyle.Fill;
        pictureBox1.Location = new Point(0, 24);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(800, 426);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        pictureBox1.DragDrop += pictureBox1_DragDrop;
        pictureBox1.DragOver += pictureBox1_DragOver;
        pictureBox1.Paint += pictureBox1_Paint;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { filesToolStripMenuItem, helpToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(800, 24);
        menuStrip1.TabIndex = 1;
        menuStrip1.Text = "menuStrip1";
        // 
        // filesToolStripMenuItem
        // 
        filesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadProfilesToolStripMenuItem, toolStripSeparator1, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
        filesToolStripMenuItem.Name = "filesToolStripMenuItem";
        filesToolStripMenuItem.Size = new Size(42, 20);
        filesToolStripMenuItem.Text = "Files";
        // 
        // loadProfilesToolStripMenuItem
        // 
        loadProfilesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { howlHeartToolStripMenuItem });
        loadProfilesToolStripMenuItem.Name = "loadProfilesToolStripMenuItem";
        loadProfilesToolStripMenuItem.Size = new Size(180, 22);
        loadProfilesToolStripMenuItem.Text = "LoadProfiles";
        // 
        // howlHeartToolStripMenuItem
        // 
        howlHeartToolStripMenuItem.Name = "howlHeartToolStripMenuItem";
        howlHeartToolStripMenuItem.Size = new Size(131, 22);
        howlHeartToolStripMenuItem.Tag = "./profiles/HowlHeart.profile";
        howlHeartToolStripMenuItem.Text = "HowlHeart";
        howlHeartToolStripMenuItem.Click += howlHeartToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(177, 6);
        // 
        // saveToolStripMenuItem
        // 
        saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        saveToolStripMenuItem.Size = new Size(180, 22);
        saveToolStripMenuItem.Text = "Save";
        saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
        // 
        // saveAsToolStripMenuItem
        // 
        saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
        saveAsToolStripMenuItem.Size = new Size(180, 22);
        saveAsToolStripMenuItem.Text = "SaveAs";
        saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click_1;
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(177, 6);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(180, 22);
        exitToolStripMenuItem.Text = "Exit";
        // 
        // helpToolStripMenuItem
        // 
        helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { howToUseToolStripMenuItem, versionsToolStripMenuItem });
        helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        helpToolStripMenuItem.Size = new Size(44, 20);
        helpToolStripMenuItem.Text = "Help";
        // 
        // howToUseToolStripMenuItem
        // 
        howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
        howToUseToolStripMenuItem.Size = new Size(135, 22);
        howToUseToolStripMenuItem.Text = "How to Use";
        // 
        // versionsToolStripMenuItem
        // 
        versionsToolStripMenuItem.Name = "versionsToolStripMenuItem";
        versionsToolStripMenuItem.Size = new Size(135, 22);
        versionsToolStripMenuItem.Text = "Infos";
        // 
        // label1
        // 
        label1.AllowDrop = true;
        label1.BackColor = SystemColors.ControlDark;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(0, 24);
        label1.Name = "label1";
        label1.Size = new Size(800, 426);
        label1.TabIndex = 2;
        label1.TextAlign = ContentAlignment.MiddleCenter;
        label1.DragDrop += label1_DragDrop;
        label1.DragEnter += label1_DragEnter;
        // 
        // saveFileDialog1
        // 
        saveFileDialog1.Filter = "PNG画像|*.png";
        // 
        // Form1
        // 
        AllowDrop = true;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(label1);
        Controls.Add(pictureBox1);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "Form1";
        Text = "プロジェクトファイルを選択してください";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private PictureBox pictureBox1;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem filesToolStripMenuItem;
    private ToolStripMenuItem loadProfilesToolStripMenuItem;
    private ToolStripMenuItem howlHeartToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem saveToolStripMenuItem;
    private ToolStripMenuItem saveAsToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem exitToolStripMenuItem;
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem howToUseToolStripMenuItem;
    private ToolStripMenuItem versionsToolStripMenuItem;
    private Label label1;
    private SaveFileDialog saveFileDialog1;
}
