using Microsoft.Maui.Graphics.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiAndroid.Model
{
    public class AndUstawienia
    {
        public static void Zapisz(int font, string background)
        {
            Preferences.Default.Set("Rozmiar_czcionki", font);
            Preferences.Default.Set("Tlo", background);
        }

        public static Parameters Wczytaj()
        {
            int font = Preferences.Default.Get("Rozmiar_czcionki", 14);
            string background = Preferences.Default.Get("Tlo", "steel2.jpg");
            return new Parameters(font, background);
        }
    }
}
