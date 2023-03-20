namespace GameLab.Eyetracking.EyetrackerControls
{
    partial class EyetrackingSettingsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EyetrackingSettingsForm));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCalibrate = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lbRightEyeCalibrationAccuracy = new System.Windows.Forms.Label();
            this.lbLeftEyeCalibrationAccuracy = new System.Windows.Forms.Label();
            this.lbAccuracy = new System.Windows.Forms.Label();
            this.btnAnalysisSettings = new System.Windows.Forms.Button();
            this.cbSmoothingType = new System.Windows.Forms.ComboBox();
            this.nudSmoothingRange = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudDwellTime = new System.Windows.Forms.NumericUpDown();
            this.gbImitationEyetrackerSettingsPanel = new System.Windows.Forms.GroupBox();
            this.cbImitationAveragedEyeEnabled = new System.Windows.Forms.CheckBox();
            this.cbImitationRightEyeEnabled = new System.Windows.Forms.CheckBox();
            this.cbImitationLeftEyeEnabled = new System.Windows.Forms.CheckBox();
            this.cbImitationAddNoise = new System.Windows.Forms.CheckBox();
            this.cbImitationUpdateOnlyWhenMouseStateChanges = new System.Windows.Forms.CheckBox();
            this.gbSmoothingFilterPanel = new System.Windows.Forms.GroupBox();
            this.gbDwellTimeControlsManagerSettingsPanel = new System.Windows.Forms.GroupBox();
            this.nudActivationTime = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOffsetCorrection = new System.Windows.Forms.Button();
            this.eyetrackerCalibrationSettingsPanel = new GameLab.Eyetracking.EyetrackerControls.EyetrackerCalibrationSettingsPanel();
            this.eyetrackerConnectionSettingsPanel = new GameLab.Eyetracking.EyetrackerControls.EyetrackerConnectionSettingsPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.cbMonitor = new System.Windows.Forms.ComboBox();
            this.timerOpóźnieniaWykrywaniaMonitorów = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.nudAnalysisInterval = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnNotes = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudSmoothingRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDwellTime)).BeginInit();
            this.gbImitationEyetrackerSettingsPanel.SuspendLayout();
            this.gbSmoothingFilterPanel.SuspendLayout();
            this.gbDwellTimeControlsManagerSettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudActivationTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnalysisInterval)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 181);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(138, 30);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Enabled = false;
            this.btnCalibrate.Location = new System.Drawing.Point(12, 636);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(120, 30);
            this.btnCalibrate.TabIndex = 3;
            this.btnCalibrate.Text = "Calibrate";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Enabled = false;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRun.Location = new System.Drawing.Point(408, 686);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(316, 30);
            this.btnRun.TabIndex = 4;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Enabled = false;
            this.btnValidate.Location = new System.Drawing.Point(138, 636);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(120, 30);
            this.btnValidate.TabIndex = 5;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(156, 181);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(138, 30);
            this.btnDisconnect.TabIndex = 6;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // lbRightEyeCalibrationAccuracy
            // 
            this.lbRightEyeCalibrationAccuracy.AutoSize = true;
            this.lbRightEyeCalibrationAccuracy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbRightEyeCalibrationAccuracy.Location = new System.Drawing.Point(91, 696);
            this.lbRightEyeCalibrationAccuracy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRightEyeCalibrationAccuracy.Name = "lbRightEyeCalibrationAccuracy";
            this.lbRightEyeCalibrationAccuracy.Size = new System.Drawing.Size(23, 17);
            this.lbRightEyeCalibrationAccuracy.TabIndex = 26;
            this.lbRightEyeCalibrationAccuracy.Text = "---";
            // 
            // lbLeftEyeCalibrationAccuracy
            // 
            this.lbLeftEyeCalibrationAccuracy.AutoSize = true;
            this.lbLeftEyeCalibrationAccuracy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbLeftEyeCalibrationAccuracy.Location = new System.Drawing.Point(91, 680);
            this.lbLeftEyeCalibrationAccuracy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLeftEyeCalibrationAccuracy.Name = "lbLeftEyeCalibrationAccuracy";
            this.lbLeftEyeCalibrationAccuracy.Size = new System.Drawing.Size(23, 17);
            this.lbLeftEyeCalibrationAccuracy.TabIndex = 25;
            this.lbLeftEyeCalibrationAccuracy.Text = "---";
            // 
            // lbAccuracy
            // 
            this.lbAccuracy.AutoSize = true;
            this.lbAccuracy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbAccuracy.Location = new System.Drawing.Point(13, 680);
            this.lbAccuracy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAccuracy.Name = "lbAccuracy";
            this.lbAccuracy.Size = new System.Drawing.Size(70, 17);
            this.lbAccuracy.TabIndex = 24;
            this.lbAccuracy.Text = "Accuracy:";
            // 
            // btnAnalysisSettings
            // 
            this.btnAnalysisSettings.Location = new System.Drawing.Point(409, 397);
            this.btnAnalysisSettings.Name = "btnAnalysisSettings";
            this.btnAnalysisSettings.Size = new System.Drawing.Size(185, 30);
            this.btnAnalysisSettings.TabIndex = 28;
            this.btnAnalysisSettings.Text = "Events analysis settings...";
            this.btnAnalysisSettings.UseVisualStyleBackColor = true;
            this.btnAnalysisSettings.Click += new System.EventHandler(this.btnAnalysisSettings_Click);
            // 
            // cbSmoothingType
            // 
            this.cbSmoothingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSmoothingType.FormattingEnabled = true;
            this.cbSmoothingType.Items.AddRange(new object[] {
            "None",
            "Simple Moving Average (SMA)",
            "Weighted Moving Average (WMA)",
            "Exponential Moving Average (EMA)"});
            this.cbSmoothingType.Location = new System.Drawing.Point(14, 47);
            this.cbSmoothingType.Name = "cbSmoothingType";
            this.cbSmoothingType.Size = new System.Drawing.Size(286, 24);
            this.cbSmoothingType.TabIndex = 29;
            // 
            // nudSmoothingRange
            // 
            this.nudSmoothingRange.Location = new System.Drawing.Point(14, 110);
            this.nudSmoothingRange.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSmoothingRange.Name = "nudSmoothingRange";
            this.nudSmoothingRange.Size = new System.Drawing.Size(126, 22);
            this.nudSmoothingRange.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 31;
            this.label1.Text = "Smoothing type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 17);
            this.label2.TabIndex = 32;
            this.label2.Text = "Range (samples):";
            // 
            // nudDwellTime
            // 
            this.nudDwellTime.Location = new System.Drawing.Point(13, 54);
            this.nudDwellTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudDwellTime.Name = "nudDwellTime";
            this.nudDwellTime.Size = new System.Drawing.Size(98, 22);
            this.nudDwellTime.TabIndex = 34;
            this.nudDwellTime.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // gbImitationEyetrackerSettingsPanel
            // 
            this.gbImitationEyetrackerSettingsPanel.Controls.Add(this.cbImitationAveragedEyeEnabled);
            this.gbImitationEyetrackerSettingsPanel.Controls.Add(this.cbImitationRightEyeEnabled);
            this.gbImitationEyetrackerSettingsPanel.Controls.Add(this.cbImitationLeftEyeEnabled);
            this.gbImitationEyetrackerSettingsPanel.Controls.Add(this.cbImitationAddNoise);
            this.gbImitationEyetrackerSettingsPanel.Controls.Add(this.cbImitationUpdateOnlyWhenMouseStateChanges);
            this.gbImitationEyetrackerSettingsPanel.Location = new System.Drawing.Point(408, 13);
            this.gbImitationEyetrackerSettingsPanel.Name = "gbImitationEyetrackerSettingsPanel";
            this.gbImitationEyetrackerSettingsPanel.Size = new System.Drawing.Size(316, 92);
            this.gbImitationEyetrackerSettingsPanel.TabIndex = 35;
            this.gbImitationEyetrackerSettingsPanel.TabStop = false;
            this.gbImitationEyetrackerSettingsPanel.Text = "Mouse-base eyetracker imitation settings";
            // 
            // cbImitationAveragedEyeEnabled
            // 
            this.cbImitationAveragedEyeEnabled.AutoSize = true;
            this.cbImitationAveragedEyeEnabled.Checked = true;
            this.cbImitationAveragedEyeEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbImitationAveragedEyeEnabled.Location = new System.Drawing.Point(211, 59);
            this.cbImitationAveragedEyeEnabled.Name = "cbImitationAveragedEyeEnabled";
            this.cbImitationAveragedEyeEnabled.Size = new System.Drawing.Size(39, 21);
            this.cbImitationAveragedEyeEnabled.TabIndex = 4;
            this.cbImitationAveragedEyeEnabled.Text = "A";
            this.cbImitationAveragedEyeEnabled.UseVisualStyleBackColor = true;
            // 
            // cbImitationRightEyeEnabled
            // 
            this.cbImitationRightEyeEnabled.AutoSize = true;
            this.cbImitationRightEyeEnabled.Checked = true;
            this.cbImitationRightEyeEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbImitationRightEyeEnabled.Location = new System.Drawing.Point(256, 59);
            this.cbImitationRightEyeEnabled.Name = "cbImitationRightEyeEnabled";
            this.cbImitationRightEyeEnabled.Size = new System.Drawing.Size(40, 21);
            this.cbImitationRightEyeEnabled.TabIndex = 3;
            this.cbImitationRightEyeEnabled.Text = "R";
            this.cbImitationRightEyeEnabled.UseVisualStyleBackColor = true;
            // 
            // cbImitationLeftEyeEnabled
            // 
            this.cbImitationLeftEyeEnabled.AutoSize = true;
            this.cbImitationLeftEyeEnabled.Checked = true;
            this.cbImitationLeftEyeEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbImitationLeftEyeEnabled.Location = new System.Drawing.Point(167, 59);
            this.cbImitationLeftEyeEnabled.Name = "cbImitationLeftEyeEnabled";
            this.cbImitationLeftEyeEnabled.Size = new System.Drawing.Size(38, 21);
            this.cbImitationLeftEyeEnabled.TabIndex = 2;
            this.cbImitationLeftEyeEnabled.Text = "L";
            this.cbImitationLeftEyeEnabled.UseVisualStyleBackColor = true;
            // 
            // cbImitationAddNoise
            // 
            this.cbImitationAddNoise.AutoSize = true;
            this.cbImitationAddNoise.Location = new System.Drawing.Point(18, 59);
            this.cbImitationAddNoise.Name = "cbImitationAddNoise";
            this.cbImitationAddNoise.Size = new System.Drawing.Size(93, 21);
            this.cbImitationAddNoise.TabIndex = 1;
            this.cbImitationAddNoise.Text = "Add noise";
            this.cbImitationAddNoise.UseVisualStyleBackColor = true;
            // 
            // cbImitationUpdateOnlyWhenMouseStateChanges
            // 
            this.cbImitationUpdateOnlyWhenMouseStateChanges.AutoSize = true;
            this.cbImitationUpdateOnlyWhenMouseStateChanges.Location = new System.Drawing.Point(18, 32);
            this.cbImitationUpdateOnlyWhenMouseStateChanges.Name = "cbImitationUpdateOnlyWhenMouseStateChanges";
            this.cbImitationUpdateOnlyWhenMouseStateChanges.Size = new System.Drawing.Size(282, 21);
            this.cbImitationUpdateOnlyWhenMouseStateChanges.TabIndex = 0;
            this.cbImitationUpdateOnlyWhenMouseStateChanges.Text = "Update only when mouse state changes";
            this.cbImitationUpdateOnlyWhenMouseStateChanges.UseVisualStyleBackColor = true;
            // 
            // gbSmoothingFilterPanel
            // 
            this.gbSmoothingFilterPanel.Controls.Add(this.cbSmoothingType);
            this.gbSmoothingFilterPanel.Controls.Add(this.nudSmoothingRange);
            this.gbSmoothingFilterPanel.Controls.Add(this.label1);
            this.gbSmoothingFilterPanel.Controls.Add(this.label2);
            this.gbSmoothingFilterPanel.Location = new System.Drawing.Point(409, 111);
            this.gbSmoothingFilterPanel.Name = "gbSmoothingFilterPanel";
            this.gbSmoothingFilterPanel.Size = new System.Drawing.Size(316, 154);
            this.gbSmoothingFilterPanel.TabIndex = 36;
            this.gbSmoothingFilterPanel.TabStop = false;
            this.gbSmoothingFilterPanel.Text = "Smoothing settings";
            // 
            // gbDwellTimeControlsManagerSettingsPanel
            // 
            this.gbDwellTimeControlsManagerSettingsPanel.Controls.Add(this.nudActivationTime);
            this.gbDwellTimeControlsManagerSettingsPanel.Controls.Add(this.label6);
            this.gbDwellTimeControlsManagerSettingsPanel.Controls.Add(this.label3);
            this.gbDwellTimeControlsManagerSettingsPanel.Controls.Add(this.nudDwellTime);
            this.gbDwellTimeControlsManagerSettingsPanel.Location = new System.Drawing.Point(409, 280);
            this.gbDwellTimeControlsManagerSettingsPanel.Name = "gbDwellTimeControlsManagerSettingsPanel";
            this.gbDwellTimeControlsManagerSettingsPanel.Size = new System.Drawing.Size(316, 98);
            this.gbDwellTimeControlsManagerSettingsPanel.TabIndex = 37;
            this.gbDwellTimeControlsManagerSettingsPanel.TabStop = false;
            this.gbDwellTimeControlsManagerSettingsPanel.Text = "Dwell-time controls manager settings";
            // 
            // nudActivationTime
            // 
            this.nudActivationTime.Location = new System.Drawing.Point(144, 54);
            this.nudActivationTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudActivationTime.Name = "nudActivationTime";
            this.nudActivationTime.Size = new System.Drawing.Size(98, 22);
            this.nudActivationTime.TabIndex = 37;
            this.nudActivationTime.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(141, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 17);
            this.label6.TabIndex = 36;
            this.label6.Text = "Activation time (ms):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 17);
            this.label3.TabIndex = 35;
            this.label3.Text = "Dwell-time (ms)";
            // 
            // btnOffsetCorrection
            // 
            this.btnOffsetCorrection.Enabled = false;
            this.btnOffsetCorrection.Location = new System.Drawing.Point(264, 636);
            this.btnOffsetCorrection.Name = "btnOffsetCorrection";
            this.btnOffsetCorrection.Size = new System.Drawing.Size(134, 30);
            this.btnOffsetCorrection.TabIndex = 40;
            this.btnOffsetCorrection.Text = "Offset correction";
            this.btnOffsetCorrection.UseVisualStyleBackColor = true;
            this.btnOffsetCorrection.Click += new System.EventHandler(this.btnOffsetCorrection_Click);
            // 
            // eyetrackerCalibrationSettingsPanel
            // 
            this.eyetrackerCalibrationSettingsPanel.CustomCalibrationScreenOptionEnabled = true;
            this.eyetrackerCalibrationSettingsPanel.CustomCalibrationSettings = null;
            this.eyetrackerCalibrationSettingsPanel.Enabled = false;
            this.eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.AcceptationRequiredForEveryCalibrationPoint = false;
            this.eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.BackgroundBrightness = 255;
            this.eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.DisplayDeviceIndex = 0;
            this.eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.ImageFilePath = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\kalibr" +
    "acja.png";
            this.eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.ImageSize = 150;
            this.eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.IncreasedSpeed = true;
            this.eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.NumberOfCalibrationPoints = 5;
            this.eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.UseCustomCalibrationScreen = true;
            this.eyetrackerCalibrationSettingsPanel.Location = new System.Drawing.Point(12, 229);
            this.eyetrackerCalibrationSettingsPanel.Name = "eyetrackerCalibrationSettingsPanel";
            this.eyetrackerCalibrationSettingsPanel.Size = new System.Drawing.Size(386, 404);
            this.eyetrackerCalibrationSettingsPanel.TabIndex = 2;
            this.eyetrackerCalibrationSettingsPanel.UseCustomCalibrationScreen = true;
            // 
            // eyetrackerConnectionSettingsPanel
            // 
            this.eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings.ClientIp = "127.0.0.1";
            this.eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings.ClientPort = 0;
            this.eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings.ServerIp = "127.0.0.1";
            this.eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings.ServerPort = 0;
            this.eyetrackerConnectionSettingsPanel.EyetrackerType = GameLab.Eyetracking.EyetrackerType.ImitationEyetracker;
            this.eyetrackerConnectionSettingsPanel.Location = new System.Drawing.Point(12, 13);
            this.eyetrackerConnectionSettingsPanel.Name = "eyetrackerConnectionSettingsPanel";
            this.eyetrackerConnectionSettingsPanel.Size = new System.Drawing.Size(390, 162);
            this.eyetrackerConnectionSettingsPanel.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(406, 448);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 17);
            this.label5.TabIndex = 42;
            this.label5.Text = "Monitor:";
            // 
            // cbMonitor
            // 
            this.cbMonitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMonitor.FormattingEnabled = true;
            this.cbMonitor.Location = new System.Drawing.Point(409, 469);
            this.cbMonitor.Margin = new System.Windows.Forms.Padding(4);
            this.cbMonitor.Name = "cbMonitor";
            this.cbMonitor.Size = new System.Drawing.Size(316, 24);
            this.cbMonitor.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(610, 381);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 17);
            this.label4.TabIndex = 44;
            this.label4.Text = "Analysis interval:";
            // 
            // nudAnalysisInterval
            // 
            this.nudAnalysisInterval.Location = new System.Drawing.Point(613, 402);
            this.nudAnalysisInterval.Name = "nudAnalysisInterval";
            this.nudAnalysisInterval.Size = new System.Drawing.Size(108, 22);
            this.nudAnalysisInterval.TabIndex = 45;
            this.nudAnalysisInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Yellow;
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(409, 500);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 133);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gaze data replay eyetracker settings";
            this.groupBox1.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 17);
            this.label9.TabIndex = 3;
            this.label9.Text = "Time: --- - ---";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 17);
            this.label8.TabIndex = 2;
            this.label8.Text = "Number of samples: ---";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "File path: ---";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnNotes
            // 
            this.btnNotes.Location = new System.Drawing.Point(313, 181);
            this.btnNotes.Name = "btnNotes";
            this.btnNotes.Size = new System.Drawing.Size(85, 30);
            this.btnNotes.TabIndex = 47;
            this.btnNotes.Text = "Notes";
            this.btnNotes.UseVisualStyleBackColor = true;
            this.btnNotes.Click += new System.EventHandler(this.btnNotes_Click);
            // 
            // EyetrackingSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 728);
            this.Controls.Add(this.btnNotes);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nudAnalysisInterval);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbMonitor);
            this.Controls.Add(this.btnOffsetCorrection);
            this.Controls.Add(this.gbDwellTimeControlsManagerSettingsPanel);
            this.Controls.Add(this.gbSmoothingFilterPanel);
            this.Controls.Add(this.gbImitationEyetrackerSettingsPanel);
            this.Controls.Add(this.btnAnalysisSettings);
            this.Controls.Add(this.lbRightEyeCalibrationAccuracy);
            this.Controls.Add(this.lbLeftEyeCalibrationAccuracy);
            this.Controls.Add(this.lbAccuracy);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnCalibrate);
            this.Controls.Add(this.eyetrackerCalibrationSettingsPanel);
            this.Controls.Add(this.eyetrackerConnectionSettingsPanel);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EyetrackingSettingsForm";
            this.Text = "Eyetracker settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EyetrackingSettingsForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudSmoothingRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDwellTime)).EndInit();
            this.gbImitationEyetrackerSettingsPanel.ResumeLayout(false);
            this.gbImitationEyetrackerSettingsPanel.PerformLayout();
            this.gbSmoothingFilterPanel.ResumeLayout(false);
            this.gbSmoothingFilterPanel.PerformLayout();
            this.gbDwellTimeControlsManagerSettingsPanel.ResumeLayout(false);
            this.gbDwellTimeControlsManagerSettingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudActivationTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnalysisInterval)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private GameLab.Eyetracking.EyetrackerControls.EyetrackerCalibrationSettingsPanel eyetrackerCalibrationSettingsPanel;
        private System.Windows.Forms.Button btnCalibrate;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lbAccuracy;
        private System.Windows.Forms.Button btnAnalysisSettings;
        private System.Windows.Forms.ComboBox cbSmoothingType;
        private System.Windows.Forms.NumericUpDown nudSmoothingRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudDwellTime;
        private System.Windows.Forms.GroupBox gbImitationEyetrackerSettingsPanel;
        private System.Windows.Forms.CheckBox cbImitationUpdateOnlyWhenMouseStateChanges;
        private System.Windows.Forms.CheckBox cbImitationAddNoise;
        private System.Windows.Forms.GroupBox gbSmoothingFilterPanel;
        private System.Windows.Forms.GroupBox gbDwellTimeControlsManagerSettingsPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOffsetCorrection;
        protected System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer timerOpóźnieniaWykrywaniaMonitorów;
        private System.Windows.Forms.Label label4;
        protected System.Windows.Forms.Label lbRightEyeCalibrationAccuracy;
        protected System.Windows.Forms.Label lbLeftEyeCalibrationAccuracy;
        private EyetrackerConnectionSettingsPanel eyetrackerConnectionSettingsPanel;
        protected System.Windows.Forms.ComboBox cbMonitor;
        protected System.Windows.Forms.NumericUpDown nudAnalysisInterval;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudActivationTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnNotes;
        private System.Windows.Forms.CheckBox cbImitationLeftEyeEnabled;
        private System.Windows.Forms.CheckBox cbImitationAveragedEyeEnabled;
        private System.Windows.Forms.CheckBox cbImitationRightEyeEnabled;
    }
}