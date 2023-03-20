using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GameLab.Eyetracking;

namespace GameLab.Eyetracking.EyetrackerControls
{
    using G = GameLab.Geometry;
    using GameLab.Geometry.WindowsForms;

    public partial class EnterAndLeaveControlsForm_Manager : Form
    {
        IEyetracker et;
        GazeSmoothingFilter filter;

        public EnterAndLeaveControlsForm_Manager(IEyetracker et, GazeSmoothingFilter filter)
        {
            this.et = et;
            this.filter = filter;

            PointF previousGazePosition = PointF.Empty;

            InitializeComponent();

            //ustawienia pozwalające uniknąć mrugania obszarów
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);            

            backgroundWorker.RunWorkerAsync();
            setupEnterAndLeaveManager();            
        }

        private GazeEnterAndLeaveControlManager gazeEnterAndLeaveControlManager;
        private Dictionary<int, bool> regionReversed = new Dictionary<int, bool>();
        
        /*
        private Point calculateCenterPosition(Rectangle rectangle)
        {
            return new Point((rectangle.Left + rectangle.Right) / 2, (rectangle.Top + rectangle.Bottom) / 2);
        }
        */

        private void setupEnterAndLeaveManager()
        {
            gazeEnterAndLeaveControlManager = new GazeEnterAndLeaveControlManager(et, filter, EyeSide.AveragedOrBestEye, TimeSpan.FromSeconds(1), GazeEnterAndLeaveSettings.Default);

            G.Rectangle region1 = new G.Rectangle(100, 100, 300, 300);
            G.Rectangle region2 = new G.Rectangle(500, 100, 300, 300);
            G.Rectangle region3 = new G.Rectangle(900, 100, 300, 300);

            Action<int,RegionStateChangedEventArgs> action =
                (int regionId, RegionStateChangedEventArgs e) =>
                {
                    regionReversed[regionId] = !regionReversed[regionId];
                };

            int regionId1 = gazeEnterAndLeaveControlManager.AddRegion(region1, action);
            int regionId2 = gazeEnterAndLeaveControlManager.AddRegion(region2, action);
            int regionId3 = gazeEnterAndLeaveControlManager.AddRegion(region3, action);

            regionReversed.Add(regionId1, false);
            regionReversed.Add(regionId2, false);
            regionReversed.Add(regionId3, false);
        }

        private const float drawedPointSize = 10;

        private void EnterAndLeaveControlsForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            GameLab.Geometry.PointF averagedEyePosition = et.AveragedEyeData.PositionF;
            RectangleF rectAveragedEye = new RectangleF(averagedEyePosition.X - drawedPointSize / 2, averagedEyePosition.Y - drawedPointSize / 2, drawedPointSize, drawedPointSize);
            Brush b = gazeEnterAndLeaveControlManager.IsInitiallyInactive ? Brushes.Red : Brushes.Lime;
            g.FillEllipse(b, rectAveragedEye);
            g.DrawEllipse(Pens.Black, rectAveragedEye);

            Pen p = new Pen(Color.Purple, 1);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            gazeEnterAndLeaveControlManager.DrawRegions(g, p, true);

            for (int i = 0; i < regionReversed.Count; ++i)
            {
                KeyValuePair<int,bool> element = regionReversed.ElementAt(i);
                bool reversed = element.Value;
                if (reversed)
                {
                    int regionId = element.Key;
                    G.IShape rectangle = gazeEnterAndLeaveControlManager.GetRegionLocation(regionId);
                    //Point centerPosition = calculateCenterPosition(rectangle);
                    Point centerPosition = rectangle.Center.ToPoint().ToSystemDrawingPoint();
                    g.FillEllipse(Brushes.Navy, centerPosition.X - 25, centerPosition.Y - 25, 50, 50);
                }
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                this.Invalidate();
                System.Threading.Thread.Sleep(100); 
            }
        }
        
        private void EnterAndLeaveControlsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
