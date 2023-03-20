namespace SampleWindowsFormsApplication
{
    partial class SettingsForm
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
            this.rbSampleEnterAndLeaveControlsManager = new System.Windows.Forms.RadioButton();
            this.rbSampleReadGazePosition_Properties = new System.Windows.Forms.RadioButton();
            this.rbSampleDwellTimeControlsManager = new System.Windows.Forms.RadioButton();
            this.rbSampleReadGazePosition_Events = new System.Windows.Forms.RadioButton();
            this.rb9DotsGestures = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnalysisInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Text = "Run sample";
            // 
            // cbMonitor
            // 
            this.cbMonitor.Size = new System.Drawing.Size(316, 24);
            // 
            // rbSampleEnterAndLeaveControlsManager
            // 
            this.rbSampleEnterAndLeaveControlsManager.AutoSize = true;
            this.rbSampleEnterAndLeaveControlsManager.Location = new System.Drawing.Point(421, 655);
            this.rbSampleEnterAndLeaveControlsManager.Name = "rbSampleEnterAndLeaveControlsManager";
            this.rbSampleEnterAndLeaveControlsManager.Size = new System.Drawing.Size(292, 21);
            this.rbSampleEnterAndLeaveControlsManager.TabIndex = 49;
            this.rbSampleEnterAndLeaveControlsManager.Text = "Enter and leave controls manager sample";
            this.rbSampleEnterAndLeaveControlsManager.UseVisualStyleBackColor = true;
            // 
            // rbSampleReadGazePosition_Properties
            // 
            this.rbSampleReadGazePosition_Properties.AutoSize = true;
            this.rbSampleReadGazePosition_Properties.Location = new System.Drawing.Point(421, 601);
            this.rbSampleReadGazePosition_Properties.Name = "rbSampleReadGazePosition_Properties";
            this.rbSampleReadGazePosition_Properties.Size = new System.Drawing.Size(304, 21);
            this.rbSampleReadGazePosition_Properties.TabIndex = 48;
            this.rbSampleReadGazePosition_Properties.TabStop = true;
            this.rbSampleReadGazePosition_Properties.Text = "Reading gaze position samples (properties)";
            this.rbSampleReadGazePosition_Properties.UseVisualStyleBackColor = true;
            // 
            // rbSampleDwellTimeControlsManager
            // 
            this.rbSampleDwellTimeControlsManager.AutoSize = true;
            this.rbSampleDwellTimeControlsManager.Location = new System.Drawing.Point(421, 628);
            this.rbSampleDwellTimeControlsManager.Name = "rbSampleDwellTimeControlsManager";
            this.rbSampleDwellTimeControlsManager.Size = new System.Drawing.Size(256, 21);
            this.rbSampleDwellTimeControlsManager.TabIndex = 47;
            this.rbSampleDwellTimeControlsManager.Text = "Dwell-time controls manager sample";
            this.rbSampleDwellTimeControlsManager.UseVisualStyleBackColor = true;
            // 
            // rbSampleReadGazePosition_Events
            // 
            this.rbSampleReadGazePosition_Events.AutoSize = true;
            this.rbSampleReadGazePosition_Events.Location = new System.Drawing.Point(421, 574);
            this.rbSampleReadGazePosition_Events.Name = "rbSampleReadGazePosition_Events";
            this.rbSampleReadGazePosition_Events.Size = new System.Drawing.Size(282, 21);
            this.rbSampleReadGazePosition_Events.TabIndex = 46;
            this.rbSampleReadGazePosition_Events.Text = "Reading gaze position samples (events)";
            this.rbSampleReadGazePosition_Events.UseVisualStyleBackColor = true;
            // 
            // rb9NGestures
            // 
            this.rb9DotsGestures.AutoSize = true;
            this.rb9DotsGestures.Checked = true;
            this.rb9DotsGestures.Location = new System.Drawing.Point(421, 547);
            this.rb9DotsGestures.Name = "rb9NGestures";
            this.rb9DotsGestures.Size = new System.Drawing.Size(192, 21);
            this.rb9DotsGestures.TabIndex = 50;
            this.rb9DotsGestures.TabStop = true;
            this.rb9DotsGestures.Text = "Gaze Nine-Dots Gestures";
            this.rb9DotsGestures.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 731);
            this.Controls.Add(this.rb9DotsGestures);
            this.Controls.Add(this.rbSampleEnterAndLeaveControlsManager);
            this.Controls.Add(this.rbSampleReadGazePosition_Properties);
            this.Controls.Add(this.rbSampleDwellTimeControlsManager);
            this.Controls.Add(this.rbSampleReadGazePosition_Events);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "SettingsForm";
            this.Text = "Sample Windows Forms Application - Settings";
            this.Controls.SetChildIndex(this.lbLeftEyeCalibrationAccuracy, 0);
            this.Controls.SetChildIndex(this.lbRightEyeCalibrationAccuracy, 0);
            this.Controls.SetChildIndex(this.cbMonitor, 0);
            this.Controls.SetChildIndex(this.nudAnalysisInterval, 0);
            this.Controls.SetChildIndex(this.btnRun, 0);
            this.Controls.SetChildIndex(this.rbSampleReadGazePosition_Events, 0);
            this.Controls.SetChildIndex(this.rbSampleDwellTimeControlsManager, 0);
            this.Controls.SetChildIndex(this.rbSampleReadGazePosition_Properties, 0);
            this.Controls.SetChildIndex(this.rbSampleEnterAndLeaveControlsManager, 0);
            this.Controls.SetChildIndex(this.rb9DotsGestures, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudAnalysisInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbSampleEnterAndLeaveControlsManager;
        private System.Windows.Forms.RadioButton rbSampleReadGazePosition_Properties;
        private System.Windows.Forms.RadioButton rbSampleDwellTimeControlsManager;
        private System.Windows.Forms.RadioButton rbSampleReadGazePosition_Events;
        private System.Windows.Forms.RadioButton rb9DotsGestures;
    }
}