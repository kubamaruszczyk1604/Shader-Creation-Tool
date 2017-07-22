namespace ShaderCreationTool
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.button1 = new System.Windows.Forms.Button();
            this.button44 = new System.Windows.Forms.Button();
            this.EditAreaPanel = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button48 = new System.Windows.Forms.Button();
            this.PreviewAreaPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PreviewTextLabel = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button29 = new System.Windows.Forms.Button();
            this.button30 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gLSLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hLSLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditAreaPanel.SuspendLayout();
            this.PreviewAreaPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(28, 367);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "LoadRenderer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button44
            // 
            this.button44.BackColor = System.Drawing.Color.Maroon;
            this.button44.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button44.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button44.Location = new System.Drawing.Point(9, 298);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(137, 28);
            this.button44.TabIndex = 14;
            this.button44.Text = "Select Mesh";
            this.button44.UseVisualStyleBackColor = false;
            // 
            // EditAreaPanel
            // 
            this.EditAreaPanel.AutoScroll = true;
            this.EditAreaPanel.BackColor = System.Drawing.Color.Transparent;
            this.EditAreaPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("EditAreaPanel.BackgroundImage")));
            this.EditAreaPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.EditAreaPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.EditAreaPanel.Controls.Add(this.button2);
            this.EditAreaPanel.Controls.Add(this.button48);
            this.EditAreaPanel.Controls.Add(this.PreviewAreaPanel);
            this.EditAreaPanel.Controls.Add(this.button1);
            this.EditAreaPanel.Controls.Add(this.button44);
            this.EditAreaPanel.Controls.Add(this.groupBox6);
            this.EditAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditAreaPanel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.EditAreaPanel.Location = new System.Drawing.Point(0, 28);
            this.EditAreaPanel.Name = "EditAreaPanel";
            this.EditAreaPanel.Size = new System.Drawing.Size(1312, 703);
            this.EditAreaPanel.TabIndex = 19;
            this.EditAreaPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.EditAreaPanel_Scroll);
            this.EditAreaPanel.Click += new System.EventHandler(this.EditAreaPanel_Click);
            this.EditAreaPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.EditAreaPanel_Paint);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(56, 481);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(132, 31);
            this.button2.TabIndex = 27;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button48
            // 
            this.button48.BackColor = System.Drawing.Color.MidnightBlue;
            this.button48.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button48.ForeColor = System.Drawing.SystemColors.Window;
            this.button48.Location = new System.Drawing.Point(267, 183);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(258, 116);
            this.button48.TabIndex = 15;
            this.button48.Text = "Rendered Object";
            this.button48.UseVisualStyleBackColor = false;
            this.button48.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button48_MouseDown);
            this.button48.MouseMove += new System.Windows.Forms.MouseEventHandler(this.button48_MouseMove);
            // 
            // PreviewAreaPanel
            // 
            this.PreviewAreaPanel.BackColor = System.Drawing.Color.Gray;
            this.PreviewAreaPanel.Controls.Add(this.pictureBox1);
            this.PreviewAreaPanel.Controls.Add(this.PreviewTextLabel);
            this.PreviewAreaPanel.Location = new System.Drawing.Point(811, 3);
            this.PreviewAreaPanel.Name = "PreviewAreaPanel";
            this.PreviewAreaPanel.Size = new System.Drawing.Size(466, 423);
            this.PreviewAreaPanel.TabIndex = 26;
            this.PreviewAreaPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PreviewAreaPanel_MouseDown);
            this.PreviewAreaPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PreviewAreaPanel_MouseMove);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(3, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(460, 401);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // PreviewTextLabel
            // 
            this.PreviewTextLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
            this.PreviewTextLabel.AllowDrop = true;
            this.PreviewTextLabel.AutoSize = true;
            this.PreviewTextLabel.BackColor = System.Drawing.Color.Transparent;
            this.PreviewTextLabel.CausesValidation = false;
            this.PreviewTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviewTextLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.PreviewTextLabel.Location = new System.Drawing.Point(198, 0);
            this.PreviewTextLabel.Name = "PreviewTextLabel";
            this.PreviewTextLabel.Size = new System.Drawing.Size(77, 16);
            this.PreviewTextLabel.TabIndex = 25;
            this.PreviewTextLabel.Text = "PREVIEW";
            this.PreviewTextLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PreviewTextLabel_MouseDown);
            this.PreviewTextLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PreviewTextLabel_MouseMove);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Gray;
            this.groupBox6.Controls.Add(this.button29);
            this.groupBox6.Controls.Add(this.button30);
            this.groupBox6.Controls.Add(this.button31);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(126, 241);
            this.groupBox6.TabIndex = 23;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "ADD";
            // 
            // button29
            // 
            this.button29.BackColor = System.Drawing.Color.Brown;
            this.button29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button29.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button29.Location = new System.Drawing.Point(6, 19);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(105, 64);
            this.button29.TabIndex = 8;
            this.button29.Text = "Add Variable";
            this.button29.UseVisualStyleBackColor = false;
            // 
            // button30
            // 
            this.button30.BackColor = System.Drawing.Color.Brown;
            this.button30.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button30.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button30.Location = new System.Drawing.Point(6, 89);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(105, 65);
            this.button30.TabIndex = 9;
            this.button30.Text = "Add Resource";
            this.button30.UseVisualStyleBackColor = false;
            // 
            // button31
            // 
            this.button31.BackColor = System.Drawing.Color.Brown;
            this.button31.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button31.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button31.Location = new System.Drawing.Point(6, 162);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(105, 67);
            this.button31.TabIndex = 10;
            this.button31.Text = "Add Material Property";
            this.button31.UseVisualStyleBackColor = false;
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.BackColor = System.Drawing.Color.Gray;
            this.MainMenuStrip.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(1312, 28);
            this.MainMenuStrip.TabIndex = 20;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.AutoToolTip = true;
            this.fileToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.openToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.saveToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.saveAsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // exportToToolStripMenuItem
            // 
            this.exportToToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.exportToToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gLSLToolStripMenuItem,
            this.hLSLToolStripMenuItem});
            this.exportToToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exportToToolStripMenuItem.Name = "exportToToolStripMenuItem";
            this.exportToToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.exportToToolStripMenuItem.Text = "Export to..";
            // 
            // gLSLToolStripMenuItem
            // 
            this.gLSLToolStripMenuItem.Name = "gLSLToolStripMenuItem";
            this.gLSLToolStripMenuItem.Size = new System.Drawing.Size(111, 24);
            this.gLSLToolStripMenuItem.Text = "GLSL";
            // 
            // hLSLToolStripMenuItem
            // 
            this.hLSLToolStripMenuItem.Name = "hLSLToolStripMenuItem";
            this.hLSLToolStripMenuItem.Size = new System.Drawing.Size(111, 24);
            this.hLSLToolStripMenuItem.Text = "HLSL";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // MainWindow
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1312, 731);
            this.Controls.Add(this.EditAreaPanel);
            this.Controls.Add(this.MainMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Shader Creator Prototype, MSc Dissertation, University of Hull, 2017";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.EditAreaPanel.ResumeLayout(false);
            this.PreviewAreaPanel.ResumeLayout(false);
            this.PreviewAreaPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button44;
        private System.Windows.Forms.Panel EditAreaPanel;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button29;
        private System.Windows.Forms.Button button30;
        private System.Windows.Forms.Button button31;
        private System.Windows.Forms.Button button48;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel PreviewAreaPanel;
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gLSLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hLSLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label PreviewTextLabel;
    }
}

