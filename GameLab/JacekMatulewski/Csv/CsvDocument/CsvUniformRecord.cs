using System;
using System.Collections.Generic;

namespace JacekMatulewski.Csv
{
    public class CsvUniformRecord<T> : ICsvRecord
    {
        private T[] values = null;

        public CsvUniformRecord() //wymagane, aby być parametrem listy
        {
        }

        public CsvUniformRecord(params T[] values)
        {
            this.values = values; //kopia referencji
        }

        public void ParseValues(string[] values, CsvRecordParam param, IFormatProvider formatProvider)
        {            
            List<T> list = new List<T>();
            for (int i = 0; i < values.Length; ++i)
            {
                string s = values[i];
                //if (string.IsNullOrEmpty(s) && i == strings.Length - 1) continue;
                T value = (T)Convert.ChangeType(s, typeof(T), formatProvider);
                list.Add(value);
            }
            this.values = list.ToArray(); //kopiowanie wartości
        }

        public string[] ToValues(CsvRecordParam param, IFormatProvider formatProvider)
        {
            if (values != null)
            {
                string[] strings = new string[values.Length];
                for(int i = 0; i<values.Length;++i) 
                    //strings[i]=values[i].ToString(formatProvider);
                    strings[i] = Convert.ToString(values[i], formatProvider);
                return strings;
            }
            else
            {
                throw new CsvException("No values");
            }
        }

        public T[] GetValues()
        {
            return values;
        }

        public override string ToString()
        {
            return values.Concat<T>(' ');
        }
    }

}
