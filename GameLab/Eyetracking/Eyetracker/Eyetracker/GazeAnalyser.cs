using System;
using System.Collections.Generic;
using System.Text;

namespace GameLab.Eyetracking
{
    using GameLab.Geometry;

    public enum GazeEvent { Unknown, Fixation, Saccade } //w czasie rzeczywistym tylko te trzy zdarzenia
    //usunięte EyeClosed, bo nie można zrobić ogólnej analizy (każdy eyetracker inaczej sygnalizuje brak danych z oka -> et.LeftEyeDetected)

    public class EyeState
    {
        public GazeEvent CurrentEvent;
        public DateTime StartTime; //czas rozpoczęcia bieżącego zdarzenia
        public PointF StartPosition;
    }

    public delegate void EyeStateChanged(EyeState eyeData);

    //Gaze events analysis at runtime 
    //TODO: wzorować się na GameLab.Eyetracking.GazeAnalysis, ale uproszczona i w runtime oraz na tym co w GCAF (GCAF.Model.RejestryZdarzen.AnalizaPozycjiOka)
    public interface IGazeRuntimeAnalyser
    {
        EyeState LeftEyeState { get; }
        EyeState RightEyeState { get; }
        EyeState AveragedEyeState { get; }

        event EyeStateChanged LeftEyeStateChanged, RightEyeStateChanged, AveragedEyeStateChanged;
    }
}
