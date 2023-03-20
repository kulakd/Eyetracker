using System;

//zob. https://github.com/EyeTribe/tet-csharp-client
//PM> Install-Package EyeTribe.ClientSdk
//PM> Install-Package EyeTribe.Controls

//CalibrationRunner używa zarówno Windows Forms (System.Windows.Forms), jak i WPF (WindowsBase, PresentationCore, PresentationFramework, System.Xaml) !!!!
using System.Windows.Forms; //tego używa CalibrationRunner !!!!!

using EyeTribe.ClientSdk;
using EyeTribe.ClientSdk.Data;
using EyeTribe.Controls.Calibration;

namespace GameLab.Eyetracking
{
    using GameLab.Geometry;

    public static class Point2DHelper
    {
        public static GameLab.Geometry.PointF ToPointF(this Point2D position)
        {
            return new Geometry.PointF(position.X, position.Y);
        }
    }

    public class TheEyeTribeEyetracker : ICalibratableEyetracker
    {        
        public string Name
        {
            get
            {
                return "The Eye Tribe Eyetracker";
            }
        }

        public static string Notes
        {
            get
            {
                return "Run EyeTribe Server";
            }
        }

        public bool IsValidationPossible { get { return false; } } //TODO: niżej są jakieś kombinacje, ale to chyba nie działa
        
        //public bool LeftEyeDetected { get; private set; }
        //public bool RightEyeDetected { get; private set; }
        private TetGazeListener tetGazeListener = null;
        public bool LeftEyeDetected 
        { 
            get
            {
                return tetGazeListener.LeftEyeDetected;
            }
        }
        public bool RightEyeDetected 
        { 
            get
            {
                return tetGazeListener.RightEyeDetected;
            }
        }

        public PointF LeftEyeOffset { get; set; } //aplikacja może to użyć lub nie
        public PointF RightEyeOffset { get; set; }
        public PointF AveragedEyeOffset { get; set; }


        public TheEyeTribeEyetracker()
        {
            //LeftEyeDetected = true;
            //RightEyeDetected = true;

            LeftEyeOffset = PointF.Zero;
            RightEyeOffset = PointF.Zero;
            AveragedEyeOffset = PointF.Zero;
        }

        #region Przesyłanie danych z TET

        class TetGazeListener : IGazeListener //wewnętrzna klasa, żeby interfejs IGazeListener nie wystawał na zewnątrz biblioteki
        {
            TheEyeTribeEyetracker et;

            public bool LeftEyeDetected { get; private set; }
            public bool RightEyeDetected { get; private set; }

            public long TimeStamp;
            public Eye LeftEye = null, RightEye = null;

            public TetGazeListener(TheEyeTribeEyetracker et)
            {
                this.et = et;
            }

            public void OnGazeUpdate(GazeData gazeData)
            {
                TimeStamp = gazeData.TimeStamp;
                bool updateBothEyesData = false;

                if (LeftEye == null || !gazeData.LeftEye.Equals(LeftEye))
                {                    
                    updateBothEyesData = true;
                    if (LeftEye.RawCoordinates.Equals(Point2D.Zero) && //dwa kolejne rekordy są równe zero na surowych danych
                        gazeData.LeftEye.RawCoordinates.Equals(Point2D.Zero)) LeftEyeDetected = false;
                    LeftEye = gazeData.LeftEye;
                    et.LeftEyeData = new EyeDataSample()
                    {
                        EyeSide = EyeSide.LeftEye,
                        PositionF = LeftEye.SmoothedCoordinates.ToPointF(),
                        PupilSize = (float)LeftEye.PupilSize,
                        OffsetCorrection = et.LeftEyeOffset
                    };
                    if (et.LeftEyeDataUpdated != null) et.LeftEyeDataUpdated(et.LeftEyeData);
                }
                if (RightEye == null || !gazeData.RightEye.Equals(RightEye))
                {
                    updateBothEyesData = true;
                    if (RightEye.RawCoordinates.Equals(Point2D.Zero) && //dwa kolejne rekordy są równe zero na surowych danych
                        gazeData.RightEye.RawCoordinates.Equals(Point2D.Zero)) RightEyeDetected = false;
                    RightEye = gazeData.RightEye;
                    et.RightEyeData = new EyeDataSample()
                    {
                        EyeSide = EyeSide.RightEye,
                        PositionF = RightEye.SmoothedCoordinates.ToPointF(),
                        PupilSize = (float)RightEye.PupilSize,
                        OffsetCorrection = et.RightEyeOffset
                    };
                    if (et.RightEyeDataUpdated != null) et.RightEyeDataUpdated(et.RightEyeData);
                }
                if (updateBothEyesData)
                {
                    et.AveragedEyeData = new EyeDataSample()
                    {
                        EyeSide = EyeSide.AveragedOrBestEye,
                        PositionF = (et.LeftEyeData.PositionF + et.RightEyeData.PositionF) / 2f,
                        PupilSize = (float)((LeftEye.PupilSize + RightEye.PupilSize) / 2),
                        OffsetCorrection = et.AveragedEyeOffset
                    };
                    if (et.AveragedEyeDataUpdated != null) et.AveragedEyeDataUpdated(et.AveragedEyeData);
                }
            }
        }

        bool calibrationProcessFinished = false;
        string calibrationMessage = null;
        bool calibrationResult = false;

        private void calibrationRunner_OnResult(object sender, CalibrationRunnerEventArgs e)
        {            
            switch (e.Result)
            {
                case CalibrationRunnerResult.Success:
                    calibrationMessage = "Calibration success " + e.CalibrationResult.AverageErrorDegree;
                    calibrationResult = true;
                    break;
                case CalibrationRunnerResult.Abort:
                    calibrationMessage = "The calibration was aborted. Reason: " + e.Message;
                    calibrationResult = false;
                    break;
                case CalibrationRunnerResult.Error:
                    calibrationMessage = "An error occured during calibration. Reason: " + e.Message;
                    calibrationResult = false;
                    break;
                case CalibrationRunnerResult.Failure:
                    calibrationMessage = "Calibration failed. Reason: " + e.Message;
                    calibrationResult = false;
                    break;
                case CalibrationRunnerResult.Unknown:
                    calibrationMessage = "Calibration exited with unknown state. Reason: " + e.Message;
                    calibrationResult = false;
                    break;
            }
            calibrationProcessFinished = true;
        }
        #endregion

        /*
        #region Kalibracja z TET
        //ICalibrationProcessHandler
        public void OnCalibrationProcessing() 
        {
            
        }

        public void OnCalibrationProgress(double progress)
        {
            
        }

        public void OnCalibrationResult(CalibrationResult calibResult)
        {
            
        }

        public void OnCalibrationStarted()
        {
            
        }
        #endregion
        */

        public EyeDataSample LeftEyeData { get; set; }
        public EyeDataSample RightEyeData { get; set; }
        public EyeDataSample AveragedEyeData { get; set; }

        public event EyeDataUpdatedEventHandler LeftEyeDataUpdated;
        public event EyeDataUpdatedEventHandler RightEyeDataUpdated;
        public event EyeDataUpdatedEventHandler AveragedEyeDataUpdated;

        public bool Connected
        {
            get 
            {
                //return GazeManager.Instance.IsConnected; //obsolete
                return GazeManager.Instance.IsActivated;
            }
        }

        public bool Calibrated
        {
            get 
            {
                return GazeManager.Instance.IsCalibrated;
            }
        }

        public bool Connect(EyetrackerConnectionSettings settings, ref string message)
        {
            //GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);
            GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0);
            tetGazeListener = new TetGazeListener(this);
            GazeManager.Instance.AddGazeListener(tetGazeListener); //zdarzenia na bieżącym obiekcie

            if (!GazeManager.Instance.IsActivated)
            {
                message = "EyeTribe Server has not been started";
                return false;
            }

            switch(GazeManager.Instance.Trackerstate)
            {
                default:
                case GazeManagerCore.TrackerState.TRACKER_UNDEFINED:
                    message = "Unrecognized state of the device";
                    break;
                case GazeManagerCore.TrackerState.TRACKER_CONNECTED:
                    message = "The device is connected";
                    break;
                case GazeManagerCore.TrackerState.TRACKER_CONNECTED_BADFW:
                case GazeManagerCore.TrackerState.TRACKER_CONNECTED_NOSTREAM:
                case GazeManagerCore.TrackerState.TRACKER_CONNECTED_NOUSB3:
                    message = "The device is connected, but some problems occured";
                    break;
                case GazeManagerCore.TrackerState.TRACKER_NOT_CONNECTED:
                    message = "The device is not connected";
                    break;
            }

            //EyeTribe.Controls.TrackBox.TrackBoxStatus tbs = new EyeTribe.Controls.TrackBox.TrackBoxStatus(); - WPF!!!

            return true;
        }

        public bool Disconnect(ref string message)
        {
            GazeManager.Instance.Deactivate();
            if(GazeManager.Instance.IsActivated)
            {
                message = "EyeTribe Server has not been deactivated";
                return false;
            }
            return true;
        }

        public bool Calibrate(EyetrackerCalibrationSettings settings, ref string message)
        {
            if (!GazeManager.Instance.IsActivated)
            {
                message = "The server is not running";
                OnCalibrationOrValidationFinished(false);
                return false;
            }

            if (GazeManager.Instance.Trackerstate == GazeManagerCore.TrackerState.TRACKER_NOT_CONNECTED || GazeManager.Instance.Trackerstate == GazeManagerCore.TrackerState.TRACKER_UNDEFINED)
            {
                message = "The device is not conected";
                OnCalibrationOrValidationFinished(false);
                return false;
            }

            try
            {
                calibrationProcessFinished = false;
                calibrationMessage = null;

                //rozpoczyna kalibrację
                //TODO: dodać własne okno kalibracji - jest interfejs, który na to pozwala
                Screen screen = Screen.AllScreens[settings.DisplayDeviceIndex];
                CalibrationRunner calibrationRunner = new CalibrationRunner(screen, screen.Bounds.Size, settings.NumberOfCalibrationPoints);
                calibrationRunner.OnResult += calibrationRunner_OnResult;
                calibrationRunner.Start();

                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (calibrationProcessFinished);
                message = calibrationMessage;
            }
            catch(Exception exc)
            {
                message = "Błąd podczas kalibracji: " + exc.Message;
                OnCalibrationOrValidationFinished(false);
                return false;
            }

            OnCalibrationOrValidationFinished(true);
            return true;
        }

        public bool Validate(EyetrackerCalibrationSettings settings, ref string message)
        {
            OnCalibrationOrValidationFinished(false);
            return false;
        }

        public bool AbortCalibrationOrValidation()
        {
            bool wynik = GazeManager.Instance.CalibrationAbort();
            return wynik;
        }

        public bool GetAccuracyDescription(bool showCalibrationResultsWindow, ref string leftEyeDescription, ref string rightEyeDescription)
        {
            throw new NotImplementedException();
        }

        private void OnCalibrationOrValidationFinished(bool result)
        {
            if (CalibrationOrValidationFinished != null) CalibrationOrValidationFinished(result);
        }

        public event CalibrationOrValidationFinishedEventHandler CalibrationOrValidationFinished;

        public bool ImagesUpdatingEnabled { get; set; }

        #region Nie używane zdarzenia i własności
        #pragma warning disable 67
        public System.Drawing.Bitmap EyeImage
        {
            get { return null; }
        }

        public System.Drawing.Bitmap SceneImage
        {
            get { return null; }
        }

        public event ImageUpdatedEventHandler EyeImageUpdated;

        public event ImageUpdatedEventHandler SceneImageUpdated;

        public System.Drawing.Bitmap EyeTrackingImage
        {
            get { return null; }
        }

        public event ImageUpdatedEventHandler EyeTrackingImageUpdated;
        #pragma warning restore 67
        #endregion
    }
}
