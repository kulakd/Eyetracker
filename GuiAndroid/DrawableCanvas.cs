using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Stream == null) return;
            var image = PlatformImage.FromStream(Stream);
            if (image != null)
                canvas.DrawImage(image, 0, 0, 400, 225);
        }
    }
}
