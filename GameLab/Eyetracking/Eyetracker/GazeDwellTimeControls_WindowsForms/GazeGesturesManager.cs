using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameLab.Eyetracking
{
    public class GazeGesturesManager
    {
        //założenie - gest jest kończony tym samym sposobem, co rozpoczynany
        public enum GestureInitiationMethod { None, BothEyesBlink, LeftEyeBlink, RightEyeBlink, Fixation }

        public enum GestureType { NotRecognized, Fixation, HorizontalMovement, VerticalMovement }

        private Timer timer;
        private IEyetracker et;                
        private GestureInitiationMethod currentGestureInitiation;
        private GestureType currentGesture;
        private float value; //TODO: to jeszcze nie jest użyte

        public bool Enabled {get;set;}

        public GazeGesturesManager(IEyetracker et)
        {
            this.et = et;
            this.timer = new Timer();
            timer.Interval = 10; //??????            
            timer.Tick += timer_Tick;
            Enabled = true;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
 	        currentGestureInitiation = detectGestureInitiation();
            currentGesture = detectGestureType();
        }

        #region Gesture detection 
        //gaze data analysis
        private GestureInitiationMethod detectGestureInitiation()
        {
            return GestureInitiationMethod.None;
        }

        private GestureType detectGestureType()
        {
            return GestureType.NotRecognized;
        }
        #endregion

        #region Events
        public class GestureEventArgs : EventArgs
        {
            public GestureInitiationMethod GestureInitiation { get; private set; }
            public GestureType Gesture { get; private set; }
            public float Value { get; private set; }

            public GestureEventArgs(GestureInitiationMethod gestureInitiation, GestureType gesture, float value)
            {
                this.GestureInitiation = gestureInitiation;
                this.Gesture = gesture;
                this.Value = value;
            }
        }

        public event EventHandler<GestureEventArgs> GestureInitiated;
        public event EventHandler<GestureEventArgs> GestureFinished;
        public event EventHandler<GestureEventArgs> GestureProgress;

        private GestureEventArgs createGestureEventArgs()
        {
            return new GestureEventArgs(currentGestureInitiation, currentGesture, value);
        }

        private void onGestureInitiated()
        {
            if (GestureInitiated != null) GestureInitiated(this, createGestureEventArgs());
        }

        private void onGestureFinished()
        {
            if (GestureFinished != null) GestureFinished(this, createGestureEventArgs());
        }

        private void onGestureProgress()
        {
            if (GestureProgress != null) GestureProgress(this, createGestureEventArgs());
        }
        #endregion
    }
}
