using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace GameLab.Eyetracking.GazeRuntimeAnalysis //por. przestrzeń JacekMatulewski.Gaze.Data.Analysis - analiza offline
{
    using GameLab.Geometry;

    public enum GazeEventsAnalysisMethod { VelocityThreshold, PositionDispersion };    

    public class GazeEventsRuntimeAnalysisSettings
    {
        //public bool DetectEyeClosure = true;
        public bool DetectFixationsAndSaccades = true;

        public int MinimumNumberOfStatesToDetectEyeClosure = 3;

        public GazeEventsAnalysisMethod EventsAnalysisMethod = GazeEventsAnalysisMethod.PositionDispersion;

        public double PDM_MaximalDispersionX = 100, PDM_MaximalDispersionY = 100;
        public int PDM_MinimalFixationNumberOfSamples = 5;

        public bool VM_UseThreePointsDerivatives = true;
        public double VM_SaccadeThresholdVelocity = 1; //w zasadzie prędkość, ale bez dzielenia przez czas

        public GazeEventsRuntimeAnalysisSettings() { } //wystarczyłby prywatny, ale publiczny umożliwia użycie SettingsManagera

        public static readonly GazeEventsRuntimeAnalysisSettings Default = new GazeEventsRuntimeAnalysisSettings();
    }

    /*private*/
    class EyeDataSampleWithTime : ICloneable //wygodniej byłoby ze strukturą, ale mogą mieć null
    {
        public DateTime Time;
        public EyeDataSample EyeData;

        public object Clone() //shallow copy
        {
            return new EyeDataSampleWithTime()
            {
                Time = this.Time,
                EyeData = this.EyeData //dane nie są klonowane, bo struktura                
            };
        }
    }

    public class EyeDataRuntimeAnalysis
    {
        private int numberOfEyeStatesStored;
        private GazeEventsRuntimeAnalysisSettings analysisSettings;
        private EyeDataSampleWithTime lastEyeDataSample;
        private Queue<EyeDataSampleWithTime> eyeDataHistory;

        public EyeDataRuntimeAnalysis(GazeEventsRuntimeAnalysisSettings analysisSettings)
        {
            this.analysisSettings = analysisSettings;
            switch (analysisSettings.EventsAnalysisMethod)
            {
                case GazeEventsAnalysisMethod.PositionDispersion:
                    numberOfEyeStatesStored = Math.Max(analysisSettings.PDM_MinimalFixationNumberOfSamples, analysisSettings.MinimumNumberOfStatesToDetectEyeClosure);
                    break;
                case GazeEventsAnalysisMethod.VelocityThreshold:
                    int numberOfStatesToCalculateVelocity = analysisSettings.VM_UseThreePointsDerivatives ? 3 : 2;
                    numberOfEyeStatesStored = Math.Max(numberOfStatesToCalculateVelocity, analysisSettings.MinimumNumberOfStatesToDetectEyeClosure);
                    break;
                default:
                    throw new Exception("Unrecognized gaze event detection method");
            }
            eyeDataHistory = new Queue<EyeDataSampleWithTime>(numberOfEyeStatesStored);
        }

        public void Update(EyeDataSample eyeData)
        {
            lock (eyeDataHistory)
            {
                lastEyeDataSample = new EyeDataSampleWithTime() { Time = DateTime.Now, EyeData = eyeData };
                eyeDataHistory.Enqueue(lastEyeDataSample);
                while (eyeDataHistory.Count > numberOfEyeStatesStored)
                    eyeDataHistory.Dequeue();
            }
        }

        private EyeDataSampleWithTime[] cloneDataFromQueue()
        {
            EyeDataSampleWithTime[] eyeDataArray;
            lock (eyeDataHistory)
            {
                eyeDataArray = new EyeDataSampleWithTime[eyeDataHistory.Count];
                eyeDataHistory.CopyTo(eyeDataArray, 0); //TODO: optymalizacja, czy to obciążające? Zał. zmiany kolejki częstsze niż wykrywanie zdarzeń            
            }
            return eyeDataArray;
        }

        /*
        private bool? detectEyeClosure_Zeros(EyeDataSampleWithTime[] eyeDataArray)
        {
            int eyeDataArrayLength = eyeDataArray.Length;
            if (eyeDataArrayLength < analysisSettings.MinimumNumberOfStatesToDetectEyeClosure) return null;
            bool onlyZeros = true;
            for (int i = 0; i < analysisSettings.MinimumNumberOfStatesToDetectEyeClosure; ++i)
            {
                EyeDataSampleWithTime eyeData = eyeDataArray[eyeDataArrayLength - 1 - i];
                if (eyeData == null) return null; //to nie powinno mieć miejsca, ale wszystko działa równolegle
                PointF position = eyeData.EyeData.PositionF;
                if (position.X != 0 || position.Y != 0)
                {
                    onlyZeros = false; //najdłużej działa, gdy zamknięte
                    break;
                }
            }
            return onlyZeros;
        }

        private bool? detectEyeClosure_NoUpdates(EyeDataSampleWithTime[] eyeDataArray)
        {

        }

        private bool? detectEyeClosure(EyeDataSampleWithTime[] eyeDataArray)
        {

        }
        */

        private GazeEvent detectFixationsAndSaccades(EyeDataSampleWithTime[] eyeDataArray)
        {
            int eyeDataArrayLength = eyeDataArray.Length;
            switch (analysisSettings.EventsAnalysisMethod)
            {
                case GazeEventsAnalysisMethod.PositionDispersion: //analiza pozycyjna
                    if (eyeDataArrayLength < analysisSettings.PDM_MinimalFixationNumberOfSamples) return GazeEvent.Unknown;
                    PointF min = eyeDataArray[eyeDataArrayLength - 1].EyeData.PositionF, max = min;
                    for (int i = 1; i < analysisSettings.PDM_MinimalFixationNumberOfSamples; ++i)
                    {
                        PointF position = eyeDataArray[eyeDataArrayLength - 1 - i].EyeData.PositionF;
                        if (position.X < min.X) min.X = position.X;
                        if (position.Y < min.Y) min.Y = position.Y;
                        if (position.X > max.X) max.X = position.X;
                        if (position.Y > max.Y) max.Y = position.Y;
                    }
                    bool pm_conditionFulfilled = max.X - min.X < analysisSettings.PDM_MaximalDispersionX && max.Y - min.Y < analysisSettings.PDM_MaximalDispersionY;
                    return pm_conditionFulfilled ? GazeEvent.Fixation : GazeEvent.Saccade;
                case GazeEventsAnalysisMethod.VelocityThreshold: //analiza prędkościowa
                    int numberOfStatesToCalculateVelocity = analysisSettings.VM_UseThreePointsDerivatives ? 3 : 2;
                    if (eyeDataArrayLength < numberOfStatesToCalculateVelocity) return GazeEvent.Unknown;
                    EyeDataSampleWithTime endSample = eyeDataArray[eyeDataArrayLength - 1];
                    if (endSample == null) return GazeEvent.Unknown;
                    PointF endPosition = endSample.EyeData.PositionF;
                    DateTime endTime = endSample.Time;
                    EyeDataSampleWithTime startSample = eyeDataArray[eyeDataArrayLength - numberOfStatesToCalculateVelocity];
                    if (startSample == null) return GazeEvent.Unknown;
                    PointF startPosition = startSample.EyeData.PositionF;
                    DateTime startTime = startSample.Time;
                    float durationMiliseconds = (endTime - startTime).Milliseconds;
                    PointF velocity = (endPosition - startPosition) / durationMiliseconds;
                    double speed = velocity.Length;
                    bool vm_conditionFulfilled = speed > analysisSettings.VM_SaccadeThresholdVelocity;
                    return vm_conditionFulfilled ? GazeEvent.Saccade : GazeEvent.Fixation;
                default:
                    throw new Exception("Unrecognized gaze event detection method");
            }
        }

        private bool detectionIsRunning = false;

        private GazeEvent detectEvents()
        {
            detectionIsRunning = true;

            GazeEvent result;            
            if (eyeDataHistory == null) { result = GazeEvent.Unknown; goto koniec; }
            EyeDataSampleWithTime[] eyeDataArray = cloneDataFromQueue(); //klonowanie, bo dane są stale zmieniane            

            /*
            //czy oko zamknięte
            if (analysisSettings.DetectEyeClosure)
            {
                bool? eyeClosed = detectEyeClosure(eyeDataArray);
                if (!eyeClosed.HasValue) { result = GazeEvent.Unknown; goto koniec; }
                else if (eyeClosed.Value) { result = GazeEvent.EyeClosed; goto koniec; }
            }
            */

            //analiza zdarzeń            
            if (analysisSettings.DetectFixationsAndSaccades)
            {
                result = detectFixationsAndSaccades(eyeDataArray);
                goto koniec;
            }
            else 
            {
                result = GazeEvent.Unknown;
                goto koniec;
            }

        koniec: //zamiast goto można try..finally, ale to chyba wolniejsze
            detectionIsRunning = false;
            return result;
        }

        private GazeEvent previousGazeEvent = GazeEvent.Unknown;
        public EyeState EyeState { get; private set; }
        public event EyeStateChanged EyeStateChanged;

        public void Analyze()
        {
            if (detectionIsRunning) return;

            GazeEvent gazeEvent = detectEvents();
            if (gazeEvent != previousGazeEvent)
            {
                previousGazeEvent = gazeEvent;
                EyeState = new EyeState() //tylko przy zmianie zdarzenia
                {
                    CurrentEvent = gazeEvent,
                    StartPosition = lastEyeDataSample.EyeData.PositionF,
                    StartTime = lastEyeDataSample.Time //stan może zacząć się wcześniej, ale to działa przy założeniu, że stany się nie zmieniają gdy zdarzenie jest to samo
                };
                onEyeStateChanged();
            }
        }

        private void onEyeStateChanged()
        {
            if (EyeStateChanged != null) EyeStateChanged(EyeState);
        }
    }

    public class GazeRuntimeAnalyser : IGazeRuntimeAnalyser
    {
        public enum EventRaisingScheme { OnEyeDataUpdate, OnPropertyRead, UseTimer }

        private IEyetracker et;
        private EventRaisingScheme eventRaisingScheme;
        private EyeDataRuntimeAnalysis leftEyeAnalyser = null, rightEyeAnalyser = null, averagedEyeAnalyser = null;

        private Timer timer;

        public GazeRuntimeAnalyser(IEyetracker et, GazeEventsRuntimeAnalysisSettings analysisSettings, EventRaisingScheme eventRaisingScheme, double timerIntervalMiliseconds = 100)
        {
            this.et = et;
            this.eventRaisingScheme = eventRaisingScheme;
            leftEyeAnalyser = new EyeDataRuntimeAnalysis(analysisSettings);
            rightEyeAnalyser = new EyeDataRuntimeAnalysis(analysisSettings);
            averagedEyeAnalyser = new EyeDataRuntimeAnalysis(analysisSettings);
            leftEyeAnalyser.EyeStateChanged += leftEyeAnalyser_EyeStateChanged;
            rightEyeAnalyser.EyeStateChanged += rightEyeAnalyser_EyeStateChanged;
            averagedEyeAnalyser.EyeStateChanged += averagedEyeAnalyser_EyeStateChanged;
            et.LeftEyeDataUpdated += et_LeftEyeDataUpdated;
            et.RightEyeDataUpdated += et_RightEyeDataUpdated;
            et.AveragedEyeDataUpdated += et_AveragedEyeDataUpdated;
            if (eventRaisingScheme == EventRaisingScheme.UseTimer)
            {
                timer = new Timer(timerIntervalMiliseconds);
                timer.Elapsed += timer_Elapsed;
                timer.Start();
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            leftEyeAnalyser.Analyze();
            rightEyeAnalyser.Analyze();
            averagedEyeAnalyser.Analyze();
        }

        public event EyeStateChanged LeftEyeStateChanged;
        public event EyeStateChanged RightEyeStateChanged;
        public event EyeStateChanged AveragedEyeStateChanged;

        private void onEyeStateChanged(EyeStateChanged eyeStateChengedEvent, EyeState eyeState)
        {
            if (eyeStateChengedEvent != null) eyeStateChengedEvent(eyeState);
        }

        private void leftEyeAnalyser_EyeStateChanged(EyeState eyeState)
        {
            onEyeStateChanged(LeftEyeStateChanged, eyeState);
        }

        private void rightEyeAnalyser_EyeStateChanged(EyeState eyeState)
        {
            onEyeStateChanged(RightEyeStateChanged, eyeState);
        }

        private void averagedEyeAnalyser_EyeStateChanged(EyeState eyeState)
        {
            onEyeStateChanged(AveragedEyeStateChanged, eyeState);
        }

        void et_LeftEyeDataUpdated(EyeDataSample eyeData)
        {
            leftEyeAnalyser.Update(eyeData);
            if (eventRaisingScheme == EventRaisingScheme.OnEyeDataUpdate) leftEyeAnalyser.Analyze();
        }

        void et_RightEyeDataUpdated(EyeDataSample eyeData)
        {
            rightEyeAnalyser.Update(eyeData);
        }

        void et_AveragedEyeDataUpdated(EyeDataSample eyeData)
        {
            averagedEyeAnalyser.Update(eyeData);
        }

        public EyeState LeftEyeState
        {
            get
            {
                if (eventRaisingScheme == EventRaisingScheme.OnPropertyRead) leftEyeAnalyser.Analyze();
                return leftEyeAnalyser.EyeState;
            }
        }

        public EyeState RightEyeState
        {
            get
            {
                if (eventRaisingScheme == EventRaisingScheme.OnPropertyRead) rightEyeAnalyser.Analyze();
                return rightEyeAnalyser.EyeState;
            }
        }

        public EyeState AveragedEyeState
        {
            get
            {
                if (eventRaisingScheme == EventRaisingScheme.OnPropertyRead) averagedEyeAnalyser.Analyze();
                return averagedEyeAnalyser.EyeState;
            }
        }
    }
}