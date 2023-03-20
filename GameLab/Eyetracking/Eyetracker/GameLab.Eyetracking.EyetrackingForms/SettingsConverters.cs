using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLab.Eyetracking.EyetrackerControls
{
    using JacekMatulewski.Settings;

    class GazeSmoothingFilterValueConverter : ISettingsValueConverter
    {
        public Type ValueType
        {
            get { return typeof(GazeSmoothingFilter); }
        }

        const char separator = ';';

        public string ConvertValueToString(object value, IFormatProvider formatProvider)
        {
            GazeSmoothingFilter filter = value as GazeSmoothingFilter;
            SmoothingType smoothingType = filter.SmoothingType;
            int smoothingSamplesRange = filter.SmoothingSamplesRange;
            string s = smoothingType.ToString() + separator + smoothingSamplesRange.ToString();
            return s;
        }

        public object ConvertValueFromString(string s, IFormatProvider formatProvider)
        {
            string[] sf = s.Split(separator);
            SmoothingType smoothingType = (SmoothingType)Enum.Parse(typeof(SmoothingType), sf[0]);
            int smoothingSamplesRange = int.Parse(sf[1]);
            return new GazeSmoothingFilter(smoothingType, smoothingSamplesRange);
        }
    }
}
