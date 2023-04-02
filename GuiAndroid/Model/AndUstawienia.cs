﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiAndroid.Model
{
    public class AndUstawienia
    { 
        public static void Zapisz(int b, int c, string t)
        {
            Preferences.Default.Set("Rozmiar_przyciskow", b);
            Preferences.Default.Set("Rozmiar_czcionki", c);
            Preferences.Default.Set("Tlo", t);
        }

        public static double Przycisk()
        {
            int b = Preferences.Default.Get("Rozmiar_przyciskow", 100);
            return b;
        }
        public static double Czcionka()
        {
            int c = Preferences.Default.Get("Rozmiar_czcionki", 18);
            return c;
        }

        public static string Tlo()
        {
            string t = Preferences.Default.Get("Tlo", "aaaa");
            return t;
        }
    }
}
