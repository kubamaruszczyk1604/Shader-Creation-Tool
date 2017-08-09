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
            this.FrameBufferWindow = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.In_Slot_Depth = new System.Windows.Forms.CheckBox();
            this.In_Slot_Colour = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FunctionNodeWindow = new System.Windows.Forms.Panel();
            this.closeButtonX = new System.Windows.Forms.Button();
            this.MiddlePanel = new System.Windows.Forms.Panel();
            this.InnerPanel = new System.Windows.Forms.Panel();
            this.Out_SlotX = new System.Windows.Forms.CheckBox();
            this.In_SlotX = new System.Windows.Forms.CheckBox();
            this.TitleLabel = new System.Windows.Forms.Label();
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
            this.FrameBufferWindow.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.FunctionNodeWindow.SuspendLayout();
            this.MiddlePanel.SuspendLayout();
            this.InnerPanel.SuspendLayout();
            this.PreviewAreaPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(158, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 32);
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
            this.button44.Location = new System.Drawing.Point(152, 41);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(137, 28);
            this.button44.TabIndex = 14;
            this.button44.Text = "Select Mesh";
            this.button44.UseVisualStyleBackColor = false;
            this.button44.Click += new System.EventHandler(this.button44_Click);
            // 
            // EditAreaPanel
            // 
            this.EditAreaPanel.AutoScroll = true;
            this.EditAreaPanel.BackColor = System.Drawing.Color.Transparent;
            this.EditAreaPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("EditAreaPanel.BackgroundImage")));
            this.EditAreaPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.EditAreaPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.EditAreaPanel.Controls.Add(this.FrameBufferWindow);
            this.EditAreaPanel.Controls.Add(this.FunctionNodeWindow);
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
            this.EditAreaPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EditAreaPanel_MouseClick);
            this.EditAreaPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EditAreaPanel_MouseMove);
            // 
            // FrameBufferWindow
            // 
            this.FrameBufferWindow.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.FrameBufferWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FrameBufferWindow.Controls.Add(this.panel4);
            this.FrameBufferWindow.Controls.Add(this.label1);
            this.FrameBufferWindow.Location = new System.Drawing.Point(814, 443);
            this.FrameBufferWindow.Name = "FrameBufferWindow";
            this.FrameBufferWindow.Size = new System.Drawing.Size(230, 103);
            this.FrameBufferWindow.TabIndex = 31;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Location = new System.Drawing.Point(-1, 22);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(230, 80);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panel5.Controls.Add(this.In_Slot_Depth);
            this.panel5.Controls.Add(this.In_Slot_Colour);
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(224, 74);
            this.panel5.TabIndex = 0;
            // 
            // In_Slot_Depth
            // 
            this.In_Slot_Depth.AutoSize = true;
            this.In_Slot_Depth.BackColor = System.Drawing.Color.Transparent;
            this.In_Slot_Depth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.In_Slot_Depth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.In_Slot_Depth.ForeColor = System.Drawing.Color.BurlyWood;
            this.In_Slot_Depth.Location = new System.Drawing.Point(0, 39);
            this.In_Slot_Depth.Name = "In_Slot_Depth";
            this.In_Slot_Depth.Size = new System.Drawing.Size(137, 20);
            this.In_Slot_Depth.TabIndex = 5;
            this.In_Slot_Depth.Text = "Fragment Depth";
            this.In_Slot_Depth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.In_Slot_Depth.UseVisualStyleBackColor = false;
            // 
            // In_Slot_Colour
            // 
            this.In_Slot_Colour.AutoSize = true;
            this.In_Slot_Colour.BackColor = System.Drawing.Color.Transparent;
            this.In_Slot_Colour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.In_Slot_Colour.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.In_Slot_Colour.ForeColor = System.Drawing.Color.BurlyWood;
            this.In_Slot_Colour.Location = new System.Drawing.Point(0, 13);
            this.In_Slot_Colour.Name = "In_Slot_Colour";
            this.In_Slot_Colour.Size = new System.Drawing.Size(141, 20);
            this.In_Slot_Colour.TabIndex = 4;
            this.In_Slot_Colour.Text = "Fragment Colour";
            this.In_Slot_Colour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.In_Slot_Colour.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "FRAME BUFFER";
            // 
            // FunctionNodeWindow
            // 
            this.FunctionNodeWindow.BackColor = System.Drawing.Color.Maroon;
            this.FunctionNodeWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FunctionNodeWindow.Controls.Add(this.closeButtonX);
            this.FunctionNodeWindow.Controls.Add(this.MiddlePanel);
            this.FunctionNodeWindow.Controls.Add(this.TitleLabel);
            this.FunctionNodeWindow.Enabled = false;
            this.FunctionNodeWindow.Location = new System.Drawing.Point(3, 620);
            this.FunctionNodeWindow.Name = "FunctionNodeWindow";
            this.FunctionNodeWindow.Size = new System.Drawing.Size(230, 76);
            this.FunctionNodeWindow.TabIndex = 30;
            this.FunctionNodeWindow.Visible = false;
            // 
            // closeButtonX
            // 
            this.closeButtonX.BackColor = System.Drawing.Color.Black;
            this.closeButtonX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButtonX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButtonX.ForeColor = System.Drawing.Color.Transparent;
            this.closeButtonX.Location = new System.Drawing.Point(207, -1);
            this.closeButtonX.Name = "closeButtonX";
            this.closeButtonX.Size = new System.Drawing.Size(22, 22);
            this.closeButtonX.TabIndex = 2;
            this.closeButtonX.Text = "X";
            this.closeButtonX.UseVisualStyleBackColor = false;
            // 
            // MiddlePanel
            // 
            this.MiddlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MiddlePanel.Controls.Add(this.InnerPanel);
            this.MiddlePanel.Location = new System.Drawing.Point(-1, 22);
            this.MiddlePanel.Name = "MiddlePanel";
            this.MiddlePanel.Size = new System.Drawing.Size(230, 55);
            this.MiddlePanel.TabIndex = 1;
            // 
            // InnerPanel
            // 
            this.InnerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.InnerPanel.Controls.Add(this.Out_SlotX);
            this.InnerPanel.Controls.Add(this.In_SlotX);
            this.InnerPanel.Location = new System.Drawing.Point(3, 3);
            this.InnerPanel.Name = "InnerPanel";
            this.InnerPanel.Size = new System.Drawing.Size(224, 48);
            this.InnerPanel.TabIndex = 0;
            // 
            // Out_SlotX
            // 
            this.Out_SlotX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Out_SlotX.AutoSize = true;
            this.Out_SlotX.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Out_SlotX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Out_SlotX.ForeColor = System.Drawing.Color.BurlyWood;
            this.Out_SlotX.Location = new System.Drawing.Point(154, 16);
            this.Out_SlotX.Name = "Out_SlotX";
            this.Out_SlotX.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Out_SlotX.Size = new System.Drawing.Size(73, 20);
            this.Out_SlotX.TabIndex = 8;
            this.Out_SlotX.Text = "OutCol";
            this.Out_SlotX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Out_SlotX.UseVisualStyleBackColor = true;
            // 
            // In_SlotX
            // 
            this.In_SlotX.AutoSize = true;
            this.In_SlotX.BackColor = System.Drawing.Color.Transparent;
            this.In_SlotX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.In_SlotX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.In_SlotX.ForeColor = System.Drawing.Color.BurlyWood;
            this.In_SlotX.Location = new System.Drawing.Point(-1, 16);
            this.In_SlotX.Name = "In_SlotX";
            this.In_SlotX.Size = new System.Drawing.Size(83, 20);
            this.In_SlotX.TabIndex = 4;
            this.In_SlotX.Text = "Ambient";
            this.In_SlotX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.In_SlotX.UseVisualStyleBackColor = false;
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(80, 4);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(68, 18);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Material";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PreviewAreaPanel
            // 
            this.PreviewAreaPanel.BackColor = System.Drawing.Color.DimGray;
            this.PreviewAreaPanel.Controls.Add(this.pictureBox1);
            this.PreviewAreaPanel.Controls.Add(this.PreviewTextLabel);
            this.PreviewAreaPanel.Location = new System.Drawing.Point(811, 3);
            this.PreviewAreaPanel.Name = "PreviewAreaPanel";
            this.PreviewAreaPanel.Size = new System.Drawing.Size(466, 423);
            this.PreviewAreaPanel.TabIndex = 26;
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
            this.button29.Click += new System.EventHandler(this.button29_Click);
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
            this.FrameBufferWindow.ResumeLayout(false);
            this.FrameBufferWindow.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.FunctionNodeWindow.ResumeLayout(false);
            this.FunctionNodeWindow.PerformLayout();
            this.MiddlePanel.ResumeLayout(false);
            this.InnerPanel.ResumeLayout(false);
            this.InnerPanel.PerformLayout();
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
        private System.Windows.Forms.Label PreviewTextLabel;
        private System.Windows.Forms.Panel FunctionNodeWindow;
        private System.Windows.Forms.Panel MiddlePanel;
        private System.Windows.Forms.CheckBox Out_SlotX;
        private System.Windows.Forms.CheckBox In_SlotX;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Button closeButtonX;
        private System.Windows.Forms.Panel InnerPanel;
        private System.Windows.Forms.Panel FrameBufferWindow;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox In_Slot_Depth;
        private System.Windows.Forms.CheckBox In_Slot_Colour;
        private System.Windows.Forms.Label label1;
    }
}

