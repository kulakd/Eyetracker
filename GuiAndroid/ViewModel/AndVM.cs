﻿using GuiAndroid.Model;

namespace GuiAndroid.ViewModel
{
    public class AndVM : MVVMKit.ViewModel
    {
        private readonly AndModel model = new AndModel();

        private MemoryStream _video;
        private MemoryStream video
        {
            get
            {
                _video.Position = 0;
                return _video;
            }
            set
            {
                MemoryStream tmp = _video;
                _video = value;
                tmp.Dispose();
                OnPropertyChanged(nameof(Video));
            }
        }

        public ImageSource Video => ImageSource.FromStream(() => video);

        public AndVM()
        {
            model.Connection.ImageReceived += (s, im) => video = im;
        }
    }
}
