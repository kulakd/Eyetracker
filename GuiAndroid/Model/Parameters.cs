using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiAndroid.Model
{
    public class Parameters
    {
        public double Buttons;
        public int Font;
        public string Background;

        public Parameters(double buttons, int font, string background)
        {
            this.Buttons = buttons;
            this.Font = font;
            this.Background = background;
        }
    }
}
