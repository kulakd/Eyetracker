using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace GameLab.Eyetracking
{
    using Geometry;

    public class TobiiEyetracker : IEyetracker
    {
        protected Host host = null;
        private EyeDataSample emptyEyeData = new EyeDataSample() { PositionF = PointF.Zero, PupilSize = 0, OffsetCorrection = PointF.Zero };

        private bool smoothing;

        public string Name
        {
            get
            {
                return "Tobii Eyetracker";
            }
        }

        public static string Notes
        {
            get
            {
                return "Connection settings are ignored. Set proper DPI scaling settings (Application) - otherwise the gaze position is incorrect. For .NET Framework 4.7 it is in manifest";
            }
        }

        public bool LeftEyeDetected { get; protected set; }
        public bool RightEyeDetected { get; protected set; }

        public PointF LeftEyeOffset { get; set; } //aplikacja może to użyć lub nie
        public PointF RightEyeOffset { get; set; }
        public PointF AveragedEyeOffset { get; set; }

        public TobiiEyetracker(bool smoothing)
        {
            this.smoothing = smoothing;

            LeftEyeDetected = true;
            RightEyeDetected = true;

            LeftEyeData = emptyEyeData;
            RightEyeData = emptyEyeData;

            LeftEyeOffset = PointF.Zero;
            RightEyeOffset = PointF.Zero;
            AveragedEyeOffset = PointF.Zero;
        }

        public bool Connected
        {
            get
            {
                return host != null;
            }
        }

        public void updateGazePoint(double x, double y, double timestamp)
        {
            AveragedEyeData = new EyeDataSample()
            {
                EyeSide = Eyetracking.EyeSide.AveragedOrBestEye,
                PositionF = new PointF((float)x, (float)y),
                OffsetCorrection = AveragedEyeOffset
            };
            if (AveragedEyeDataUpdated != null) AveragedEyeDataUpdated(AveragedEyeData);
        }

        public bool Connect(EyetrackerConnectionSettings settings, ref string message)
        {
            try
            {
                host = new Host();
                GazePointDataStream gazePointDataStream = host.Streams.CreateGazePointDataStream(smoothing ? GazePointDataMode.LightlyFiltered : GazePointDataMode.Unfiltered);
                gazePointDataStream.GazePoint(updateGazePoint);
                //gazePointDataStream.Next += gazePointDataStream_Next;
                //EyePositionStream eyePositionDataStream = host.Streams.CreateEyePositionStream();
                //eyePositionDataStream.EyePosition();
                return true;
            }
            catch //(Exception exc)
            {
                return false;
            }
        }

        //void gazePointDataStream_Next(object sender, StreamData<GazePointData> e)
        //{
        //}

        public bool Disconnect(ref string message)
        {
            if(host != null) 
            {
                host.DisableConnection();
                host = null;                
            }
            return true;
        }

        public event EyeDataUpdatedEventHandler LeftEyeDataUpdated;

        public event EyeDataUpdatedEventHandler RightEyeDataUpdated;

        public event EyeDataUpdatedEventHandler AveragedEyeDataUpdated;

        public EyeDataSample LeftEyeData { get; protected set; }

        public EyeDataSample RightEyeData { get; protected set; }

        public EyeDataSample AveragedEyeData { get; protected set; }
    }
}
