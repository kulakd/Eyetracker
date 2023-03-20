using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameLab.Eyetracking.EyetrackerControls
{
    public partial class OffsetCorrectionForm : Form
    {
        private int intervalMs;
        private IEyetracker et = null;
        private DateTime startTime;

        //przenieść na właściwy ekran
        public OffsetCorrectionForm(IEyetracker et, Image image = null, int intervalMs = 3000)
        {
            if(et == null)
            {
                throw new GameLabException("Brak obiektu eyetrackera");
            }
            this.et = et;

            InitializeComponent();

            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //this.WindowState = FormWindowState.Normal;

            if (image != null) pictureBox.Image = image;

            this.intervalMs = intervalMs;
            timer.Interval = intervalMs / 100;
            startTime = DateTime.Now;
            timer.Start();
        }

        private void RecalibrationForm_Resize(object sender, EventArgs e)
        {
            Rectangle r = ClientRectangle; //rozmiar obszaru użytkownika okna
            Point clientAreaCenter = new Point(r.X + r.Width / 2, r.Y + r.Height / 2);
            int size = r.Height / 5;

            pictureBox.Width = size;
            pictureBox.Height = size;
            pictureBox.Left = clientAreaCenter.X - size / 2;
            pictureBox.Top = clientAreaCenter.Y - size / 2;

            //Point positionOnScreen = getImageCenterOnScreen();
            //label.Text = positionOnScreen.ToString();
        }

        private Point getImageCenterInScreenCoordinates()
        {
            int x = pictureBox.Left + pictureBox.Width / 2;
            int y = pictureBox.Top + pictureBox.Height / 2;
            Point positionInClientArea = new Point(x, y);
            return this.PointToScreen(positionInClientArea);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int timeFromStart = (int)(DateTime.Now - startTime).TotalMilliseconds;
            if (timeFromStart >= intervalMs)
            {
                timer.Enabled = false;
                Point centrePositionOnScreen = getImageCenterInScreenCoordinates();
                et.LeftEyeOffset = new GameLab.Geometry.PointF(et.LeftEyeData.PositionF.X - centrePositionOnScreen.X, et.LeftEyeData.PositionF.Y - centrePositionOnScreen.Y);
                et.RightEyeOffset = new GameLab.Geometry.PointF(et.RightEyeData.PositionF.X - centrePositionOnScreen.X, et.RightEyeData.PositionF.Y - centrePositionOnScreen.Y);
                et.AveragedEyeOffset = new GameLab.Geometry.PointF(et.AveragedEyeData.PositionF.X - centrePositionOnScreen.X, et.AveragedEyeData.PositionF.Y - centrePositionOnScreen.Y);
                Close();
            }
            else
            {
                //miejsce na animację
            }
        }
    }
}
