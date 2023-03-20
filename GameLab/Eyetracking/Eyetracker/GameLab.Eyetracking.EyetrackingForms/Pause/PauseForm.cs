using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameLab.Eyetracking.EyetrackingForms
{
    public partial class PauseForm : BasePauseForm
    {
        private int visiblePeriodSeconds = 10;
        private int maximalBrightness = 50;

        public PauseForm() //dla designera
        {
            InitializeComponent();
        }

        private void PauseForm_Shown(object sender, EventArgs e)
        {
            int margin = ClientRectangle.Width / 10;
            pictureBox.Left = margin;
            pictureBox.Width = ClientRectangle.Width - 2 * margin;
            pictureBox.Height = (int)((0.8 * pictureBox.Image.Height / pictureBox.Width) * pictureBox.Width);
            pictureBox.Top = ClientRectangle.Height / 2 - pictureBox.Height / 2;

            label.Top = ClientRectangle.Height / 2 - label.Height / 2;
            label.Left = ClientRectangle.Width / 2 - label.Width / 2;
        }

        protected override void timerTick(TimeSpan timeElapsed, TimeSpan timeLeft)
        {
            label.Text = ((int)timeLeft.TotalSeconds + 1).ToString();
            label.Visible = !(timeLeft.TotalSeconds > visiblePeriodSeconds && timeElapsed.TotalSeconds > visiblePeriodSeconds);

            int visiblePeriodMiliseconds = visiblePeriodSeconds * 1000;
            if (timeElapsed.TotalSeconds < visiblePeriodSeconds)
            {
                int percent = (int)(maximalBrightness * (1.0 - timeElapsed.TotalMilliseconds / visiblePeriodMiliseconds));
                label.ForeColor = Color.FromArgb(percent, percent, percent);
                //label.Text = percent.ToString();
            }
            /*
            if (timeLeft.Seconds < visiblePeriodSeconds)
            {                
                int percent = (int)(maximalBrightness * (1.0 - timeLeft.TotalMilliseconds / visiblePeriodMiliseconds));
                label.ForeColor = Color.FromArgb(percent, percent, percent);
                //label.Text = percent.ToString();
            }
            */
        }
    }
}
