using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleWindowsFormsApplication
{
    using GameLab.Eyetracking;
    using GameLab.Eyetracking.EyetrackerControls;
    //using GameLab.Eyetracking.GazeRuntimeAnalysis;

    public partial class SettingsForm : EyetrackingSettingsForm
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        protected override void run(Screen screen, GazeSmoothingFilter filter, IGazeRuntimeAnalyser analyser)
        {
            if (rbSampleReadGazePosition_Events.Checked)
                new ReadingGazePositionForm(et, filter, analyser, EyetrackerReadingMode.SubscribeUpdateEvents).ShowDialog();
            if (rbSampleReadGazePosition_Properties.Checked)
                new ReadingGazePositionForm(et, filter, analyser, EyetrackerReadingMode.ReadingProperties).ShowDialog();
            if (rbSampleDwellTimeControlsManager.Checked)
                new DwellTimeControlsForm(et, filter, dwellTime, activationTime).ShowDialog();
            if (rbSampleEnterAndLeaveControlsManager.Checked)
                new EnterAndLeaveControlsForm_Manager(et, filter).ShowDialog();
        }
    }
}
