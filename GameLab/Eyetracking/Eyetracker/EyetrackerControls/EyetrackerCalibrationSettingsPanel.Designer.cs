namespace GameLab.Eyetracking.EyetrackerControls
{
    partial class EyetrackerCalibrationSettingsPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnCustomCalibrationScreenSettings = new System.Windows.Forms.Button();
            this.cbUseCustomCalibrationScreen = new System.Windows.Forms.CheckBox();
            this.pbCalibrationImage = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnChooseCalibrationImage = new System.Windows.Forms.Button();
            this.cbIncreasedSpeed = new System.Windows.Forms.CheckBox();
            this.lbBackgroundBrightness = new System.Windows.Forms.Label();
            this.tbBackgroundBrightness = new System.Windows.Forms.TrackBar();
            this.label14 = new System.Windows.Forms.Label();
            this.nudImageSize = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nudNumberOfCalibrationPoints = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cbDisplayDevice = new System.Windows.Forms.ComboBox();
            this.cbAcceptationRequiredForEveryCalibrationPoint = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCalibrationImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBackgroundBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfCalibrationPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnCustomCalibrationScreenSettings);
            this.groupBox4.Controls.Add(this.cbUseCustomCalibrationScreen);
            this.groupBox4.Controls.Add(this.pbCalibrationImage);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.btnChooseCalibrationImage);
            this.groupBox4.Controls.Add(this.cbIncreasedSpeed);
            this.groupBox4.Controls.Add(this.lbBackgroundBrightness);
            this.groupBox4.Controls.Add(this.tbBackgroundBrightness);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.nudImageSize);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.nudNumberOfCalibrationPoints);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.cbDisplayDevice);
            this.groupBox4.Controls.Add(this.cbAcceptationRequiredForEveryCalibrationPoint);
            this.groupBox4.Location = new System.Drawing.Point(4, 4);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(380, 394);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Calibration";
            // 
            // btnCustomCalibrationScreenSettings
            // 
            this.btnCustomCalibrationScreenSettings.Location = new System.Drawing.Point(253, 25);
            this.btnCustomCalibrationScreenSettings.Name = "btnCustomCalibrationScreenSettings";
            this.btnCustomCalibrationScreenSettings.Size = new System.Drawing.Size(109, 32);
            this.btnCustomCalibrationScreenSettings.TabIndex = 33;
            this.btnCustomCalibrationScreenSettings.Text = "Settings...";
            this.btnCustomCalibrationScreenSettings.UseVisualStyleBackColor = true;
            this.btnCustomCalibrationScreenSettings.Click += new System.EventHandler(this.btnCustomCalibrationScreenSettings_Click);
            // 
            // cbUseCustomCalibrationScreen
            // 
            this.cbUseCustomCalibrationScreen.AutoSize = true;
            this.cbUseCustomCalibrationScreen.BackColor = System.Drawing.SystemColors.Control;
            this.cbUseCustomCalibrationScreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbUseCustomCalibrationScreen.Location = new System.Drawing.Point(19, 32);
            this.cbUseCustomCalibrationScreen.Margin = new System.Windows.Forms.Padding(4);
            this.cbUseCustomCalibrationScreen.Name = "cbUseCustomCalibrationScreen";
            this.cbUseCustomCalibrationScreen.Size = new System.Drawing.Size(220, 21);
            this.cbUseCustomCalibrationScreen.TabIndex = 32;
            this.cbUseCustomCalibrationScreen.Text = "Use custom calibration screen";
            this.cbUseCustomCalibrationScreen.UseVisualStyleBackColor = false;
            this.cbUseCustomCalibrationScreen.CheckedChanged += new System.EventHandler(this.cbUseCustomCalibrationScreen_CheckedChanged);
            // 
            // pbCalibrationImage
            // 
            this.pbCalibrationImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCalibrationImage.ErrorImage = null;
            this.pbCalibrationImage.Image = global::GameLab.Eyetracking.EyetrackerControls.Properties.Resources.kalibracja;
            this.pbCalibrationImage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pbCalibrationImage.InitialImage = null;
            this.pbCalibrationImage.Location = new System.Drawing.Point(20, 286);
            this.pbCalibrationImage.Margin = new System.Windows.Forms.Padding(4);
            this.pbCalibrationImage.Name = "pbCalibrationImage";
            this.pbCalibrationImage.Size = new System.Drawing.Size(106, 98);
            this.pbCalibrationImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCalibrationImage.TabIndex = 31;
            this.pbCalibrationImage.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(16, 265);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(198, 17);
            this.label13.TabIndex = 30;
            this.label13.Text = "Image used during calibration:";
            // 
            // btnChooseCalibrationImage
            // 
            this.btnChooseCalibrationImage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChooseCalibrationImage.Location = new System.Drawing.Point(134, 286);
            this.btnChooseCalibrationImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnChooseCalibrationImage.Name = "btnChooseCalibrationImage";
            this.btnChooseCalibrationImage.Size = new System.Drawing.Size(109, 32);
            this.btnChooseCalibrationImage.TabIndex = 29;
            this.btnChooseCalibrationImage.Text = "Choose...";
            this.btnChooseCalibrationImage.UseVisualStyleBackColor = true;
            this.btnChooseCalibrationImage.Click += new System.EventHandler(this.btnChooseCalibrationImage_Click);
            // 
            // cbIncreasedSpeed
            // 
            this.cbIncreasedSpeed.AutoSize = true;
            this.cbIncreasedSpeed.Checked = true;
            this.cbIncreasedSpeed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncreasedSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbIncreasedSpeed.Location = new System.Drawing.Point(20, 201);
            this.cbIncreasedSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.cbIncreasedSpeed.Name = "cbIncreasedSpeed";
            this.cbIncreasedSpeed.Size = new System.Drawing.Size(135, 21);
            this.cbIncreasedSpeed.TabIndex = 27;
            this.cbIncreasedSpeed.Text = "Increased speed";
            this.cbIncreasedSpeed.UseVisualStyleBackColor = true;
            // 
            // lbBackgroundBrightness
            // 
            this.lbBackgroundBrightness.AutoSize = true;
            this.lbBackgroundBrightness.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbBackgroundBrightness.Location = new System.Drawing.Point(150, 131);
            this.lbBackgroundBrightness.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBackgroundBrightness.Name = "lbBackgroundBrightness";
            this.lbBackgroundBrightness.Size = new System.Drawing.Size(158, 17);
            this.lbBackgroundBrightness.TabIndex = 26;
            this.lbBackgroundBrightness.Text = "Background brightness:";
            // 
            // tbBackgroundBrightness
            // 
            this.tbBackgroundBrightness.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbBackgroundBrightness.Location = new System.Drawing.Point(150, 150);
            this.tbBackgroundBrightness.Margin = new System.Windows.Forms.Padding(4);
            this.tbBackgroundBrightness.Maximum = 255;
            this.tbBackgroundBrightness.Name = "tbBackgroundBrightness";
            this.tbBackgroundBrightness.Size = new System.Drawing.Size(213, 56);
            this.tbBackgroundBrightness.TabIndex = 25;
            this.tbBackgroundBrightness.TickFrequency = 15;
            this.tbBackgroundBrightness.Value = 255;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(20, 131);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 17);
            this.label14.TabIndex = 22;
            this.label14.Text = "Image size:";
            // 
            // nudImageSize
            // 
            this.nudImageSize.Location = new System.Drawing.Point(20, 154);
            this.nudImageSize.Margin = new System.Windows.Forms.Padding(4);
            this.nudImageSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudImageSize.Name = "nudImageSize";
            this.nudImageSize.Size = new System.Drawing.Size(111, 22);
            this.nudImageSize.TabIndex = 21;
            this.nudImageSize.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(146, 69);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 17);
            this.label10.TabIndex = 20;
            this.label10.Text = "Display device:";
            // 
            // nudNumberOfCalibrationPoints
            // 
            this.nudNumberOfCalibrationPoints.Location = new System.Drawing.Point(20, 90);
            this.nudNumberOfCalibrationPoints.Margin = new System.Windows.Forms.Padding(4);
            this.nudNumberOfCalibrationPoints.Name = "nudNumberOfCalibrationPoints";
            this.nudNumberOfCalibrationPoints.Size = new System.Drawing.Size(109, 22);
            this.nudNumberOfCalibrationPoints.TabIndex = 17;
            this.nudNumberOfCalibrationPoints.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudNumberOfCalibrationPoints.ValueChanged += new System.EventHandler(this.nudNumberOfCalibrationPoints_ValueChanged);
            this.nudNumberOfCalibrationPoints.Leave += new System.EventHandler(this.nudNumberOfCalibrationPoints_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(16, 69);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Number of points:";
            // 
            // cbDisplayDevice
            // 
            this.cbDisplayDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisplayDevice.FormattingEnabled = true;
            this.cbDisplayDevice.Location = new System.Drawing.Point(150, 89);
            this.cbDisplayDevice.Margin = new System.Windows.Forms.Padding(4);
            this.cbDisplayDevice.Name = "cbDisplayDevice";
            this.cbDisplayDevice.Size = new System.Drawing.Size(212, 24);
            this.cbDisplayDevice.TabIndex = 13;
            // 
            // cbAcceptationRequiredForEveryCalibrationPoint
            // 
            this.cbAcceptationRequiredForEveryCalibrationPoint.AutoSize = true;
            this.cbAcceptationRequiredForEveryCalibrationPoint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbAcceptationRequiredForEveryCalibrationPoint.Location = new System.Drawing.Point(20, 229);
            this.cbAcceptationRequiredForEveryCalibrationPoint.Margin = new System.Windows.Forms.Padding(4);
            this.cbAcceptationRequiredForEveryCalibrationPoint.Name = "cbAcceptationRequiredForEveryCalibrationPoint";
            this.cbAcceptationRequiredForEveryCalibrationPoint.Size = new System.Drawing.Size(325, 21);
            this.cbAcceptationRequiredForEveryCalibrationPoint.TabIndex = 28;
            this.cbAcceptationRequiredForEveryCalibrationPoint.Text = "Acceptation required for every calibration point";
            this.cbAcceptationRequiredForEveryCalibrationPoint.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "jpg";
            this.openFileDialog.Filter = "Pliki obrazów|*.jpg;*.jpeg;*.jpe;*.png;*.bmp|Wszystkie pliki|*.*";
            this.openFileDialog.Title = "Wybierz pliku obrazu kalibracji";
            // 
            // EyetrackerCalibrationSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "EyetrackerCalibrationSettingsPanel";
            this.Size = new System.Drawing.Size(386, 404);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCalibrationImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBackgroundBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfCalibrationPoints)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbUseCustomCalibrationScreen;
        private System.Windows.Forms.CheckBox cbIncreasedSpeed;
        private System.Windows.Forms.Label lbBackgroundBrightness;
        private System.Windows.Forms.TrackBar tbBackgroundBrightness;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudImageSize;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudNumberOfCalibrationPoints;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbDisplayDevice;
        private System.Windows.Forms.CheckBox cbAcceptationRequiredForEveryCalibrationPoint;
        private System.Windows.Forms.PictureBox pbCalibrationImage;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnChooseCalibrationImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnCustomCalibrationScreenSettings;
    }
}
