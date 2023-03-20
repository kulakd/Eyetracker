using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLab.Eyetracking
{
    using Geometry;

    public enum SmoothingType { None, SMA, WMA, EMA }; //https://en.wikipedia.org/wiki/Moving_average

    public class GazeSmoothingFilter
    {
        public SmoothingType SmoothingType { get; private set; }
        public int SmoothingSamplesRange { get; private set; }
        private Queue<PointF> gazeHistory;

        public GazeSmoothingFilter()
            :this(SmoothingType.None, 0)
        {
        }

        public GazeSmoothingFilter(SmoothingType smoothingType = SmoothingType.None, int smoothingSamplesRange = 0)
        {
            this.SmoothingType = smoothingType;
            this.SmoothingSamplesRange = smoothingSamplesRange;
            if (smoothingType != SmoothingType.None)
            {
                if (smoothingSamplesRange < 0) throw new ArgumentException("Smoothing range cannot be less than zero");
                if (smoothingSamplesRange > 0) gazeHistory = new Queue<PointF>(smoothingSamplesRange);
            }
        }

        public void AddEyeDataSample(EyeDataSample eyeDataSample)
        {
            gazeHistory.Enqueue(eyeDataSample.PositionF);
            while (gazeHistory.Count > SmoothingSamplesRange)
            {
                gazeHistory.Dequeue();
            }
        }

        public PointF CalculateSmoothedGazePosition()
        {
            int N = Math.Min(SmoothingSamplesRange, gazeHistory.Count); //ile wyrazów wstecz obejmujemy średnią kroczącą

            //obliczanie mianownika średniej kroczącej
            PointF smoothedPosition = PointF.Zero;
            float weightsSum = 0;
            float w = 1; //tylko dla EMA
            float alfa = 2f / (N + 1f);
            for (int n = 0; n < N; ++n)
            {
                float weight = 0;
                switch (SmoothingType)
                {
                    case SmoothingType.SMA: //zwykła średnia arytmetyczna
                        weight = 1f;
                        break;
                    case SmoothingType.WMA: //waga maleje w postępie arytmetycznym
                        weight = n + 1;
                        break;
                    case SmoothingType.EMA: //waga maleje w postępie wykładniczym                            
                        weight = w;
                        w *= 1 - alfa;
                        break;
                }
                weightsSum += weight;

                PointF dataSample = gazeHistory.ElementAt(N - 1 - n);
                smoothedPosition.X += weight * dataSample.X;
                smoothedPosition.Y += weight * dataSample.Y;
            }
            smoothedPosition /= weightsSum;
            return smoothedPosition;
        }

        //to jest wersja IdfTxtDocument.GetGazePosition, która działa w czasie rzeczywistym
        public EyeDataSample SmoothData(EyeDataSample eyeDataSample)
        {
            if (SmoothingType == SmoothingType.None || SmoothingSamplesRange == 0) return eyeDataSample;
            lock (this)
            {
                AddEyeDataSample(eyeDataSample);
                eyeDataSample.PositionF = CalculateSmoothedGazePosition();
                return eyeDataSample;
            }
        }

        //dodane na potrzeby metody "Enter & Leave velocity"
        //TODO: powtórzony kod
        public double GetSmoothedGazeDirection() 
        {
            if (gazeHistory == null) return double.NaN;
            lock (this)
            {
                int N = Math.Min(SmoothingSamplesRange, gazeHistory.Count); //ile wyrazów wstecz obejmujemy średnią kroczącą

                //obliczanie mianownika średniej kroczącej
                double smoothedDirection = 0.0;
                float weightsSum = 0;
                float w = 1; //tylko dla EMA
                float alfa = 2f / (N + 1f);
                for (int n = 0; n < N - 1; ++n)
                {
                    float weight = 0;
                    switch (SmoothingType)
                    {
                        case SmoothingType.SMA: //zwykła średnia arytmetyczna
                            weight = 1f;
                            break;
                        case SmoothingType.WMA: //waga maleje w postępie arytmetycznym
                            weight = n + 1;
                            break;
                        case SmoothingType.EMA: //waga maleje w postępie wykładniczym                            
                            weight = w;
                            w *= 1 - alfa;
                            break;
                    }
                    weightsSum += weight;

                    PointF currentdataSample = gazeHistory.ElementAt(N - 1 - n);
                    PointF previousDataSample = gazeHistory.ElementAt(N - 1 - n - 1);
                    float dx = currentdataSample.X - previousDataSample.X;
                    float dy = currentdataSample.Y - previousDataSample.Y;                    
                    double gazeDirection = Math.Atan2(dx, -dy); //zero do góry

                    smoothedDirection += weight * smoothedDirection;
                }
                smoothedDirection /= weightsSum;
                return smoothedDirection;
            }
        }
    }
}
