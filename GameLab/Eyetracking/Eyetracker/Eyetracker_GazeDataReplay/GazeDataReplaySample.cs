using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLab.Eyetracking
{
    using GameLab.Geometry;
    using JacekMatulewski.Csv;

    //Ta klasa jest siostrzana z klasą GazeCoordinatesCsvRecord z projektu GazeDataExport
    public class GazeDataReplaySample : ICsvRecord
    {
        public long Ticks;
        public PointF LeftEyePosition;
        public PointF RightEyePosition;

        public void ParseValues(string[] values, CsvRecordParam param, IFormatProvider formatProvider)
        {
            Ticks = long.Parse(values[0], formatProvider);
            LeftEyePosition.X = (float)double.Parse(values[1], formatProvider);
            LeftEyePosition.Y = (float)double.Parse(values[2], formatProvider);
            RightEyePosition.X = (float)double.Parse(values[3], formatProvider);
            RightEyePosition.Y = (float)double.Parse(values[4], formatProvider);
        }

        public static string ColumnNames(char separator)
        {
            return "Ticks" + separator +
                "LeftEyeX" + separator + "LeftEyeY" + separator +
                "RightEyeX" + separator + "RightEyeY";
        }

        public string[] ToValues(CsvRecordParam param, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}
