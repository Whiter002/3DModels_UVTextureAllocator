namespace TextureProjectCreator;

partial class Form1
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
        components = new System.ComponentModel.Container();
        pictureBox1 = new PictureBox();
        splitContainer1 = new SplitContainer();
        tableLayoutPanel1 = new TableLayoutPanel();
        panel4 = new Panel();
        comboBox1 = new ComboBox();
        trackBar1 = new TrackBar();
        groupBox2 = new GroupBox();
        panel2 = new Panel();
        Pupil_Label = new Label();
        SelectPupilsButton = new Button();
        groupBox3 = new GroupBox();
        panel3 = new Panel();
        BackGround_Label = new Label();
        SelectEyeBackGround_Textures = new Button();
        groupBox1 = new GroupBox();
        panel1 = new Panel();
        EyeTexturesLabel = new Label();
        SelectEyeTextures = new Button();
        class1BindingSource = new BindingSource(components);
        toolTip1 = new ToolTip(components);
        timer1 = new System.Windows.Forms.Timer(components);
        menuStrip1 = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        saveAsProfileToolStripMenuItem = new ToolStripMenuItem();
        saveFileDialog1 = new SaveFileDialog();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        panel4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
        groupBox2.SuspendLayout();
        panel2.SuspendLayout();
        groupBox3.SuspendLayout();
        panel3.SuspendLayout();
        groupBox1.SuspendLayout();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)class1BindingSource).BeginInit();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // pictureBox1
        // 
        pictureBox1.Dock = DockStyle.Fill;
        pictureBox1.Location = new Point(0, 0);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(500, 471);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        pictureBox1.SizeChanged += pictureBox1_SizeChanged;
        pictureBox1.Click += pictureBox1_Click;
        pictureBox1.Paint += pictureBox1_Paint;
        pictureBox1.MouseDown += pictureBox1_MouseDown;
        pictureBox1.MouseMove += pictureBox1_MouseMove;
        pictureBox1.MouseUp += pictureBox1_MouseUp;
        // 
        // splitContainer1
        // 
        splitContainer1.AllowDrop = true;
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 24);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.AllowDrop = true;
        splitContainer1.Panel1.Controls.Add(pictureBox1);
        splitContainer1.Panel1.DragDrop += splitContainer1_Panel1_DragDrop;
        splitContainer1.Panel1.DragEnter += splitContainer1_Panel1_DragEnter;
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(tableLayoutPanel1);
        splitContainer1.Size = new Size(800, 471);
        splitContainer1.SplitterDistance = 500;
        splitContainer1.TabIndex = 1;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(panel4, 0, 0);
        tableLayoutPanel1.Controls.Add(groupBox2, 0, 3);
        tableLayoutPanel1.Controls.Add(groupBox3, 0, 2);
        tableLayoutPanel1.Controls.Add(groupBox1, 0, 1);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 4;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.Size = new Size(296, 471);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // panel4
        // 
        panel4.Controls.Add(comboBox1);
        panel4.Controls.Add(trackBar1);
        panel4.Dock = DockStyle.Fill;
        panel4.Location = new Point(3, 3);
        panel4.Name = "panel4";
        panel4.Size = new Size(290, 86);
        panel4.TabIndex = 4;
        // 
        // comboBox1
        // 
        comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        comboBox1.FormattingEnabled = true;
        comboBox1.Items.AddRange(new object[] { "INALL", "BACKGROUND_AND_PUPIL" });
        comboBox1.Location = new Point(3, 54);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new Size(284, 23);
        comboBox1.TabIndex = 2;
        // 
        // trackBar1
        // 
        trackBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        trackBar1.Location = new Point(3, 3);
        trackBar1.Maximum = 255;
        trackBar1.Name = "trackBar1";
        trackBar1.Size = new Size(284, 45);
        trackBar1.TabIndex = 3;
        trackBar1.TickFrequency = 66;
        trackBar1.ValueChanged += trackBar1_ValueChanged;
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(panel2);
        groupBox2.Dock = DockStyle.Fill;
        groupBox2.Location = new Point(3, 347);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(290, 121);
        groupBox2.TabIndex = 2;
        groupBox2.TabStop = false;
        groupBox2.Text = "瞳テクスチャ";
        // 
        // panel2
        // 
        panel2.AutoScroll = true;
        panel2.Controls.Add(Pupil_Label);
        panel2.Controls.Add(SelectPupilsButton);
        panel2.Dock = DockStyle.Fill;
        panel2.Location = new Point(3, 19);
        panel2.Name = "panel2";
        panel2.Size = new Size(284, 99);
        panel2.TabIndex = 0;
        // 
        // Pupil_Label
        // 
        Pupil_Label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        Pupil_Label.Location = new Point(3, 38);
        Pupil_Label.Name = "Pupil_Label";
        Pupil_Label.Size = new Size(278, 58);
        Pupil_Label.TabIndex = 1;
        // 
        // SelectPupilsButton
        // 
        SelectPupilsButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        SelectPupilsButton.Location = new Point(3, 3);
        SelectPupilsButton.Name = "SelectPupilsButton";
        SelectPupilsButton.Size = new Size(278, 32);
        SelectPupilsButton.TabIndex = 0;
        SelectPupilsButton.Text = "瞳テクスチャとして設定";
        SelectPupilsButton.UseVisualStyleBackColor = true;
        SelectPupilsButton.Click += SelectTextureSettingButtonClicked;
        // 
        // groupBox3
        // 
        groupBox3.Controls.Add(panel3);
        groupBox3.Dock = DockStyle.Fill;
        groupBox3.Location = new Point(3, 221);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new Size(290, 120);
        groupBox3.TabIndex = 2;
        groupBox3.TabStop = false;
        groupBox3.Text = "背景テクスチャ";
        // 
        // panel3
        // 
        panel3.AutoScroll = true;
        panel3.Controls.Add(BackGround_Label);
        panel3.Controls.Add(SelectEyeBackGround_Textures);
        panel3.Dock = DockStyle.Fill;
        panel3.Location = new Point(3, 19);
        panel3.Name = "panel3";
        panel3.Size = new Size(284, 98);
        panel3.TabIndex = 0;
        // 
        // BackGround_Label
        // 
        BackGround_Label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        BackGround_Label.Location = new Point(3, 38);
        BackGround_Label.Name = "BackGround_Label";
        BackGround_Label.Size = new Size(275, 60);
        BackGround_Label.TabIndex = 1;
        // 
        // SelectEyeBackGround_Textures
        // 
        SelectEyeBackGround_Textures.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        SelectEyeBackGround_Textures.Location = new Point(3, 3);
        SelectEyeBackGround_Textures.Name = "SelectEyeBackGround_Textures";
        SelectEyeBackGround_Textures.Size = new Size(278, 32);
        SelectEyeBackGround_Textures.TabIndex = 0;
        SelectEyeBackGround_Textures.Text = "背景テクスチャとして設定";
        SelectEyeBackGround_Textures.UseVisualStyleBackColor = true;
        SelectEyeBackGround_Textures.Click += SelectTextureSettingButtonClicked;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(panel1);
        groupBox1.Dock = DockStyle.Fill;
        groupBox1.Location = new Point(3, 95);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(290, 120);
        groupBox1.TabIndex = 1;
        groupBox1.TabStop = false;
        groupBox1.Text = "単一テクスチャ";
        // 
        // panel1
        // 
        panel1.AutoScroll = true;
        panel1.Controls.Add(EyeTexturesLabel);
        panel1.Controls.Add(SelectEyeTextures);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(3, 19);
        panel1.Name = "panel1";
        panel1.Size = new Size(284, 98);
        panel1.TabIndex = 0;
        // 
        // EyeTexturesLabel
        // 
        EyeTexturesLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        EyeTexturesLabel.Location = new Point(3, 38);
        EyeTexturesLabel.Name = "EyeTexturesLabel";
        EyeTexturesLabel.Size = new Size(278, 60);
        EyeTexturesLabel.TabIndex = 1;
        // 
        // SelectEyeTextures
        // 
        SelectEyeTextures.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        SelectEyeTextures.Location = new Point(3, 3);
        SelectEyeTextures.Name = "SelectEyeTextures";
        SelectEyeTextures.Size = new Size(278, 32);
        SelectEyeTextures.TabIndex = 0;
        SelectEyeTextures.Tag = "";
        SelectEyeTextures.Text = "一つのテクスチャとして設定";
        SelectEyeTextures.UseVisualStyleBackColor = true;
        SelectEyeTextures.Click += SelectTextureSettingButtonClicked;
        // 
        // timer1
        // 
        timer1.Interval = 5000;
        timer1.Tick += timer1_Tick;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(800, 24);
        menuStrip1.TabIndex = 2;
        menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveAsProfileToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new Size(37, 20);
        fileToolStripMenuItem.Text = "File";
        // 
        // saveAsProfileToolStripMenuItem
        // 
        saveAsProfileToolStripMenuItem.Name = "saveAsProfileToolStripMenuItem";
        saveAsProfileToolStripMenuItem.Size = new Size(148, 22);
        saveAsProfileToolStripMenuItem.Text = "Save as Profile";
        saveAsProfileToolStripMenuItem.Click += saveAsProfileToolStripMenuItem_Click;
        // 
        // saveFileDialog1
        // 
        saveFileDialog1.Filter = "プロファイルファイル|*.profile";
        // 
        // Form1
        // 
        AllowDrop = true;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 495);
        Controls.Add(splitContainer1);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "Form1";
        Text = "Form1";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        panel4.ResumeLayout(false);
        panel4.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
        groupBox2.ResumeLayout(false);
        panel2.ResumeLayout(false);
        groupBox3.ResumeLayout(false);
        panel3.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        panel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)class1BindingSource).EndInit();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private PictureBox pictureBox1;
    private SplitContainer splitContainer1;
    private BindingSource class1BindingSource;
    private Button SelectEyeBackGround_Textures;
    private Button SelectEyeTextures;
    private ToolTip toolTip1;
    private GroupBox groupBox2;
    private Panel panel2;
    private Button SelectPupilsButton;
    private GroupBox groupBox3;
    private Panel panel3;
    private GroupBox groupBox1;
    private Panel panel1;
    private TableLayoutPanel tableLayoutPanel1;
    private Label Pupil_Label;
    private Label BackGround_Label;
    private Label EyeTexturesLabel;
    private TrackBar trackBar1;
    private System.Windows.Forms.Timer timer1;
    private ComboBox comboBox1;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem saveAsProfileToolStripMenuItem;
    private SaveFileDialog saveFileDialog1;
    private Panel panel4;
}
