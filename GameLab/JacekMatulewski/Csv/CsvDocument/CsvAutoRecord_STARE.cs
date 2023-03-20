using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace JacekMatulewski.Csv
{
    public class CsvAutoRecord<T> : ICsvRecord
        where T : new()
    {
        private T values = default(T);
        //List<string> publicMembersNames = null;

        public CsvAutoRecord() //wymagane, aby być parametrem listy
        {
        }

        public CsvAutoRecord(T values/*, string[] publicMembersNames*/)
        {
            this.values = values; //kopia referencji
            //this.publicMembersNames = new List<string>(publicMembersNames); //nie zadziała w static, poza tym trzeba by przekazywać do każdej instancji
        }

        public T GetValues()
        {
            return values;
        }

        //można ogólnie zrobić: private static bool isProper(MemberInfo member)

        private static bool isProper(FieldInfo field)
        {
            bool result = !field.IsNotSerialized && !field.IsStatic;
            CsvWriteToFileAttribute attribute = field.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
            if(attribute!=null) result = result && attribute.Write;
            return result;
        }

        private static bool isProper(PropertyInfo property)
        {
            bool result = property.CanWrite;
            CsvWriteToFileAttribute attribute = property.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
            if (attribute != null) result = result && attribute.Write;
            return result;
        }

        public void ParseValues(string[] strings, CsvRecordParam param)
        {
            values = new T();
            //values = Activator.CreateInstance<T>();
            
            List<string> list = strings.ToList();

            FieldInfo[] fields = typeof(T).GetFields(); //tylko publiczne
            foreach (FieldInfo field in fields)
            {
                if (isProper(field))
                {
                    string s = list.First();
                    object o = null;
                    if (field.FieldType.IsEnum) o = Enum.Parse(field.FieldType, s);
                    else o = Convert.ChangeType(s, field.FieldType, CultureInfo.InvariantCulture);
                    field.SetValue(values, o);
                    list.RemoveAt(0);
                }
            }

            PropertyInfo[] properties = typeof(T).GetProperties(); //tylko publiczne
            foreach (PropertyInfo property in properties)
            {
                if (isProper(property))
                {
                    string s = list.First();
                    object o = null;
                    if (property.PropertyType.IsEnum) o = Enum.Parse(property.PropertyType, s);
                    else Convert.ChangeType(s, property.PropertyType, CultureInfo.InvariantCulture);
                    property.SetValue(values, o); //to nie działa dla struktur
                    list.RemoveAt(0);
                }
            }            
        }

        public string[] ToValues(CsvRecordParam param)
        {
            List<string> list = new List<string>();
            if (values != null)
            {
                FieldInfo[] fields = typeof(T).GetFields();
                foreach (FieldInfo field in fields)
                {
                    if (isProper(field))
                        list.Add(field.GetValue(values).ToString());
                }

                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (isProper(property))
                        list.Add(property.GetValue(values).ToString());
                }

                return list.ToArray();
            }
            else
            {
                throw new CsvException("No values");
            }            
        }

        public static string GetColumnTypesLine(char separator)
        {
            string line = "";

            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo field in fields)
            {
                if (isProper(field))
                    line += field.FieldType.Name + separator;
            }

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (isProper(property))
                {
                    line += property.PropertyType.Name + separator;
                }
            }

            return line;
        }

        public static string GetColumnNamesLine(char separator)
        {
            string line = "";

            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo field in fields)
            {
                if (isProper(field))
                {
                    string columnName = field.Name;
                    CsvWriteToFileAttribute attribute = field.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
                    if (attribute != null && attribute.ColumnName != null) columnName = attribute.ColumnName;
                    line += columnName + separator;
                }
            }

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (isProper(property))
                {
                    string columnName = property.Name;
                    CsvWriteToFileAttribute attribute = property.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
                    if (attribute != null && attribute.ColumnName != null) columnName = attribute.ColumnName;
                    line += columnName + separator;
                }
            }

            return line;
        }

        public override string ToString()
        {
            return values.ToString();
        }
    }

    //https://msdn.microsoft.com/en-us/library/sw480ze8.aspx
    //https://msdn.microsoft.com/en-us/library/a4a92379(v=vs.110).aspx
    //https://msdn.microsoft.com/en-us/library/aa288454(v=vs.71).aspx#vcwlkattributestutorialanchor3 - tutorial
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public class CsvWriteToFileAttribute : Attribute
    {
        private bool write; //position parameter
        public string ColumnName = null; //named parameter

        public CsvWriteToFileAttribute(bool write)
        {
            this.write = write;
        }

        public bool Write
        {
            get
            {
                return write;
            }
        }
    }

    public static partial class CsvExtensions
    {
        public static CsvDocument<CsvAutoRecord<T>> ToCsvDocument<T>(this List<T> list) where T : new()
        {
            //i tak jest kopiowanie danych, bez wzgledu na ustawienie drugiego parametru konstruktora CsvDocument
            List<CsvAutoRecord<T>> alist = new List<CsvAutoRecord<T>>();
            foreach (T element in list) alist.Add(new CsvAutoRecord<T>(element));
            return new CsvDocument<CsvAutoRecord<T>>(alist);
        } 
    }
}
