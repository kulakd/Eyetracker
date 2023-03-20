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

namespace GameLab.Eyetracking.V2
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

    public class ControlStateChangedEventArgs : RegionStateChangedEventArgs
    {
        public Control Control { get; private set; } //może być null

        public ControlStateChangedEventArgs(int regionId, Control control, D.Point gazePosition, D.Point regionCenterPosition)
            :base(regionId, gazePosition.ToGameLabPoint(), regionCenterPosition.ToGameLabPoint())
        {
            this.Control = control;            
        }
    }

    public class ControlActivationProgressEventArgs : RegionActivationProgressEventArgs
    {
        public Control Control { get; private set; } //może być null        

        public ControlActivationProgressEventArgs(int regionId, Control control, TimeSpan timeInterval, TimeSpan dwellTime)
            :base(regionId, timeInterval, dwellTime)
        {
            this.Control = control;            
        }
    }

    public class GazeDwellTimeControlsManagerSettings : GazeDwellTimeRegionsManagerSettings //DTO używane przede wszystkim w nowszych projektach (np. GazeTextEntryApps)
    {
        public Control Container;
    }

    public class GazeDwellTimeControlsManager : GazeDwellTimeRegionsManager
    {
        private Control container = null;
        private Dictionary<int, Control> controls = new Dictionary<int, Control>(); //skojarzenie kontrolek z regionami -> nie można ruszać tych słowników

        public Control GetControl(int regionId) //może być null, gdy region bez kontrolki
        {
            return controls[regionId];
        }

        //TODO: rozważyć czy potrzebny container - przetestować!!!!!! 
        public GazeDwellTimeControlsManager(IEyetracker et, GazeSmoothingFilter filter, TimeSpan dwellTime, EyeSide eyeSide, TimeSpan initialInactivityTime, ManagerControlType controlType = ManagerControlType.Active, Control container = null)
            : base(et, filter, dwellTime, eyeSide, initialInactivityTime, controlType/*, (container == null) ? (G.Rectangle?)null : container.Bounds.ToGameLabRectangle()*/)
        {
            this.container = container;
        }

        public GazeDwellTimeControlsManager(GazeDwellTimeControlsManagerSettings settings)
            :this(settings.Eyetracker, settings.SmoothingFilter, settings.DwellTime, settings.EyeSide, settings.InitialInactivityTime, settings.GazeControlType, settings.Container)
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
                bounds = _container.RectangleToScreen(bounds);
                //Rectangle testBounds = _container.RectangleToClient(bounds);
            }
            //return bounds;
            return bounds.ToGameLabRectangle();
        }

        //współrzędne region we współrzędnych ekranu
        public int AddRegion(Control control, G.IShape region = null, bool forceReturnToNormalState = false, Action<int, RegionStateChangedEventArgs> reactionAction = null, Action<int, RegionStateChangedEventArgs> activationAction = null, Action<int, RegionStateChangedEventArgs> returnToNormalAction = null, Action<int, RegionActivationProgressEventArgs> activationProgressAction = null)
        {
            if (control == null) throw new ArgumentNullException("control");
            if (region == null) region = boundsInScreenCoordinates(control);
            int regionId = base.AddRegion(region, forceReturnToNormalState, reactionAction, activationAction, returnToNormalAction, activationProgressAction);
            controls.Add(regionId, control);
            return regionId;
        }

        private int findControlId(Control control)
        {
            int id = controls.First(p => p.Value == control).Key;
            return id;
        }

        public void UpdateRegion(Control control, G.IShape region = null)
        {            
            if (region == null) region = boundsInScreenCoordinates(control);
            int regionId = findControlId(control);
            UpdateRegion(regionId, region);
        }

        public void UpdateAllControlsRegions()
        {
            //niewydajne, choć z pozoru elegantsze, bo w każdej iteracji szuka kontrolek
            //foreach (Control control in controls.Values) UpdateRegion(control);
            foreach(KeyValuePair<int,Control> p in controls)
            {
                UpdateRegion(p.Key, boundsInScreenCoordinates(p.Value));
            }
        }

        public void RemoveRegion(Control control)
        {
            int regionId = findControlId(control);
            RemoveRegion(regionId);
        }

        public void SuspendRegionGazeInteraction(Control control, bool hideControl)
        {
            int regionId = findControlId(control);
            SuspendRegionGazeInteraction(regionId);
            if (hideControl) control.Hide();
        }

        public void ResumeRegionGazeInteraction(Control control, bool showControl)
        {
            int id = findControlId(control);
            ResumeRegionGazeInteraction(id);
            if (showControl) control.Show();
        }

        protected override void onRegionActivation(RegionData regionData, G.Point gazePosition)
        {
            base.onRegionActivation(regionData, gazePosition);
            if (controls.ContainsKey(regionData.Id)) onControlActivation(controls[regionData.Id], gazePosition.ToSystemDrawingPoint());
        }

        protected override void onRegionReaction(RegionData regionData, G.Point gazePosition)
        {
            base.onRegionReaction(regionData, gazePosition);
            if (controls.ContainsKey(regionData.Id)) onControlReaction(controls[regionData.Id], gazePosition.ToSystemDrawingPoint());
        }

        protected override void onRegionReturnToNormal(RegionData regionData, G.Point gazePosition)
        {
            base.onRegionReturnToNormal(regionData, gazePosition);
            if (controls.ContainsKey(regionData.Id)) onControlReturnToNormal(controls[regionData.Id], gazePosition.ToSystemDrawingPoint());
        }

        protected override void onRegionActivationProgress(RegionData regionData, RegionActivationProgressEventArgs e)
        {
            base.onRegionActivationProgress(regionData, e);
            if (controls.ContainsKey(regionData.Id)) onControlActivationProgress(controls[regionData.Id], e);
        }

        #region Controls
        //Kod powtórzony względem metod dla regionów
        //Założenie - te własności odczytywane są na tyle rzadko, że obliczane są w momencie czytania
        private Control[] getControlsInState(RegionGazeState state)
        {
            IEnumerable<Control> controlsInState = from int regionId in controls
                                                   where regions[regionId].GazeState == state
                                                   select controls[regionId];
            return controlsInState.ToArray();
        }

        public Control[] ControlsInNormalState
        {
            get
            {
                return getControlsInState(RegionGazeState.Normal);
            }
        }

        public Control[] ControlsAtGazePosition
        {
            get
            {
                G.Point gazePosition = getGazePosition();
                IEnumerable<Control> activatedControl = from RegionData regionData in regions
                                                        where regionData.Region.Contains(gazePosition)
                                                        let regionId = regionData.Id
                                                        select controls[regionId];
                return activatedControl.ToArray();
            }
        }

        //powtórzenie kodu względem GetRegionClosestToGazePosition
        public Control GetControlClosestToGazePosition()
        {            
            G.Point gazePosition = getGazePosition();

            int minimalDistanceSquare = int.MaxValue;
            Control controlAtMinimalDistance = null; //pozostaje null jedynie, gdy nie ma w ogóle kontrolek
            for (int i = 0; i < regions.Count; ++i)
            {
                G.IShape region = regions.ElementAt(i).Value.Region;
                //Point center = new Point(region.Left + region.Width / 2, region.Top + region.Height / 2);
                G.Point center = region.Center.ToPoint();
                int x = gazePosition.X - center.X;
                int y = gazePosition.Y - center.Y;
                int distanceSquare = x * x + y * y;
                int regionId = regions.ElementAt(i).Value.Id;
                if (distanceSquare < minimalDistanceSquare && controls.ContainsKey(regionId))
                {
                    minimalDistanceSquare = distanceSquare;
                    controlAtMinimalDistance = controls[regionId];
                }
            }
            return controlAtMinimalDistance;
        }

        public Control[] ControlsInActivationState
        {
            get
            {
                return getControlsInState(RegionGazeState.Activation);
            }
        }

        public Control[] ControlsInReactionState
        {
            get
            {
                return getControlsInState(RegionGazeState.Reaction);
            }
        }
        #endregion

        #region Control Events 
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
    }
}
