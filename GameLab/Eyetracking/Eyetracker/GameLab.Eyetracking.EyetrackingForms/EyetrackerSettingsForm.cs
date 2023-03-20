using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace GameLab.Eyetracking.EyetrackerControls
{
    using GameLab.Eyetracking;
    using GameLab.Eyetracking.GazeRuntimeAnalysis;
    using JacekMatulewski.Settings;

    //nie może być abstract, bo designer sobie z tym nie radzi
    public partial class EyetrackingSettingsForm : Form
    {
        private string profileName;

        public bool ShowSuccessfulConnectionMessage { get; set; } = true;

        public EyetrackingSettingsForm() //dla designera
            : this(null)
        { }

        public EyetrackingSettingsForm(string profileName)
        {
            this.profileName = profileName;

            InitializeComponent();

            cbSmoothingType.SelectedIndex = 1;
            nudSmoothingRange.Value = 10;

            if (!DesignMode) loadSettings(profileName);

            WykryjMonitory();
        }

        protected IEyetracker et = null;
        protected GazeEventsRuntimeAnalysisSettings analysisSettings = GazeEventsRuntimeAnalysisSettings.Default;
        protected TimeSpan dwellTime
        {
            get
            {
                return TimeSpan.FromMilliseconds((double)nudDwellTime.Value);
            }
            set
            {
                nudDwellTime.Value = (decimal)value.TotalMilliseconds;
            }
        }
        protected TimeSpan activationTime
        {
            get
            {
                return TimeSpan.FromMilliseconds((double)nudActivationTime.Value);
            }
            set
            {
                nudActivationTime.Value = (decimal)value.TotalMilliseconds;
            }
        }
        protected double analysisTimerIntervalMiliseconds
        {
            get
            {
                return (double)nudAnalysisInterval.Value;
            }
            set
            {
                nudAnalysisInterval.Value = (decimal)value;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            switch (eyetrackerConnectionSettingsPanel.EyetrackerType)
            {
                default:
                case EyetrackerType.ImitationEyetracker:
                    et = new ImitationEyetracker(cbImitationUpdateOnlyWhenMouseStateChanges.Checked, cbImitationAddNoise.Checked, cbImitationLeftEyeEnabled.Checked, cbImitationRightEyeEnabled.Checked, cbImitationAveragedEyeEnabled.Checked);
                    break;
                case EyetrackerType.SMIEyetracker:
                    et = new SMIEyetracker();
                    break;
                case EyetrackerType.MirametrixEyetracker:
                    Screen screen = Screen.FromControl(this);
                    et = new OpenEyeGazeEyetracker(screen.Bounds.Width, screen.Bounds.Height, true, true, true, 10, 100);
                    break;
                case EyetrackerType.TheEyeTribeEyetracker:
                    et = new TheEyeTribeEyetracker();
                    break;
                case EyetrackerType.TobiiEyetracker:
                    //et = new TobiiEyetracker(true);
                    et = new TobiiEyeXEyetracker(true);
                    break;
                case EyetrackerType.GazeDataReplay:
                    //throw new NotImplementedException();
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "CSV file with gaze positions (exported from GDE)|*.csv|Any file|*.csv";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        GazeDataReplaySample[] samples = GazeDataReplayEyetracker.LoadGazeCoordinatesFromCsvFile(ofd.FileName, ';');
                        et = new GazeDataReplayEyetracker(samples);
                    }
                    else
                    {
                        et = null;
                    }
                    break;
                case EyetrackerType.ZeroEyetracker:
                    et = new ZeroEyetracker();
                    break;
            }

            string message = "";
            if (et.Connect(eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings, ref message))
            {
                onEyetrackerConnected();
                if(ShowSuccessfulConnectionMessage) MessageBox.Show("Connection to eyetracker established", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Connection to eyetracker server failed:\n" + message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (et is ICalibratable)
            {
                eyetrackerCalibrationSettingsPanel.Enabled = true;
                btnCalibrate.Enabled = true;
            }
            btnOffsetCorrection.Enabled = true;
            btnRun.Enabled = true;
            gbImitationEyetrackerSettingsPanel.Enabled = false;
        }        

        private EyetrackerCalibrationSettings initCustomCalibrationForm()
        {
            EyetrackerCalibrationSettings calibrationSettings = eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings;    
            try
            {
                calibrationSettings.InitCustomCalibrationForm(eyetrackerCalibrationSettingsPanel.CustomCalibrationSettings);
                if (calibrationSettings.CustomCalibrationScreen != null)
                {
                    //TODO: animacja rysunku podczas kalibracji
                    //rozwiązaniem na brak animacji zmniejszania rysunków jest puszczenie całego okna w osobnym wątku 
                    //animacja samego rysunku nic nie da, bo wątek i tak stoi
                    calibrationSettings.CustomCalibrationScreen.Show();
                    //(calibrationSettings.CustomCalibrationScreen as Form).ShowDialog(); //to rozwiązuje część problemów
                    if (calibrationSettings.CustomCalibrationScreen is Form)
                    {
                        Form customCalibrationForm = calibrationSettings.CustomCalibrationScreen as Form;
                        Screen screen = Screen.AllScreens[eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.DisplayDeviceIndex];
                        customCalibrationForm.StartPosition = FormStartPosition.Manual;
                        customCalibrationForm.WindowState = FormWindowState.Normal;
                        customCalibrationForm.Left = screen.Bounds.Left;
                        customCalibrationForm.Top = screen.Bounds.Top;
                        customCalibrationForm.Width = screen.Bounds.Width;
                        customCalibrationForm.Height = screen.Bounds.Height;
                        customCalibrationForm.WindowState = FormWindowState.Maximized;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error while initiating custom calibration screen: " + exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return calibrationSettings;
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            if (et is ICalibratable)
            {
                EyetrackerCalibrationSettings calibrationSettings = initCustomCalibrationForm();                

                ICalibratable etc = et as ICalibratable;
                string message = "";
                if (etc.Calibrate(calibrationSettings, ref message))
                {
                    MessageBox.Show("Eyetracker calibration succeded", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (etc.IsValidationPossible) btnValidate.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Eyetracker calibration failed:\n" + message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (calibrationSettings.CustomCalibrationScreen != null) calibrationSettings.CustomCalibrationScreen.Close();
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (et is ICalibratable)
            {
                EyetrackerCalibrationSettings calibrationSettings = initCustomCalibrationForm();                

                ICalibratable etc = et as ICalibratable;
                string message = "";
                if (etc.Validate(calibrationSettings, ref message))
                {
                    MessageBox.Show("Eyetracker validation succeded", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string leftEyeDescription = "", rightEyeDescription = "";
                    etc.GetAccuracyDescription(false, ref leftEyeDescription, ref rightEyeDescription);
                    lbLeftEyeCalibrationAccuracy.Text = leftEyeDescription;
                    lbRightEyeCalibrationAccuracy.Text = rightEyeDescription;
                }
                else
                {
                    MessageBox.Show("Eyetracker validation failed:\n" + message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (calibrationSettings.CustomCalibrationScreen != null) calibrationSettings.CustomCalibrationScreen.Close();
            }
        }

        #region Monitory
        private void WykryjMonitory()
        {
            //karta eksperymentu
            IEnumerable<string> nazwyEkranów = from Screen ekran in Screen.AllScreens
                                               select ekran.DeviceName + " " + ekran.WorkingArea.ToString();

            cbMonitor.Items.Clear();
            cbMonitor.Items.AddRange(nazwyEkranów.ToArray());
            cbMonitor.SelectedIndex = 0;

            //wskaż monitor, na którym jest to okno
            for (int i = 0; i < Screen.AllScreens.Length; ++i)
                if (Screen.FromControl(this).Equals(Screen.AllScreens[i]))
                {
                    cbMonitor.SelectedIndex = i;
                    break;
                }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DISPLAYCHANGE = 0x007e;

            switch (m.Msg)
            {
                case WM_DISPLAYCHANGE: //zmiany dopiero nastąpią - dlatego ten wybieg z timerem
                    timerOpóźnieniaWykrywaniaMonitorów.Enabled = true;
                    //WykryjMonitory();
                    break;
            }

            base.WndProc(ref m);
        }

        private void timerOpóźnieniaWykrywaniaMonitorów_Tick(object sender, EventArgs e)
        {
            //this.Text += "+";
            timerOpóźnieniaWykrywaniaMonitorów.Enabled = false;
            WykryjMonitory();
        }
        #endregion

        //nie może być abstract, bo designer sobie z tym nie radzi
        protected virtual void run(Screen screen, GazeSmoothingFilter filter, IGazeRuntimeAnalyser analyser)
        { }

        protected GazeSmoothingFilter getSmoothingFilter()
        {
            return new GazeSmoothingFilter((SmoothingType)cbSmoothingType.SelectedIndex, (int)nudSmoothingRange.Value);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (et == null || !et.Connected)
            {
                MessageBox.Show("Eyetracker is not connected", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                GazeSmoothingFilter filter = getSmoothingFilter();
                IGazeRuntimeAnalyser analyser = new GazeRuntimeAnalyser(et, analysisSettings, GazeRuntimeAnalyser.EventRaisingScheme.UseTimer, (double)nudAnalysisInterval.Value);
                run(Screen.AllScreens[cbMonitor.SelectedIndex], filter, analyser);
            }
            //pokazywanie na wybranym ekranie - tym zajmuje się klasa potomna
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (et == null) return;

            string message = "";
            if (et.Disconnect(ref message))
            {
                eyetrackerCalibrationSettingsPanel.Enabled = false;
                btnCalibrate.Enabled = false;
                btnRun.Enabled = false;
                btnOffsetCorrection.Enabled = false;
                gbImitationEyetrackerSettingsPanel.Enabled = true;

                onEyetrackerDisconnected();
            }
            else
            {
                MessageBox.Show("Eyetracker disconection failed:\n" + message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnDisconnect_Click(null, EventArgs.Empty);
            saveSettings(profileName);
        }

        private void btnAnalysisSettings_Click(object sender, EventArgs e)
        {
            GazeRuntimeAnalysisSettingsForm form = new GazeRuntimeAnalysisSettingsForm(analysisSettings);
            if (form.ShowDialog() == DialogResult.OK)
                analysisSettings = form.Settings;
        }

        private void btnOffsetCorrection_Click(object sender, EventArgs e)
        {
            new OffsetCorrectionSettingsForm(et).ShowDialog();
            MessageBox.Show("Offset correction for left eye: " + et.LeftEyeOffset.ToString() + ", for right eye: " + et.RightEyeOffset.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region Storing settings
        private const StoringMethod storingMethod = StoringMethod.XmlFile;

        public static string GetSettingsDirectory(string profileName)
        {
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (profileName != null) exeDirectory = Path.Combine(exeDirectory, profileName);
            string folderPath = Path.Combine(exeDirectory, "Settings");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        private static string getSettingsFilePath(string settingsName, string profileName)
        {
            return Path.Combine(GetSettingsDirectory(profileName), settingsName + ".cfg");
        }

        private static List<ISettingsValueConverter> getMemberConverters()
        {
            List<ISettingsValueConverter> memberConvertes = new List<ISettingsValueConverter>();
            memberConvertes.Add(new GazeSmoothingFilterValueConverter());
            memberConvertes.Add(new ColorValueConverter());
            return memberConvertes;
        }

        protected void loadSettings(string profileName)
        {
            bool debug = false;

            //connection settings
            EyetrackerConnectionSettings2 eyetrackerConnectionSettings2;
            try
            {
                SettingsManager<EyetrackerConnectionSettings2> sm = SettingsManager<EyetrackerConnectionSettings2>.Load(storingMethod, getSettingsFilePath("EyetrackerConnectionSettings", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                eyetrackerConnectionSettings2 = sm.GetSettingsObject();
                eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings = new EyetrackerConnectionSettings()
                {
                    ClientIp = eyetrackerConnectionSettings2.ClientIp,
                    ClientPort = eyetrackerConnectionSettings2.ClientPort,
                    ServerIp = eyetrackerConnectionSettings2.ServerIp,
                    ServerPort = eyetrackerConnectionSettings2.ServerPort
                };
                eyetrackerConnectionSettingsPanel.EyetrackerType = eyetrackerConnectionSettings2.EyetrackerType;
            }
            catch (Exception exc)
            {
                //brak reakcji - kontrolki zostaną w domyślnych ustawieniach
                //tak może być na początku
                if (debug) MessageBox.Show("Debug: " + exc.Message);
            }

            //calibration settings
            try
            {
                SettingsManager<EyetrackerCalibrationSettings> sm = SettingsManager<EyetrackerCalibrationSettings>.Load(storingMethod, getSettingsFilePath("EyetrackerCalibrationSettings", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings = sm.GetSettingsObject();
            }
            catch (Exception exc)
            {
                //brak reakcji
                if (debug) MessageBox.Show("Debug: " + exc.Message);
            }

            //custom calibration settings
            try
            {
                SettingsManager<CustomCalibrationSettingsEx> sm = SettingsManager<CustomCalibrationSettingsEx>.Load(storingMethod, getSettingsFilePath("EyetrackerCustomCalibrationSettingsEx", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                eyetrackerCalibrationSettingsPanel.CustomCalibrationSettings = sm.GetSettingsObject();
            }
            catch(Exception exc)
            {
                eyetrackerCalibrationSettingsPanel.CustomCalibrationSettings = CustomCalibrationSettingsEx.GetDefault(eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings.NumberOfCalibrationPoints);
                if (debug) MessageBox.Show("Debug: " + exc.Message);
            }

            //analysis settings
            try
            {
                SettingsManager<GazeEventsRuntimeAnalysisSettings> sm = SettingsManager<GazeEventsRuntimeAnalysisSettings>.Load(storingMethod, getSettingsFilePath("EventsRuntimeAnalysisSettings", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                analysisSettings = sm.GetSettingsObject();
            }
            catch (Exception exc)
            {
                //brak reakcji
                if (debug) MessageBox.Show("Debug: " + exc.Message);
            }

            try
            {
                SettingsManager<EyetrackerOtherSettings> sm = SettingsManager<EyetrackerOtherSettings>.Load(storingMethod, getSettingsFilePath("EyetrackerOtherParameters", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                EyetrackerOtherSettings eyetrackerOtherSettings = sm.GetSettingsObject();
                dwellTime = eyetrackerOtherSettings.DwellTime;
                activationTime = eyetrackerOtherSettings.ActivationTime;
                cbSmoothingType.SelectedIndex = (int)eyetrackerOtherSettings.SmoothingFilter.SmoothingType;
                nudSmoothingRange.Value = eyetrackerOtherSettings.SmoothingFilter.SmoothingSamplesRange;
            }
            catch (Exception exc)
            {
                //brak reakcji
                if (debug) MessageBox.Show("Debug: " + exc.Message);
            }
        }

        class EyetrackerConnectionSettings2 : EyetrackerConnectionSettings //DTO
        {
            public EyetrackerType EyetrackerType;
        }

        class EyetrackerOtherSettings //DTO
        {
            public GazeSmoothingFilter SmoothingFilter;
            public TimeSpan DwellTime, ActivationTime;
        }

        protected void saveSettings(string profileName)
        {
            //connection settings
            EyetrackerConnectionSettings2 eyetrackerConnectionSettings2 = new EyetrackerConnectionSettings2()
            {
                EyetrackerType = eyetrackerConnectionSettingsPanel.EyetrackerType,
                ClientIp = eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings.ClientIp,
                ClientPort = eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings.ClientPort,
                ServerIp = eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings.ServerIp,
                ServerPort = eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings.ServerPort
            };
            try
            {
                SettingsManager<EyetrackerConnectionSettings2> sm = new SettingsManager<EyetrackerConnectionSettings2>(eyetrackerConnectionSettings2, storingMethod, getSettingsFilePath("EyetrackerConnectionSettings", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                sm.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error while saving eyetracker connection settings: " + exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //calibration settings
            EyetrackerCalibrationSettings eyetrackerCalibrationSettings = eyetrackerCalibrationSettingsPanel.EyetrackerCalibrationSettings;
            try
            {
                SettingsManager<EyetrackerCalibrationSettings> sm = new SettingsManager<EyetrackerCalibrationSettings>(eyetrackerCalibrationSettings, storingMethod, getSettingsFilePath("EyetrackerCalibrationSettings", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                sm.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error while saving eyetracker calibration settings: " + exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //custom calibration settings
            CustomCalibrationSettingsEx customCalibrationSettingsEx = eyetrackerCalibrationSettingsPanel.CustomCalibrationSettings;
            try
            {
                SettingsManager<CustomCalibrationSettingsEx> sm = new SettingsManager<CustomCalibrationSettingsEx>(customCalibrationSettingsEx, storingMethod, getSettingsFilePath("EyetrackerCustomCalibrationSettingsEx", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                sm.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error while saving eyetracker custom calibration settings: " + exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //analysis settings
            try
            {
                SettingsManager<GazeEventsRuntimeAnalysisSettings> sm = new SettingsManager<GazeEventsRuntimeAnalysisSettings>(analysisSettings, storingMethod, getSettingsFilePath("EventsRuntimeAnalysisSettings", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                sm.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error while saving events runtime analysis settings: " + exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //other settings
            EyetrackerOtherSettings eyetrackerOtherSettings = new EyetrackerOtherSettings()
            {
                DwellTime = dwellTime,
                ActivationTime = activationTime,
                SmoothingFilter = getSmoothingFilter()
            };
            try
            {
                SettingsManager<EyetrackerOtherSettings> sm = new SettingsManager<EyetrackerOtherSettings>(eyetrackerOtherSettings, storingMethod, getSettingsFilePath("EyetrackerOtherParameters", profileName), getMemberConverters(), formatProvider: CultureInfo.InvariantCulture);
                sm.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error while saving eyetracker other settings: " + exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Events
        public event EventHandler EyetrackerConnected;
        public event EventHandler EyetrackerDisconnected;

        private void onEyetrackerConnected() { if (EyetrackerConnected != null) EyetrackerConnected(this, EventArgs.Empty); }
        private void onEyetrackerDisconnected() { if (EyetrackerDisconnected != null) EyetrackerDisconnected(this, EventArgs.Empty); }
        #endregion

        //do experymentu Martina
        protected void lockEyetrackerConnectionPanel(EyetrackerType eyetrackerType, EyetrackerConnectionSettings eyetrackerConnectionSettings, bool lockEyetrackerTypeOnly)
        {
            eyetrackerConnectionSettingsPanel.EyetrackerType = eyetrackerType;
            eyetrackerConnectionSettingsPanel.EyetrackerConnectionSettings = eyetrackerConnectionSettings;
            if (lockEyetrackerTypeOnly)
            {
                eyetrackerConnectionSettingsPanel.LockEyetrackerType();
            }
            else
            {
                eyetrackerConnectionSettingsPanel.Enabled = false;
            }

            gbImitationEyetrackerSettingsPanel.Enabled = false;
        }

        private string getNotes(EyetrackerType eyetrackerType)
        {
            Type eyetrackerClassType;
            switch (eyetrackerConnectionSettingsPanel.EyetrackerType)
            {
                default:
                case EyetrackerType.ImitationEyetracker:
                    eyetrackerClassType = typeof(ImitationEyetracker);
                    break;
                case EyetrackerType.SMIEyetracker:
                    eyetrackerClassType = typeof(SMIEyetracker);
                    break;
                case EyetrackerType.MirametrixEyetracker:
                    eyetrackerClassType = typeof(OpenEyeGazeEyetracker);
                    break;
                case EyetrackerType.TheEyeTribeEyetracker:
                    eyetrackerClassType = typeof(TheEyeTribeEyetracker);
                    break;
                case EyetrackerType.TobiiEyetracker:
                    //eyetrackerClassType = typeof(TobiiEyetracker);
                    eyetrackerClassType = typeof(TobiiEyeXEyetracker);
                    break;
                case EyetrackerType.GazeDataReplay:
                    eyetrackerClassType = typeof(GazeDataReplayEyetracker);
                    break;
                case EyetrackerType.ZeroEyetracker:
                    eyetrackerClassType = typeof(ZeroEyetracker);
                    break;
            }            
            System.Reflection.PropertyInfo notesProperty = eyetrackerClassType.GetProperty("Notes", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (notesProperty != null) return notesProperty.GetValue(null) as string;
            else return null;
        }

        private void btnNotes_Click(object sender, EventArgs e)
        {
            string notes = getNotes(eyetrackerConnectionSettingsPanel.EyetrackerType);
            if (string.IsNullOrWhiteSpace(notes)) notes = "No notes for this eyetracker";
            MessageBox.Show(notes, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EyetrackingSettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.R) btnRun_Click(btnRun, EventArgs.Empty);
        }
    }
}
