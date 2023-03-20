using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GameLab.Eyetracking
{
    using Geometry;

    public delegate void CalibrationOrValidationFinishedEventHandler(bool result);

    public class EyetrackerCalibrationSettings
    {
        public int NumberOfCalibrationPoints { get; set; }
        public int DisplayDeviceIndex { get; set; }
        public string ImageFilePath { get; set; }
        public int ImageSize { get; set; }
        public bool IncreasedSpeed { get; set; }
        public bool AcceptationRequiredForEveryCalibrationPoint { get; set; }
        public int BackgroundBrightness { get; set; }
        public ICalibrationScreen CustomCalibrationScreen { get; set; }

        public EyetrackerCalibrationSettings()
        {
            NumberOfCalibrationPoints = 5;
            DisplayDeviceIndex = 0;
            ImageFilePath = "";
            ImageSize = 150;
            IncreasedSpeed = false;
            AcceptationRequiredForEveryCalibrationPoint = false;
            BackgroundBrightness = 255;
            CustomCalibrationScreen = null;
        }
    }

    public class EyetrackerCalibrationSettingsTypeConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true; //wyświetlanie "+" przed własnością
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(EyetrackerCalibrationSettings));
        }
    }

    public interface ICalibratable
    {
        bool Calibrated { get; }

        //kalibracja i walidacja
        /*
        bool Calibrate(
            int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath,
            int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints,
            int backgroundBrightness, ref string message, ICalibrationScreen customCalibrationScreen);
        bool Validate(
            int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath,
            int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints,
            int backgroundBrightness, ref string message, ICalibrationScreen customCalibrationScreen);
        */

        bool Calibrate(EyetrackerCalibrationSettings settings, ref string message);
        bool Validate(EyetrackerCalibrationSettings settings, ref string message);
        bool AbortCalibrationOrValidation();
        bool GetAccuracyDescription(bool showCalibrationResultsWindow, ref string leftEyeDescription, ref string rightEyeDescription /*ref string komunikat*/);

        event CalibrationOrValidationFinishedEventHandler CalibrationOrValidationFinished;
    }

    public interface ICalibratableEyetracker : IEyetracker, ICalibratable
    {
    }

    public struct CalibrationPoint
    {
        public int Index;
        public Point Position;
    }

    public interface ICalibrationScreen
    {
        CalibrationPoint CalibrationPoint { set; }

        void Show();
        void Close();
        void Settings(string imageFilePath, int imageSize, int backgroundBrightness, int animationTime);

        int NumberOfCalibrationPointsShown { get; } //liczba pokazanych punktów kalibracji

        Size ScreenSize { get; }
    }
}
