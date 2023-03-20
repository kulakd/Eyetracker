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
    public partial class OffsetCorrectionSettingsForm : Form
    {
        private IEyetracker et = null;

        public OffsetCorrectionSettingsForm()
        {
            InitializeComponent();
        }

        public OffsetCorrectionSettingsForm(IEyetracker et)
        {
            InitializeComponent();

            this.et = et;
            toControls();
        }

        private void toControls()
        {
            nudLeftEyeOffsetX.Value = (decimal)et.LeftEyeOffset.X;
            nudLeftEyeOffsetY.Value = (decimal)et.LeftEyeOffset.Y;
            nudRightEyeOffsetX.Value = (decimal)et.RightEyeOffset.X;
            nudRightEyeOffsetY.Value = (decimal)et.RightEyeOffset.Y;
        }

        private void fromControls()
        {
            et.LeftEyeOffset = new Geometry.PointF((float)nudLeftEyeOffsetX.Value, (float)nudLeftEyeOffsetY.Value);
            et.RightEyeOffset = new Geometry.PointF((float)nudRightEyeOffsetX.Value, (float)nudRightEyeOffsetY.Value);
        }

        private void btnAutomatic_Click(object sender, EventArgs e)
        {
            OffsetCorrectionForm rf = new OffsetCorrectionForm(et);
            rf.ShowDialog();
            toControls();
            //MessageBox.Show("Offset correction for left eye: " + et.LeftEyeOffset.ToString() + ", for right eye: " + et.RightEyeOffset.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            fromControls();
            //Close();
        }
    }
}
