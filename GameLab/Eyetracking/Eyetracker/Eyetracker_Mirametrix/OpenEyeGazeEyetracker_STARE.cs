using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace GameLab.Eyetracking
{
    using GameLab.Geometry;
    using RafałLinowiecki.Open_Eye_gaze_Interface;
    using RafałLinowiecki.OpenEyeGazeInterface;

    //TODO: jeżeli nie uda się zrobić kalibrację, to zmienić interfejs
    public class OpenEyeGazeEyetracker : ICalibratableEyetracker
    {
        private Calibration.CALIB_RESULT_SUMMARY_STRUCT summaryCalibrationResult;
        private List<Calibration.CALIB_RESULT_STRUCT> calibrationResult;
        private bool leftEyeEnabled;
        private bool rightEyeEnabled;
        private bool bestEyeEnabled;
        private int refreshTime; //okres odświeżania
        private int screenWidth;
        private int screenHeight;
        private int numberOfAveragedPackets; //liczba uśrednianych pakietów
        public bool Connected
        {
            get
            {
                return Connection.IsConnected();
            }
        }
        public bool Calibrated { get; private set; }

        public string Name
        {
            get
            {
                return "Open Eye-Gaze Compatible Eyetrackers (Mirametrix, GazePoint)";
            }
        }

        public static string Notes
        {
            get
            {
                return "GazePoint: run GazePoint Control, ip address: 127.0.0.1, port: 4242\nMirametrix: ????";
            }
        }

        public bool IsValidationPossible { get { return false; } }

        public bool LeftEyeDetected { get; private set; }
        public bool RightEyeDetected { get; private set; }

        public PointF LeftEyeOffset { get; set; } //aplikacja może to użyć lub nie
        public PointF RightEyeOffset { get; set; }
        public PointF AveragedEyeOffset { get; set; }

        public OpenEyeGazeEyetracker(int screenWidth, int screenHeight, bool leftEyeEnabled = true, bool rightEyeEnabled = true, bool bestEyeEnabled = true, int refreshTime = 10, int averagingTime = 100) //ostatnie to okres uśredniania
        {
            this.leftEyeEnabled = leftEyeEnabled;
            this.rightEyeEnabled = rightEyeEnabled;
            this.bestEyeEnabled = bestEyeEnabled;
            this.refreshTime = refreshTime;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            numberOfAveragedPackets = averagingTime / refreshTime;
            if (numberOfAveragedPackets == 0)
                numberOfAveragedPackets = 1;

            LeftEyeDetected = true;
            RightEyeDetected = true;

            LeftEyeOffset = PointF.Zero;
            RightEyeOffset = PointF.Zero;
            AveragedEyeOffset = PointF.Zero;
        }

        public bool Connect(string ipServer, int portServer, string ipClient, int portClient, ref string message)
        {
            //ipKlienta i portKlienta są ignorowane
            if (string.IsNullOrEmpty(ipServer)) ipServer = "127.0.0.1";
            if (portServer < 0) portServer = 4242;

            try
            {
                Connection.Connect(ipServer, portServer);
                getData();
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }

            return Connection.IsConnected();
        }

        public bool Connect(EyetrackerConnectionSettings settings, ref string message)
        {
            return Connect(settings.ServerIp, settings.ServerPort, settings.ClientIp, settings.ClientPort, ref message);
        }

        public bool Disconnect(ref string message)
        {
            try
            {
                Connection.Disconnect();
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }

            return !Connection.IsConnected();
        }

        public bool Calibrate(int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath, int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints, int backgroundBrightness, ref string message, ICustomCalibrationScreen customCalibrationScreen)
        {
            Calibration calibration = new Calibration();

            calibration.CALIBRATE_FAST(increasedSpeed);
            calibration.CALIBRATE_SHOW();
            calibration.CALIBRATE_START();

            if (numberOfCalibrationPoints != 5 && numberOfCalibrationPoints != 9)
            {
                message = "Nieprawidłowa liczba punktów";
                OnCalibrationOrValidationFinished(false);
                return false;
            }

            calibration.CALIB_RESULT_SUMMARY(out summaryCalibrationResult);
            calibration.CALIB_RESULT(out calibrationResult);

            OnCalibrationOrValidationFinished(true);

            return true;
        }

        public bool Calibrate(EyetrackerCalibrationSettings settings, ref string message)
        {
            return Calibrate(
                settings.NumberOfCalibrationPoints, settings.DisplayDeviceIndex, settings.ImageFilePath, settings.ImageSize,
                settings.IncreasedSpeed, settings.AcceptationRequiredForEveryCalibrationPoint, settings.BackgroundBrightness, ref message, settings.CustomCalibrationScreen);
        }

        public bool Validate(int numberOfCalibrationPoints, int displayDeviceIndex, string imageFilePath, int imageSize, bool increasedSpeed, bool acceptationRequiredForEveryCalibrationPoints, int backgroundBrightness, ref string message, ICustomCalibrationScreen customCalibrationScreen)
        {
            OnCalibrationOrValidationFinished(true);

            return true;
        }

        public bool Validate(EyetrackerCalibrationSettings settings, ref string message)
        {
            return Validate(
                settings.NumberOfCalibrationPoints, settings.DisplayDeviceIndex, settings.ImageFilePath, settings.ImageSize,
                settings.IncreasedSpeed, settings.AcceptationRequiredForEveryCalibrationPoint, settings.BackgroundBrightness, ref message, settings.CustomCalibrationScreen);
        }

        public bool AbortCalibrationOrValidation()
        {
            OnCalibrationOrValidationFinished(false);
            return false;
        }

        public bool GetAccuracyDescription(bool showCalibrationResultsWindow, ref string leftEyeDescription, ref string rightEyeDescription)
        {
            leftEyeDescription = summaryCalibrationResult.AVE_ERROR.ToString();
            rightEyeDescription = leftEyeDescription;

            return true;
        }

        private void OnCalibrationOrValidationFinished(bool result)
        {
            if (CalibrationOrValidationFinished != null) CalibrationOrValidationFinished(result);
        }

        public event CalibrationOrValidationFinishedEventHandler CalibrationOrValidationFinished;

        /*
        public bool ImagesUpdatingEnabled { get; set; }

        public System.Drawing.Bitmap EyeImage
        {
            get { return null; }
        }

        #region Nie używane zdarzenia i własności
        #pragma warning disable 67
        public event ImageUpdatedEventHandler EyeImageUpdated;

        public System.Drawing.Bitmap SceneImage
        {
            get { return null; }
        }

        public event ImageUpdatedEventHandler SceneImageUpdated;

        public System.Drawing.Bitmap EyeTrackingImage
        {
            get { return null; }
        }

        public event ImageUpdatedEventHandler EyeTrackingImageUpdated;
        #pragma warning restore 67
        #endregion
        */

        public EyeDataSample LeftEyeData { get; private set; }
        public EyeDataSample RightEyeData { get; private set; }
        public EyeDataSample AveragedEyeData { get; private set; }

        public event EyeDataUpdatedEventHandler LeftEyeDataUpdated;

        public event EyeDataUpdatedEventHandler RightEyeDataUpdated;

        public event EyeDataUpdatedEventHandler AveragedEyeDataUpdated;

        /*
        public bool IsDataRecordingPossible
        { 
            get
            {
                return false;
            }
        }

        public bool DataRecordingEnabled
        {
            get { return false; }
        }

        public bool BeginDataRecording(ref string komunikat)
        {
            return false;
        }

        public bool EndDataRecording(ref string komunikat)
        {
            return false;
        }

        public bool SetMarker(string tekst, ref string komunikat)
        {
            return false;
        }

        public bool SaveData(string ścieżkaPliku, string nazwaBadanego, string opisBadania, ref string komunikat)
        {
            return false;
        }
        */

        private void getData()
        {
            var remoteData = new RemoteData();

            //remoteData.ENABLE_SEND_TIME(true);
            if (leftEyeEnabled)
            {
                remoteData.ENABLE_SEND_POG_LEFT(true);
                remoteData.ENABLE_SEND_PUPIL_LEFT(true);
            }
            if (rightEyeEnabled)
            {
                remoteData.ENABLE_SEND_POG_RIGHT(true);
                remoteData.ENABLE_SEND_PUPIL_RIGHT(true);
            }
            if (bestEyeEnabled)
                remoteData.ENABLE_SEND_POG_BEST(true);
            remoteData.ENABLE_SEND_DATA(true);

            new Thread(updateEyeData).Start();
        }

        private void updateEyeData()
        {
            Queue<Dictionary<string, double>> historyOfAveragedParameters = new Queue<Dictionary<string, double>>();
            List<Dictionary<string, string>> parameterList;
            Dictionary<string, double> averagedParameters;
            SmoothingType smoothingType = SmoothingType.WMA;

            // lista parametrów do uśrednienia
            List<string> parametersNames = new List<string>();
            if (leftEyeEnabled)
            {
                parametersNames.Add("LPOGX");
                parametersNames.Add("LPOGY");
                parametersNames.Add("LPD");
            }
            if (rightEyeEnabled)
            {
                parametersNames.Add("RPOGX");
                parametersNames.Add("RPOGY");
                parametersNames.Add("RPD");
            }
            if (bestEyeEnabled)
            {
                parametersNames.Add("BPOGX");
                parametersNames.Add("BPOGY");
            }

            while (Connection.IsConnected())
            {
                Thread.Sleep(refreshTime);

                var query = Command.RecreateREC(Connection.ReceiveREC(), out parameterList);

                if (parameterList == null || !parameterList.Any())
                    continue;

                historyOfAveragedParameters.Enqueue(SmoothingData.Smoothing(parameterList, parametersNames, smoothingType));
                if (historyOfAveragedParameters.Count > numberOfAveragedPackets)
                    historyOfAveragedParameters.Dequeue();
                averagedParameters = SmoothingData.Smoothing(historyOfAveragedParameters.ToList(), parametersNames, smoothingType);

                if (leftEyeEnabled)
                {
                    EyeDataSample data = new EyeDataSample();
                    data.EyeSide = EyeSide.LeftEye;
                    float x = (float)averagedParameters["LPOGX"] * screenWidth;
                    float y = (float)averagedParameters["LPOGY"] * screenHeight;
                    data.PositionF = new PointF(x, y);
                    data.PupilSize = (float)averagedParameters["LPD"];
                    if ((data.Position.X != LeftEyeData.Position.X || data.Position.Y != LeftEyeData.Position.Y) && LeftEyeDataUpdated != null)
                        LeftEyeDataUpdated(data);
                    data.OffsetCorrection = LeftEyeOffset;
                    LeftEyeData = data;
                }
                if (rightEyeEnabled)
                {
                    EyeDataSample data = new EyeDataSample();
                    data.EyeSide = EyeSide.RightEye;
                    float x = (float)averagedParameters["RPOGX"] * screenWidth;
                    float y = (float)averagedParameters["RPOGY"] * screenHeight;
                    data.PositionF = new PointF(x, y);
                    data.PupilSize = (float)averagedParameters["RPD"];
                    if ((data.Position.X != RightEyeData.Position.X || data.Position.Y != RightEyeData.Position.Y) && RightEyeDataUpdated != null)
                        RightEyeDataUpdated(data);
                    data.OffsetCorrection = RightEyeOffset;
                    RightEyeData = data;
                }
                if (bestEyeEnabled)
                {
                    EyeDataSample data = new EyeDataSample();
                    data.EyeSide = EyeSide.AveragedOrBestEye;
                    float x = (float)averagedParameters["BPOGX"] * screenWidth;
                    float y = (float)averagedParameters["BPOGY"] * screenHeight;
                    data.PositionF = new PointF(x, y);
                    data.PupilSize = 0;
                    if ((data.Position.X != AveragedEyeData.Position.X || data.Position.Y != AveragedEyeData.Position.Y) && AveragedEyeDataUpdated != null)
                        AveragedEyeDataUpdated(data);
                    data.OffsetCorrection = AveragedEyeOffset;
                    AveragedEyeData = data;
                }
            }
        }
    }
}
