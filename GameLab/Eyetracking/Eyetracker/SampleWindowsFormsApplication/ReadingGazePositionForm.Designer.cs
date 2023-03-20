namespace GameLab.Eyetracking.EyetrackerControls
{
    partial class ReadingGazePositionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadingGazePositionForm));
            this.panelDebug = new System.Windows.Forms.Panel();
            this.lbEyetrackerName = new System.Windows.Forms.Label();
            this.lbEyeClosed = new System.Windows.Forms.Label();
            this.lbEyesState2 = new System.Windows.Forms.Label();
            this.lbEyesState = new System.Windows.Forms.Label();
            this.pbEyetrackerImage = new System.Windows.Forms.PictureBox();
            this.lbAverEyePosition1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbAverEyePosition = new System.Windows.Forms.Label();
            this.lbLeftEyePosition = new System.Windows.Forms.Label();
            this.lbRightEyePosition = new System.Windows.Forms.Label();
            this.panelDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEyetrackerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDebug
            // 
            this.panelDebug.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDebug.Controls.Add(this.lbRightEyePosition);
            this.panelDebug.Controls.Add(this.lbLeftEyePosition);
            this.panelDebug.Controls.Add(this.lbAverEyePosition);
            this.panelDebug.Controls.Add(this.label3);
            this.panelDebug.Controls.Add(this.label1);
            this.panelDebug.Controls.Add(this.lbEyetrackerName);
            this.panelDebug.Controls.Add(this.lbEyeClosed);
            this.panelDebug.Controls.Add(this.lbEyesState2);
            this.panelDebug.Controls.Add(this.lbEyesState);
            this.panelDebug.Controls.Add(this.pbEyetrackerImage);
            this.panelDebug.Controls.Add(this.lbAverEyePosition1);
            this.panelDebug.Location = new System.Drawing.Point(13, 13);
            this.panelDebug.Name = "panelDebug";
            this.panelDebug.Size = new System.Drawing.Size(263, 411);
            this.panelDebug.TabIndex = 0;
            // 
            // lbEyetrackerName
            // 
            this.lbEyetrackerName.AutoSize = true;
            this.lbEyetrackerName.Location = new System.Drawing.Point(3, 10);
            this.lbEyetrackerName.Name = "lbEyetrackerName";
            this.lbEyetrackerName.Size = new System.Drawing.Size(123, 17);
            this.lbEyetrackerName.TabIndex = 5;
            this.lbEyetrackerName.Text = "Eyetracker name: ";
            // 
            // lbEyeClosed
            // 
            this.lbEyeClosed.AutoSize = true;
            this.lbEyeClosed.Location = new System.Drawing.Point(3, 128);
            this.lbEyeClosed.Name = "lbEyeClosed";
            this.lbEyeClosed.Size = new System.Drawing.Size(121, 17);
            this.lbEyeClosed.TabIndex = 4;
            this.lbEyeClosed.Text = "Eyes detected: ---";
            // 
            // lbEyesState2
            // 
            this.lbEyesState2.AutoSize = true;
            this.lbEyesState2.Location = new System.Drawing.Point(84, 111);
            this.lbEyesState2.Name = "lbEyesState2";
            this.lbEyesState2.Size = new System.Drawing.Size(23, 17);
            this.lbEyesState2.TabIndex = 3;
            this.lbEyesState2.Text = "---";
            // 
            // lbEyesState
            // 
            this.lbEyesState.AutoSize = true;
            this.lbEyesState.Location = new System.Drawing.Point(3, 94);
            this.lbEyesState.Name = "lbEyesState";
            this.lbEyesState.Size = new System.Drawing.Size(104, 17);
            this.lbEyesState.TabIndex = 2;
            this.lbEyesState.Text = "Gaze event: ---";
            // 
            // pbEyetrackerImage
            // 
            this.pbEyetrackerImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbEyetrackerImage.ErrorImage = null;
            this.pbEyetrackerImage.InitialImage = null;
            this.pbEyetrackerImage.Location = new System.Drawing.Point(6, 155);
            this.pbEyetrackerImage.Name = "pbEyetrackerImage";
            this.pbEyetrackerImage.Size = new System.Drawing.Size(249, 249);
            this.pbEyetrackerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbEyetrackerImage.TabIndex = 1;
            this.pbEyetrackerImage.TabStop = false;
            // 
            // lbAverEyePosition1
            // 
            this.lbAverEyePosition1.AutoSize = true;
            this.lbAverEyePosition1.Location = new System.Drawing.Point(3, 36);
            this.lbAverEyePosition1.Name = "lbAverEyePosition1";
            this.lbAverEyePosition1.Size = new System.Drawing.Size(125, 17);
            this.lbAverEyePosition1.TabIndex = 0;
            this.lbAverEyePosition1.Text = "Aver. eye position:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Left eye position:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Right eye position:";
            // 
            // lbAverEyePosition
            // 
            this.lbAverEyePosition.AutoSize = true;
            this.lbAverEyePosition.Location = new System.Drawing.Point(134, 36);
            this.lbAverEyePosition.Name = "lbAverEyePosition";
            this.lbAverEyePosition.Size = new System.Drawing.Size(23, 17);
            this.lbAverEyePosition.TabIndex = 8;
            this.lbAverEyePosition.Text = "---";
            // 
            // lbLeftEyePosition
            // 
            this.lbLeftEyePosition.AutoSize = true;
            this.lbLeftEyePosition.Location = new System.Drawing.Point(134, 53);
            this.lbLeftEyePosition.Name = "lbLeftEyePosition";
            this.lbLeftEyePosition.Size = new System.Drawing.Size(23, 17);
            this.lbLeftEyePosition.TabIndex = 9;
            this.lbLeftEyePosition.Text = "---";
            // 
            // lbRightEyePosition
            // 
            this.lbRightEyePosition.AutoSize = true;
            this.lbRightEyePosition.Location = new System.Drawing.Point(134, 70);
            this.lbRightEyePosition.Name = "lbRightEyePosition";
            this.lbRightEyePosition.Size = new System.Drawing.Size(23, 17);
            this.lbRightEyePosition.TabIndex = 10;
            this.lbRightEyePosition.Text = "---";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.panelDebug);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.panelDebug.ResumeLayout(false);
            this.panelDebug.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEyetrackerImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDebug;
        private System.Windows.Forms.Label lbAverEyePosition1;
        private System.Windows.Forms.PictureBox pbEyetrackerImage;
        private System.Windows.Forms.Label lbEyesState;
        private System.Windows.Forms.Label lbEyesState2;
        private System.Windows.Forms.Label lbEyeClosed;
        private System.Windows.Forms.Label lbEyetrackerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbLeftEyePosition;
        private System.Windows.Forms.Label lbAverEyePosition;
        private System.Windows.Forms.Label lbRightEyePosition;
    }
}

