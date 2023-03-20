using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace GameLab.Eyetracking.EyetrackerControls
{
    //dodana zmiana rysunków i dźwięki

    public class CustomCalibrationFormEx : CustomCalibrationForm
    {
        private Image[] images;
        private string[] soundFilesPaths;

        private SoundPlayer soundPlayer = new SoundPlayer();

        public CustomCalibrationFormEx(CustomCalibrationSettingsEx customCalibrationSettings)
            :base(customCalibrationSettings)
        {
            //this.BackColor = customCalibrationSettings.BackgroundColor;
            this.images = customCalibrationSettings.Images;
            this.soundFilesPaths = customCalibrationSettings.SoundFilePaths;

            pictureBox.Image = images[0];
            
            //ustawienia mogą być zmieniane, więc przenoszę to sprawdzanie to momentu zmieniania rysunku
            //if (numberOfCalibrationPoints != images.Length) throw new GameLabException("Incorrect number of images in custom calibration form (expected: " + numberOfCalibrationPoints + ", actual: " + images.Length + ")");
            //if (numberOfCalibrationPoints != soundFilesPaths.Length) throw new GameLabException("Incorrect number of sound files paths in custom calibration form (expected: " + numberOfCalibrationPoints + ", actual: " + soundFilesPaths.Length + ")");
        }

        protected override void updateImage()
        {
            if (numberOfCalibrationPoints != images.Length) throw new GameLabException("Incorrect number of images in custom calibration form (expected: " + numberOfCalibrationPoints + ", actual: " + images.Length + ")");
            if (numberOfCalibrationPoints != soundFilesPaths.Length) throw new GameLabException("Incorrect number of sound files paths in custom calibration form (expected: " + numberOfCalibrationPoints + ", actual: " + soundFilesPaths.Length + ")");

            base.updateImage(); //tu inkrementacja NumberOfCalibrationPointsShown

            if (NumberOfCalibrationPointsShown >= numberOfCalibrationPoints) return; //ignorowanie nadmiarowych punktów kalibracji

            pictureBox.Image = images[NumberOfCalibrationPointsShown];
            if (pictureBox.Image == null) pictureBox.Image = DefaultImage;

            if (soundFilesPaths != null && soundFilesPaths.Length > NumberOfCalibrationPointsShown)
            {
                string soundFilePath = soundFilesPaths[NumberOfCalibrationPointsShown];
                if (!soundFilePath.Equals("")) //puste elementy oznaczają po prostu brak dźwięku
                {
                    if (!string.IsNullOrWhiteSpace(soundFilePath) && System.IO.File.Exists(soundFilesPaths[NumberOfCalibrationPointsShown]))
                    {
                        soundPlayer.Stop();
                        soundPlayer.SoundLocation = soundFilePath;
                        soundPlayer.Load(); //synchronicznie
                        soundPlayer.Play(); //asynchronicznie
                    }
                    else throw new GameLabException("Error while playing sound in custom calibration screen: incorrect sound file path");
                }
            }
            else throw new GameLabException("Error while playing sound in custom calibration screen: error in sound paths array");
        }
    }
}
