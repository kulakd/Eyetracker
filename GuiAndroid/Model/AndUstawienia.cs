using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiAndroid.Model
{
    public class AndUstawienia
    {
        public static void Zapisz(double buttons, int font, string background)
        {
            Preferences.Default.Set("Rozmiar_przyciskow", buttons);
            Preferences.Default.Set("Rozmiar_czcionki", font);
            Preferences.Default.Set("Tlo", background);
        }

        public static Parameters Wczytaj()
        {
            double buttons = Preferences.Default.Get("Rozmiar_przyciskow", 1.0);
            int font = Preferences.Default.Get("Rozmiar_czcionki", 18);
            string background = Preferences.Default.Get("Tlo", "aaaa");
            return new Parameters(buttons, font, background);
        }
    }
}
