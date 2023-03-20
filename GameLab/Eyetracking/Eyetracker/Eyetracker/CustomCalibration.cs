using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameLab.Eyetracking
{
    using GameLab.Geometry;

    public struct CalibrationPoint
    {
        public int Index;
        public Point Position;

        public override string ToString()
        {
            return Index.ToString() + ". " + Position.ToString();
        }
    }

    //tak naprawdę ten interfejs nie jest po to, żeby umożliwiać różne okna spersonalizowanej kalibracji, ale po to, żeby rozdzielić warstwy
    //dlatego tu są też ustawienia do rozszerzonego okna spersonalizowanej kalibracji
    public interface ICustomCalibrationScreen
    {
        CalibrationPoint CalibrationPoint { set; } //przypisanie nowej pozycji jest sygnałem dla formy do zmiany miejsca rysunku, ale także obrazu i dźwięku

        void Show();
        //void ShowDialog();
        void Close();
        //void Settings(string imageFilePath, int imageSize, int backgroundBrightness, int animationTime);
        EyetrackerCalibrationSettings Settings { set; }

        int NumberOfCalibrationPointsShown { get; } //liczba pokazanych punktów kalibracji
        //nie ma IndexOfCalibrationPoint bo na początku (przed pokazaniem pierwszego punktu) jest zamieszanie

        Size ScreenSize { get; }
    }

    //dla CustomCalibrationForm
    public class CustomCalibrationSettings
    {
        public bool ImageShrinkingEnabled { get; set; }
        public TimeSpan ImageShrinkingTime { get; set; } //zmniejszanie rysunku, custom screen only
        public Color BackgroundColor { get; set; }
    }

    //dla CustomCalibrationFormEx
    public class CustomCalibrationSettingsEx : CustomCalibrationSettings
    {
        public bool UseExtendedForm; //dawne UseMultipleImagesAndSounds
        public string[] ImageFilePaths;
        public string[] SoundFilePaths;        

        public static CustomCalibrationSettingsEx GetDefault(int numberOfCalibrationPoints)
        {
            CustomCalibrationSettingsEx settings = new CustomCalibrationSettingsEx()
            {
                BackgroundColor = SystemColors.Control,
                ImageShrinkingTime = TimeSpan.FromSeconds(1),
                UseExtendedForm = false,
                ImageFilePaths = new string[numberOfCalibrationPoints],
                SoundFilePaths = new string[numberOfCalibrationPoints]                
            };
            for (int i = 0; i < numberOfCalibrationPoints; ++i)
            {
                settings.ImageFilePaths[i] = "";
                settings.SoundFilePaths[i] = "";
            }
            return settings;
        }

        public static CustomCalibrationSettingsEx Default
        {
            get
            {
                return GetDefault(0);
            }
        }

        private static Image[] loadImages(string[] imageFilePaths)
        {
            //GC.Collect();
            Image[] images = new Image[imageFilePaths.Length];
            for (int i = 0; i < images.Length; ++i)
            {
                if (System.IO.File.Exists(imageFilePaths[i])) images[i] = Image.FromFile(imageFilePaths[i]);
                else images[i] = null;
            }
            return images;
        }

        public Image[] Images //nie należy tego nadużywać, ale za to zawsze aktualne
        {
            get
            {
                return loadImages(ImageFilePaths);
            }
        }
    }
}
