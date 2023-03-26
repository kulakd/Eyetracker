using EyeTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace MauiGui.Model
{
    [SupportedOSPlatform("windows")]
    class WinModel
    {
        private static WinModel instance;
        public static WinModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new WinModel();
                return instance;
            }
        }

        private CamTracker cam = new CamTracker();
        public CamTracker Camera => cam;
    }
}
