using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GameLab.Eyetracking.OpenEyeGazeInterface
{
    public static class SmoothingData
    {
        public static Dictionary<string, double> Smoothing(List<Dictionary<string, string>> listOfParameters, List<string> listOfNameParameters, SmoothingType smoothingType = SmoothingType.SMA)
        {
            var resultParameters = new List<Dictionary<string, double>>();
            double tmp;
            for(int i=0; i<listOfParameters.Count; i++)
            {
                var dictionary = new Dictionary<string, double>();
                for(int j = 0; j<listOfParameters[i].Count; j++)
                {
                    var item = listOfParameters[i].ElementAt(j);                    
                    double.TryParse(item.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp);
                    dictionary.Add(item.Key, tmp);
                }
                resultParameters.Add(dictionary);
            }

            return Smoothing(resultParameters, listOfNameParameters, smoothingType);
        }


        public static Dictionary<string, double> Smoothing(List<Dictionary<string, double>> listOfParameters, List<string> listOfNameParameters, SmoothingType smoothingType = SmoothingType.SMA)
        {
            var resultParameters = new Dictionary<string, double>();
            foreach (var parameter in listOfNameParameters)
                resultParameters.Add(parameter, 0);

            double N = listOfParameters.Count;
            int parametersCount = resultParameters.Count;

            //obliczanie mianownika średniej kroczącej
            double weightsSum = 0;
            double w = 1; //tylko dla EMA
            double alfa = 2.0 / (N + 1.0);
            for (int n = 0; n < N; ++n)
            {
                double weight = 0;
                switch (smoothingType)
                {
                    case SmoothingType.None:
                        weight = n == N - 1 ? 1 : 0;
                        break;
                    case SmoothingType.SMA: //zwykła średnia arytmetyczna
                        weight = 1.0;
                        break;
                    case SmoothingType.WMA: //waga maleje w postępie arytmetycznym
                        weight = n + 1;
                        break;
                    case SmoothingType.EMA: //waga maleje w postępie wykładniczym                           
                        weight = w;
                        w *= 1 - alfa;
                        break;
                }
                weightsSum += weight;

                foreach (var parameter in listOfNameParameters)
                {
                    if (!listOfParameters[n].Any(x => x.Key == parameter))
                        continue;

                    resultParameters[parameter] += weight * listOfParameters[n][parameter];
                }
            }

            foreach (var parameter in listOfNameParameters)
                resultParameters[parameter] /= weightsSum;

            return resultParameters;
        }
    }

    public enum SmoothingType { None, SMA, WMA, EMA }; //https://en.wikipedia.org/wiki/Moving_average
}



