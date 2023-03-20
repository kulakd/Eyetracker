using System;
using System.Collections.Generic;
using System.Text;

namespace GameLab.Eyetracking
{
    using GameLab.Geometry;

    public class ZeroEyetracker : IEyetracker
    {
        public string Name { get { return "Zero Eyetracker"; } }

        public bool Connected { get { return true; } }

        public bool Connect(EyetrackerConnectionSettings settings, ref string message)
        {            
            return true;
        }

        public bool Disconnect(ref string message)
        {
            return true;
        }

        public bool LeftEyeDetected { get { return false; } }

        public bool RightEyeDetected { get { return false; } }

        private EyeDataSample leftEyeData = new EyeDataSample() { EyeSide = EyeSide.LeftEye, PositionF = PointF.Zero, PupilSize = 0, OffsetCorrection = PointF.Zero };
        private EyeDataSample rightEyeData = new EyeDataSample() { EyeSide = EyeSide.RightEye, PositionF = PointF.Zero, PupilSize = 0, OffsetCorrection = PointF.Zero };
        private EyeDataSample averagedEyeData = new EyeDataSample() { EyeSide = EyeSide.AveragedOrBestEye, PositionF = PointF.Zero, PupilSize = 0, OffsetCorrection = PointF.Zero };

        public EyeDataSample LeftEyeData { get { return leftEyeData; } }

        public EyeDataSample RightEyeData { get { return rightEyeData; } }

        public EyeDataSample AveragedEyeData { get { return averagedEyeData; } }

        public event EyeDataUpdatedEventHandler LeftEyeDataUpdated;

        public event EyeDataUpdatedEventHandler RightEyeDataUpdated;

        public event EyeDataUpdatedEventHandler AveragedEyeDataUpdated;

        public GameLab.Geometry.PointF LeftEyeOffset
        {
            get
            {
                return PointF.Zero;
            }
            set { }
        }

        public GameLab.Geometry.PointF RightEyeOffset
        {
            get
            {
                return PointF.Zero;
            }
            set { }
        }

        public GameLab.Geometry.PointF AveragedEyeOffset
        {
            get
            {
                return PointF.Zero;
            }
            set { }
        }
    }
}
