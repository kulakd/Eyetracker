using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EyeXFramework;
using Tobii.EyeX.Framework;

namespace GameLab.Eyetracking
{
    public class TobiiEyeXEyetracker : TobiiEyetracker, ICalibratableEyetracker
    {
        EyeXHost eyeXHost;

        public new string Name
        {
            get
            {
                return "Tobii Eyetracker (EyeX)";
            }
        }

        public TobiiEyeXEyetracker(bool smoothing)
            : base(smoothing)
        {            
            Calibrated = false;
            eyeXHost = new EyeXHost(); //to może powinno być w using
            eyeXHost.Start();

            EyePositionDataStream eyePositionDataStream = eyeXHost.CreateEyePositionDataStream();
            eyePositionDataStream.Next += eyePositionDataStream_Next;

            //nie można pozycji strumienia z lewego i prawego: http://developer.tobii.com/community/forums/topic/gazedata-for-both-right-and-left-eye/

            //ten strumień jest dokładnie używany w klasie bazowej
            //GazePointDataStream gazePointDataStream = eyeXHost.CreateGazePointDataStream(smoothing?GazePointDataMode.LightlyFiltered:GazePointDataMode.Unfiltered);
            //gazePointDataStream.Next += gazePointDataStream_Next;
        }

        public bool IsValidationPossible { get { return true; } }

        void eyePositionDataStream_Next(object sender, EyePositionEventArgs e)
        {
            LeftEyeDetected = e.LeftEye.IsValid;
            RightEyeDetected = e.RightEye.IsValid;
            //uwaga! e.LeftEye i e.RightEye to chyba pozycje oczu na obrazie z kamery, a nie pozycje spojrzenia
        }

        //void gazePointDataStream_Next(object sender, GazePointEventArgs e)
        //{
        //    AveragedEyeData = new EyeDataSample() { EyeSide = Eyetracking.EyeSide.AveragedOrBestEye, PositionF = new Geometry.PointF((float)e.X, (float)e.Y) }; //nie ma pupil size
        //}

        public bool Calibrated { get; private set; }

        public bool Calibrate(EyetrackerCalibrationSettings settings, ref string message)
        {
            try
            {
                if (settings.NumberOfCalibrationPoints <= 5) eyeXHost.LaunchGuestCalibration();
                else eyeXHost.LaunchRecalibration();
                //throw new ArgumentException("Only 4 or 7 calibration points are allowed");
                Calibrated = true;
                if (CalibrationOrValidationFinished != null) CalibrationOrValidationFinished(true);
                return true;
            }
            catch
            {
                Calibrated = false;
                if (CalibrationOrValidationFinished != null) CalibrationOrValidationFinished(false);
                return false;
            }
        }

        public bool Validate(EyetrackerCalibrationSettings settings, ref string message)
        {
            try
            {
                eyeXHost.LaunchCalibrationTesting();
                if (CalibrationOrValidationFinished != null) CalibrationOrValidationFinished(true);
                return true;
            }
            catch
            {
                if (CalibrationOrValidationFinished != null) CalibrationOrValidationFinished(false);
                return false;
            }
        }

        public bool AbortCalibrationOrValidation()
        {
            return false;
        }

        public bool GetAccuracyDescription(bool showCalibrationResultsWindow, ref string leftEyeDescription, ref string rightEyeDescription)
        {
            return false;
        }

        public event CalibrationOrValidationFinishedEventHandler CalibrationOrValidationFinished;
    }
}
