//kopia z projektu GCAF

namespace GameLab
{
    using System.Globalization;

    public static class TimeHelper
    {
        private const long ilośćTikówWMilisekundzie = 10000;

        public static double Ticks2Milisekundy(long ticks)
        {
            double wynik = (double)ticks / (double)ilośćTikówWMilisekundzie;
            return wynik;
        }

        public static long Milisekundy2Ticks(long milisekundy)
        {
            long wynik = milisekundy * ilośćTikówWMilisekundzie;
            return wynik;
        }

        public static long Milisekundy2Ticks(decimal milisekundy)
        {
            long wynik = (long)(milisekundy * ilośćTikówWMilisekundzie);
            return wynik;
        }        

        //wygląda jak powtórzenie, ale mnożenie dla innych typów
        public static long Milisekundy2Ticks(double milisekundy)
        {
            long wynik = (long)(milisekundy * ilośćTikówWMilisekundzie);
            return wynik;
        }

        //----------------------------------------------------------

        public static NumberFormatInfo FormatLiczb = new NumberFormatInfo() { NumberGroupSeparator = " ", NumberDecimalSeparator = "." };
    }
}
