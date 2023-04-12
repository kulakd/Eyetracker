using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiAndroid.Model
{
    public class Parameters
    {
        public double B;
        public int C;
        public string T;

        public Parameters(double b, int c, string t)
        {
            this.B = b;
            this.C = c;
            this.T = t;
        }
    }
}
