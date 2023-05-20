using Microsoft.Maui.Graphics.Platform;
using System.Diagnostics;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace GuiAndroid
{
    internal class DrawableCanvas : IDrawable
    {
        private static MemoryStream _stream;
        public static MemoryStream Stream
        {
            get
            {
                if (_stream == null)
                    return null;
                _stream.Position = 0;
                return _stream;
            }
            set
            {
                MemoryStream old = _stream;
                _stream = value;
                if (old != null)
                    old.Dispose();
            }
        }

        private IImage shownImage;

        private Stopwatch _watch = Stopwatch.StartNew();
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {

            //if (_watch.ElapsedMilliseconds < 1000)
            //    return;
            //_watch.Restart();
            bool useFromBuffer = false;
            if (Stream == null || Stream.Length == 0)
                useFromBuffer = true;

            IImage image = PlatformImage.FromStream(Stream);
            try
            {
                if (image != null && image.Height != 0 && !useFromBuffer)
                {
                    shownImage = image;
                }
                else
                {
                    image = shownImage;
                }
                canvas.DrawImage(image, 0, 0, 400, 225);
            }
            catch
            {
                if (shownImage != null)
                    canvas.DrawImage(shownImage, 0, 0, 400, 225);
            }
        }
    }
}
