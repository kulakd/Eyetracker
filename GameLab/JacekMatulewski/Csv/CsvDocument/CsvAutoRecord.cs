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
        private static FieldInfo[] fields = getFields();
        private static PropertyInfo[] properties = getProperties();

        //private static readonly CultureInfo cultureInfo = CultureInfo.InvariantCulture; //może to powinno być opcjonalnie ustawiene dla całego CsvDocument

        //można ogólnie zrobić: private static bool isProper(MemberInfo member)
        private static bool isProper(FieldInfo field)
        {
            bool result = !field.IsNotSerialized && !field.IsStatic;
            //CsvWriteToFileAttribute attribute = field.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute; //.NET 4.5
            CsvWriteToFileAttribute attribute = Attribute.GetCustomAttribute(field, typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
            if (attribute != null) result = result && attribute.Write;
            return result;
        }

        private static bool isProper(PropertyInfo property)
        {
            //tu można już zrobić filtrowanie z użyciem isProper (Where)
            bool result = property.CanWrite;
            //CsvWriteToFileAttribute attribute = property.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
            CsvWriteToFileAttribute attribute = Attribute.GetCustomAttribute(property, typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
            if (attribute != null) result = result && attribute.Write;
            return result;
        }

        public static FieldInfo[] getFields()
        {
            //FieldInfo[] fields = typeof(T).GetFields(); //tylko publiczne
            FieldInfo[] fields = typeof(T).GetFields().Where(isProper).ToArray(); //tylko publiczne
            Comparison<FieldInfo> fieldComparison = (FieldInfo fieldX, FieldInfo fieldY) => { return fieldX.Name.CompareTo(fieldY.Name); };
            Array.Sort(fields, fieldComparison); //sortowanie, bo GetFields nie gwarantuje żadnego uporządkowania
            return fields;
        }

        public static PropertyInfo[] getProperties()
        {
            PropertyInfo[] properties = typeof(T).GetProperties().Where(isProper).ToArray(); //tylko publiczne
            Comparison<PropertyInfo> propertyComparison = (PropertyInfo propertyX, PropertyInfo propertyY) => { return propertyX.Name.CompareTo(propertyY.Name); };
            Array.Sort(properties, propertyComparison); //sortowanie, bo GetProperties nie gwarantuje żadnego uporządkowania
            return properties;
        }

        public CsvAutoRecord() //wymagane, aby być parametrem listy
        {
            this.values = new T();
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

        public void ParseValues(string[] strings, CsvRecordParam param, IFormatProvider formatProvider = null)
        {
            if (formatProvider == null) formatProvider = System.Globalization.CultureInfo.InvariantCulture;
            values = new T();
            //values = Activator.CreateInstance<T>();            
            List<string> list = strings.ToList(); //to powinna być kolejka!!!!!!!!!!!!!!
            foreach (FieldInfo field in fields)
            {
                try
                {
                    //if (isProper(field))
                    {
                        string s = list.First();
                        object o = null;
                        if (field.FieldType.IsEnum) o = Enum.Parse(field.FieldType, s);
                        else o = Convert.ChangeType(s, field.FieldType, formatProvider);
                        field.SetValue(values, o);
                        list.RemoveAt(0);
                    }
                }
                catch//(Exception exc)
                {
                    //ignorowanie elementów, których nie mżna sparsować
                    //int i = 0;
                }
            }

            foreach (PropertyInfo property in properties)
            {
                //if (isProper(property))
                {
                    string s = list.First();
                    object o = null;
                    if (property.PropertyType.IsEnum) o = Enum.Parse(property.PropertyType, s);
                    else Convert.ChangeType(s, property.PropertyType, formatProvider);
                    property.SetValue(values, o, null); //to nie działa dla struktur
                    list.RemoveAt(0);
                }
            }            
        }

        public string[] ToValues(CsvRecordParam param, IFormatProvider formatProvider)
        {
            List<string> list = new List<string>();
            if (values != null)
            {
                foreach (FieldInfo field in fields)
                {
                    //if (isProper(field))
                    {
                        object o = field.GetValue(values); //konwersja enum w tę stronę nie stwarza problemów
                        string s = Convert.ToString(o, formatProvider);
                        list.Add(s);
                    }
                }

                foreach (PropertyInfo property in properties)
                {
                    //if (isProper(property))
                    {
                        object o = property.GetValue(values, null);
                        string s = Convert.ToString(o, formatProvider);
                        list.Add(s);
                    }
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

            foreach (FieldInfo field in fields)
            {
                //if (isProper(field))
                    line += field.FieldType.Name + separator;
            }

            foreach (PropertyInfo property in properties)
            {
                //if (isProper(property))
                {
                    line += property.PropertyType.Name + separator;
                }
            }

            return line;
        }

        public static string GetColumnNamesLine(char separator)
        {
            string line = "";

            foreach (FieldInfo field in fields)
            {
                //if (isProper(field))
                {
                    string columnName = field.Name;
                    //CsvWriteToFileAttribute attribute = field.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute; //.NET 4.5
                    CsvWriteToFileAttribute attribute = Attribute.GetCustomAttribute(field, typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
                    if (attribute != null && attribute.ColumnName != null) columnName = attribute.ColumnName;
                    line += columnName + separator;
                }
            }

            foreach (PropertyInfo property in properties)
            {
                //if (isProper(property))
                {
                    string columnName = property.Name;
                    //CsvWriteToFileAttribute attribute = property.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
                    CsvWriteToFileAttribute attribute = Attribute.GetCustomAttribute(property, typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
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

        /*
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            CsvAutoRecord<T> other = obj as CsvAutoRecord<T>;
            //metoda 1
            return values.Equals(other.values);
            //metoda 2
            //string[] thisValues = ToValues();
            //string[] otherValues = other.ToValues();            
        }
        */
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
