using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLab.Eyetracking.EyetrackerControls
{
    public static class EyetrackerCalibrationSettingsHelper
    {
        private static ICustomCalibrationScreen createCustomCalibrationForm(this EyetrackerCalibrationSettings calibrationSettings, CustomCalibrationSettingsEx customCalibrationSettings)
        {
            ICustomCalibrationScreen customCalibrationForm;
            if(calibrationSettings.UseCustomCalibrationScreen)
            {
                if (customCalibrationSettings.UseExtendedForm) customCalibrationForm = new CustomCalibrationFormEx(customCalibrationSettings);
                else customCalibrationForm = new CustomCalibrationForm(customCalibrationSettings);
                customCalibrationForm.Settings = calibrationSettings;
            }
            else customCalibrationForm = null;
            return customCalibrationForm;
        }

        public static void InitCustomCalibrationForm(this EyetrackerCalibrationSettings calibrationSettings, CustomCalibrationSettingsEx customCalibrationSettings)
        {
            calibrationSettings.CustomCalibrationScreen = createCustomCalibrationForm(calibrationSettings, customCalibrationSettings);
        }
    }
}
