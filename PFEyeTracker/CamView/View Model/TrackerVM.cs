using EyeTracker;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace CamView.View_Model
{
    [SupportedOSPlatform("windows")]
    public class TrackerVM : INotifyPropertyChanged
    {
        private readonly CamTracker cam;
        public TrackerVM()
        {
            cam = new CamTracker();
            cam.NewFrameEvent += HandleNewFrame;
        }

        private MemoryStream video;
        public MemoryStream Video
        {
            get
            {
                if (video != null)
                    video.Position = 0;
                return video;
            }
            set
            {
                MemoryStream old = video;
                video = value;
                if (old != null)
                    old.Dispose();
                onPropretyChanged(nameof(Video));
            }
        }
        public string[] Cameras => cam.Cameras;
        public int Index
        {
            get => cam.Index;
            set
            {
                if (value < 0 || cam.Index == value) return;
                cam.Index = value;
                onPropretyChanged(nameof(Index));
                onPropretyChanged(nameof(Streaming));
                onPropretyChanged(nameof(CurrentCamera));
            }
        }

        public float Padding { get => cam.Padding; set => cam.Padding = value; }

        public bool Streaming => cam.isStreaming;
        public string CurrentCamera => Streaming ? cam.Cameras[cam.Index] : "None";

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropretyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void HandleNewFrame(object sender, MemoryStream args)
        {
            Video = args;
        }
    }
}
