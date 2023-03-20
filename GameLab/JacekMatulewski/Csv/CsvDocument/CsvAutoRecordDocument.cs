using System;
using System.Collections.Generic;
using System.Linq;

namespace JacekMatulewski.Csv
{
    public class CsvAutoRecordDocument<T> : CsvDocument<CsvAutoRecord<T>>
        where T : new()
    {
        public CsvAutoRecordDocument(CsvRecordParam param = null) 
        {
            base.param = param;
        }

        public CsvAutoRecordDocument(IEnumerable<T> records, CsvRecordParam param = null)
        {
            this.param = param;
            foreach (T record in records) AddRecord(record, false);
        }

        private CsvAutoRecordDocument(List<CsvAutoRecord<T>> records, CsvRecordParam param = null)
        {
            this.param = param;
            this.records = records;
        }

        public static CsvAutoRecordDocument<T> Load(string filename, char separatorChar, CsvRecordParam param = null, IFormatProvider formatProvider = null)
        {
            if (formatProvider == null) formatProvider = System.Globalization.CultureInfo.InvariantCulture;
            //przy takim rozwiązaniu jest List -> Array -> List
            CsvDocument<CsvAutoRecord<T>> csv = CsvDocument<CsvAutoRecord<T>>.Load(filename, separatorChar, param);
            CsvAutoRecord<T>[] records = csv.GetRecords();
            CsvAutoRecordDocument<T> autoCsv = new CsvAutoRecordDocument<T>(records.ToList(), param);
            autoCsv.filename = csv.Filename;
            autoCsv.headerComment = csv.HeaderComment;
            autoCsv.headerColumnNames = csv.HeaderColumnNames;
            autoCsv.separatorChar = csv.SeparatorChar;
            return autoCsv;
        }

        public new T[] GetRecords()
        {
            CsvAutoRecord<T>[] autoArray = base.GetRecords();
            List<T> list = new List<T>();
            foreach (CsvAutoRecord<T> autoElement in autoArray) list.Add(autoElement.GetValues());
            return list.ToArray();
        }

        public void AddRecord(T newRecord, bool saveToFile = true)
        {
            CsvAutoRecord<T> newAutoRecord = new CsvAutoRecord<T>(newRecord);
            records.Add(newAutoRecord);
            if (saveToFile) AddRecordToFile(newAutoRecord);
        }

        public void RemoveRecord(T record, bool saveToFile = true)
        {
            CsvAutoRecord<T> autoRecord = records.Find(r => r.GetValues().Equals(record));
            if (autoRecord == null) throw new CsvException("Record not found");
            records.Remove(autoRecord);
            if (saveToFile) Save();
        }

        public void UpdateRecord(T record, T newRecord, bool saveToFile = true)
        {
            CsvAutoRecord<T> autoRecord = new CsvAutoRecord<T>(record);
            int index = records.FindIndex(r => r.Equals(record));
            if (index < 0) throw new CsvException("Record not found");
            records[index] = new CsvAutoRecord<T>(newRecord);            
            if (saveToFile) Save();
        }

        //dodać wersje Add, Remove, Update z indeksami
    }

    public static partial class CsvExtensions
    {
        public static void ExportToCsvFile<T>(this IEnumerable<T> collection, string filename, char separatorChar = ';', string headerComment = null, string headerColumnNames = null, IFormatProvider formatProvider = null)
             where T : new()
        {
            CsvAutoRecordDocument<T> csv = new CsvAutoRecordDocument<T>(collection);                
            csv.SaveAs(filename, separatorChar, headerComment, headerColumnNames, formatProvider);
        }
    }
}
