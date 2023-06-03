namespace EyetrackerApp
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectButton = new System.Windows.Forms.Button();
            this.PipeStatusLabel = new System.Windows.Forms.Label();
            this.ConnectEyeTrackerButton = new System.Windows.Forms.Button();
            this.EyeTrackerStatusLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GazePointLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SleepStatusLabel = new System.Windows.Forms.Label();
            this.AlarmStartComboBox = new System.Windows.Forms.ComboBox();
            this.AlarmEndComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Do = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(268, 398);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(158, 23);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Połącz z aplikacja";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // PipeStatusLabel
            // 
            this.PipeStatusLabel.AutoSize = true;
            this.PipeStatusLabel.Location = new System.Drawing.Point(206, 82);
            this.PipeStatusLabel.Name = "PipeStatusLabel";
            this.PipeStatusLabel.Size = new System.Drawing.Size(35, 13);
            this.PipeStatusLabel.TabIndex = 1;
            this.PipeStatusLabel.Text = "label1";
            // 
            // ConnectEyeTrackerButton
            // 
            this.ConnectEyeTrackerButton.Location = new System.Drawing.Point(47, 398);
            this.ConnectEyeTrackerButton.Name = "ConnectEyeTrackerButton";
            this.ConnectEyeTrackerButton.Size = new System.Drawing.Size(154, 23);
            this.ConnectEyeTrackerButton.TabIndex = 2;
            this.ConnectEyeTrackerButton.Text = "Połącz z eyetrackerem";
            this.ConnectEyeTrackerButton.UseVisualStyleBackColor = true;
            this.ConnectEyeTrackerButton.Click += new System.EventHandler(this.ConnectEyeTrackerButton_Click);
            // 
            // EyeTrackerStatusLabel
            // 
            this.EyeTrackerStatusLabel.AutoSize = true;
            this.EyeTrackerStatusLabel.Location = new System.Drawing.Point(206, 57);
            this.EyeTrackerStatusLabel.Name = "EyeTrackerStatusLabel";
            this.EyeTrackerStatusLabel.Size = new System.Drawing.Size(35, 13);
            this.EyeTrackerStatusLabel.TabIndex = 3;
            this.EyeTrackerStatusLabel.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Połączenie z EyEtrackerem";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "polaczenie z Aplikacja";
            // 
            // GazePointLabel
            // 
            this.GazePointLabel.AutoSize = true;
            this.GazePointLabel.Location = new System.Drawing.Point(206, 105);
            this.GazePointLabel.Name = "GazePointLabel";
            this.GazePointLabel.Size = new System.Drawing.Size(35, 13);
            this.GazePointLabel.TabIndex = 6;
            this.GazePointLabel.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Polozenie wzroku";
            // 
            // SleepStatusLabel
            // 
            this.SleepStatusLabel.AutoSize = true;
            this.SleepStatusLabel.Location = new System.Drawing.Point(209, 131);
            this.SleepStatusLabel.Name = "SleepStatusLabel";
            this.SleepStatusLabel.Size = new System.Drawing.Size(35, 13);
            this.SleepStatusLabel.TabIndex = 8;
            this.SleepStatusLabel.Text = "label4";
            // 
            // AlarmStartComboBox
            // 
            this.AlarmStartComboBox.FormattingEnabled = true;
            this.AlarmStartComboBox.Location = new System.Drawing.Point(209, 163);
            this.AlarmStartComboBox.Name = "AlarmStartComboBox";
            this.AlarmStartComboBox.Size = new System.Drawing.Size(121, 21);
            this.AlarmStartComboBox.TabIndex = 9;
            // 
            // AlarmEndComboBox
            // 
            this.AlarmEndComboBox.FormattingEnabled = true;
            this.AlarmEndComboBox.Location = new System.Drawing.Point(209, 206);
            this.AlarmEndComboBox.Name = "AlarmEndComboBox";
            this.AlarmEndComboBox.Size = new System.Drawing.Size(121, 21);
            this.AlarmEndComboBox.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Alarm od";
            // 
            // Do
            // 
            this.Do.AutoSize = true;
            this.Do.Location = new System.Drawing.Point(44, 213);
            this.Do.Name = "Do";
            this.Do.Size = new System.Drawing.Size(48, 13);
            this.Do.TabIndex = 12;
            this.Do.Text = "Alarm do";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Czy spi";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Do);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AlarmEndComboBox);
            this.Controls.Add(this.AlarmStartComboBox);
            this.Controls.Add(this.SleepStatusLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GazePointLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EyeTrackerStatusLabel);
            this.Controls.Add(this.ConnectEyeTrackerButton);
            this.Controls.Add(this.PipeStatusLabel);
            this.Controls.Add(this.ConnectButton);
            this.Name = "Form1";
            this.Text = "Eyetracker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label PipeStatusLabel;
        private System.Windows.Forms.Button ConnectEyeTrackerButton;
        private System.Windows.Forms.Label EyeTrackerStatusLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label GazePointLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SleepStatusLabel;
        private System.Windows.Forms.ComboBox AlarmStartComboBox;
        private System.Windows.Forms.ComboBox AlarmEndComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Do;
        private System.Windows.Forms.Label label5;
    }
}

