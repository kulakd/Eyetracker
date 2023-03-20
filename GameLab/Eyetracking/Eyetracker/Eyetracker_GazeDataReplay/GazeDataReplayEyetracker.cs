using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameLab.Eyetracking
{
    using JacekMatulewski.Csv;    

    public class GazeDataReplayEyetracker : IEyetracker
    {
        public static GazeDataReplaySample[] LoadGazeCoordinatesFromCsvFile(string filename, char separatorChar)
        {
            CsvDocument<GazeDataReplaySample> csv = CsvDocument<GazeDataReplaySample>.Load(filename, separatorChar);
            return csv.GetRecords();
        }

        GazeDataReplaySample[] gazeData;
        int currentIndex = -1;

        public GazeDataReplayEyetracker(IEnumerable<GazeDataReplaySample> gazeData)
        {
            this.gazeData = gazeData.ToArray();

            Connected = false;
        }

        public string Name
        {
            get { return "Gaze Data Replay Eyetracker"; }
        }

        public bool Connected { get; private set; }

        private EyeDataSample calculateAveragedEyeData()
        {
            EyeDataSample averagedEyeDataSample = new EyeDataSample()
            {
                EyeSide = EyeSide.AveragedOrBestEye,
                PositionF = (LeftEyeData.PositionF + RightEyeData.PositionF) / 2f,
                PupilSize = (LeftEyeData.PupilSize + RightEyeData.PupilSize) / 2f,
                OffsetCorrection = AveragedEyeOffset
            };
            return averagedEyeDataSample;
        }

        private int getMillisecondsToNext()
        {
            return (int)GameLab.TimeHelper.Ticks2Milisekundy(gazeData[currentIndex+1].Ticks - gazeData[currentIndex].Ticks);            
        }

        private void threadWork()
        {
            for(int i = 0; i < gazeData.Length; ++i)
            {
                bool updatedAveragedEyeDataSample = false;
                if(i == 0 || gazeData[i].LeftEyePosition != gazeData[i-1].LeftEyePosition)
                {
                    EyeDataSample eyeDataSample = new EyeDataSample()
                    {
                        EyeSide = EyeSide.LeftEye,
                        PositionF = gazeData[i].LeftEyePosition,
                        PupilSize = -1,
                        OffsetCorrection = LeftEyeOffset
                    };
                    LeftEyeData = eyeDataSample;
                    if (LeftEyeDataUpdated != null) LeftEyeDataUpdated(LeftEyeData);
                    updatedAveragedEyeDataSample = true;
                }
                if (i == 0 || gazeData[i].RightEyePosition != gazeData[i - 1].RightEyePosition)
                {
                    EyeDataSample eyeDataSample = new EyeDataSample()
                    {
                        EyeSide = EyeSide.RightEye,
                        PositionF = gazeData[i].RightEyePosition,
                        PupilSize = -1,
                        OffsetCorrection = RightEyeOffset
                    };
                    LeftEyeData = eyeDataSample;
                    if (LeftEyeDataUpdated != null) LeftEyeDataUpdated(LeftEyeData);
                    updatedAveragedEyeDataSample = true;
                }
                if (updatedAveragedEyeDataSample)
                {
                    AveragedEyeData = calculateAveragedEyeData();
                    if (AveragedEyeDataUpdated != null) AveragedEyeDataUpdated(AveragedEyeData);                    
                }
                Thread.Sleep(getMillisecondsToNext());
            }
        }

        public void Replay()
        {
            //czyta i wyznacza okresy do kolejnego wystąpienia
            ThreadStart ts = new ThreadStart(threadWork);
            Thread t = new Thread(ts);
            t.Start();
        }

        public bool Connect(EyetrackerConnectionSettings settings, ref string message)
        {
            Connected = true;
            message = null;
            return true;
        }

        public bool Disconnect(ref string message)
        {
            Connected = false;
            message = null;
            return true;
        }

        public bool LeftEyeDetected
        {
            get { return true; }
        }

        public bool RightEyeDetected
        {
            get { return true; }
        }

        public EyeDataSample LeftEyeData { get; private set; }

        public EyeDataSample RightEyeData { get; private set; }

        public EyeDataSample AveragedEyeData { get; private set; }

        public event EyeDataUpdatedEventHandler LeftEyeDataUpdated;

        public event EyeDataUpdatedEventHandler RightEyeDataUpdated;

        public event EyeDataUpdatedEventHandler AveragedEyeDataUpdated;

        public GameLab.Geometry.PointF LeftEyeOffset { get; set; }

        public GameLab.Geometry.PointF RightEyeOffset { get; set; }

        public GameLab.Geometry.PointF AveragedEyeOffset { get; set; }
    }
}
