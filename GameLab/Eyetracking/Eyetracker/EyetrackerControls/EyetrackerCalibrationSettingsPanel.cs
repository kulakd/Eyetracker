using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GameLab.Eyetracking.EyetrackerControls
{
    public partial class EyetrackerCalibrationSettingsPanel : UserControl
    {
        public EyetrackerCalibrationSettingsPanel()
        {
            InitializeComponent();

            wykryjMonitory();
        }

        private void wykryjMonitory()
        {
            IEnumerable<string> screenDescriptions = from Screen ekran in Screen.AllScreens
                                               select ekran.DeviceName + " " + ekran.WorkingArea.ToString();

            int currentScreenIndex = 0;
            Screen currentScreen = Screen.FromControl(this);
            for (int i = 0; i < Screen.AllScreens.Length; ++i)
            {
                Screen screen = Screen.AllScreens[i];
                if (currentScreen.DeviceName.Equals(screen.DeviceName))
                    currentScreenIndex = i;
            }

            cbDisplayDevice.Items.Clear();
            cbDisplayDevice.Items.AddRange(screenDescriptions.ToArray());
            cbDisplayDevice.SelectedIndex = currentScreenIndex;
        }

        [Category("Eyetracker")]
        public bool CustomCalibrationScreenOptionEnabled
        {
            get
            {
                return cbUseCustomCalibrationScreen.Enabled;
            }
            set
            {
                cbUseCustomCalibrationScreen.Enabled = value;
            }
        }

        [Category("Eyetracker")]
        public bool UseCustomCalibrationScreen
        {
            get
            {
                return cbUseCustomCalibrationScreen.Checked;
            }
            set
            {
                cbUseCustomCalibrationScreen.Checked = value;
            }
        }

        [Category("Eyetracker")]
        [TypeConverter(typeof(EyetrackerCalibrationSettingsTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EyetrackerCalibrationSettings EyetrackerCalibrationSettings
        {
            get
            {
                return gainSettings();
            }
            set
            {
                nudNumberOfCalibrationPoints.Value = value.NumberOfCalibrationPoints;
                cbDisplayDevice.SelectedIndex = value.DisplayDeviceIndex;
                imageFilePath = value.ImageFilePath;
                if(File.Exists(imageFilePath)) pbCalibrationImage.Image = Image.FromFile(imageFilePath);
                nudImageSize.Value = value.ImageSize;
                cbIncreasedSpeed.Checked = value.IncreasedSpeed;
                cbAcceptationRequiredForEveryCalibrationPoint.Checked = value.AcceptationRequiredForEveryCalibrationPoint;
                tbBackgroundBrightness.Value = value.BackgroundBrightness;
                cbUseCustomCalibrationScreen.Checked = value.UseCustomCalibrationScreen;
                //gubiona jest informacja o instancji CustomCalibrationScreen
            }
        }

        private EyetrackerCalibrationSettings gainSettings()
        {
            EyetrackerCalibrationSettings ecs = new EyetrackerCalibrationSettings();
            ecs.NumberOfCalibrationPoints = (int)nudNumberOfCalibrationPoints.Value;
            ecs.DisplayDeviceIndex = cbDisplayDevice.SelectedIndex;
            ecs.ImageFilePath = imageFilePath;
            ecs.ImageSize = (int)nudImageSize.Value;
            ecs.IncreasedSpeed = cbIncreasedSpeed.Checked;
            ecs.AcceptationRequiredForEveryCalibrationPoint = cbAcceptationRequiredForEveryCalibrationPoint.Checked;
            ecs.BackgroundBrightness = tbBackgroundBrightness.Value;
            ecs.UseCustomCalibrationScreen = cbUseCustomCalibrationScreen.Checked;
            return ecs;
        }

        string imageFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "kalibracja.png");

        [Category("Eyetracker")]
        public Image CalibrationImage
        {
            get
            {
                if (File.Exists(imageFilePath)) return Image.FromFile(imageFilePath);
                else return null;
            }
        }

        private void btnChooseCalibrationImage_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(imageFilePath);
                openFileDialog.FileName = Path.GetFileName(imageFilePath);
            }
            catch
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(".");
                openFileDialog.FileName = Path.GetFileName("kalibracja.png");
            }
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                imageFilePath = openFileDialog.FileName;
                pbCalibrationImage.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void btnCustomCalibrationScreenSettings_Click(object sender, EventArgs e)
        {
            if (CustomCalibrationSettings == null) CustomCalibrationSettings = GameLab.Eyetracking.CustomCalibrationSettingsEx.GetDefault((int)nudNumberOfCalibrationPoints.Value);
            CustomCalibrationSettingsForm form = new CustomCalibrationSettingsForm(CustomCalibrationSettings, (int)nudNumberOfCalibrationPoints.Value);
            /*if (form.ShowDialog() == DialogResult.OK)*/ form.ShowDialog(); CustomCalibrationSettings = form.Settings;
        }

        private void cbUseCustomCalibrationScreen_CheckedChanged(object sender, EventArgs e)
        {
            btnCustomCalibrationScreenSettings.Enabled = cbUseCustomCalibrationScreen.Checked;
            lbBackgroundBrightness.Enabled = !cbUseCustomCalibrationScreen.Checked;
            tbBackgroundBrightness.Enabled = !cbUseCustomCalibrationScreen.Checked;
        }

        public CustomCalibrationSettingsEx CustomCalibrationSettings { get; set; }

        bool showCustomCalibrationSettingsForm = false;

        private void nudNumberOfCalibrationPoints_ValueChanged(object sender, EventArgs e)
        {
            if (cbUseCustomCalibrationScreen.Checked) showCustomCalibrationSettingsForm = true;
        }

        private void nudNumberOfCalibrationPoints_Leave(object sender, EventArgs e)
        {
            if (showCustomCalibrationSettingsForm)
            {
                MessageBox.Show("Changing the number of calibration points in case of using custom calibration screen requires to adjust the images array", "Eyetracker settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnCustomCalibrationScreenSettings_Click(sender, EventArgs.Empty);
            }
        }
    }
}
