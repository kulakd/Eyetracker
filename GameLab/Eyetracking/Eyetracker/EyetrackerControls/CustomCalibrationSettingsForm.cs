using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace GameLab.Eyetracking.EyetrackerControls
{
    public partial class CustomCalibrationSettingsForm : Form
    {
        int numberOfCalibrationPoints = 0;
        private List<string> imageFilePaths = new List<string>();
        private List<string> soundFilePaths = new List<string>();

        //dla designera
        public CustomCalibrationSettingsForm()
        {
            InitializeComponent();
        }

        public CustomCalibrationSettingsForm(CustomCalibrationSettingsEx settings, int numberOfCalibrationPoints)
        {
            InitializeComponent();

            this.numberOfCalibrationPoints = numberOfCalibrationPoints;
            toControls(settings);            
        }

        private void toControls(CustomCalibrationSettingsEx settings)
        {
            cbImageShrinkingEnabled.Checked = settings.ImageShrinkingEnabled;
            nudImageShrinkingTime.Value = (int)settings.ImageShrinkingTime.TotalMilliseconds;
            pnlBackgroundColor.BackColor = settings.BackgroundColor;

            lbNumberOfCalibrationPoints.Text += numberOfCalibrationPoints.ToString();
            nudCalibrationPointIndex.Maximum = numberOfCalibrationPoints - 1;

            cbUseMultipleImagesAndSounds.Checked = settings.UseExtendedForm;
            imageFilePaths = settings.ImageFilePaths.ToList();
            soundFilePaths = settings.SoundFilePaths.ToList();
            if (imageFilePaths.Count < numberOfCalibrationPoints)
                for (int i = imageFilePaths.Count; i < numberOfCalibrationPoints; ++i)
                    imageFilePaths.Add("");
            if (soundFilePaths.Count < numberOfCalibrationPoints)
                for (int i = soundFilePaths.Count; i < numberOfCalibrationPoints; ++i)
                    soundFilePaths.Add("");            

            cbUseMultipleImagesAndSounds_CheckedChanged(this, EventArgs.Empty);
            nudCalibrationPointIndex_ValueChanged(this, EventArgs.Empty);
        }

        private CustomCalibrationSettingsEx fromControls()
        {
            return new CustomCalibrationSettingsEx()
            {
                ImageShrinkingEnabled = cbImageShrinkingEnabled.Checked,
                ImageShrinkingTime = TimeSpan.FromMilliseconds((int)nudImageShrinkingTime.Value),
                UseExtendedForm = cbUseMultipleImagesAndSounds.Checked,
                ImageFilePaths = imageFilePaths.Take(numberOfCalibrationPoints).ToArray(),
                SoundFilePaths = soundFilePaths.Take(numberOfCalibrationPoints).ToArray(),
                BackgroundColor = pnlBackgroundColor.BackColor
            };
        }

        public CustomCalibrationSettingsEx Settings
        {
            get
            {
                return fromControls();
            }
        }

        private void btnChooseColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = pnlBackgroundColor.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pnlBackgroundColor.BackColor = colorDialog.Color;
            }
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            try
            {
                string calibrationImageFilePath = imageFilePaths[(int)nudCalibrationPointIndex.Value];
                imageOpenFileDialog.InitialDirectory = Path.GetDirectoryName(calibrationImageFilePath);
                imageOpenFileDialog.FileName = Path.GetFileName(calibrationImageFilePath);
            }
            catch
            {
                imageOpenFileDialog.InitialDirectory = Path.GetDirectoryName(".");
                imageOpenFileDialog.FileName = Path.GetFileName("kalibracja.png");
            }
            DialogResult dr = imageOpenFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                imageFilePaths[(int)nudCalibrationPointIndex.Value] = imageOpenFileDialog.FileName;
                pbCalibrationImagePreview.Image = Image.FromFile(imageOpenFileDialog.FileName);
            }
        }

        private SoundPlayer soundPlayer = new SoundPlayer();

        private void btnChooseSound_Click(object sender, EventArgs e)
        {
            try
            {
                string calibrationSoundFilePath = soundFilePaths[(int)nudCalibrationPointIndex.Value];
                soundOpenFileDialog.InitialDirectory = Path.GetDirectoryName(calibrationSoundFilePath);
                soundOpenFileDialog.FileName = Path.GetFileName(calibrationSoundFilePath);
            }
            catch
            {
                soundOpenFileDialog.InitialDirectory = Path.GetDirectoryName(".");
                soundOpenFileDialog.FileName = Path.GetFileName("kalibracja.wav");
            }
            DialogResult dr = soundOpenFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                soundFilePaths[(int)nudCalibrationPointIndex.Value] = soundOpenFileDialog.FileName;
                soundPlayer.Stop();
                soundPlayer.SoundLocation = soundOpenFileDialog.FileName;
                soundPlayer.Load();
                soundPlayer.Play();
            }
        }

        private void cbUseMultipleImagesAndSounds_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = cbUseMultipleImagesAndSounds.Checked;
            lbCalibrationPointIndex.Enabled = enabled;
            nudCalibrationPointIndex.Enabled = enabled;
            pbCalibrationImagePreview.Enabled = enabled;
            lbImagesAndSoundsUsedDuringCalibration.Enabled = enabled;
            btnChooseImage.Enabled = enabled;
            btnChooseSound.Enabled = enabled;
        }

        private void nudCalibrationPointIndex_ValueChanged(object sender, EventArgs e)
        {
            int index = (int)nudCalibrationPointIndex.Value;
            string imageFilePath = imageFilePaths[index];
            if (File.Exists(imageFilePath)) pbCalibrationImagePreview.Image = Image.FromFile(imageFilePath);
            else pbCalibrationImagePreview.Image = null;
            string soundFilePath = soundFilePaths[index];
            if (File.Exists(soundFilePath) && cbUseMultipleImagesAndSounds.Checked)
            {
                new System.Threading.Thread(
                    () =>
                    {
                        soundPlayer.Stop();
                        soundPlayer.SoundLocation = soundFilePath;
                        soundPlayer.Load();
                        soundPlayer.Play();
                    }).Start();
                
            }
        }

        private void CustomCalibrationSettingsForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void btnClearSound_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
            soundFilePaths[(int)nudCalibrationPointIndex.Value] = "";
        }
    }
}
