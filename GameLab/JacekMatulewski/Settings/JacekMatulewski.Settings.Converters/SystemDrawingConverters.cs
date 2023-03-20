using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacekMatulewski.Settings
{
    public class ColorValueConverter : ISettingsValueConverter
    {
        public Type ValueType
        {
            get { return typeof(Color); }
        }

        const char separator = ';';

        public string ConvertValueToString(object value, IFormatProvider formatProvider)
        {
            Color color = (Color)value;
            //return color.Name;
            string s = "" + color.A + separator + color.R + separator + color.G + separator + color.B;
            return s;
        }

        public object ConvertValueFromString(string s, IFormatProvider formatProvider)
        {
            //Color c = Color.FromName(s.ToUpper()); //to nie parsuje z haszem
            //return c;

            string[] sargb = s.Split(separator);
            byte[] argb = new byte[4];
            try
            {
                for (int i = 0; i < argb.Length; ++i) argb[i] = byte.Parse(sargb[i]);
                return Color.FromArgb(argb[0], argb[1], argb[2], argb[3]);
            }
            catch
            {
                return Color.Black;
            }
            
        }
    }

    //oryginalnie był w GazePlotSettings.cs
    public class ColorValueConverter2 : ISettingsValueConverter
    {
        public Type ValueType
        {
            get { return typeof(Color); }
        }

        ColorConverter colorConverter = new ColorConverter();

        public string ConvertValueToString(object value, IFormatProvider formatProvider)
        {
            Color color = (Color)value;
            return colorConverter.ConvertToString(value);
        }

        public object ConvertValueFromString(string s, IFormatProvider formatProvider)
        {
            return colorConverter.ConvertFromString(s);
        }
    }

    //oryginalnie był w GazePlotSettings.cs
    public class ColorArrayValueConverter : ISettingsValueConverter
    {
        public Type ValueType
        {
            get { return typeof(Color[]); }
        }

        const char separator = ':';
        ColorValueConverter colorConverter = new ColorValueConverter();

        public string ConvertValueToString(object value, IFormatProvider formatProvider)
        {
            Color[] colors = (Color[])value;
            string s = "";
            foreach (Color color in colors) s += colorConverter.ConvertValueToString(color, formatProvider) + separator;
            return s.TrimEnd(separator);
        }

        public object ConvertValueFromString(string s, IFormatProvider formatProvider)
        {
            string[] svalues = s.Split(separator);
            Color[] colors = new Color[svalues.Length];
            for (int i = 0; i < colors.Length; ++i) colors[i] = (Color)colorConverter.ConvertValueFromString(svalues[i], formatProvider);
            return colors;
        }
    }

    public class FontValueConverter : ISettingsValueConverter
    {
        public Type ValueType
        {
            get { return typeof(Font); }
        }

        FontConverter fontConverter = new FontConverter();

        public string ConvertValueToString(object value, IFormatProvider formatProvider)
        {
            return fontConverter.ConvertToString(value);
        }

        public object ConvertValueFromString(string s, IFormatProvider formatProvider)
        {
            return fontConverter.ConvertFromString(s);
        }
    }
}
