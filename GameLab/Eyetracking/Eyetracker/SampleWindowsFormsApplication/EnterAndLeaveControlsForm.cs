using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameLab.Eyetracking.EyetrackerControls
{
    using GameLab.Eyetracking;
    using G = GameLab.Geometry;
    using GameLab.Geometry.WindowsForms;

    public partial class EnterAndLeaveControlsForm : Form
    {
        IEyetracker et;
        GazeSmoothingFilter filter;

        public EnterAndLeaveControlsForm(IEyetracker et, GazeSmoothingFilter filter)
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

        GazeDwellTimeControlsManager gazeDwellTimeControlManager;

        int lastEnterRegionId = -1, lastLeaveRegionId = -1;
        DateTime lastEnterRegionTime = DateTime.Now, lastLeaveRegionTime = DateTime.Now;
        int lastEnterAngle = -1, lastLeaveAngle = -1;
        PointF lastEnterVelocity = PointF.Empty, lastLeaveVelocity = PointF.Empty;
        
        private class RegionInfo
        {
            public G.IShape Region;
            public int RegionId;
            public bool Reversed;

            public Point CenterPosition
            {
                get
                {
                    //return new Point((Region.Left + Region.Right) / 2, (Region.Top + Region.Bottom) / 2);
                    return Region.Center.ToPoint().ToSystemDrawingPoint();
                }
            }
        }
        RegionInfo[] regions = new RegionInfo[3];
        
        private void setupEnterAndLeaveManager()
        {
            gazeDwellTimeControlManager = new GazeDwellTimeControlsManager(et, filter, TimeSpan.FromMinutes(1), TimeSpan.Zero, EyeSide.AveragedOrBestEye, TimeSpan.FromSeconds(1));            

            //osobne metody określające zachowanie kontrolki            
            for (int i = 0; i < regions.Length; ++i)
            {
                regions[i] = new RegionInfo();
                regions[i].Region = new G.Rectangle(100 + 400 * i, 100, 300, 300);
            }

            Func<double, int> rad2deg =
                (double angleRadians) =>
                {
                    int degrees = (int)Math.Round(180 * angleRadians / Math.PI);
                    if (degrees < 0) degrees += 360;
                    return degrees;
                };

            int angleThresholdDeg = 60;
            TimeSpan minimalTimeThresholdMs = TimeSpan.FromMilliseconds(300), maximalTimeThresholdMs = TimeSpan.FromMilliseconds(1000);

            Action<int, RegionStateChangedEventArgs> enterAction = (int regionId, RegionStateChangedEventArgs e) => 
            { 
                lastEnterRegionId = regionId;
                lastEnterRegionTime = DateTime.Now;
                lastEnterAngle = rad2deg(e.GazePositionAngleRelativeToRegionCenter); 
            };
            Action<int, RegionStateChangedEventArgs> leaveAction = (int regionId, RegionStateChangedEventArgs e) => 
            { 
                lastLeaveRegionId = regionId;
                lastLeaveRegionTime = DateTime.Now;
                lastLeaveAngle = rad2deg(e.GazePositionAngleRelativeToRegionCenter);
                TimeSpan duration = lastLeaveRegionTime - lastEnterRegionTime;
                int angleDifference = Math.Abs(lastLeaveAngle - lastEnterAngle); 
                bool samePositionDirection = angleDifference < angleThresholdDeg || angleDifference > 360 - angleThresholdDeg;

                if(lastLeaveRegionId == lastEnterRegionId && duration > minimalTimeThresholdMs && duration < maximalTimeThresholdMs && samePositionDirection)
                {
                    for (int i = 0; i < regions.Length; ++i)
                    {
                        RegionInfo regionInfo = regions[i];
                        if (regionInfo.RegionId == lastLeaveRegionId)
                        {
                            regionInfo.Reversed = !regionInfo.Reversed;
                            break;
                        }
                    }
                }
            };

            //liczy się pozycja przy krawędzi, więc potrzebne stosunkowo częste próbkowanie
            for (int i = 0; i < regions.Length; ++i) 
                regions[i].RegionId = gazeDwellTimeControlManager.AddRegion(regions[i].Region, false, null, enterAction, leaveAction, null);
        }

        private const float drawedPointSize = 10;

        private void EnterAndLeaveControlsForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            GameLab.Geometry.PointF averagedEyePosition = et.AveragedEyeData.PositionF;
            RectangleF rectAveragedEye = new RectangleF(averagedEyePosition.X - drawedPointSize / 2, averagedEyePosition.Y - drawedPointSize / 2, drawedPointSize, drawedPointSize);
            Brush b = gazeDwellTimeControlManager.IsInitiallyInactive ? Brushes.Red : Brushes.Lime;
            g.FillEllipse(b, rectAveragedEye);
            g.DrawEllipse(Pens.Black, rectAveragedEye);

            Pen p = new Pen(Color.Purple, 1);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            gazeDwellTimeControlManager.DrawRegions(g, p, true);

            for (int i = 0; i < regions.Length; ++i)
                if (regions[i].Reversed)
                    g.FillEllipse(Brushes.Navy, regions[i].CenterPosition.X - 25, regions[i].CenterPosition.Y - 25, 50, 50);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Action _aktualizujOpis = 
                new Action(() => 
                {
                    lbLastEnter.Text = lastEnterRegionId.ToString() + " / " + lastEnterAngle;
                    lbLastLeave.Text = lastLeaveRegionId.ToString() + " / " + lastLeaveAngle;
                    lbAngleDifference.Text = Math.Abs(lastLeaveAngle - lastEnterAngle).ToString();
                    lbInRegionTime.Text = (lastLeaveRegionTime - lastEnterRegionTime).TotalMilliseconds.ToString();
                });

            while (true)
            {
                try
                {
                    lbLastEnter.Invoke(_aktualizujOpis);
                }
                catch
                {
                    //wyjątek zgłaszany dopóki okno nie zostanie utworzone
                }

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
