namespace TextureAllocator;

partial class UpdateNotification
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tableLayoutPanel1 = new TableLayoutPanel();
        panel1 = new Panel();
        SkipButton = new Button();
        UpdateButton = new Button();
        panel2 = new Panel();
        textBox1 = new TextBox();
        tableLayoutPanel1.SuspendLayout();
        panel1.SuspendLayout();
        panel2.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(panel1, 0, 1);
        tableLayoutPanel1.Controls.Add(panel2, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tableLayoutPanel1.Size = new Size(475, 450);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // panel1
        // 
        panel1.Controls.Add(SkipButton);
        panel1.Controls.Add(UpdateButton);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(3, 411);
        panel1.Name = "panel1";
        panel1.Size = new Size(469, 36);
        panel1.TabIndex = 0;
        // 
        // SkipButton
        // 
        SkipButton.Dock = DockStyle.Left;
        SkipButton.Location = new Point(0, 0);
        SkipButton.Name = "SkipButton";
        SkipButton.Size = new Size(132, 36);
        SkipButton.TabIndex = 1;
        SkipButton.Text = "更新をスキップする";
        SkipButton.UseVisualStyleBackColor = true;
        SkipButton.Click += Button_Click;
        // 
        // UpdateButton
        // 
        UpdateButton.Dock = DockStyle.Right;
        UpdateButton.Location = new Point(337, 0);
        UpdateButton.Name = "UpdateButton";
        UpdateButton.Size = new Size(132, 36);
        UpdateButton.TabIndex = 0;
        UpdateButton.Text = "更新する";
        UpdateButton.UseVisualStyleBackColor = true;
        // 
        // panel2
        // 
        panel2.Controls.Add(textBox1);
        panel2.Dock = DockStyle.Fill;
        panel2.Location = new Point(3, 3);
        panel2.Name = "panel2";
        panel2.Size = new Size(469, 402);
        panel2.TabIndex = 1;
        // 
        // textBox1
        // 
        textBox1.Dock = DockStyle.Fill;
        textBox1.Location = new Point(0, 0);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.ReadOnly = true;
        textBox1.Size = new Size(469, 402);
        textBox1.TabIndex = 0;
        // 
        // UpdateNotification
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(475, 450);
        Controls.Add(tableLayoutPanel1);
        Name = "UpdateNotification";
        Text = "Form2";
        Load += UpdateNotification_Load;
        tableLayoutPanel1.ResumeLayout(false);
        panel1.ResumeLayout(false);
        panel2.ResumeLayout(false);
        panel2.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel1;
    private Button UpdateButton;
    private Button SkipButton;
    private Panel panel2;
    private TextBox textBox1;
}