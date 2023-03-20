using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLab.Eyetracking.V2
{
    using G = GameLab.Geometry;

    public class RegionStateChangedEventArgs : EventArgs
    {
        public int RegionId { get; private set; }
        public G.Point GazePosition { get; private set; }
        public G.Point GazePositionRelativeToRegionCenter { get; private set; } //pozwala sprawdzić, którędy spojrzenie wchodzi do regionu
        public double GazePositionAngleRelativeToRegionCenter { get; private set; } //pozwala sprawdzić, czy kierunek wejścia jest taki sam, jak kierunek wyjścia z regionu

        public RegionStateChangedEventArgs(int regionId, G.Point gazePosition, G.Point regionCenterPosition)
        {
            this.RegionId = regionId;
            this.GazePosition = gazePosition;
            int dx = gazePosition.X - regionCenterPosition.X;
            int dy = gazePosition.Y - regionCenterPosition.Y;
            this.GazePositionRelativeToRegionCenter = new G.Point(-dy, dx); //zero do góry
            this.GazePositionAngleRelativeToRegionCenter = Math.Atan2(this.GazePositionRelativeToRegionCenter.Y, this.GazePositionRelativeToRegionCenter.X);
        }
    }

    public class RegionActivationProgressEventArgs : EventArgs
    {
        public int RegionId { get; private set; }
        public TimeSpan TimeInterval { get; private set; }
        public TimeSpan DwellTime { get; private set; }
        public double Percent { get; private set; }

        public RegionActivationProgressEventArgs(int regionId, TimeSpan timeInterval, TimeSpan dwellTime)
        {
            this.RegionId = regionId;
            this.TimeInterval = timeInterval;
            this.DwellTime = dwellTime;
            this.Percent = timeInterval.TotalSeconds / dwellTime.TotalSeconds;
            if (this.Percent > 1) this.Percent = 1;
            //Percent *= 100;            
        }
    }

    public class GazeDwellTimeRegionsManagerSettings //DTO używane przede wszystkim w nowszych projektach (np. GazeTextEntryApps)
    {
        public IEyetracker Eyetracker;
        public GazeSmoothingFilter SmoothingFilter;
        public TimeSpan DwellTime;
        public EyeSide EyeSide;
        public TimeSpan InitialInactivityTime;
        public GazeDwellTimeRegionsManager.ManagerControlType GazeControlType;
        //public G.Rectangle? ContainerBounds;

        public static GazeDwellTimeRegionsManagerSettings Default
        {
            get
            {
                return new GazeDwellTimeRegionsManagerSettings()
                {
                    Eyetracker = null,
                    SmoothingFilter = new GazeSmoothingFilter(SmoothingType.SMA, 5),
                    DwellTime = TimeSpan.FromSeconds(2),
                    EyeSide = EyeSide.AveragedOrBestEye,
                    InitialInactivityTime = TimeSpan.FromSeconds(5),
                    GazeControlType = GazeDwellTimeRegionsManager.ManagerControlType.Active,
                    //ContainerBounds = null
                };
            }
        }
    }

    public class GazeDwellTimeRegionsManager : IDisposable
    {
        public enum ManagerControlType { OnlyPassive, Active }    

        public enum RegionGazeState { Normal, Activation, Reaction }

        protected class RegionData
        {
            public int Id;            
            public G.IShape Region;
            public Action<int, RegionStateChangedEventArgs> ActivationAction;
            public Action<int, RegionStateChangedEventArgs> ReactionAction;
            public Action<int, RegionStateChangedEventArgs> ReturnToNormalAction;
            public Action<int, RegionActivationProgressEventArgs> ActivationProgressAction;
            public RegionGazeState GazeState;
            public DateTime GazeStateStartTime;
            public bool ForceReturnToNormalStateAfterReaction;
            public bool GazeInteractionEnabled = true;

            public G.Point CenterPosition
            {
                get
                {
                    //return new Point((Region.Left + Region.Right) / 2, (Region.Top + Region.Bottom) / 2);
                    return Region.Center.ToPoint();
                }
            }
        }

        //można by przechowywać instancję GazeDwellTimeControlsManagerSettings, ale już nie zmieniam
        private IEyetracker et;
        private GazeSmoothingFilter filter;
        public TimeSpan DwellTime { get; set; }
        private EyeSide eyeSide;
        private DateTime startTime;        
        private TimeSpan initialInactivityTime;
        //private G.Rectangle? containerBounds;
        protected Dictionary<int, RegionData> regions = new Dictionary<int, RegionData>();

        public bool Enabled { get; set; }        

        public G.IShape GetRegionShape(int regionId)
        {
            return regions[regionId].Region;
        }

        //TODO: wywoływanie tej funkcji jest mało wydajne, wystarczy raz i zmieniać przy Update
        public Dictionary<int, G.IShape> GetAllRegionsShapes()
        {
            Dictionary<int, G.IShape> shapes = new Dictionary<int, G.IShape>();
            foreach(RegionData region in regions.Values)
            {
                shapes.Add(region.Id, region.Region);
            }
            return shapes;
        }

        //TODO: rozważyć czy potrzebny container - przetestować!!!!!! 
        public GazeDwellTimeRegionsManager(IEyetracker et, GazeSmoothingFilter filter, TimeSpan dwellTime, EyeSide eyeSide, TimeSpan initialInactivityTime, ManagerControlType controlType = ManagerControlType.Active/*, G.Rectangle? containerBounds = null*/)
        {
            this.et = et;
            this.filter = filter;
            this.DwellTime = dwellTime;
            this.eyeSide = eyeSide;
            this.startTime = DateTime.Now;
            this.initialInactivityTime = initialInactivityTime;

            this.Enabled = true;

            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) => { Update(); };

            this.ControlType = controlType;
            if (this.ControlType == ManagerControlType.Active) timer.Start();

            //this.containerBounds = containerBounds;
        }

        public GazeDwellTimeRegionsManager(GazeDwellTimeRegionsManagerSettings settings)
            :this(settings.Eyetracker, settings.SmoothingFilter, settings.DwellTime, settings.EyeSide, settings.InitialInactivityTime, settings.GazeControlType/*, settings.ContainerBounds*/)
        { }

        //współrzędne region we współrzędnych ekranu
        public int AddRegion(G.IShape region, bool forceReturnToNormalState = false, Action<int, RegionStateChangedEventArgs> reactionAction = null, Action<int, RegionStateChangedEventArgs> activationAction = null, Action<int, RegionStateChangedEventArgs> returnToNormalAction = null, Action<int, RegionActivationProgressEventArgs> activationProgressAction = null)
        {
            //if (!region.HasValue) region = boundsInScreenCoordinates(control);
            RegionData regionData = new RegionData()
            {                
                //Region = region.Value,
                Region = region,
                ActivationAction = activationAction,
                ReactionAction = reactionAction,
                ReturnToNormalAction = returnToNormalAction,
                ActivationProgressAction = activationProgressAction,
                GazeState = RegionGazeState.Normal,
                GazeStateStartTime = DateTime.Now,
                ForceReturnToNormalStateAfterReaction = forceReturnToNormalState
            };
            int id = 0;
            if(regions.Count > 0) id = regions.Keys.Max() + 1;
            regionData.Id = id;
            regions.Add(id, regionData);
            return id;
        }

        //współrzędne region we współrzędnych ekranu
        public void UpdateRegion(int id, G.IShape region)
        {
            RegionData regionData = regions[id];
            regionData.Region = region;
        }

        public void RemoveRegion(int id)
        {
            regions.Remove(id);
        }

        public void RemoveAllRegions()
        {
            regions.Clear();
        }

        public int Count
        {
            get
            {
                return regions.Count;
            }
        }

        public virtual void SuspendRegionGazeInteraction(int regionId)
        {
            RegionData regionData = regions[regionId];
            regionData.GazeInteractionEnabled = false;            
            if (regionData.GazeState != RegionGazeState.Normal)
            {                
                regionData.GazeState = RegionGazeState.Normal;
                regionData.GazeStateStartTime = DateTime.Now;
                G.Point gazePosition = getGazePosition();
                onRegionReturnToNormal(regionData, gazePosition);
            }            
        }

        public void ResumeRegionGazeInteraction(int regionId)
        {
            regions[regionId].GazeInteractionEnabled = true;
        }

        public void AllRegionsToNormalState()
        {
            foreach (RegionData regionData in regions.Values)
            {
                onRegionReturnToNormal(regionData, getGazePosition());
            }
        }

        /*
        private Point toSystemDrawingPoint(G.Point point)
        {
            return new Point(point.X, point.Y);
        }
        */

        protected G.Point getGazePosition()
        {
            EyeDataSample eyeData;
            switch (eyeSide)
            {
                case EyeSide.LeftEye:
                    eyeData = et.LeftEyeData;
                    break;
                case EyeSide.RightEye:
                    eyeData = et.RightEyeData;
                    break;
                case EyeSide.AveragedOrBestEye:
                default:
                    eyeData = et.AveragedEyeData;
                    break;
            }
            if (filter != null) eyeData = filter.SmoothData(eyeData);
            G.Point position = eyeData.Position;
            //return toSystemDrawingPoint(position);
            return position;
        }

        protected virtual void onRegionActivation(RegionData regionData, G.Point gazePosition)
        {
            if (regionData.ActivationAction != null) regionData.ActivationAction(regionData.Id, new RegionStateChangedEventArgs(regionData.Id, gazePosition, regionData.CenterPosition));
        }

        protected virtual void onRegionReaction(RegionData regionData, G.Point gazePosition)
        {
            if (regionData.ReactionAction != null) regionData.ReactionAction(regionData.Id, new RegionStateChangedEventArgs(regionData.Id, gazePosition, regionData.CenterPosition));
        }

        protected virtual void onRegionReturnToNormal(RegionData regionData, G.Point gazePosition)
        {
            if (regionData.ReturnToNormalAction != null) regionData.ReturnToNormalAction(regionData.Id, new RegionStateChangedEventArgs(regionData.Id, gazePosition, regionData.CenterPosition));
        }

        protected virtual void onRegionActivationProgress(RegionData regionData, RegionActivationProgressEventArgs e)
        {
            if (regionData.ActivationProgressAction != null) regionData.ActivationProgressAction(regionData.Id, e);
        }

        public bool IsInitiallyInactive
        {
            get
            {
                return (DateTime.Now - startTime) < initialInactivityTime;
            }
        }

        private bool updateIsRunning = false;

        public virtual void Update()
        {
            if (et == null) return;
            if (updateIsRunning) return;

            //TODO: regiony są zapisywane we współrzędnych ekranu

            G.Point gazePosition = getGazePosition();
            if (IsInitiallyInactive) return;
            if (!Enabled) return; //rozdzielone do debugowania

            updateIsRunning = true;

            DateTime now = DateTime.Now;
            for (int i = 0; i < regions.Count; ++i)
            {
                RegionData regionData = regions.ElementAt(i).Value;
                bool isInRegion = regionData.Region.Contains(gazePosition);
                TimeSpan timeInterval = now - regionData.GazeStateStartTime;

                if (!regionData.GazeInteractionEnabled) continue;

                if (regionData.GazeState != RegionGazeState.Normal && !isInRegion)
                {
                    regionData.GazeState = RegionGazeState.Normal;
                    regionData.GazeStateStartTime = now;
                    onRegionReturnToNormal(regionData, gazePosition);                    
                    //Console.WriteLine(regionData.Control.Name + ":ReturnToNormal");
                    continue;
                }
                if (regionData.GazeState == RegionGazeState.Normal && isInRegion)
                {
                    regionData.GazeState = RegionGazeState.Activation;
                    regionData.GazeStateStartTime = now;
                    onRegionActivation(regionData, gazePosition);
                    //Console.WriteLine(regionData.Control.Name + ":Activation");
                    continue;
                }
                if (regionData.GazeState == RegionGazeState.Activation && isInRegion)
                {
                    RegionActivationProgressEventArgs e = new RegionActivationProgressEventArgs(regionData.Id, timeInterval, DwellTime);
                    onRegionActivationProgress(regionData, e);
                    //Console.WriteLine(regionData.Control.Name + ":Activation-progress:" + (100*e.Percent).ToString());
                    if(timeInterval >= DwellTime)
                    {
                        regionData.GazeState = RegionGazeState.Reaction;
                        regionData.GazeStateStartTime = now;                        
                        onRegionReaction(regionData, gazePosition);
                        //Console.WriteLine(regionData.Control.Name + ":Reaction");
                        if (regionData.ForceReturnToNormalStateAfterReaction) regionData.GazeState = RegionGazeState.Normal;
                    }                    
                    continue;                    
                }
            }
            updateIsRunning = false;
        }

        #region Regions
        public RegionGazeState GetRegionState(int regionId)
        {
            return regions[regionId].GazeState;
        }

        public int[] GetRegionsInState(RegionGazeState state)
        {
            IEnumerable<int> regionsInState = from RegionData regionData in regions
                                              where regionData.GazeState == state
                                              select regionData.Id;
            return regionsInState.ToArray();
        }

        public int[] RegionsAtGazePosition
        {
            get
            {
                G.Point gazePosition = getGazePosition();
                IEnumerable<int> regionsAtGazePosition = from RegionData regionData in regions
                                                         where regionData.Region.Contains(gazePosition)
                                                         select regionData.Id;
                return regionsAtGazePosition.ToArray();
            }
        }

        public int GetRegionClosestToGazePosition()
        {
            G.Point gazePosition = getGazePosition();

            int minimalDistanceSquare = int.MaxValue;
            int regionIdAtMinimalDistance = -1;
            for (int i = 0; i < regions.Count; ++i)
            {
                G.IShape region = regions.ElementAt(i).Value.Region;
                //Point center = new Point(region.Left + region.Width / 2, region.Top + region.Height / 2);
                G.Point center = region.Center.ToPoint();
                int x = gazePosition.X - center.X;
                int y = gazePosition.Y - center.Y;
                int distanceSquare = x * x + y * y;
                if (distanceSquare < minimalDistanceSquare)
                {
                    minimalDistanceSquare = distanceSquare;
                    regionIdAtMinimalDistance = regions.ElementAt(i).Value.Id;
                }
            }
            return regionIdAtMinimalDistance;
        }
        #endregion

        #region Active management
        public ManagerControlType ControlType
        {
            get
            {
                return (timer.Enabled) ? ManagerControlType.Active : ManagerControlType.OnlyPassive;
            }
            set
            {
                switch (value)
                {
                    case ManagerControlType.OnlyPassive:
                        timer.Enabled = false;
                        break;
                    case ManagerControlType.Active:
                        timer.Enabled = true;
                        break;
                }
            }
        }

        private System.Timers.Timer timer; //wszystkie wywołania metod są w wątku, który nie jest wątkiem UI -> kombinacje po stronie klienta

        public double ActiveControlInterval
        {
            get
            {
                return timer.Interval;
            }
            set
            {
                timer.Interval = value;
            }
        }
        #endregion

        /*
        public void InvokeRegionReaction(int regionId)
        {
            onRegionReaction(regions[regionId], getGazePosition());
        }
        */

        public void Dispose()
        {
            Enabled = false;
            timer.Stop();
            RemoveAllRegions();
        }

        public GameLab.Geometry.PointF SmoothedGazePosition
        {
            get
            {
                return filter.CalculateSmoothedGazePosition();
            }
        }

        public double SmoothedGazeDirection
        {
            get
            {
                return filter.GetSmoothedGazeDirection();
            }
        }
    }
}
