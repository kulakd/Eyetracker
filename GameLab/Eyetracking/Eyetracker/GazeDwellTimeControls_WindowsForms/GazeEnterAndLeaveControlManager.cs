using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameLab.Eyetracking
{
    using G = GameLab.Geometry;

    public enum AngleCalculatingAlgorithm { Position, Velocity }

    public class GazeEnterAndLeaveSettings
    {
        public int AngleThresholdDeg = 60;
        public TimeSpan MinimalTimeThreshold = TimeSpan.FromMilliseconds(50);
        public TimeSpan MaximalTimeThreshold = TimeSpan.FromMilliseconds(1000);
        public AngleCalculatingAlgorithm AngleCalculatingAlgorithm = AngleCalculatingAlgorithm.Position;

        public static GazeEnterAndLeaveSettings Default
        {
            get
            {
                return new GazeEnterAndLeaveSettings(); //niepotrzebne, bo domyślny konstruktor daje to samo
            }
        }
    }

    public class GazeEnterAndLeaveControlManager : IDisposable
    {
        private class RegionData //reszta danych jest w GazeDwellTimeControlManager.RegionData
        {
            public int Id; //uzgodnienie z RegionData w GazeDwellTimeControlManager
            public Action<int, RegionStateChangedEventArgs> Action;
        }

        //private GazeSmoothingFilter filter;
        private GazeEnterAndLeaveSettings settings;
        private GazeDwellTimeControlsManager dwellTimeManager = null;
        private Dictionary<int, RegionData> regions = new Dictionary<int,RegionData>();        

        public bool Enabled
        {
            get
            {
                return dwellTimeManager.Enabled;
            }
            set
            {
                dwellTimeManager.Enabled = value;
            }
        }

        public GazeEnterAndLeaveControlManager(IEyetracker et, GazeSmoothingFilter filter, EyeSide eyeSide, TimeSpan initialInactivityTime, GazeEnterAndLeaveSettings settings)
        {
            //this.filter = filter;
            this.settings = settings;
            dwellTimeManager = new GazeDwellTimeControlsManager(et, filter, TimeSpan.FromMinutes(1), TimeSpan.Zero, eyeSide, initialInactivityTime, GazeDwellTimeControlsManager.ManagerControlType.Active); //ograniczam typ tylko do aktywnego tj. sam manager zgłasza zdarzenia (z lenistwa)
        }

        public void UpdateSettings(GazeEnterAndLeaveSettings settings)
        {
            this.settings = settings;
        }

        private int lastEnterRegionId = -1, lastLeaveRegionId = -1;
        private DateTime lastEnterRegionTime = DateTime.Now, lastLeaveRegionTime = DateTime.Now;
        private int lastEnterAngle = -1, lastLeaveAngle = -1;
        //private PointF lastEnterVelocity = PointF.Empty, lastLeaveVelocity = PointF.Empty;

        private int rad2deg(double angleRadians)
        {
            int degrees = (int)Math.Round(180 * angleRadians / Math.PI);
            if (degrees < 0) degrees += 360;
            return degrees;
        }

        Point previousGazePosition = Point.Empty;

        private double calculateGazeDirection(Point gazePosition)
        {
            int dx = gazePosition.X - previousGazePosition.X;
            int dy = gazePosition.Y - previousGazePosition.Y;
            previousGazePosition = gazePosition;
            return Math.Atan2(dx, -dy); //zero do góry
        }

        private void enterAction(int regionId, RegionStateChangedEventArgs e)
        {
            lastEnterRegionId = regionId;
            lastEnterRegionTime = DateTime.Now;
            switch(settings.AngleCalculatingAlgorithm)
            {
                default:
                case AngleCalculatingAlgorithm.Position:
                    lastEnterAngle = rad2deg(e.GazePositionAngleRelativeToRegionCenter);
                    break;
                case AngleCalculatingAlgorithm.Velocity:
                    double gazeDirection = calculateGazeDirection(e.GazePosition);
                    lastEnterAngle = rad2deg(gazeDirection);
                    //MessageBox.Show("E: " + lastEnterAngle.ToString());
                    break;
            }
        }

        private void leaveAction(int regionId, RegionStateChangedEventArgs e)
        {
            lastLeaveRegionId = regionId;
            lastLeaveRegionTime = DateTime.Now;
            switch(settings.AngleCalculatingAlgorithm)
            {
                default:
                case AngleCalculatingAlgorithm.Position:
                    lastLeaveAngle = rad2deg(e.GazePositionAngleRelativeToRegionCenter);
                    break;
                case AngleCalculatingAlgorithm.Velocity:
                    //double gazeDirection = calculateGazeDirection(e.GazePosition);
                    double gazeDirection = dwellTimeManager.SmoothedGazeDirection;
                    if (gazeDirection == double.NaN) gazeDirection = calculateGazeDirection(e.GazePosition);
                    lastLeaveAngle = rad2deg(gazeDirection);
                    //MessageBox.Show("L: " + lastLeaveAngle.ToString());
                    break;
            }            
            TimeSpan duration = lastLeaveRegionTime - lastEnterRegionTime;
            int angleDifference = Math.Abs(lastLeaveAngle - lastEnterAngle);
            bool samePositionDirection = angleDifference < settings.AngleThresholdDeg || angleDifference > 360 - settings.AngleThresholdDeg;

            if (lastLeaveRegionId == lastEnterRegionId && duration > settings.MinimalTimeThreshold && duration < settings.MaximalTimeThreshold && samePositionDirection)
            {
                RegionData region = regions[lastEnterRegionId];
                Control control = dwellTimeManager.GetControl(region.Id);
                if (region.Action != null) region.Action(region.Id, e);
                onControlAction(control, e.GazePosition);
            }
        }

        public int AddRegion(Control control, G.IShape region = null, Action<int, RegionStateChangedEventArgs> action = null)
        {
            int id = dwellTimeManager.AddRegion(control, region, false, null, enterAction, leaveAction, null);
            regions.Add(id, new RegionData() { Id = id, Action = action });
            return id;
        }

        public int AddRegion(G.IShape region, Action<int, RegionStateChangedEventArgs> action = null)
        {
            return AddRegion(null, region, action);
        }

        public void UpdateAllControlsRegions()
        {
            dwellTimeManager.UpdateAllControlsRegions();
        }

        #region Events
        //zdarzenia, które są tylko gdy z region związana jest kontrolka
        private void onControlAction(Control control, Point gazePosition)
        {
            if (ControlAction != null && control != null) ControlAction(control, new ControlGazeStateChangedEventArgs(control, gazePosition));
        }

        public event EventHandler<ControlGazeStateChangedEventArgs> ControlAction;
        #endregion

        #region Udostępnianie metod i własności
        public void DrawRegions(Graphics g, Pen pen/*, Brush brush*/, bool debugInformation = false, Control container = null)
        {
            dwellTimeManager.DrawRegions(g, pen, debugInformation, container);
        }

        public Control GetControl(int regionId) //może być null, gdy region bez kontrolki
        {
            return dwellTimeManager.GetControl(regionId);
        }

        public G.IShape GetRegionLocation(int regionId)
        {
            return dwellTimeManager.GetRegionShape(regionId);
        }

        public bool IsInitiallyInactive
        {
            get
            {
                return dwellTimeManager.IsInitiallyInactive;
            }
        }

        public void SuspendRegionGazeInteraction(int regionId)
        {
            dwellTimeManager.SuspendRegionGazeInteraction(regionId);
        }

        public void ResumeRegionGazeInteraction(int regionId)
        {
            dwellTimeManager.ResumeRegionGazeInteraction(regionId);
        }

        public void SuspendRegionGazeInteraction(Control control, bool hideControl)
        {
            dwellTimeManager.SuspendRegionGazeInteraction(control, hideControl);
        }

        public void ResumeRegionGazeInteraction(Control control, bool showControl)
        {
            dwellTimeManager.ResumeRegionGazeInteraction(control, showControl);
        }
        #endregion

        public void Dispose()
        {
            Enabled = false;
            dwellTimeManager.Dispose();
        }

        public GameLab.Geometry.PointF SmoothedGazePosition
        {
            get
            {
                return dwellTimeManager.SmoothedGazePosition;
            }
        }

        public double SmoothedGazeDirection
        {
            get
            {
                return dwellTimeManager.SmoothedGazeDirection;
            }
        }
    }
}
