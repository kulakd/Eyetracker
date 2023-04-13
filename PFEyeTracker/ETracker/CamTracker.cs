using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace EyeTracker
{
    [SupportedOSPlatform("windows")]
    public class CamTracker
    {
        private static readonly CascadeClassifier faceCascadeClassifier =
            new CascadeClassifier(AppDomain.CurrentDomain.BaseDirectory + "\\HaarCascades\\haarcascade_frontalface_alt_tree.xml");

        #region Fields
        private FilterInfoCollection filters;
        private VideoCaptureDevice device;
        private int index;
        private Size res;
        private Queue<Rectangle> faceRects = new Queue<Rectangle>();

        private Stopwatch stopwatch;
        private long msBtwFrames;
        private float padding;
        #endregion

        public event EventHandler<MemoryStream> NewFrameEvent;

        #region Properties
        public int Index
        {
            get => index;
            set
            {
                if (value == index) return;
                if (value < 0 || value > filters.Count)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "No Camera of this index!");

                if (device != null)
                    device.Stop();

                index = value;
                device = new VideoCaptureDevice(filters[value].MonikerString);
                device.NewFrame += Device_NewFrame;
                faceRects.Clear();
                device.Start();
            }
        }

        public string[] Cameras
        {
            get
            {
                string[] result = new string[filters.Count];
                for (int i = 0; i < filters.Count; i++)
                    result[i] = filters[i].Name;
                return result;
            }
        }

        public bool isStreaming => device != null && device.IsRunning;

        public float Padding
        {
            get => padding;
            set
            {
                if (value < 0 || value > 1f)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Must be between 0 and 1");
                padding = value;
            }
        }

#if DEBUG
        public bool DebugFullCam { get; set; } = false;
#endif
        public bool DebugFPSCounter { get; set; } = false;

        public float FPS => 1000f / msBtwFrames;

        public int Width
        {
            get => res.Width;
        }
        public int Heigh
        {
            get => res.Height;
        }
        #endregion

        public CamTracker()
        {
            RefreshDeviceList();
            index = -1;
        }

        public void Stop()
        {
            if (isStreaming)
                device.Stop();
            faceRects.Clear();
            index = -1;
        }

        public void RefreshDeviceList()
        {
            filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        private void Device_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            //diagnostics
            if (DebugFPSCounter)
            {
                if (stopwatch == null) stopwatch = Stopwatch.StartNew();
                else
                {
                    msBtwFrames = stopwatch.ElapsedMilliseconds;
                    stopwatch.Restart();
                }
            }

            MemoryStream stream = new MemoryStream();
            Bitmap btm = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, byte> image = btm.ToImage<Bgr, byte>();
            Rectangle[] rects = faceCascadeClassifier.DetectMultiScale(image);

#if DEBUG
            //Draw face rects
            if (DebugFullCam)
            {
                foreach (Rectangle r in rects)
                    using (Graphics graphics = Graphics.FromImage(btm))
                    {
                        using (Pen pen = new Pen(Color.Red, 4))
                            graphics.DrawRectangle(pen, r);
                    }
            }
#endif

            if (rects.Length > 0)
            {
                Rectangle rect = rects.OrderByDescending(r => (r.Height * r.Width)).First();

                Size ROISize = new Size((int)(btm.Width * (1f - Padding)), (int)(btm.Height * (1f - Padding)));
                Point ROILocation = rect.Location;

                ROILocation.X += rect.Width / 2;
                ROILocation.Y += rect.Height / 2;

                ROILocation.X -= ROISize.Width / 2;
                ROILocation.Y -= ROISize.Height / 2;

                Bitmap result;
#if DEBUG // jeżeli jest na release, nie ma ifa
                if (!DebugFullCam)
                {
#endif
                    image.ROI = new Rectangle(ROILocation, ROISize);
                    result = (Bitmap)image.AsBitmap().Clone();
                    image.ROI = Rectangle.Empty;
#if DEBUG
                }
                else
                    result = btm;
#endif                

                try
                {
                    if (result != null)
                        result.Save(stream, ImageFormat.Png);
                }
                catch { }

#if DEBUG
                if (!DebugFullCam && result != null)
                    result.Dispose();
#endif
            }

            btm.Dispose();
            FireNewFrameEvent(stream);
        }




        private void FireNewFrameEvent(MemoryStream ms)
        {
            if (NewFrameEvent != null)
                NewFrameEvent(this, ms);
        }
    }
}

