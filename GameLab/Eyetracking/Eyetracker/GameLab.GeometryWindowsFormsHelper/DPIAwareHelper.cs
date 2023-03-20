using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GameLab
{
    //najlepsze wyniki daje skalowanie w app.config (zob. przykład w SampleWindowsFormsApplication)

    public static class DPIAwareHelper
    {
        //Windows Vista [desktop apps only]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();

        //---------------------------------------------------

        public enum ProcessDPIAwareness
        {
            Unaware = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }

        //Windows 8.1 [desktop apps only]
        [DllImport("shcore.dll")]
        public static extern int SetProcessDpiAwareness(ProcessDPIAwareness value);

        public enum DpiAwarenessContext
        {
            Unaware = -1,
            SystemAware = -2,
            PerMonitorAware = -3,
            PerMonitorAwareV2 = -4,
            UnawareGdiScaled = -5
        }

        //Windows 10, version 1703 [desktop apps only]
        [DllImport("user32.dll")]
        public static extern bool SetProcessDpiAwarenessContext(DpiAwarenessContext value);

        //https://docs.microsoft.com/en-us/previous-versions/windows/desktop/legacy/mt846517(v%3Dvs.85)
        //https://docs.microsoft.com/en-us/dotnet/framework/winforms/high-dpi-support-in-windows-forms
        //https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/winforms/index
    }
}
