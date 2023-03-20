using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameLab.Eyetracking.EyetrackingForms
{
    public partial class RestForm : BasePauseForm
    {
        Bitmap restImage; int nx; int ny;

        public RestForm()
        {
            Screen screen = Screen.FromControl(this);
            nx = screen.Bounds.Width;
            ny = screen.Bounds.Height;
            restImage = getRestImage(nx, ny);

            InitializeComponent();
                       
            pictureBox.Image = restImage;
        }

        private Bitmap getRestImage(int nx, int ny)
        {
            Bitmap bmp = new Bitmap(nx, ny);
            double maxDistance = Math.Sqrt(nx * nx + ny * ny) / 1.4;
            int wx = nx / 2; int wy = 0; //white point
            for (int ix = 0; ix < nx; ++ix)
                for (int iy = 0; iy < ny; ++iy)
                {
                    double dx = ix - wx;
                    double dy = iy - wy;
                    double d = Math.Sqrt(dx * dx + dy * dy);
                    d = 255.0 * (1 - d / maxDistance);
                    if (d < 0) d = 0;
                    Color c = Color.FromArgb(0, (int)d, 0);
                    bmp.SetPixel(ix, iy, c);
                }
            return bmp;
        }
    }
}
