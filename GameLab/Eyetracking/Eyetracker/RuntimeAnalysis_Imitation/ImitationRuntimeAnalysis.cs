using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace GameLab.Eyetracking.GazeRuntimeAnalysis
{
    using GameLab.Geometry;

    public class RuntimeAnalysis_Imitation : IGazeRuntimeAnalyser
    {
        private const int period = 100;
        private Timer timer; //niezależny timer, który pobiera położenia niezależnie od zdarzeń z eyetrackera

        private const float saccadeMinimumPositionChange = 10;
        private ImitationEyetracker et;

        private PointF previousLeftEyePosition, previousRightEyePosition, previousAveragedEyePosition;
        private EyeState PreviousLeftEyeState, CurrentLeftEyeState;
        private EyeState PreviousRightEyeState, CurrentRightEyeState;
        private EyeState PreviousAveragedEyeState, CurrentAveragedEyeState;

        //public string opis = ""; //TEST

        private static GazeEvent detectEvent(PointF gazePosition, PointF previousGazePosition, float pupilSize, EyeState previousEyeState)
        {
            GazeEvent gazeEvent;
            if (previousEyeState == null) gazeEvent = GazeEvent.Unknown;
            else
            {
                float distance = PointF.Distance(gazePosition, previousGazePosition);
                if (distance < saccadeMinimumPositionChange) gazeEvent = GazeEvent.Fixation;
                else gazeEvent = GazeEvent.Saccade;

            }
            return gazeEvent;
        }

        private void analyze()
        {
            //Console.Beep(100, 10);
            //opis = et.LeftEyeData.PupilSize.ToString() + " " + et.RightEyeData.PupilSize.ToString() + " " + et.AveragedEyeData.PupilSize.ToString();

            //left eye
            {                
                GazeEvent gazeEvent = detectEvent(et.LeftEyeData.PositionF, previousLeftEyePosition, et.LeftEyeData.PupilSize, PreviousLeftEyeState);
                if (PreviousLeftEyeState == null || gazeEvent != PreviousLeftEyeState.CurrentEvent)
                {
                    PreviousLeftEyeState = CurrentLeftEyeState;
                    CurrentLeftEyeState = new EyeState()
                    {
                        CurrentEvent = gazeEvent,
                        StartPosition = et.LeftEyeData.PositionF,
                        StartTime = DateTime.Now
                    };
                    onLeftEyeStateChanged();
                }
                previousLeftEyePosition = et.LeftEyeData.PositionF;
            }

            //right eye
            {
                GazeEvent gazeEvent = detectEvent(et.RightEyeData.PositionF, previousRightEyePosition, et.RightEyeData.PupilSize, PreviousRightEyeState);
                if (PreviousRightEyeState == null || gazeEvent != PreviousRightEyeState.CurrentEvent)
                {
                    PreviousRightEyeState = CurrentRightEyeState;
                    CurrentRightEyeState = new EyeState()
                    {
                        CurrentEvent = gazeEvent,
                        StartPosition = et.RightEyeData.PositionF,
                        StartTime = DateTime.Now
                    };
                    onRightEyeStateChanged();
                }
                previousRightEyePosition = et.RightEyeData.PositionF;
            }

            //averaged eye
            {
                GazeEvent gazeEvent = detectEvent(et.AveragedEyeData.PositionF, previousAveragedEyePosition, et.AveragedEyeData.PupilSize, PreviousAveragedEyeState);
                if (PreviousAveragedEyeState == null || gazeEvent != PreviousAveragedEyeState.CurrentEvent)
                {
                    PreviousAveragedEyeState = CurrentAveragedEyeState;
                    CurrentAveragedEyeState = new EyeState()
                    {
                        CurrentEvent = gazeEvent,
                        StartPosition = et.AveragedEyeData.PositionF,
                        StartTime = DateTime.Now
                    };
                    onAveragedEyeStateChanged();
                }
                previousAveragedEyePosition = et.AveragedEyeData.PositionF;
            }
        }

        public RuntimeAnalysis_Imitation(ImitationEyetracker et)
        {            
            this.et = et;
            timer = new Timer(period);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            analyze();
        }

        public EyeState LeftEyeState
        {
            get 
            {
                return CurrentLeftEyeState;
            }
        }

        public EyeState RightEyeState
        {
            get 
            {
                return CurrentRightEyeState;
            }
        }

        public EyeState AveragedEyeState
        {
            get
            {
                return CurrentAveragedEyeState;
            }
        }


        public event EyeStateChanged LeftEyeStateChanged;

        public event EyeStateChanged RightEyeStateChanged;

        public event EyeStateChanged AveragedEyeStateChanged;

        private void onLeftEyeStateChanged()
        {
            if(LeftEyeStateChanged!=null) LeftEyeStateChanged(LeftEyeState);
        }

        private void onRightEyeStateChanged()
        {
            if (RightEyeStateChanged != null) RightEyeStateChanged(RightEyeState);
        }

        private void onAveragedEyeStateChanged()
        {
            if (AveragedEyeStateChanged != null) AveragedEyeStateChanged(AveragedEyeState);
        }
    }
}
