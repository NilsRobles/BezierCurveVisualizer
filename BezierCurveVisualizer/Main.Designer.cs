namespace BezierCurveVisualizer
{
    partial class Main
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
            this.settingsMenu = new System.Windows.Forms.GroupBox();
            this.algorithmSetting = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.followSpeedSetting = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.followCountSetting = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.curvePictureBox = new System.Windows.Forms.PictureBox();
            this.settingsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.followSpeedSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.followCountSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.curvePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsMenu
            // 
            this.settingsMenu.Controls.Add(this.algorithmSetting);
            this.settingsMenu.Controls.Add(this.label3);
            this.settingsMenu.Controls.Add(this.followSpeedSetting);
            this.settingsMenu.Controls.Add(this.label2);
            this.settingsMenu.Controls.Add(this.followCountSetting);
            this.settingsMenu.Controls.Add(this.label1);
            this.settingsMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.settingsMenu.Location = new System.Drawing.Point(640, 0);
            this.settingsMenu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.settingsMenu.Name = "settingsMenu";
            this.settingsMenu.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.settingsMenu.Size = new System.Drawing.Size(160, 450);
            this.settingsMenu.TabIndex = 0;
            this.settingsMenu.TabStop = false;
            this.settingsMenu.Text = "Settings";
            // 
            // algorithmSetting
            // 
            this.algorithmSetting.CausesValidation = false;
            this.algorithmSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.algorithmSetting.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.algorithmSetting.FormattingEnabled = true;
            this.algorithmSetting.Items.AddRange(new object[] {
            "DeCasteljau",
            "Bernstein"});
            this.algorithmSetting.Location = new System.Drawing.Point(3, 112);
            this.algorithmSetting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.algorithmSetting.Name = "algorithmSetting";
            this.algorithmSetting.Size = new System.Drawing.Size(153, 23);
            this.algorithmSetting.TabIndex = 5;
            this.algorithmSetting.TextChanged += new System.EventHandler(this.algorithmSetting_TextUpdate);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Algorithm";
            // 
            // followSpeedSetting
            // 
            this.followSpeedSetting.DecimalPlaces = 2;
            this.followSpeedSetting.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.followSpeedSetting.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.followSpeedSetting.Location = new System.Drawing.Point(3, 75);
            this.followSpeedSetting.Margin = new System.Windows.Forms.Padding(3, 38, 3, 2);
            this.followSpeedSetting.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.followSpeedSetting.Name = "followSpeedSetting";
            this.followSpeedSetting.Size = new System.Drawing.Size(152, 23);
            this.followSpeedSetting.TabIndex = 1;
            this.followSpeedSetting.ThousandsSeparator = true;
            this.followSpeedSetting.Value = new decimal(new int[] {
            6,
            0,
            0,
            65536});
            this.followSpeedSetting.ValueChanged += new System.EventHandler(this.SpeedSetting_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Point Speed";
            // 
            // followCountSetting
            // 
            this.followCountSetting.Location = new System.Drawing.Point(3, 38);
            this.followCountSetting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.followCountSetting.Name = "followCountSetting";
            this.followCountSetting.Size = new System.Drawing.Size(152, 23);
            this.followCountSetting.TabIndex = 2;
            this.followCountSetting.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.followCountSetting.ValueChanged += new System.EventHandler(this.FollowCountSetting_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Point Count";
            // 
            // curvePictureBox
            // 
            this.curvePictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.curvePictureBox.Location = new System.Drawing.Point(0, 0);
            this.curvePictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.curvePictureBox.Name = "curvePictureBox";
            this.curvePictureBox.Size = new System.Drawing.Size(637, 450);
            this.curvePictureBox.TabIndex = 3;
            this.curvePictureBox.TabStop = false;
            this.curvePictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.CurvePictureBox_Paint);
            this.curvePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CurveBox_MouseClick);
            this.curvePictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CurveBox_MouseMove);
            this.curvePictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CurveBox_MouseUp);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.curvePictureBox);
            this.Controls.Add(this.settingsMenu);
            this.Name = "Main";
            this.Text = "Bezier Curve Visualizer";
            this.Load += new System.EventHandler(this.OnLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.settingsMenu.ResumeLayout(false);
            this.settingsMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.followSpeedSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.followCountSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.curvePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ListView listView1;
        private ColumnHeader columnHeader1;
        private GroupBox settingsMenu;
        private NumericUpDown followSpeedSetting;
        private PictureBox curvePictureBox;
        private NumericUpDown followCountSetting;
        private Label label1;
        private Label label2;
        private ComboBox algorithmSetting;
        private Label label3;
    }
}