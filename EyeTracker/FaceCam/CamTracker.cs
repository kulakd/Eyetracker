using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace FaceCam
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
        private Queue<Rectangle> faceRects = new Queue<Rectangle>();

        private float rectPercent = 1f;

        private (int w, int h) aspect = (16, 9);
        private (int w, int h) imgSize = (400, 225);
        #endregion

        public event EventHandler<MemoryStream> NewFrameEvent;

        #region Properties
        public int Index
        {
            get => index;
            set
            {
                if (value == index) return;
                if (value < 0 || value >= filters.Count)
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
            get => 1f - rectPercent;
            set
            {
                if (value < 0 || value >= 1f)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Must be between 0 and 1");
                rectPercent = 1f - value;
            }
        }

        public int AspectWidth { get => aspect.w; set => aspect.w = value; }
        public int AspectHeight { get => aspect.h; set => aspect.h = value; }
        public int VideoWidth
        {
            get => imgSize.w;
            set
            {
                imgSize.w = value;
                imgSize.h = value * aspect.h / aspect.w;
            }
        }
        public int VideoHeight
        {
            get => imgSize.h;
            set
            {
                imgSize.h = value;
                imgSize.w = value * aspect.w / aspect.h;
            }
        }

        public ImageFormat Format { get; set; } = ImageFormat.Jpeg;
        #endregion

        public CamTracker()
        {
            RefreshDeviceList();
            index = -1;
        }

        public void Stop()
        {
            try
            {
                if (isStreaming)
                    device.Stop();
            }
            catch { }

            faceRects.Clear();
            index = -1;
        }

        public void RefreshDeviceList()
        {
            filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        private void Device_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap btm = eventArgs.Frame;
            Image<Bgr, byte> image = btm.ToImage<Bgr, byte>();
            Rectangle[] rects = faceCascadeClassifier.DetectMultiScale(image);

            //Jeżeli nie ma twarzy, wybiera cały ekran
            Rectangle rect = rects.Length > 0 ?
                rects.OrderByDescending(r => (r.Height * r.Width)).First() :
                new Rectangle(0, 0, btm.Width, btm.Height);

            Size ROISize = new Size((int)(btm.Width * rectPercent), (int)(btm.Height * rectPercent));
            Point ROILocation = rect.Location;

            ROILocation.X += rect.Width / 2;
            ROILocation.Y += rect.Height / 2;

            ROILocation.X -= ROISize.Width / 2;
            ROILocation.Y -= ROISize.Height / 2;

            image.ROI = new Rectangle(ROILocation, ROISize);
            Bitmap imgROI = image.AsBitmap();

            ResizeBilinear resize = new ResizeBilinear(imgSize.w, imgSize.h);
            Bitmap result = resize.Apply(imgROI);

            MemoryStream stream = new MemoryStream();
            try
            {
                result.Save(stream, Format);
            }
            catch { }

            result.Dispose();
            image.Dispose();
            FireNewFrameEvent(stream);
        }




        private void FireNewFrameEvent(MemoryStream ms)
        {
            if (NewFrameEvent != null)
                NewFrameEvent(this, ms);
        }
    }
}

