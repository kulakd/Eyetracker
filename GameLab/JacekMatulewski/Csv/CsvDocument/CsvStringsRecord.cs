using System;

namespace JacekMatulewski.Csv
{
    public class CsvStringsRecord : ICsvRecord
    {
        private string[] values = null;

        public CsvStringsRecord() //wymagane, aby być parametrem listy
        {
        }

        public CsvStringsRecord(params string[] values)
        {
            this.values = values; //kopia referencji
        }

        public void ParseValues(string[] values, CsvRecordParam param, IFormatProvider formatProvider)
        {
            this.values = values;
        }

        public string[] ToValues(CsvRecordParam param, IFormatProvider formatProvider)
        {
            if (values != null) return values;
            else throw new CsvException("No values");
        }

        public override string ToString()
        {
            return values.Concat(' ');
        }
    }
}
