namespace GameLab.Eyetracking.EyetrackerControls
{
    partial class EnterAndLeaveControlsForm
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
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.lbLastEnter = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbLastLeave = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbInRegionTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbAngleDifference = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            // 
            // lbLastEnter
            // 
            this.lbLastEnter.AutoSize = true;
            this.lbLastEnter.Location = new System.Drawing.Point(209, 43);
            this.lbLastEnter.Name = "lbLastEnter";
            this.lbLastEnter.Size = new System.Drawing.Size(23, 17);
            this.lbLastEnter.TabIndex = 0;
            this.lbLastEnter.Text = "---";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(383, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Controls are inactive for first 1 seconds (marked by red dot)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Last enter control and angle:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Last leave control and angle:";
            // 
            // lbLastLeave
            // 
            this.lbLastLeave.AutoSize = true;
            this.lbLastLeave.Location = new System.Drawing.Point(209, 60);
            this.lbLastLeave.Name = "lbLastLeave";
            this.lbLastLeave.Size = new System.Drawing.Size(23, 17);
            this.lbLastLeave.TabIndex = 10;
            this.lbLastLeave.Text = "---";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "In-region time:";
            // 
            // lbInRegionTime
            // 
            this.lbInRegionTime.AutoSize = true;
            this.lbInRegionTime.Location = new System.Drawing.Point(209, 94);
            this.lbInRegionTime.Name = "lbInRegionTime";
            this.lbInRegionTime.Size = new System.Drawing.Size(23, 17);
            this.lbInRegionTime.TabIndex = 12;
            this.lbInRegionTime.Text = "---";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Angle difference:";
            // 
            // lbAngleDifference
            // 
            this.lbAngleDifference.AutoSize = true;
            this.lbAngleDifference.Location = new System.Drawing.Point(209, 77);
            this.lbAngleDifference.Name = "lbAngleDifference";
            this.lbAngleDifference.Size = new System.Drawing.Size(23, 17);
            this.lbAngleDifference.TabIndex = 14;
            this.lbAngleDifference.Text = "---";
            // 
            // EnterAndExitControlsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 404);
            this.Controls.Add(this.lbAngleDifference);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbInRegionTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbLastLeave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbLastEnter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "EnterAndExitControlsForm";
            this.Text = "EnterAndExitControlsForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.EnterAndLeaveControlsForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterAndLeaveControlsForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label lbLastEnter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbLastLeave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbInRegionTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbAngleDifference;
    }
}