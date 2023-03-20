using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JacekMatulewski.Settings
{
    public static class SettingsHelper
    {
        public static string GetSettingsSubdirectory(string profileName)
        {
            //string exeDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string exeDirectory = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            if (profileName != null) exeDirectory = System.IO.Path.Combine(exeDirectory, profileName);
            return System.IO.Path.Combine(exeDirectory, "Settings");
        }
    }
}
