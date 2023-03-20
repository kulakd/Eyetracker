using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//TODO: Ta klasa jest bardzo podobna do dwóch pozostałych - warto rozważyć wspólną klasę bazową, chociażby do zarządzania regionami
//zarządca regionami parametryzowany z interfejsem IRegionData

namespace GameLab.Eyetracking
{
    using GazeRuntimeAnalysis;

    public class GazeFixationControlsManager : IDisposable
    {
        public enum ControlGazeState { Normal, Reaction } //jak rozszerzyć o activation

        private class RegionData
        {
            public int Id;
            public Control Control; //to w zasadzie wcale nie jest potrzebne (służy tylko jako identyfikator), por. GCAF
            public Rectangle Region;
            public Action<int, RegionStateChangedEventArgs> ReactionAction;
            public Action<int, RegionStateChangedEventArgs> ReturnToNormalAction;
            public ControlGazeState GazeState;
            public DateTime GazeStateStartTime;
            public bool GazeInteractionEnabled = true;

            public Point CenterPosition
            {
                get
                {
                    return new Point((Region.Left + Region.Right) / 2, (Region.Top + Region.Bottom) / 2);
                }
            }
        }
        
        private IEyetracker et;
        private IGazeRuntimeAnalyser analyser;
        private DateTime startTime;
        private TimeSpan initialInactivityTime;
        private Dictionary<int, RegionData> regions = new Dictionary<int, RegionData>();
        public bool Enabled { get; set; }

        public GazeFixationControlsManager(IEyetracker et, GazeEventsRuntimeAnalysisSettings analyserSettings, EyeSide eyeSide, double timerIntervalMiliseconds, TimeSpan initialInactivityTime)
        {
            this.et = et;
            analyser = new GazeRuntimeAnalysis.GazeRuntimeAnalyser(et, analyserSettings, GazeRuntimeAnalysis.GazeRuntimeAnalyser.EventRaisingScheme.UseTimer, timerIntervalMiliseconds);
            this.startTime = DateTime.Now;
            this.initialInactivityTime = initialInactivityTime;

            switch(eyeSide)
            {
                case EyeSide.LeftEye:
                    analyser.LeftEyeStateChanged += analyser_EyeStateChanged;
                    break;
                case EyeSide.RightEye:
                    analyser.RightEyeStateChanged += analyser_EyeStateChanged;
                    break;
                case EyeSide.AveragedOrBestEye:
                    analyser.AveragedEyeStateChanged += analyser_EyeStateChanged;
                    break;
            }            

            this.Enabled = true;
        }

        void analyser_EyeStateChanged(EyeState eyeData)
        {
            if (!Enabled) return;

            switch(eyeData.CurrentEvent)
            {
                case GazeEvent.Fixation:
                    GameLab.Geometry.Point fixationStartPosition = eyeData.StartPosition.ToPoint();
                    Point fixationPosition = new Point(fixationStartPosition.X, fixationStartPosition.Y);
                    foreach (RegionData regionData in regions.Values)
                    {
                        bool handled = false;
                        if (regionData.Region.Contains(fixationPosition) && regionData.GazeState != ControlGazeState.Reaction)
                        {
                            handled = true;
                            regionData.GazeState = ControlGazeState.Reaction;
                            onRegionReaction(regionData, fixationPosition);
                            onControlReaction(regionData.Control, fixationPosition);
                        }
                        if (!handled && !regionData.Region.Contains(fixationPosition) && regionData.GazeState != ControlGazeState.Normal)
                        {
                            handled = true;
                            regionData.GazeState = ControlGazeState.Normal;
                            onRegionReturnToNormal(regionData, fixationPosition);
                            onControlReturnToNormal(regionData.Control, fixationPosition);                            
                        }
                    }
                    break;
                case GazeEvent.Saccade:
                    foreach(RegionData regionData in regions.Values)
                    {
                        if(regionData.GazeState != ControlGazeState.Normal)
                        {
                            regionData.GazeState = ControlGazeState.Normal;
                            GameLab.Geometry.Point saccadeStartPosition = eyeData.StartPosition.ToPoint();
                            Point saccadePosition = new Point(saccadeStartPosition.X, saccadeStartPosition.Y);
                            onRegionReturnToNormal(regionData, saccadePosition);
                            onControlReturnToNormal(regionData.Control, saccadePosition);
                        }
                    }
                    break;
            }
        }

        public Control GetControl(int regionId) //może być null, gdy region bez kontrolki
        {
            return regions[regionId].Control;
        }

        public Rectangle GetRegionLocation(int regionId)
        {
            return regions[regionId].Region;
        }

        private Rectangle boundsInScreenCoordinates(Control control)
        {
            Control container = control.Parent;
            Rectangle bounds = control.Bounds;
            if (container != null)
            {
                //Point location = container.PointToScreen(bounds.Location);
                //bounds.Location = location;
                //Rectangle originalBounds = bounds;
                bounds = container.RectangleToScreen(bounds);
                //Rectangle testBounds = container.RectangleToClient(bounds);
            }
            return bounds;
        }

        public int AddRegion(Control control, Rectangle? region = null, Action<int, RegionStateChangedEventArgs> reactionAction = null, Action<int, RegionStateChangedEventArgs> returnToNormalAction = null)
        {
            if (!region.HasValue) region = boundsInScreenCoordinates(control);
            RegionData regionData = new RegionData()
            {
                Control = control,
                Region = region.Value,
                ReactionAction = reactionAction,
                ReturnToNormalAction = returnToNormalAction,
                GazeState = ControlGazeState.Normal,
                GazeStateStartTime = DateTime.Now
            };
            int id = 0;
            if (regions.Count > 0) id = regions.Keys.Max() + 1;
            regionData.Id = id;
            regions.Add(id, regionData);
            return id;
        }

        private int findControlId(Control control)
        {
            int id = regions.First(p => p.Value.Control == control).Key;
            return id;
        }

        //współrzędne region we współrzędnych ekranu
        public int AddRegion(Rectangle region, Action<int, RegionStateChangedEventArgs> reactionAction = null, Action<int, RegionStateChangedEventArgs> returnToNormalAction = null)
        {
            return AddRegion(null, region, reactionAction, returnToNormalAction);
        }

        public void UpdateRegion(int id, Rectangle region)
        {
            RegionData regionData = regions[id];
            regionData.Region = region;
        }

        public void UpdateRegion(Control control, Rectangle? region = null)
        {
            if (!region.HasValue) region = boundsInScreenCoordinates(control);
            int id = findControlId(control);
            UpdateRegion(id, region.Value);
        }

        public void UpdateAllControlsRegions()
        {
            foreach (RegionData regionData in regions.Values)
                if (regionData.Control != null)
                {
                    UpdateRegion(regionData.Control);
                }
        }

        public void RemoveRegion(int id)
        {
            regions.Remove(id);
        }

        public void RemoveRegion(Control control)
        {
            int id = findControlId(control);
            RemoveRegion(id);
        }

        private void onRegionReaction(RegionData regionData, Point gazePosition)
        {
            if (regionData.ReactionAction != null) regionData.ReactionAction(regionData.Id, new RegionStateChangedEventArgs(regionData.Id, regionData.Control, gazePosition, regionData.CenterPosition));
        }

        private void onRegionReturnToNormal(RegionData regionData, Point gazePosition)
        {
            if (regionData.ReturnToNormalAction != null) regionData.ReturnToNormalAction(regionData.Id, new RegionStateChangedEventArgs(regionData.Id, regionData.Control, gazePosition, regionData.CenterPosition));
        }

        public bool IsInitiallyInactive
        {
            get
            {
                return (DateTime.Now - startTime) < initialInactivityTime;
            }
        }


        #region Events
        //zdarzenia, które są tylko gdy z region związana jest kontrolka
        private void onControlReturnToNormal(Control control, Point gazePosition)
        {
            if (ControlReturnToNormal != null && control != null) ControlReturnToNormal(control, new ControlGazeStateChangedEventArgs(control, gazePosition));
        }
        private void onControlReaction(Control control, Point gazePosition)
        {
            if (ControlReaction != null && control != null) ControlReaction(control, new ControlGazeStateChangedEventArgs(control, gazePosition));
        }

        public event EventHandler<ControlGazeStateChangedEventArgs> ControlReturnToNormal;
        public event EventHandler<ControlGazeStateChangedEventArgs> ControlReaction;
        #endregion

        public void DrawRegions(Graphics g, Pen pen, bool debugInformation = false, Control container = null)
        {
            //UpdateControlRegions();

            int penWidth = (int)pen.Width / 2;
            for (int i = 0; i < regions.Count; ++i)
            {
                RegionData regionData = regions.ElementAt(i).Value;
                if (!regionData.GazeInteractionEnabled) continue;
                Rectangle region = regionData.Region;
                if (container != null) region = container.RectangleToClient(region);
                Rectangle _region = new Rectangle(region.Left - 1 - penWidth, region.Top - 1 - penWidth, region.Width + 1 + 2 * penWidth, region.Height + 1 + 2 * penWidth);
                if (pen != null) g.DrawRectangle(pen, _region);
                if (debugInformation) g.DrawString(regionData.GazeState.ToString(), new Font(FontFamily.GenericSansSerif, 10), Brushes.Black, region.Left, region.Top);

                //trzeba sprawdzić kąt wejscia i wyjscia i wyłączyć kółka
            }

            //Point gazePosition = getGazePosition();
            //int size = 6;
            //g.FillEllipse(Brushes.Navy, gazePosition.X - size / 2, gazePosition.Y - size / 2, size, size);
        }

        public void Dispose()
        {
            Enabled = false;  
        }
    }
}
