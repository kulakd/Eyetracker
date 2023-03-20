using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace GameLab.Geometry.WindowsForms
{
    using D = System.Drawing;    

    public class BitmapHelper
    {
        //długie, ale tylko raz
        private static void ChangeBitmapColor2(D.Bitmap bmp, D.Color color)
        {
            D.Color patternForeColor = D.Color.Magenta;
            for (int ix = 0; ix < bmp.Width; ++ix)
                for (int iy = 0; iy < bmp.Height; ++iy)
                {
                    D.Color pixel = bmp.GetPixel(ix, iy);
                    if (pixel.R == patternForeColor.R && pixel.G == patternForeColor.G && pixel.B == patternForeColor.B)
                        bmp.SetPixel(ix, iy, color);
                }
        }

        //https://www.codeproject.com/Articles/617613/Fast-pixel-operations-in-NET-with-and-without-unsa
        public static void ChangeBitmapColor(D.Bitmap bmp, D.Color color)
        {
            D.Color patternForeColor = D.Color.Magenta;

            D.Rectangle bmpBounds = new D.Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData imageData = bmp.LockBits(bmpBounds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            byte[] imageBytes = new byte[Math.Abs(imageData.Stride) * bmp.Height];
            IntPtr scan0 = imageData.Scan0;

            System.Runtime.InteropServices.Marshal.Copy(scan0, imageBytes, 0, imageBytes.Length);

            for (int i = 0; i < imageBytes.Length; i += 4)
            {
                byte b = imageBytes[i + 0];
                byte g = imageBytes[i + 1];
                byte r = imageBytes[i + 2];
                byte a = imageBytes[i + 3];

                if (r == patternForeColor.R && g == patternForeColor.G && b == patternForeColor.B)
                {
                    imageBytes[i + 0] = color.B; ;
                    imageBytes[i + 1] = color.G;
                    imageBytes[i + 2] = color.R;
                    imageBytes[i + 3] = a;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(imageBytes, 0, scan0, imageBytes.Length);
            bmp.UnlockBits(imageData);
        }
    }
}
