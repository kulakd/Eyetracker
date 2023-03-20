using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Globalization;

//wersja 1.08 (2015-10-08)

namespace JacekMatulewski.Csv
{
    public class CsvException : Exception 
    {
        public CsvException(string message)
            :base(message)
        {            
        }
    }

    public class CsvFormatException : CsvException 
    {
        public CsvFormatException(string message)
            :base(message)
        {
        }
    }
    public class CsvNoFilenameException : CsvException 
    {
        public CsvNoFilenameException(string message)
            :base(message)
        {
        }
    }

    public static partial class CsvExtensions
    {
        public static bool IsWithinQuotationMarks(this string s, char quoatationMark = '\"')
        {
            return s.First() == quoatationMark && s.Last() == quoatationMark;
        }

        public static string PutInQuotationMarks(this string s, char quoatationMark = '\"')
        {
            if (!IsWithinQuotationMarks(s)) return "" + quoatationMark + s + quoatationMark;
            else return s;
        }

        public static string[] SplitTakingIntoAccountQuotationMarks(this string line, char separatorChar, char quotationMarkChar)
        {
            if (!line.Contains(quotationMarkChar)) return line.Split(separatorChar);
            if (line.Count((char c) => { return c == quotationMarkChar; }) % 2 != 0) throw new FormatException("Odd number of quotation marks");
            List<string> strings = new List<string>();
            bool insideQuotation = false;
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < line.Length; ++i)
            {
                char c = line[i];
                if (c == quotationMarkChar)
                {
                    insideQuotation = !insideQuotation;
                }
                else if (!insideQuotation && c == separatorChar)
                {
                    strings.Add(s.ToString());
                    s.Clear();
                }
                else s.Append(c);
                if (i == line.Length - 1) //ostatni znak
                {
                    strings.Add(s.ToString());
                    s.Clear();
                }
            }
            return strings.ToArray();
        }

        public static string Concat(this IEnumerable<string> list, char separatorChar, char quotationMarkChar = '"')
        {
            string s = "";
            //foreach(string element in list) s += element + separatorChar;
            for (int i = 0; i < list.Count(); ++i)
            {
                string element = list.ElementAt(i);
                if (element.Contains(separatorChar) && !element.IsWithinQuotationMarks(quotationMarkChar)) throw new CsvException("Accidental separator char '" + separatorChar + "' inside the strings being concatened");
                s += element;
                if (i < list.Count() - 1) s += separatorChar;
            }
            return s;
        }

        public static string Concat<T>(this IEnumerable<T> list, char separatorChar)
        {
            string s = "";
            int numbersOfElements = list.Count();
            for (int i = 0; i < numbersOfElements; ++i)
            {
                T element = list.ElementAt(i);
                if (element != null) s += element.ToString();
                if (i < numbersOfElements - 1) s += separatorChar;
            }            
            return s;
        }
    }

    public interface ICsvRecord
    {
        //static ICsvRecord ParseLine(string line, char separator);
        //void ParseLine(string line, char separator, CsvRecordParam param);
        //string ToLine(char separator, CsvRecordParam param);

        void ParseValues(string[] values, CsvRecordParam param, IFormatProvider formatProvider);
        string[] ToValues(CsvRecordParam param, IFormatProvider formatProvider);
    }

    public class CsvRecordParam : Object //niepotrzebne, ale żeby zaznaczyć, że puste
    {
        public static readonly CsvRecordParam Empty;
    }

    public class CsvDocument<T> : IEnumerable<T>
        where T : ICsvRecord, new()
    {
        protected CsvRecordParam param;
        private IFormatProvider formatProvider;
        public const char CommentChar = '#';
        protected string filename = null;
        protected string headerComment = null;
        protected string headerColumnNames = null;
        protected char separatorChar;
        protected List<T> records = new List<T>();

        public const char QuotationMarkChar = '\"';

        public int Count
        {
            get
            {
                return records.Count;
            }
        }

        public string Filename
        {
            get
            {
                return filename;
            }
        }

        public string HeaderComment
        {
            get
            {
                return headerComment;
            }
        }

        public string HeaderColumnNames
        {
            get
            {
                return headerColumnNames;
            }
        }

        public char SeparatorChar
        {
            get
            {
                return separatorChar;
            }
        }

        public T this[int index]
        {
            get
            {
                return records[index];
            }
        }

        public T[] GetRecords()
        {
            return records.ToArray();
        }

        public CsvDocument(CsvRecordParam param = null, IFormatProvider formatProvider = null) 
        {
            this.param = param;
            if (formatProvider == null) formatProvider = CultureInfo.InvariantCulture;
            this.formatProvider = formatProvider;            
        }

        //copyData jest na wyrost, bo robi tylko nową listę z tymi samymi referencjami
        //można dla ICloneable: if (record is ICloneable) _record = (T)((ICloneable)record).Clone(); else _record = record;
        public CsvDocument(IEnumerable<T> records, CsvRecordParam param = null, bool copyData = false, IFormatProvider formatProvider = null) 
            : this(param, formatProvider)
        {
            if (!copyData) this.records = records.ToList(); //ToList() chyba kopiuje
            else foreach (T record in records) AddRecord(record, false); 
        }

        public static CsvDocument<T> CreateEmptyFile(string filename, char separatorChar, string headerComment, string headerColumnNames, CsvRecordParam param = null, IFormatProvider formatProvider = null)
        {
            CsvDocument<T> csv = new CsvDocument<T>(param, formatProvider);
            csv.filename = filename;
            //csv.separatorChar = separatorChar;
            //csv.headerComment = headerComment;
            //csv.headerColumnNames = headerColumnNames;
            csv.SaveAs(filename, separatorChar, headerComment, headerColumnNames);
            return csv;
        }

        /*
        private static bool TryParseRecordSecured(ref T record, string line, char separatorChar, CsvRecordParam param, IFormatProvider formatProvider)
        {
            try
            {
                line = line.Replace("^R", "\r"); //powrót karetki
                line = line.Replace("^N", "\n"); //nowe linie                
                //string[] strings = line.Trim(separatorChar).Split(separatorChar);
                string[] strings = line.Split(separatorChar);
                record.ParseValues(strings, param, formatProvider);
                return true;
            }
            catch(Exception exc)
            {                
                return false;
            }
        }
        */

        //dodana obsługa cudzysłowów
        private static bool TryParseRecordSecured(ref T record, string line, char separatorChar, CsvRecordParam param, IFormatProvider formatProvider)
        {
            try
            {                
                line = line.Replace("^R", "\r"); //powrót karetki
                line = line.Replace("^N", "\n"); //nowe linie                

                //string[] strings = line.Trim(separatorChar).Split(separatorChar);
                //string[] strings = line.Split(separatorChar);

                line = line.Replace(""+QuotationMarkChar+QuotationMarkChar, "^Q"); //cudzysłowy zakodowane dwoma cudzysłowami obok siebie
                string[] strings = line.SplitTakingIntoAccountQuotationMarks(separatorChar, QuotationMarkChar);
                for(int i = 0; i < strings.Length; i++) strings[i] = strings[i].Replace("^Q", "" + QuotationMarkChar);

                record.ParseValues(strings, param, formatProvider);
                return true;
            }
            catch //(Exception exc)
            {
                return false;
            }
        }

        public static CsvDocument<T> Load(string filename, char separatorChar, CsvRecordParam param = null, IFormatProvider formatProvider = null, Func<T, bool> filter = null)
        {
            CsvDocument<T> csv = new CsvDocument<T>(param, formatProvider); //tu jest inicjowany formatProvider, jeżeli podany w argumencie jest równy null -> dalej należy używać pola, a nie argumentu
            csv.filename = filename;
            csv.separatorChar = separatorChar;
            List<string> comments = new List<string>();
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.StartsWith(CommentChar.ToString()))
                    {
                        T record = new T();
                        //record.ParseLine(line, separatorChar);
                        if (!TryParseRecordSecured(ref record, line, separatorChar, param, csv.formatProvider))
                            throw new CsvFormatException("Nie udało się odczytać lub zanalizować linii: " + line + " w pliku '" + filename + "'");
                        else
                        {
                            if (filter == null || filter(record))
                                csv.records.Add(record); //dodaję bezpośrednio, a nie przez AddRecord(record, false)
                        }
                    }
                    else comments.Add(line);
                }
            }
            switch (comments.Count)
            {
                case 0:
                    csv.headerComment = null;
                    csv.headerColumnNames = null;
                    break;
                case 1:
                    csv.headerComment = null;
                    csv.headerColumnNames = comments[0];
                    break;
                default:
                    csv.headerComment = comments[0];
                    csv.headerColumnNames = comments[1];
                    break;
            }
            return csv;
        }

        private static string ValuesToLine(string[] values, char separatorChar)
        {            
            return values.Concat(separatorChar);
        }        

        private static string RecordToLineSecured(ICsvRecord record, char separatorChar, CsvRecordParam param, IFormatProvider formatProvider)
        {
            //dodawanie cudzysłowów
            string[] values = record.ToValues(param, formatProvider);
            for(int i = 0; i < values.Length; ++i)
            {
                if (!string.IsNullOrEmpty(values[i]))
                {
                    values[i] = values[i].Replace("" + QuotationMarkChar, "" + QuotationMarkChar + QuotationMarkChar);
                    if (values[i].Contains(separatorChar)) values[i] = values[i].PutInQuotationMarks(QuotationMarkChar);
                }
            }

            string line = ValuesToLine(values, separatorChar);
            line = line.Replace('^', '!'); //usuwam znak używany do kodowania znaków specjalnych
            line = line.Replace("\r", "^R"); //powrót karetki
            line = line.Replace("\n", "^N"); //nowe linie
            return line;
        }

        public void SaveAs(string filename, char separatorChar, string headerComment = null, string headerColumnNames = null, IFormatProvider formatProvider = null)
        {
            if (formatProvider == null) formatProvider = System.Globalization.CultureInfo.InvariantCulture;
            //nadpisywanie
            this.separatorChar = separatorChar;
            if (headerComment != null) this.headerComment = headerComment;
            if (headerColumnNames != null) this.headerColumnNames = headerColumnNames;
            if (filename != null) this.filename = filename;

            //zapis
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                if (!string.IsNullOrEmpty(headerComment)) sw.WriteLine(CommentChar + headerComment);
                if (!string.IsNullOrEmpty(headerColumnNames)) sw.WriteLine(CommentChar + headerColumnNames);
                foreach (ICsvRecord record in records)
                {
                    sw.WriteLine(RecordToLineSecured(record, separatorChar, param, formatProvider));
                }
            }
        }

        private void AddLineToFile(string line)
        {
            using (StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8))
            {
                if(!string.IsNullOrWhiteSpace(line))
                    sw.WriteLine(line);
            }
        }

        protected void AddRecordToFile(T newRecord)
        {
            //AddLineToFile(newRecord.ToLine(separatorChar));
            AddLineToFile(RecordToLineSecured(newRecord, separatorChar, param, formatProvider));
        }

        private void AddCommentToFile(string comment)
        {
            AddLineToFile(comment);
        }

        public void AddRecord(T newRecord, bool saveToFileIfOpen = true)
        {
            records.Add(newRecord); //nie robi klonów, można by: if(record is ICloneable) records.Add(newRecord.Clone());
            if(saveToFileIfOpen && filename != null) AddRecordToFile(newRecord);
        }

        public void AddRecordRange(IEnumerable<T> newRecords, bool saveToFileIfOpen = true)
        {
            foreach(T newRecord in newRecords) AddRecord(newRecord, saveToFileIfOpen);            
        }

        public void RemoveRecord(T record, bool saveToFileIfOpen = true)
        {
            records.Remove(record);
            if (saveToFileIfOpen && filename != null) Save();
        }

        public void RemoveRecord(int index, bool saveToFileIfOpen = true)
        {
            records.RemoveAt(index);
            if (saveToFileIfOpen && filename != null) Save();
        }

        public void UpdateRecord(int index, T newRecord, bool saveToFileIfOpen = true)
        {
            records[index] = newRecord;
            if (saveToFileIfOpen && filename != null) Save();
        }

        public void UpdateRecord(T record, T newRecord, bool saveToFileIfOpen = true)
        {
            int index = records.FindIndex(r => r.Equals(record) );
            if (index < 0) throw new CsvException("Record not found");
            UpdateRecord(index, newRecord, saveToFileIfOpen);
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(filename)) throw new CsvNoFilenameException("No filename specified");
            SaveAs(filename, separatorChar, headerComment, headerColumnNames);
        }

        public List<T> ToList()
        {
            //kopiuje tylko referencje elementów
            return records.ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return records.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
