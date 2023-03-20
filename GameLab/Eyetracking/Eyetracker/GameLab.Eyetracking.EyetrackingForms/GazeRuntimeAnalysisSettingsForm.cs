using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GameLab.Eyetracking.GazeRuntimeAnalysis;

namespace GameLab.Eyetracking.EyetrackerControls
{
    public partial class GazeRuntimeAnalysisSettingsForm : Form
    {
        public GazeRuntimeAnalysisSettingsForm(GazeEventsRuntimeAnalysisSettings analysisSettings)
        {
            InitializeComponent();

            //cbDetectEyeClosure.Checked = analysisSettings.DetectEyeClosure;
            cbDetectFixationsAndSaccades.Checked = analysisSettings.DetectFixationsAndSaccades;
            switch (analysisSettings.EventsAnalysisMethod)
            {
                case GazeEventsAnalysisMethod.VelocityThreshold:
                    cbFixationDetectionMethod.SelectedIndex = 0;
                    break;
                case GazeEventsAnalysisMethod.PositionDispersion:
                    cbFixationDetectionMethod.SelectedIndex = 1;
                    break;
                default:
                    throw new Exception("Unknown events analysis method");
            }
            nudMinimalNumberOfStatesToDetectEyeClosure.Value = analysisSettings.MinimumNumberOfStatesToDetectEyeClosure;
            nudDispersionX.Value = (decimal)analysisSettings.PDM_MaximalDispersionX;
            nudDispersionY.Value = (decimal)analysisSettings.PDM_MaximalDispersionY;
            nudMinimalFixationNumberOfSamples.Value = (decimal)analysisSettings.PDM_MinimalFixationNumberOfSamples;
            nudSaccadeThresholdVelocity.Value = (decimal)analysisSettings.VM_SaccadeThresholdVelocity;
            cbUseThreePointsDerivation.Checked = analysisSettings.VM_UseThreePointsDerivatives;            
        }

        public GazeEventsRuntimeAnalysisSettings Settings
        {
            get
            {
                return gainSettings();
            }
        }

        private GazeEventsRuntimeAnalysisSettings gainSettings()
        {
            GazeEventsRuntimeAnalysisSettings analysisSettings = GazeEventsRuntimeAnalysisSettings.Default;
            //analysisSettings.DetectEyeClosure = cbDetectEyeClosure.Checked;
            analysisSettings.DetectFixationsAndSaccades = cbDetectFixationsAndSaccades.Checked;            
            switch(cbFixationDetectionMethod.SelectedIndex)
            {
                case 0:
                    analysisSettings.EventsAnalysisMethod = GazeEventsAnalysisMethod.VelocityThreshold;
                    break;
                case 1:
                    analysisSettings.EventsAnalysisMethod = GazeEventsAnalysisMethod.PositionDispersion;
                    break;
                default:
                    throw new Exception("Unknown events analysis method");
            }
            analysisSettings.MinimumNumberOfStatesToDetectEyeClosure = (int)nudMinimalNumberOfStatesToDetectEyeClosure.Value;
            analysisSettings.PDM_MaximalDispersionX = (double)nudDispersionX.Value;
            analysisSettings.PDM_MaximalDispersionY = (double)nudDispersionY.Value;
            analysisSettings.PDM_MinimalFixationNumberOfSamples = (int)nudMinimalFixationNumberOfSamples.Value;
            analysisSettings.VM_SaccadeThresholdVelocity = (double)nudSaccadeThresholdVelocity.Value;
            analysisSettings.VM_UseThreePointsDerivatives = cbUseThreePointsDerivation.Checked;
            return analysisSettings;
        }
    }
}
