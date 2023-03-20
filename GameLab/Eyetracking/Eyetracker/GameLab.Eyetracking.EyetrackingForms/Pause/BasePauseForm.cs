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
    public partial class BasePauseForm : Form
    {
        private DateTime startTime = DateTime.Now;
        private DateTime endTime;        

        public BasePauseForm()
        {
            InitializeComponent();            
        }

        public void ShowDialog(TimeSpan time)
        {
            endTime = startTime.Add(time);
            timer.Start();
            //if (!this.DesignMode) 
            Cursor.Hide();
            base.ShowDialog();            
        }


        private void Form_Shown(object sender, EventArgs e)
        {
            for (double opacity = 0; opacity <= 1.0; opacity += 0.01)
            {
                //label.Text = opacity.ToString(); label.Refresh();
                this.Opacity = opacity;
                System.Threading.Thread.Sleep(3);
            }
            this.Opacity = 1.0;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime _now = DateTime.Now;
            if(_now >= endTime) Close();
            TimeSpan timeLeft = endTime - _now;
            TimeSpan timeElapsed = _now - startTime;            
            timerTick(timeElapsed, timeLeft);
        }

        protected virtual void timerTick(TimeSpan timeElapsed, TimeSpan timeLeft) { }

        private void PauseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Enabled = false;
            for (; Opacity > 0.05; Opacity -= 0.01) System.Threading.Thread.Sleep(50);
            Cursor.Show();
        }
    }
}
