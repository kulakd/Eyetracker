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
    using GameLab.Geometry.WindowsForms;

    public partial class DwellTimeControlsForm : Form
    {
        IEyetracker et;
        GazeSmoothingFilter filter;
        TimeSpan dwellTime, activationTime;

        public DwellTimeControlsForm(IEyetracker et, GazeSmoothingFilter filter, TimeSpan dwellTime, TimeSpan activationTime)
        {
            //ustawienia pozwalające uniknąć mrugania obszarów
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.et = et;
            this.filter = filter;
            this.dwellTime = dwellTime;
            this.activationTime = activationTime;

            InitializeComponent();

            backgroundWorker.RunWorkerAsync();
            setupDwellTimeManager();            
            //TODO: jak zrobić żeby kropka była przed przyciskami (por. panelDebug w MainForm)
        }

        GazeDwellTimeControlsManager gazeDwellTimeControlManager;

        private void setupDwellTimeManager()
        {
            gazeDwellTimeControlManager = new GazeDwellTimeControlsManager(et, filter, dwellTime, activationTime, EyeSide.AveragedOrBestEye, TimeSpan.FromSeconds(3));            

            //zdarzenia określające zachowanie dla wszystkich kontrolek
            gazeDwellTimeControlManager.ControlActivation += gazeDwellTimeControlManager_ControlActivation;
            gazeDwellTimeControlManager.ControlReaction += gazeDwellTimeControlManager_ControlReaction;
            gazeDwellTimeControlManager.ControlReturnToNormal += gazeDwellTimeControlManager_ControlReturnToNormal;
            gazeDwellTimeControlManager.ControlActivationProgress += gazeDwellTimeControlManager_ControlActivationProgress;
            //gazeDwellTimeControlManager.AddControl(panelDebug, null);
            gazeDwellTimeControlManager.AddRegion(button1);

            //osobne metody określające zachowanie kontrolki
            Rectangle button2Region = button2.Bounds;
            button2Region.Width *= 3;
            button2Region.Height *= 3;
            gazeDwellTimeControlManager.AddRegion(
                button2, button2Region.ToGameLabRectangle(), true,
                (int regionId, RegionStateChangedEventArgs e) =>
                {
                    button2.FlatAppearance.BorderColor = reactionBorderColor;
                    button2.Text = "Reaction";
                },
                (int regionId, RegionStateChangedEventArgs e) =>
                {
                    button2.FlatAppearance.BorderColor = activationBorderColor;
                    button2.Text = "Activation";
                },
                (int regionId, RegionStateChangedEventArgs e) =>
                {
                    button2.FlatAppearance.BorderColor = normalBorderColor;
                    button2.Text = "Normal";
                },
                (int regionId, RegionActivationProgressEventArgs e) =>
                {
                    Color color = lerp(activationBorderColor, reactionBorderColor, e.Percent);
                    button2.FlatAppearance.BorderColor = color;
                    button2.Text = "Activation (" + Math.Round(100 * e.Percent).ToString() + ")";
                });

            //obszary w innych kształtach            
            gazeDwellTimeControlManager.AddRegion(new GameLab.Geometry.Ellipse(800, 600, 150, 100));
            gazeDwellTimeControlManager.AddRegion(new GameLab.Geometry.Polygon(new GameLab.Geometry.Point(800, 300), new GameLab.Geometry.Point(1000, 300), new GameLab.Geometry.Point(900, 100)));
            gazeDwellTimeControlManager.AddRegion(new GameLab.Geometry.Polygon(new GameLab.Geometry.Point(1300, 300), new GameLab.Geometry.Point(1100, 300), new GameLab.Geometry.Point(1200, 100))); //inaczej nawinięty
            gazeDwellTimeControlManager.AddRegion(new GameLab.Geometry.Triangle(new GameLab.Geometry.Point(1200, 600), new GameLab.Geometry.Point(1400, 600), new GameLab.Geometry.Point(1300, 400)));
        }

        private readonly Color normalBorderColor = Color.DarkGray;
        private readonly Color activationBorderColor = Color.Lime;
        private readonly Color reactionBorderColor = Color.DarkGreen;

        void gazeDwellTimeControlManager_ControlActivation(object sender, ControlGazeStateChangedEventArgs e)
        {
            if (sender == button1)
            {
                button1.FlatAppearance.BorderColor = activationBorderColor;
                button1.Text = "Activation";
            }
        }

        void gazeDwellTimeControlManager_ControlReaction(object sender, ControlGazeStateChangedEventArgs e)
        {
            if (sender == button1)
            {
                button1.FlatAppearance.BorderColor = reactionBorderColor;
                button1.Text = "Reaction";
            }
        }

        void gazeDwellTimeControlManager_ControlReturnToNormal(object sender, ControlGazeStateChangedEventArgs e)
        {
            if (sender == button1)
            {
                button1.FlatAppearance.BorderColor = normalBorderColor;
                button1.Text = "Normal";
            }
        }

        private static byte lerp(byte n1, byte n2, double percent)
        {
            return (byte)(n1 + (n2 - n1) * percent);
        }

        private static Color lerp(Color c1, Color c2, double percent)
        {
            byte r = lerp(c1.R, c2.R, percent);
            byte g = lerp(c1.G, c2.G, percent);
            byte b = lerp(c1.B, c2.B, percent);
            return Color.FromArgb(r, g, b);
        }

        void gazeDwellTimeControlManager_ControlActivationProgress(object sender, RegionActivationProgressEventArgs e)
        {
            if (sender == button1)
            {
                Color color = lerp(activationBorderColor, reactionBorderColor, e.Percent);
                button1.FlatAppearance.BorderColor = color;
                button1.Text = "Activation (" + Math.Round(100 * e.Percent).ToString() + ")";
            }
        }

        private void DwellTimeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private const float drawedPointSize = 10;

        private void DwellTimeControlsForm_Paint(object sender, PaintEventArgs e)
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
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.Invalidate();
                System.Threading.Thread.Sleep(100);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) gazeDwellTimeControlManager.SuspendRegionGazeInteraction(button1, false);
            else gazeDwellTimeControlManager.ResumeRegionGazeInteraction(button1, false);
        }
    }
}
