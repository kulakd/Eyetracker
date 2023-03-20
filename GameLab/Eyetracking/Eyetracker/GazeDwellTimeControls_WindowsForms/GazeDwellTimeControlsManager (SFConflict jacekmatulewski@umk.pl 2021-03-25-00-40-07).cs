using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//Z założenie metody publiczne używają Point z przestrzeni System.Drawing, 
//bo menedżer będzie używany w projektach aplikacji Windows Forms.
//Ale IShape jest z GameLab.Geometry

//można pomyśleć o rozdzieleniu części opartej na GameLab.Geometry i IEyetracker i części Windows Forms,
//ale to nie ma sensu, póki nie będzie w planie wersji WPF - a tej na razie nie ma

namespace GameLab.Eyetracking
{
    using D = System.Drawing;
    using G = GameLab.Geometry;
    using GameLab.Geometry.WindowsForms;

    public class ControlGazeStateChangedEventArgs : EventArgs
    {
        public Control Control { get; private set; }
        public D.Point GazePosition { get; private set; }

        public ControlGazeStateChangedEventArgs(Control control, D.Point gazePosition)
        {
            this.Control = control;
            this.GazePosition = gazePosition;
        }
    }

    public class RegionStateChangedEventArgs : EventArgs
    {
        public int RegionId { get; private set; }
        public Control Control { get; private set; } //może być null
        public D.Point GazePosition { get; private set; }
        public D.Point GazePositionRelativeToRegionCenter { get; private set; } //pozwala sprawdzić, którędy spojrzenie wchodzi do regionu
        public double GazePositionAngleRelativeToRegionCenter { get; private set; } //pozwala sprawdzić, czy kierunek wejścia jest taki sam, jak kierunek wyjścia z regionu

        public RegionStateChangedEventArgs(int regionId, Control control, D.Point gazePosition, D.Point regionCenterPosition)
        {
            this.RegionId = regionId;
            this.Control = control;
            this.GazePosition = gazePosition;
            int dx = gazePosition.X - regionCenterPosition.X;
            int dy = gazePosition.Y - regionCenterPosition.Y;
            this.GazePositionRelativeToRegionCenter = new D.Point(-dy, dx); //zero do góry
            this.GazePositionAngleRelativeToRegionCenter = Math.Atan2(this.GazePositionRelativeToRegionCenter.Y, this.GazePositionRelativeToRegionCenter.X);
        }
    }

    public class RegionActivationProgressEventArgs : EventArgs
    {
        public int RegionId { get; private set; }
        public Control Control { get; private set; } //może być null
        public TimeSpan TimeInterval { get; private set; }
        public TimeSpan DwellTime { get; private set; }
        public double Percent { get; private set; }

        public RegionActivationProgressEventArgs(int regionId, Control control, TimeSpan timeInterval, TimeSpan dwellTime)
        {
            this.RegionId = regionId;
            this.Control = control;
            this.TimeInterval = timeInterval;
            this.DwellTime = dwellTime;
            this.Percent = timeInterval.TotalSeconds / dwellTime.TotalSeconds;
            if (this.Percent > 1) this.Percent = 1;
            //Percent *= 100;            
        }
    }

    public class GazeDwellTimeControlsManagerSettings //DTO używane przede wszystkim w nowszych projektach (np. GazeTextEntryApps)
    {
        public IEyetracker Eyetracker;
        public GazeSmoothingFilter SmoothingFilter;
        public TimeSpan DefaultActivationTime, DefaultDwellTime;
        public EyeSide EyeSide;
        public TimeSpan InitialInactivityTime;
        public GazeDwellTimeControlsManager.ManagerControlType GazeControlType;
        public Control Container;

        public static GazeDwellTimeControlsManagerSettings Default
        {
            get
            {
                return new GazeDwellTimeControlsManagerSettings()
                {
                    Eyetracker = null,
                    SmoothingFilter = new GazeSmoothingFilter(SmoothingType.SMA, 5),
                    DefaultActivationTime = TimeSpan.Zero,
                    DefaultDwellTime = TimeSpan.FromSeconds(2),
                    EyeSide = EyeSide.AveragedOrBestEye,
                    InitialInactivityTime = TimeSpan.FromSeconds(5),
                    GazeControlType = GazeDwellTimeControlsManager.ManagerControlType.Active,
                    Container = null
                };
            }
        }
    }

    public class GazeDwellTimeControlsManager : IDisposable
    {
        public enum ManagerControlType { OnlyPassive, Active }    

        public enum ControlGazeState { Normal, GazeInRegion, Activation, Reaction }

        private class RegionData
        {
            public int Id;
            public Control Control; //to w zasadzie wcale nie jest potrzebne (służy tylko jako identyfikator), por. GCAF
            public G.IShape Region;
            public TimeSpan DwellTime; //można by też dodać ActivationTime, ale to raczej cecha interfejsu
            public Action<int, RegionStateChangedEventArgs> ActivationAction;
            public Action<int, RegionStateChangedEventArgs> ReactionAction;
            public Action<int, RegionStateChangedEventArgs> ReturnToNormalAction;
            public Action<int, RegionActivationProgressEventArgs> ActivationProgressAction;
            public ControlGazeState GazeState;
            public DateTime GazeStateStartTime;
            public bool ForceReturnToNormalStateAfterReaction;
            public bool GazeInteractionEnabled = true;

            public D.Point CenterPosition
            {
                get
                {
                    //return new Point((Region.Left + Region.Right) / 2, (Region.Top + Region.Bottom) / 2);
                    return Region.Center.ToPoint().ToSystemDrawingPoint();
                }
            }
        }

        //można by przechowywać instancję GazeDwellTimeControlsManagerSettings, ale już nie zmieniam
        private IEyetracker et;
        private GazeSmoothingFilter filter;
        private TimeSpan activationTime;
        public TimeSpan DefaultDwellTime { get; set; }
        private EyeSide eyeSide;
        private DateTime startTime;        
        private TimeSpan initialInactivityTime;
        private Control container;
        private Dictionary<int, RegionData> regions = new Dictionary<int, RegionData>();

        public bool Enabled { get; set; }
        
        public Control GetControl(int regionId) //może być null, gdy region bez kontrolki
        {
            return regions[regionId].Control;
        }

        public G.IShape GetRegionShape(int regionId) //TODO: zmienić na GetRegion?
        {
            return regions[regionId].Region;
        }

        //TODO: rozważyć czy potrzebny container - przetestować!!!!!! 
        public GazeDwellTimeControlsManager(IEyetracker et, GazeSmoothingFilter filter, TimeSpan defaultDwellTime, TimeSpan defaultActivationTime, EyeSide eyeSide, TimeSpan initialInactivityTime, ManagerControlType controlType = ManagerControlType.Active, Control container = null)
        {
            this.et = et;
            this.filter = filter;            
            this.DefaultDwellTime = defaultDwellTime;
            this.activationTime = defaultActivationTime;
            this.eyeSide = eyeSide;
            this.startTime = DateTime.Now;
            this.initialInactivityTime = initialInactivityTime;

            if (this.activationTime > this.DefaultDwellTime) throw new GameLabException("Activation time must be equal or shorter than dwell-time");

            this.Enabled = true;

            timer = new Timer();
            timer.Interval = 10; //Uwaga! To arbitralnie wybrany czas!!!!!!!!!!!!!!!!!!!!!!!
            timer.Tick += (object sender, EventArgs e) => { Update(); };

            this.ControlType = controlType;
            if (this.ControlType == Eyetracking.GazeDwellTimeControlsManager.ManagerControlType.Active) timer.Start();

            this.container = container;
        }

        public GazeDwellTimeControlsManager(GazeDwellTimeControlsManagerSettings settings)
            :this(settings.Eyetracker, settings.SmoothingFilter, settings.DefaultDwellTime, settings.DefaultActivationTime, settings.EyeSide, settings.InitialInactivityTime, settings.GazeControlType, settings.Container)
        { }

        private G.Rectangle boundsInScreenCoordinates(Control control)
        {
            Control _container = control.Parent;
            if (this.container != null) _container = this.container;
            D.Rectangle bounds = control.Bounds;
            if (_container != null)
            {
                //Point location = _container.PointToScreen(bounds.Location);
                //bounds.Location = location;
                //Rectangle originalBounds = bounds;
                bounds = _container.RectangleToScreen(bounds); //TODO: zły wynik, bo dziwne Bound w control.Parent
                //Rectangle testBounds = _container.RectangleToClient(bounds);
            }
            //return bounds;
            return bounds.ToGameLabRectangle();
        }

        //współrzędne region we współrzędnych ekranu
        public int AddRegion(Control control, G.IShape region = null, bool forceReturnToNormalState = false, Action<int, RegionStateChangedEventArgs> reactionAction = null, Action<int, RegionStateChangedEventArgs> activationAction = null, Action<int, RegionStateChangedEventArgs> returnToNormalAction = null, Action<int, RegionActivationProgressEventArgs> activationProgressAction = null, TimeSpan? dwellTime = null)
        {
            //if (!region.HasValue) region = boundsInScreenCoordinates(control);
            if (region == null) region = boundsInScreenCoordinates(control);
            RegionData regionData = new RegionData()
            {
                Control = control,
                //Region = region.Value,
                Region = region,
                DwellTime = (dwellTime.HasValue) ? dwellTime.Value : DefaultDwellTime,
                ActivationAction = activationAction,
                ReactionAction = reactionAction,
                ReturnToNormalAction = returnToNormalAction,
                ActivationProgressAction = activationProgressAction,
                GazeState = ControlGazeState.Normal,
                GazeStateStartTime = DateTime.Now,
                ForceReturnToNormalStateAfterReaction = forceReturnToNormalState
            };
            int id = 0;
            if(regions.Count > 0) id = regions.Keys.Max() + 1;
            regionData.Id = id;
            regions.Add(id, regionData);
            return id;
        }

        private int findControlId(Control control)
        {            
            //TODO: tu pojawiają się błędy informujące, że nie ma elementu w zbiorze
            int id = regions.First(p => p.Value.Control == control).Key;
            return id;
        }

        //współrzędne region we współrzędnych ekranu
        public int AddRegion(G.IShape region, bool forceReturnToNormalState = false, Action<int, RegionStateChangedEventArgs> reactionAction = null, Action<int, RegionStateChangedEventArgs> activationAction = null, Action<int, RegionStateChangedEventArgs> returnToNormalAction = null, Action<int, RegionActivationProgressEventArgs> activationProgressAction = null, TimeSpan? dwellTime = null)
        {
            return AddRegion(null, region, forceReturnToNormalState, reactionAction, activationAction, returnToNormalAction, activationProgressAction, dwellTime);
        }

        //współrzędne region we współrzędnych ekranu
        public void UpdateRegion(int id, G.IShape region)
        {
            RegionData regionData = regions[id];
            regionData.Region = region;
        }

        public void UpdateRegion(Control control, G.IShape region = null)
        {            
            if (region == null) region = boundsInScreenCoordinates(control);
            int id = findControlId(control);
            UpdateRegion(id, region);
        }

        public void UpdateAllControlsRegions()
        {
            foreach(RegionData regionData in regions.Values)
                if(regionData.Control != null)
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

        public void SuspendRegionGazeInteraction(int regionId)
        {
            RegionData regionData = regions[regionId];            
            regionData.GazeInteractionEnabled = false;            
            if (regionData.GazeState != ControlGazeState.Normal)
            {
                regionData.GazeState = ControlGazeState.Normal;
                regionData.GazeStateStartTime = DateTime.Now;
                D.Point gazePosition = getGazePosition();
                onRegionReturnToNormal(regionData, gazePosition);
                onControlReturnToNormal(regionData.Control, gazePosition);
            }            
        }

        public void ResumeRegionGazeInteraction(int regionId)
        {
            regions[regionId].GazeInteractionEnabled = true;
        }

        public void SuspendRegionGazeInteraction(Control control, bool hideControl)
        {
            int id = findControlId(control);
            SuspendRegionGazeInteraction(id);
            if (hideControl) control.Hide();
        }

        public void ResumeRegionGazeInteraction(Control control, bool showControl)
        {
            int id = findControlId(control);
            ResumeRegionGazeInteraction(id);
            if (showControl) control.Show();
        }

        public void AllRegionsToNormalState()
        {
            foreach (RegionData regionData in regions.Values)
            {
                regionData.GazeStateStartTime = DateTime.Now; //TODO: zmiana, sprawdzić w GTE
                onRegionReturnToNormal(regionData, getGazePosition());
            }
        }

        private D.Point getGazePosition()
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
            return position.ToSystemDrawingPoint();
        }

        private void onRegionActivation(RegionData regionData, D.Point gazePosition)
        {
            if (regionData.ActivationAction != null) regionData.ActivationAction(regionData.Id, new RegionStateChangedEventArgs(regionData.Id, regionData.Control, gazePosition, regionData.CenterPosition));
        }

        private void onRegionReaction(RegionData regionData, D.Point gazePosition)
        {
            if (regionData.ReactionAction != null) regionData.ReactionAction(regionData.Id, new RegionStateChangedEventArgs(regionData.Id, regionData.Control, gazePosition, regionData.CenterPosition));
        }

        private void onRegionReturnToNormal(RegionData regionData, D.Point gazePosition)
        {
            if (regionData.ReturnToNormalAction != null) regionData.ReturnToNormalAction(regionData.Id, new RegionStateChangedEventArgs(regionData.Id, regionData.Control, gazePosition, regionData.CenterPosition));
        }

        private void onRegionActivationProgress(RegionData regionData, RegionActivationProgressEventArgs e)
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

        public void Update()
        {
            if (et == null) return;
            if (updateIsRunning) return;

            //TODO: regiony są zapisywane we współrzędnych ekranu

            D.Point gazePosition = getGazePosition();
            if (IsInitiallyInactive) return;
            if (!Enabled) return; //rozdzielone do debugowania

            updateIsRunning = true;

            DateTime now = DateTime.Now;
            for (int i = 0; i < regions.Count; ++i)
            {
                RegionData regionData = regions.ElementAt(i).Value;
                bool isInRegion = regionData.Region.Contains(gazePosition.ToGameLabPoint());
                TimeSpan timeInterval = now - regionData.GazeStateStartTime;

                if (!regionData.GazeInteractionEnabled) continue;

                if (regionData.GazeState != ControlGazeState.Normal && !isInRegion)
                {
                    regionData.GazeState = ControlGazeState.Normal;
                    regionData.GazeStateStartTime = now;
                    onRegionReturnToNormal(regionData, gazePosition);
                    onControlReturnToNormal(regionData.Control, gazePosition);
                    //Console.WriteLine(regionData.Control.Name + ":ReturnToNormal");
                    continue;
                }
                if (regionData.GazeState == ControlGazeState.Normal && isInRegion && timeInterval >= activationTime)
                {
                    //regionData.GazeState = ControlGazeState.Activation;
                    regionData.GazeState = ControlGazeState.GazeInRegion;
                    regionData.GazeStateStartTime = now;
                    //onRegionActivation(regionData, gazePosition);
                    //onControlActivation(regionData.Control, gazePosition);
                    //Console.WriteLine(regionData.Control.Name + ":GazeInRegion");
                    continue;
                }
                //ukryty dodatkowy stan (normalny, ale już w którymś regionie)
                if (regionData.GazeState == ControlGazeState.GazeInRegion && isInRegion && timeInterval >= activationTime)
                {
                    regionData.GazeState = ControlGazeState.Activation;
                    onRegionActivation(regionData, gazePosition);
                    onControlActivation(regionData.Control, gazePosition);
                    //nie resetuję czasu
                    //Console.WriteLine(regionData.Control.Name + ":Activation");
                }
                if (regionData.GazeState == ControlGazeState.Activation && isInRegion)
                {
                    RegionActivationProgressEventArgs e = new RegionActivationProgressEventArgs(regionData.Id, regionData.Control, timeInterval, regionData.DwellTime);
                    onRegionActivationProgress(regionData, e);
                    onControlActivationProgress(regionData.Control, e);
                    //Console.WriteLine(regionData.Control.Name + ":Activation-progress:" + (100*e.Percent).ToString());
                    if(timeInterval >= regionData.DwellTime)
                    {
                        regionData.GazeState = ControlGazeState.Reaction;
                        regionData.GazeStateStartTime = now;                        
                        onRegionReaction(regionData, gazePosition);
                        onControlReaction(regionData.Control, gazePosition);
                        //Console.WriteLine(regionData.Control.Name + ":Reaction");
                        if (regionData.ForceReturnToNormalStateAfterReaction) regionData.GazeState = ControlGazeState.Normal;
                    }                    
                    continue;                    
                }
            }
            updateIsRunning = false;
        }

        #region Regions
        public ControlGazeState GetRegionState(int regionId)
        {
            return regions[regionId].GazeState;
        }

        public int[] GetRegionsInState(ControlGazeState state)
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
                D.Point gazePosition = getGazePosition();
                IEnumerable<int> regionsAtGazePosition = from RegionData regionData in regions
                                                         where regionData.Region.Contains(gazePosition.ToGameLabPoint())
                                                         select regionData.Id;
                return regionsAtGazePosition.ToArray();
            }
        }
        #endregion

        #region Controls
        //Kod powtórzony względem metod dla regionów
        //Założenie - te własności odczytywane są na tyle rzadko, że obliczane są w momencie czytania
        private Control[] getControlsInState(ControlGazeState state)
        {
            IEnumerable<Control> controlsInState = from KeyValuePair<int, RegionData> region in regions
                                                   where region.Value.GazeState == state
                                                   select region.Value.Control;
            return controlsInState.ToArray();
        }

        public Control[] ControlsInNormalState
        {
            get
            {
                return getControlsInState(ControlGazeState.Normal);
            }
        }

        public Control[] ControlsAtGazePosition
        {
            get
            {
                D.Point gazePosition = getGazePosition();
                IEnumerable<Control> activatedControl = from RegionData regionData in regions
                                                        where regionData.Region.Contains(gazePosition.ToGameLabPoint())
                                                        select regionData.Control;
                return activatedControl.ToArray();
            }
        }

        public Control GetControlClosestToGazePosition()
        {
            D.Point gazePosition = getGazePosition();

            int minimalDistanceSquare = int.MaxValue;
            Control controlAtMinimalDistance = null;
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
                    controlAtMinimalDistance = regions.ElementAt(i).Value.Control;
                }
            }
            return controlAtMinimalDistance;
        }

        public Control[] ControlsInActivationState
        {
            get
            {
                return getControlsInState(ControlGazeState.Activation);
            }
        }

        public Control[] ControlsInReactionState
        {
            get
            {
                return getControlsInState(ControlGazeState.Reaction);
            }
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

        private Timer timer;

        public int ActiveControlInterval
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

        #region Events 
        //zdarzenia, które są tylko gdy z region związana jest kontrolka
        private void onControlReturnToNormal(Control control, D.Point gazePosition)
        {
            if (ControlReturnToNormal != null && control != null) ControlReturnToNormal(control, new ControlGazeStateChangedEventArgs(control, gazePosition));
        }
        private void onControlActivation(Control control, D.Point gazePosition)
        {
            if (ControlActivation != null && control != null) ControlActivation(control, new ControlGazeStateChangedEventArgs(control, gazePosition));
        }
        private void onControlReaction(Control control, D.Point gazePosition)
        {
            if (!Enabled) return;
            if (ControlReaction != null && control != null) ControlReaction(control, new ControlGazeStateChangedEventArgs(control, gazePosition));
        }
        private void onControlActivationProgress(Control control, RegionActivationProgressEventArgs e)
        {
            if (ControlActivationProgress != null && control != null) ControlActivationProgress(control, e);
        }

        public event EventHandler<ControlGazeStateChangedEventArgs> ControlReturnToNormal;
        public event EventHandler<ControlGazeStateChangedEventArgs> ControlActivation;
        public event EventHandler<ControlGazeStateChangedEventArgs> ControlReaction;
        public event EventHandler<RegionActivationProgressEventArgs> ControlActivationProgress;
        #endregion

        //zob. uwagi na początku pliku
        public void DrawRegions(D.Graphics g, D.Pen pen, bool showDebugInformation = false, Control container = null)
        {
            //UpdateControlRegions();
            
            for (int i = 0; i < regions.Count; ++i)
            {
                RegionData regionData = regions.ElementAt(i).Value;
                if (!regionData.GazeInteractionEnabled) continue;
                G.IShape region = regionData.Region;
                G.WindowsForms.ShapeDrawingHelper.Draw(region, g, pen, showDebugInformation?regionData.GazeState.ToString():null, container);

                //trzeba sprawdzić kąt wejscia i wyjscia i wyłączyć kółka
            }

            //Point gazePosition = getGazePosition();
            //int size = 6;
            //g.FillEllipse(Brushes.Navy, gazePosition.X - size / 2, gazePosition.Y - size / 2, size, size);
        }

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
