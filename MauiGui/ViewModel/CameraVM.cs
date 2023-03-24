using MauiGui.Model;

namespace MauiGui.ViewModel
{
    class CameraVM : ViewModel
    {
        #region Video
        private MemoryStream _vid = new MemoryStream();
        private MemoryStream video
        {
            get
            {
                _vid.Position = 0;
                return _vid;
            }
            set
            {
                MemoryStream tmp = _vid;
                _vid = value;
                tmp.Dispose();
                OnPropertyChanged(nameof(Video));
            }
        }

        public ImageSource Video => ImageSource.FromStream(() => video);
        #endregion

#if WINDOWS
        private readonly WinModel model;

        public CameraVM()
        {
            model = WinModel.Instance;
            model.Camera.NewFrameEvent += (s, f) => video = f;
            model.Camera.Index = 0; //TODO: make it settings dependent, and close after page changing
        }
#endif
    }
}
