namespace GameLab.Eyetracking.EyetrackerControls
{
    partial class GazeRuntimeAnalysisSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GazeRuntimeAnalysisSettingsForm));
            this.nudDispersionY = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.nudDispersionX = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.cbFixationDetectionMethod = new System.Windows.Forms.ComboBox();
            this.nudMinimalFixationNumberOfSamples = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDetectEyeClosure = new System.Windows.Forms.CheckBox();
            this.cbDetectFixationsAndSaccades = new System.Windows.Forms.CheckBox();
            this.nudMinimalNumberOfStatesToDetectEyeClosure = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.nudSaccadeThresholdVelocity = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cbUseThreePointsDerivation = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudDispersionY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDispersionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimalFixationNumberOfSamples)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimalNumberOfStatesToDetectEyeClosure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSaccadeThresholdVelocity)).BeginInit();
            this.SuspendLayout();
            // 
            // nudDispersionY
            // 
            this.nudDispersionY.Location = new System.Drawing.Point(294, 181);
            this.nudDispersionY.Name = "nudDispersionY";
            this.nudDispersionY.Size = new System.Drawing.Size(88, 22);
            this.nudDispersionY.TabIndex = 33;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 183);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(176, 17);
            this.label17.TabIndex = 32;
            this.label17.Text = "Maksimal dispertion (X, Y):";
            // 
            // nudDispersionX
            // 
            this.nudDispersionX.Location = new System.Drawing.Point(200, 181);
            this.nudDispersionX.Name = "nudDispersionX";
            this.nudDispersionX.Size = new System.Drawing.Size(88, 22);
            this.nudDispersionX.TabIndex = 31;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 115);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(169, 17);
            this.label16.TabIndex = 35;
            this.label16.Text = "Fixation detection method";
            // 
            // cbFixationDetectionMethod
            // 
            this.cbFixationDetectionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFixationDetectionMethod.FormattingEnabled = true;
            this.cbFixationDetectionMethod.Items.AddRange(new object[] {
            "Gaze velocity threshold",
            "Gaze position dispersion"});
            this.cbFixationDetectionMethod.Location = new System.Drawing.Point(12, 135);
            this.cbFixationDetectionMethod.Name = "cbFixationDetectionMethod";
            this.cbFixationDetectionMethod.Size = new System.Drawing.Size(276, 24);
            this.cbFixationDetectionMethod.TabIndex = 34;
            // 
            // nudMinimalFixationNumberOfSamples
            // 
            this.nudMinimalFixationNumberOfSamples.Location = new System.Drawing.Point(294, 213);
            this.nudMinimalFixationNumberOfSamples.Name = "nudMinimalFixationNumberOfSamples";
            this.nudMinimalFixationNumberOfSamples.Size = new System.Drawing.Size(88, 22);
            this.nudMinimalFixationNumberOfSamples.TabIndex = 37;
            this.nudMinimalFixationNumberOfSamples.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 17);
            this.label1.TabIndex = 38;
            this.label1.Text = "Minimal fixation number of samples:";
            // 
            // cbDetectEyeClosure
            // 
            this.cbDetectEyeClosure.AutoSize = true;
            this.cbDetectEyeClosure.Enabled = false;
            this.cbDetectEyeClosure.Location = new System.Drawing.Point(12, 12);
            this.cbDetectEyeClosure.Name = "cbDetectEyeClosure";
            this.cbDetectEyeClosure.Size = new System.Drawing.Size(148, 21);
            this.cbDetectEyeClosure.TabIndex = 39;
            this.cbDetectEyeClosure.Text = "Detect eye closure";
            this.cbDetectEyeClosure.UseVisualStyleBackColor = true;
            // 
            // cbDetectFixationsAndSaccades
            // 
            this.cbDetectFixationsAndSaccades.AutoSize = true;
            this.cbDetectFixationsAndSaccades.Location = new System.Drawing.Point(12, 40);
            this.cbDetectFixationsAndSaccades.Name = "cbDetectFixationsAndSaccades";
            this.cbDetectFixationsAndSaccades.Size = new System.Drawing.Size(218, 21);
            this.cbDetectFixationsAndSaccades.TabIndex = 40;
            this.cbDetectFixationsAndSaccades.Text = "Detect fixations and saccades";
            this.cbDetectFixationsAndSaccades.UseVisualStyleBackColor = true;
            // 
            // nudMinimalNumberOfStatesToDetectEyeClosure
            // 
            this.nudMinimalNumberOfStatesToDetectEyeClosure.Location = new System.Drawing.Point(294, 78);
            this.nudMinimalNumberOfStatesToDetectEyeClosure.Name = "nudMinimalNumberOfStatesToDetectEyeClosure";
            this.nudMinimalNumberOfStatesToDetectEyeClosure.Size = new System.Drawing.Size(88, 22);
            this.nudMinimalNumberOfStatesToDetectEyeClosure.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 17);
            this.label2.TabIndex = 42;
            this.label2.Text = "Number of states to detect eye closure:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOK.Location = new System.Drawing.Point(307, 331);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 32);
            this.btnOK.TabIndex = 43;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // nudSaccadeThresholdVelocity
            // 
            this.nudSaccadeThresholdVelocity.DecimalPlaces = 2;
            this.nudSaccadeThresholdVelocity.Location = new System.Drawing.Point(294, 256);
            this.nudSaccadeThresholdVelocity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nudSaccadeThresholdVelocity.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudSaccadeThresholdVelocity.Name = "nudSaccadeThresholdVelocity";
            this.nudSaccadeThresholdVelocity.Size = new System.Drawing.Size(91, 22);
            this.nudSaccadeThresholdVelocity.TabIndex = 44;
            this.nudSaccadeThresholdVelocity.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 17);
            this.label3.TabIndex = 46;
            this.label3.Text = "Saccade threshold velocity (px/ms):";
            // 
            // cbUseThreePointsDerivation
            // 
            this.cbUseThreePointsDerivation.AutoSize = true;
            this.cbUseThreePointsDerivation.Location = new System.Drawing.Point(15, 287);
            this.cbUseThreePointsDerivation.Name = "cbUseThreePointsDerivation";
            this.cbUseThreePointsDerivation.Size = new System.Drawing.Size(200, 21);
            this.cbUseThreePointsDerivation.TabIndex = 47;
            this.cbUseThreePointsDerivation.Text = "Use three points derivation";
            this.cbUseThreePointsDerivation.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(226, 331);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 48;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // GazeRuntimeAnalysisSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 379);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbUseThreePointsDerivation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudSaccadeThresholdVelocity);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudMinimalNumberOfStatesToDetectEyeClosure);
            this.Controls.Add(this.cbDetectFixationsAndSaccades);
            this.Controls.Add(this.cbDetectEyeClosure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudMinimalFixationNumberOfSamples);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.cbFixationDetectionMethod);
            this.Controls.Add(this.nudDispersionY);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.nudDispersionX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GazeRuntimeAnalysisSettingsForm";
            this.Text = "GazeRuntimeAnalysisSettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.nudDispersionY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDispersionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimalFixationNumberOfSamples)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimalNumberOfStatesToDetectEyeClosure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSaccadeThresholdVelocity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudDispersionY;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown nudDispersionX;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbFixationDetectionMethod;
        private System.Windows.Forms.NumericUpDown nudMinimalFixationNumberOfSamples;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbDetectEyeClosure;
        private System.Windows.Forms.CheckBox cbDetectFixationsAndSaccades;
        private System.Windows.Forms.NumericUpDown nudMinimalNumberOfStatesToDetectEyeClosure;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.NumericUpDown nudSaccadeThresholdVelocity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbUseThreePointsDerivation;
        private System.Windows.Forms.Button btnCancel;
    }
}