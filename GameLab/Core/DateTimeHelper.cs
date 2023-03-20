using System;
using System.Collections.Generic;
using System.Text;

namespace GameLab
{
    public static class DateTimeHelper
    {
        private static string intToString(int number, bool threeDigits = false)
        {
            string _s = number.ToString();
            if (number < 10) _s = "0" + _s;
            if (threeDigits && number < 100) _s = "0" + _s;
            return _s;
        }

        //tworzy łańcuch o formacie, który nadaje się do ścieżki pliku
        //public static string ToPathString(this DateTime dateTime)
        public static string DateTimeToPathString(/*this*/ DateTime dateTime)
        {
            string s = intToString(dateTime.Year);
            s += "-" + intToString(dateTime.Month);
            s += "-" + intToString(dateTime.Day);

            s += " " + intToString(dateTime.Hour);
            s += "-" + intToString(dateTime.Minute);
            s += "-" + intToString(dateTime.Second);

            s += " " + intToString(dateTime.Millisecond, true);

            return s;
        }

        public static string CurrentDateTimePathString
        {
            get
            {
                return DateTimeToPathString(DateTime.Now);
            }
        }
    }
}
