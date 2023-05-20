using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiAndroid.Model
{
    public class Parameters
    {
        public int Font;
        public string Background;

        public Parameters(int font, string background)
        {
            this.Font = font;
            this.Background = background;
        }
    }
}
