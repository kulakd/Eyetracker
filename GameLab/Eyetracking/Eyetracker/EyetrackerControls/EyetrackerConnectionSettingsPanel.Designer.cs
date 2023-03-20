namespace GameLab.Eyetracking.EyetrackerControls
{
    partial class EyetrackerConnectionSettingsPanel
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
            this.label44 = new System.Windows.Forms.Label();
            this.cbEyetrackerType = new System.Windows.Forms.ComboBox();
            this.gbEyetrackerConnection = new System.Windows.Forms.GroupBox();
            this.tbSendIp = new System.Windows.Forms.TextBox();
            this.cbReveiveIp = new System.Windows.Forms.ComboBox();
            this.tbReceivePort = new System.Windows.Forms.TextBox();
            this.tbSendPort = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.Label();
            this.receive = new System.Windows.Forms.Label();
            this.gbEyetrackerConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label44.Location = new System.Drawing.Point(1, 7);
            this.label44.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(111, 17);
            this.label44.TabIndex = 0;
            this.label44.Text = "Eyetracker type:";
            // 
            // cbEyetrackerType
            // 
            this.cbEyetrackerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEyetrackerType.FormattingEnabled = true;
            this.cbEyetrackerType.Items.AddRange(new object[] {
            "Eyetracker imitation using mouse",
            "SMI Eyetrackers",
            "Open Eye-Gaze Interface (Mirametrix, GazePoint)",
            "The Eye Tribe",
            "Tobii Gaming Eyetrackers (EyeX, 4C)",
            "Gaze Data Replay Eyetracker",
            "Zero Eyetracker (dummy)"});
            this.cbEyetrackerType.Location = new System.Drawing.Point(4, 27);
            this.cbEyetrackerType.Margin = new System.Windows.Forms.Padding(4);
            this.cbEyetrackerType.Name = "cbEyetrackerType";
            this.cbEyetrackerType.Size = new System.Drawing.Size(379, 24);
            this.cbEyetrackerType.TabIndex = 1;
            this.cbEyetrackerType.SelectedIndexChanged += new System.EventHandler(this.cbEyetracker_SelectedIndexChanged);
            // 
            // gbEyetrackerConnection
            // 
            this.gbEyetrackerConnection.Controls.Add(this.tbSendIp);
            this.gbEyetrackerConnection.Controls.Add(this.cbReveiveIp);
            this.gbEyetrackerConnection.Controls.Add(this.tbReceivePort);
            this.gbEyetrackerConnection.Controls.Add(this.tbSendPort);
            this.gbEyetrackerConnection.Controls.Add(this.send);
            this.gbEyetrackerConnection.Controls.Add(this.receive);
            this.gbEyetrackerConnection.Location = new System.Drawing.Point(4, 64);
            this.gbEyetrackerConnection.Margin = new System.Windows.Forms.Padding(4);
            this.gbEyetrackerConnection.Name = "gbEyetrackerConnection";
            this.gbEyetrackerConnection.Padding = new System.Windows.Forms.Padding(4);
            this.gbEyetrackerConnection.Size = new System.Drawing.Size(380, 91);
            this.gbEyetrackerConnection.TabIndex = 2;
            this.gbEyetrackerConnection.TabStop = false;
            this.gbEyetrackerConnection.Text = "IP adresses and port numbers";
            // 
            // tbSendIp
            // 
            this.tbSendIp.Location = new System.Drawing.Point(173, 55);
            this.tbSendIp.Margin = new System.Windows.Forms.Padding(4);
            this.tbSendIp.Name = "tbSendIp";
            this.tbSendIp.Size = new System.Drawing.Size(132, 22);
            this.tbSendIp.TabIndex = 7;
            this.tbSendIp.Text = "127.0.0.1";
            // 
            // cbReveiveIp
            // 
            this.cbReveiveIp.FormattingEnabled = true;
            this.cbReveiveIp.Location = new System.Drawing.Point(173, 23);
            this.cbReveiveIp.Margin = new System.Windows.Forms.Padding(4);
            this.cbReveiveIp.Name = "cbReveiveIp";
            this.cbReveiveIp.Size = new System.Drawing.Size(132, 24);
            this.cbReveiveIp.TabIndex = 4;
            this.cbReveiveIp.Text = "127.0.0.1";
            // 
            // tbReceivePort
            // 
            this.tbReceivePort.Location = new System.Drawing.Point(314, 25);
            this.tbReceivePort.Margin = new System.Windows.Forms.Padding(4);
            this.tbReceivePort.Name = "tbReceivePort";
            this.tbReceivePort.Size = new System.Drawing.Size(53, 22);
            this.tbReceivePort.TabIndex = 5;
            this.tbReceivePort.Text = "0";
            // 
            // tbSendPort
            // 
            this.tbSendPort.Location = new System.Drawing.Point(314, 55);
            this.tbSendPort.Margin = new System.Windows.Forms.Padding(4);
            this.tbSendPort.Name = "tbSendPort";
            this.tbSendPort.Size = new System.Drawing.Size(53, 22);
            this.tbSendPort.TabIndex = 8;
            this.tbSendPort.Text = "0";
            // 
            // send
            // 
            this.send.AutoSize = true;
            this.send.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.send.Location = new System.Drawing.Point(13, 59);
            this.send.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(156, 17);
            this.send.TabIndex = 6;
            this.send.Text = "Remote IP addr. && port:";
            // 
            // receive
            // 
            this.receive.AutoSize = true;
            this.receive.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.receive.Location = new System.Drawing.Point(13, 27);
            this.receive.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.receive.Name = "receive";
            this.receive.Size = new System.Drawing.Size(141, 17);
            this.receive.TabIndex = 3;
            this.receive.Text = "Local IP addr. && port:";
            // 
            // EyetrackerConnectionSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label44);
            this.Controls.Add(this.cbEyetrackerType);
            this.Controls.Add(this.gbEyetrackerConnection);
            this.Name = "EyetrackerConnectionSettingsPanel";
            this.Size = new System.Drawing.Size(390, 162);
            this.gbEyetrackerConnection.ResumeLayout(false);
            this.gbEyetrackerConnection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.ComboBox cbEyetrackerType;
        private System.Windows.Forms.GroupBox gbEyetrackerConnection;
        private System.Windows.Forms.TextBox tbSendIp;
        private System.Windows.Forms.ComboBox cbReveiveIp;
        private System.Windows.Forms.TextBox tbReceivePort;
        private System.Windows.Forms.TextBox tbSendPort;
        private System.Windows.Forms.Label send;
        private System.Windows.Forms.Label receive;
    }
}
