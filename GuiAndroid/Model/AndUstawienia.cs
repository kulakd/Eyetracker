using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiAndroid.Model
{
    public class AndUstawienia
    {
        public static void Zapisz(double b, int c, string t)
        {
            Preferences.Default.Set("Rozmiar_przyciskow", b);
            Preferences.Default.Set("Rozmiar_czcionki", c);
            Preferences.Default.Set("Tlo", t);
        }

        public static Parameters Wczytaj()
        {
            double b = Preferences.Default.Get("Rozmiar_przyciskow", 1.0);
            int c = Preferences.Default.Get("Rozmiar_czcionki", 18);
            string t = Preferences.Default.Get("Tlo", "aaaa.png");
            return new Parameters(b, c, t);
        }
    }
}
