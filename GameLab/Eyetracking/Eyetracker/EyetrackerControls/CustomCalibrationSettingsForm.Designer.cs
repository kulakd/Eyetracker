namespace GameLab.Eyetracking.EyetrackerControls
{
    partial class CustomCalibrationSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomCalibrationSettingsForm));
            this.pnlBackgroundColor = new System.Windows.Forms.Panel();
            this.btnChooseColor = new System.Windows.Forms.Button();
            this.btnChooseSound = new System.Windows.Forms.Button();
            this.lbCalibrationPointIndex = new System.Windows.Forms.Label();
            this.nudCalibrationPointIndex = new System.Windows.Forms.NumericUpDown();
            this.cbUseMultipleImagesAndSounds = new System.Windows.Forms.CheckBox();
            this.pbCalibrationImagePreview = new System.Windows.Forms.PictureBox();
            this.lbImagesAndSoundsUsedDuringCalibration = new System.Windows.Forms.Label();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.imageOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.soundOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lbNumberOfCalibrationPoints = new System.Windows.Forms.Label();
            this.nudImageShrinkingTime = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cbImageShrinkingEnabled = new System.Windows.Forms.CheckBox();
            this.btnClearSound = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudCalibrationPointIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCalibrationImagePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageShrinkingTime)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBackgroundColor
            // 
            this.pnlBackgroundColor.BackColor = System.Drawing.Color.White;
            this.pnlBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackgroundColor.Location = new System.Drawing.Point(16, 30);
            this.pnlBackgroundColor.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBackgroundColor.Name = "pnlBackgroundColor";
            this.pnlBackgroundColor.Size = new System.Drawing.Size(37, 32);
            this.pnlBackgroundColor.TabIndex = 39;
            // 
            // btnChooseColor
            // 
            this.btnChooseColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChooseColor.Location = new System.Drawing.Point(59, 30);
            this.btnChooseColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnChooseColor.Name = "btnChooseColor";
            this.btnChooseColor.Size = new System.Drawing.Size(181, 32);
            this.btnChooseColor.TabIndex = 38;
            this.btnChooseColor.Text = "Choose color...";
            this.btnChooseColor.UseVisualStyleBackColor = true;
            this.btnChooseColor.Click += new System.EventHandler(this.btnChooseColor_Click);
            // 
            // btnChooseSound
            // 
            this.btnChooseSound.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChooseSound.Location = new System.Drawing.Point(250, 243);
            this.btnChooseSound.Margin = new System.Windows.Forms.Padding(4);
            this.btnChooseSound.Name = "btnChooseSound";
            this.btnChooseSound.Size = new System.Drawing.Size(109, 32);
            this.btnChooseSound.TabIndex = 46;
            this.btnChooseSound.Text = "Sound...";
            this.btnChooseSound.UseVisualStyleBackColor = true;
            this.btnChooseSound.Click += new System.EventHandler(this.btnChooseSound_Click);
            // 
            // lbCalibrationPointIndex
            // 
            this.lbCalibrationPointIndex.AutoSize = true;
            this.lbCalibrationPointIndex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbCalibrationPointIndex.Location = new System.Drawing.Point(12, 188);
            this.lbCalibrationPointIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCalibrationPointIndex.Name = "lbCalibrationPointIndex";
            this.lbCalibrationPointIndex.Size = new System.Drawing.Size(151, 17);
            this.lbCalibrationPointIndex.TabIndex = 45;
            this.lbCalibrationPointIndex.Text = "Calibration point index:";
            // 
            // nudCalibrationPointIndex
            // 
            this.nudCalibrationPointIndex.Location = new System.Drawing.Point(171, 186);
            this.nudCalibrationPointIndex.Margin = new System.Windows.Forms.Padding(4);
            this.nudCalibrationPointIndex.Name = "nudCalibrationPointIndex";
            this.nudCalibrationPointIndex.Size = new System.Drawing.Size(60, 22);
            this.nudCalibrationPointIndex.TabIndex = 44;
            this.nudCalibrationPointIndex.ValueChanged += new System.EventHandler(this.nudCalibrationPointIndex_ValueChanged);
            // 
            // cbUseMultipleImagesAndSounds
            // 
            this.cbUseMultipleImagesAndSounds.AutoSize = true;
            this.cbUseMultipleImagesAndSounds.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbUseMultipleImagesAndSounds.Location = new System.Drawing.Point(16, 123);
            this.cbUseMultipleImagesAndSounds.Margin = new System.Windows.Forms.Padding(4);
            this.cbUseMultipleImagesAndSounds.Name = "cbUseMultipleImagesAndSounds";
            this.cbUseMultipleImagesAndSounds.Size = new System.Drawing.Size(353, 21);
            this.cbUseMultipleImagesAndSounds.TabIndex = 43;
            this.cbUseMultipleImagesAndSounds.Text = "Use extended screen (multiple images and sounds)";
            this.cbUseMultipleImagesAndSounds.UseVisualStyleBackColor = true;
            this.cbUseMultipleImagesAndSounds.CheckedChanged += new System.EventHandler(this.cbUseMultipleImagesAndSounds_CheckedChanged);
            // 
            // pbCalibrationImagePreview
            // 
            this.pbCalibrationImagePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCalibrationImagePreview.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pbCalibrationImagePreview.Location = new System.Drawing.Point(16, 243);
            this.pbCalibrationImagePreview.Margin = new System.Windows.Forms.Padding(4);
            this.pbCalibrationImagePreview.Name = "pbCalibrationImagePreview";
            this.pbCalibrationImagePreview.Size = new System.Drawing.Size(106, 98);
            this.pbCalibrationImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCalibrationImagePreview.TabIndex = 42;
            this.pbCalibrationImagePreview.TabStop = false;
            // 
            // lbImagesAndSoundsUsedDuringCalibration
            // 
            this.lbImagesAndSoundsUsedDuringCalibration.AutoSize = true;
            this.lbImagesAndSoundsUsedDuringCalibration.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbImagesAndSoundsUsedDuringCalibration.Location = new System.Drawing.Point(12, 222);
            this.lbImagesAndSoundsUsedDuringCalibration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbImagesAndSoundsUsedDuringCalibration.Name = "lbImagesAndSoundsUsedDuringCalibration";
            this.lbImagesAndSoundsUsedDuringCalibration.Size = new System.Drawing.Size(283, 17);
            this.lbImagesAndSoundsUsedDuringCalibration.TabIndex = 41;
            this.lbImagesAndSoundsUsedDuringCalibration.Text = "Images and sounds used during calibration:";
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChooseImage.Location = new System.Drawing.Point(130, 243);
            this.btnChooseImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(109, 32);
            this.btnChooseImage.TabIndex = 40;
            this.btnChooseImage.Text = "Image...";
            this.btnChooseImage.UseVisualStyleBackColor = true;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 17);
            this.label1.TabIndex = 47;
            this.label1.Text = "Background color:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOK.Location = new System.Drawing.Point(251, 355);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 32);
            this.btnOK.TabIndex = 48;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            this.colorDialog.Color = System.Drawing.Color.White;
            this.colorDialog.FullOpen = true;
            // 
            // imageOpenFileDialog
            // 
            this.imageOpenFileDialog.DefaultExt = "jpg";
            this.imageOpenFileDialog.Filter = "Image files (*.jpg;*.jpe;*.jpeg;*.bmp;*.png)|*.png|All files (*.*)|*.*";
            this.imageOpenFileDialog.Title = "Choose calibration image";
            // 
            // soundOpenFileDialog
            // 
            this.soundOpenFileDialog.Filter = "Sound files (*.wav; *.mp3)|*.wav;*.mp3|All files (*.*)|*.*";
            // 
            // lbNumberOfCalibrationPoints
            // 
            this.lbNumberOfCalibrationPoints.AutoSize = true;
            this.lbNumberOfCalibrationPoints.Location = new System.Drawing.Point(12, 158);
            this.lbNumberOfCalibrationPoints.Name = "lbNumberOfCalibrationPoints";
            this.lbNumberOfCalibrationPoints.Size = new System.Drawing.Size(193, 17);
            this.lbNumberOfCalibrationPoints.TabIndex = 49;
            this.lbNumberOfCalibrationPoints.Text = "Number of calibration points: ";
            // 
            // nudImageShrinkingTime
            // 
            this.nudImageShrinkingTime.Location = new System.Drawing.Point(189, 78);
            this.nudImageShrinkingTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudImageShrinkingTime.Name = "nudImageShrinkingTime";
            this.nudImageShrinkingTime.Size = new System.Drawing.Size(60, 22);
            this.nudImageShrinkingTime.TabIndex = 51;
            this.nudImageShrinkingTime.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 17);
            this.label3.TabIndex = 52;
            this.label3.Text = "ms";
            // 
            // cbImageShrinkingEnabled
            // 
            this.cbImageShrinkingEnabled.AutoSize = true;
            this.cbImageShrinkingEnabled.Location = new System.Drawing.Point(16, 79);
            this.cbImageShrinkingEnabled.Name = "cbImageShrinkingEnabled";
            this.cbImageShrinkingEnabled.Size = new System.Drawing.Size(167, 21);
            this.cbImageShrinkingEnabled.TabIndex = 53;
            this.cbImageShrinkingEnabled.Text = "Image animation time:";
            this.cbImageShrinkingEnabled.UseVisualStyleBackColor = true;
            // 
            // btnClearSound
            // 
            this.btnClearSound.Location = new System.Drawing.Point(251, 282);
            this.btnClearSound.Name = "btnClearSound";
            this.btnClearSound.Size = new System.Drawing.Size(109, 32);
            this.btnClearSound.TabIndex = 54;
            this.btnClearSound.Text = "Clear sound";
            this.btnClearSound.UseVisualStyleBackColor = true;
            this.btnClearSound.Click += new System.EventHandler(this.btnClearSound_Click);
            // 
            // CustomCalibrationSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 399);
            this.Controls.Add(this.btnClearSound);
            this.Controls.Add(this.cbImageShrinkingEnabled);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudImageShrinkingTime);
            this.Controls.Add(this.lbNumberOfCalibrationPoints);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChooseSound);
            this.Controls.Add(this.lbCalibrationPointIndex);
            this.Controls.Add(this.nudCalibrationPointIndex);
            this.Controls.Add(this.cbUseMultipleImagesAndSounds);
            this.Controls.Add(this.pbCalibrationImagePreview);
            this.Controls.Add(this.lbImagesAndSoundsUsedDuringCalibration);
            this.Controls.Add(this.btnChooseImage);
            this.Controls.Add(this.pnlBackgroundColor);
            this.Controls.Add(this.btnChooseColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomCalibrationSettingsForm";
            this.Text = "Custom calibration settings";
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CustomCalibrationSettingsForm_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudCalibrationPointIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCalibrationImagePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageShrinkingTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackgroundColor;
        private System.Windows.Forms.Button btnChooseColor;
        private System.Windows.Forms.Button btnChooseSound;
        private System.Windows.Forms.Label lbCalibrationPointIndex;
        private System.Windows.Forms.NumericUpDown nudCalibrationPointIndex;
        private System.Windows.Forms.CheckBox cbUseMultipleImagesAndSounds;
        private System.Windows.Forms.PictureBox pbCalibrationImagePreview;
        private System.Windows.Forms.Label lbImagesAndSoundsUsedDuringCalibration;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.OpenFileDialog imageOpenFileDialog;
        private System.Windows.Forms.OpenFileDialog soundOpenFileDialog;
        private System.Windows.Forms.Label lbNumberOfCalibrationPoints;
        private System.Windows.Forms.NumericUpDown nudImageShrinkingTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbImageShrinkingEnabled;
        private System.Windows.Forms.Button btnClearSound;
    }
}