using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Collections;

//wersja 0.3 (22-03-2016)

namespace JacekMatulewski.Settings
{
    public class SettingsException : Exception 
    {
        public SettingsException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }

    public enum StoringMethod { SystemRegistry, XmlFile, JsonFile };

    public class SettingsManager<T>
        where T : class, new()
    {
        //private T settingsObject;
        private Dictionary<string, object> values = new Dictionary<string, object>();
        private StoringMethod storingMethod;
        private string path;
        private bool useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue;

        private Dictionary<Type, ISettingsValueConverter> memberConverters = null;
        private IFormatProvider formatProvider; //TODO: trzeba przetestować korzystanie z formatProvidera

        private static Dictionary<Type, ISettingsValueConverter> buildConvertersDictionary(IEnumerable<ISettingsValueConverter> memberConverters)
        {
            if (memberConverters != null)
            {
                Dictionary<Type, ISettingsValueConverter> _memberConverters = new Dictionary<Type, ISettingsValueConverter>();                
                foreach (ISettingsValueConverter memberConverter in memberConverters)
                    _memberConverters.Add(memberConverter.ValueType, memberConverter);
                return _memberConverters;
            }
            else return null;
        }

        private SettingsManager(Dictionary<string, object> values, StoringMethod storingMethod, string path, IEnumerable<ISettingsValueConverter> memberConverters, bool useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue, IFormatProvider formatProvider)
        {
            this.values = values;
            this.storingMethod = storingMethod;
            this.path = path;
            this.useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue = useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue;
            this.memberConverters = buildConvertersDictionary(memberConverters); //przy wołaniu drugiej wersji konstruktora, ta metoda jest wywoływana dwa razy
            this.formatProvider = formatProvider;
            if (this.formatProvider == null) this.formatProvider = System.Globalization.CultureInfo.InvariantCulture;
        }

        public SettingsManager(T settingsObject, StoringMethod storingMethod, string path, IEnumerable<ISettingsValueConverter> memberConverters = null, bool useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue = false, IFormatProvider formatProvider = null)
            : this(extractValues(settingsObject, buildConvertersDictionary(memberConverters), formatProvider), storingMethod, path, memberConverters, useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue, formatProvider)
        {
        }

        public static bool Exists(StoringMethod storingMethod, string path)
        {
            bool wynik = false;
            switch (storingMethod)
            {
                case StoringMethod.SystemRegistry:
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(path, false);
                    wynik = key != null;
                    if (key != null) key.Close();
                    break;
                case StoringMethod.XmlFile:
                case StoringMethod.JsonFile:
                    wynik = System.IO.File.Exists(path);
                    break;
            }
            return wynik;
        }

        private static bool isProper(FieldInfo field)
        {
            bool result = !field.IsNotSerialized && !field.IsStatic;
            //CsvWriteToFileAttribute attribute = field.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
            //if (attribute != null) result = result && attribute.Write;
            return result;
        }

        private static bool isProper(PropertyInfo property)
        {
            bool result = property.CanWrite;
            //CsvWriteToFileAttribute attribute = property.GetCustomAttribute(typeof(CsvWriteToFileAttribute)) as CsvWriteToFileAttribute;
            //if (attribute != null) result = result && attribute.Write;
            return result;
        }

        private static bool isCollection(Type type)
        {
            //nie można oprzeć na IEnumerable, bo może nie mieć Add, którego potrzebuję, a poza tym 
            //np. string też implementuje IEnumerable. Stąd dodatkowy warunek: typ należy do przestrzeni nazw System.Collections lub zawartej w niej
            //Dodatkowo: tylko parametryczne kolekcja, "stare" nie są obsługiwane
            return type.GetInterface(typeof(ICollection<>).FullName) != null && type.Namespace.StartsWith("System.Collections"); // || field.FieldType.GetInterface(typeof(ICollection).FullName) != null;
        }

        private static bool isArray(Type type)
        {
            return type.IsArray;
        }

        //--------------------------------------------------------------------------------------------------

        const char collectionElementsSeparator = ';';
        const char arrayElementsSeparator = ';';

        private static object prepareExtractedValue(object value, Type type, ISettingsValueConverter converter, IFormatProvider formatProvider)
        {
            //if (value == null && type == typeof(string)) value = ""; //nie wiem, czy to OK
            if (value == null) throw new NullReferenceException("Member value cannot be null");
            if (converter != null)
            {
                if (converter.ValueType != type) throw new SettingsException("Converter value type is different than member type");
                else return converter.ConvertValueToString(value, formatProvider);
            }
            else if (isCollection(type))
            {
                string scollection = "";
                IEnumerable collection = value as IEnumerable;
                foreach (object element in collection) scollection += element.ToString() + collectionElementsSeparator;
                scollection = scollection.TrimEnd(collectionElementsSeparator);
                return scollection;
            }
            else if (isArray(type))
            {
                string sarray = "";
                Array array = value as Array;
                foreach (object element in array) sarray += element.ToString() + arrayElementsSeparator;
                //sarray = sarray.TrimEnd(arrayElementsSeparator); //usuwa wiele średników, jeżeli są puste pola
                if(sarray.Length > 0 && sarray.Last() == arrayElementsSeparator) sarray = sarray.Substring(0, sarray.Length - 1);
                return sarray;
            }
            return value;
        }

        private static Dictionary<string, object> extractValues(T settingsObject, Dictionary<Type, ISettingsValueConverter> converters, IFormatProvider formatProvider)
        {
            if (settingsObject != null)
            {
                Dictionary<string, object> values = new Dictionary<string, object>();

                FieldInfo[] fields = typeof(T).GetFields();
                foreach (FieldInfo field in fields)
                {
                    if (isProper(field))
                    {
                        object value = field.GetValue(settingsObject);
                        ISettingsValueConverter converter = null;
                        if (converters != null && converters.Keys.Contains(field.FieldType)) converter = converters[field.FieldType];
                        values.Add(field.Name, prepareExtractedValue(value, field.FieldType, converter, formatProvider));
                    }
                }

                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (isProper(property))
                    {
                        object value = property.GetValue(settingsObject, null);
                        ISettingsValueConverter converter = null;
                        if (converters != null && converters.Keys.Contains(property.PropertyType)) converter = converters[property.PropertyType];
                        values.Add(property.Name, prepareExtractedValue(value, property.PropertyType, converter, formatProvider));
                    }
                }

                return values;
            }
            else
            {
                throw new SettingsException("No values in setting object");
            }
        }

        private static void saveToRegistry(Dictionary<string,object> values, string path, IFormatProvider formatProvider)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);
            if (key == null) key = Registry.CurrentUser.CreateSubKey(path);
            foreach(KeyValuePair<string,object> value in values)
            {
                //key.SetValue(value.Key, value.Value);
                key.SetValue(value.Key, Convert.ToString(value.Value, formatProvider));
            }
            key.Close();
        }

        private static void saveToXmlFile(Dictionary<string, object> values, string path, IFormatProvider formatProvider)
        {
            XDocument xml = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
                 new XElement("Settings",
                     from KeyValuePair<string, object> value in values
                     orderby value.Key
                     select new XElement("Setting",
                        new XAttribute("Name", value.Key),
                        new XAttribute("Type", value.Value.GetType().Name),
                        //value.Value.ToString()
                        Convert.ToString(value.Value, formatProvider)
                     )
                 )
            );
            xml.Save(path);
        }

        private static bool saveToJsonFile(Dictionary<string, object> values, string path, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            try
            {
                switch (storingMethod)
                {
                    case StoringMethod.SystemRegistry:
                        saveToRegistry(values, path, formatProvider);
                        break;
                    case StoringMethod.XmlFile:
                        saveToXmlFile(values, path, formatProvider);
                        break;
                    case StoringMethod.JsonFile:
                        saveToJsonFile(values, path, formatProvider);
                        break;
                }
            }
            catch(Exception exc)
            {
                throw new SettingsException("Settings saving error", exc);
            }
        }

        //-----------------------------------------------------------------------------

        private static List<string> extractMembers()
        {
            List<string> membersNames = new List<string>();

            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo field in fields)
            {
                if (isProper(field))
                {
                    membersNames.Add(field.Name);
                }
            }

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (isProper(property))
                {
                    membersNames.Add(property.Name);
                }
            }

            return membersNames;
        }

        //nie chcę robić rekurencji - w tabelach jest też tylko jeden poziom wgłąb
        private static object buildMemberCollection(Type memberType, string svalue, IFormatProvider formatProvider)
        {
            string[] svalues = svalue.Split(collectionElementsSeparator);
            Type[] genericArguments = memberType.GetGenericArguments();
            if (genericArguments.Count() > 1) throw new SettingsException("Dictionaries are not supported");
            MethodInfo addMethod = memberType.GetMethod("Add"); //nie mam jak zrzutować na ICollection, bo nie znam parametru
            if (addMethod == null) throw new SettingsException("No Add method found");
            object instance = Activator.CreateInstance(memberType);
            foreach (string selement in svalues)
            {
                object[] parameters = { Convert.ChangeType(selement, genericArguments[0], formatProvider) };
                addMethod.Invoke(instance, parameters);
            }
            return instance;
        }

        private static object buildMemberArray(Type memberType, string svalue, IFormatProvider formatProvider)
        {
            //zwykłe Split nie zachowuje liczby elementów tablicy, jeżeli są dwa średniki obok siebie ;; (traktuje jak jeden)
            string[] svalues = svalue.Split(arrayElementsSeparator);
            Type elementType = memberType.GetElementType();
            Array instance = Array.CreateInstance(elementType, svalues.Length);
            int from = instance.GetLowerBound(0);
            int to = svalues.GetUpperBound(0);
            for (int i = from; i <= to; ++i)
            {
                instance.SetValue(Convert.ChangeType(svalues[i - from], elementType, formatProvider), i);
            }
            return instance;
        }

        private static object buildMemberObject(Type memberType, object memberValue, ISettingsValueConverter memberConverter, IFormatProvider formatProvider)
        {
            if (memberValue == null) return null;
            string svalue = memberValue.ToString();
            if (memberType != typeof(string) && svalue.Equals(memberType.ToString())) throw new SettingsException("Value contains the type name: " + svalue + ". Probably the type of the field has no proper ToString method");
            object value;
            if (memberConverter != null)
            {
                if (memberConverter.ValueType != memberType) throw new SettingsException("Converter value type is different than member type");
                else value = memberConverter.ConvertValueFromString(svalue, formatProvider);
            }
            else if (memberType.IsEnum) value = Enum.Parse(memberType, svalue);
            else if (memberType.Equals(typeof(TimeSpan))) value = TimeSpan.Parse(svalue, formatProvider);
            else if (isCollection(memberType)) value = buildMemberCollection(memberType, svalue, formatProvider);
            else if (isArray(memberType)) value = buildMemberArray(memberType, svalue, formatProvider);
            else value = Convert.ChangeType(memberValue, memberType, formatProvider);
            return value;
        }

        //http://stackoverflow.com/questions/325426/programmatic-equivalent-of-defaulttype
        private static object getDefaultValue(Type type)
        {
            if (type.IsValueType) return Activator.CreateInstance(type);
            else return null;
        }

        private static T buildObject(Dictionary<string, object> values, Dictionary<Type, ISettingsValueConverter> memberConverters, bool useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue, IFormatProvider formatProvider)
        {
            T settingsObject = new T();

            //powinno dać się ujednolicić własności i pola (na bazie MemberInfo)
            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo field in fields)
            {                
                if (isProper(field))
                {
                    object value = null;
                    if (!values.ContainsKey(field.Name))
                    {
                        if (useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue) value = getDefaultValue(field.FieldType);
                        else throw new SettingsException("Building object failed - no value for field '" + field.Name + "'");
                    }
                    else
                    {
                        ISettingsValueConverter memberConverter = null;
                        if (memberConverters != null && memberConverters.Keys.Contains(field.FieldType)) memberConverter = memberConverters[field.FieldType];
                        value = buildMemberObject(field.FieldType, values[field.Name], memberConverter, formatProvider);
                    }
                    field.SetValue(settingsObject, value);                    
                }
            }

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (isProper(property))
                {
                    object value = null;
                    if (!values.ContainsKey(property.Name))
                    {
                        if (useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue) value = getDefaultValue(property.PropertyType);
                        else throw new SettingsException("Building object failed - no value for property '" + property.Name + "'");
                    }
                    else
                    {
                        ISettingsValueConverter memberConverter = null;
                        if (memberConverters != null && memberConverters.Keys.Contains(property.PropertyType)) memberConverter = memberConverters[property.PropertyType];
                        value = buildMemberObject(property.PropertyType, values[property.Name], memberConverter, formatProvider);
                    }
                    property.SetValue(settingsObject, value, null);
                }
            }

            return settingsObject;
        }

        private static Dictionary<string,object> loadFromRegistry(List<string> memberNames, string path)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(path, false);
            if (key == null) throw new SettingsException("Registry key not found");
            Dictionary<string, object> values = new Dictionary<string, object>();
            foreach (string memberName in memberNames) values.Add(memberName, key.GetValue(memberName)); //ignorowane są nadwyżkowe wartości
            key.Close();
            return values;
        }

        private static Dictionary<string, object> loadFromXmlFile(List<string> memberNames, string path)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            foreach(XElement element in XDocument.Load(path).Descendants("Setting"))
            {
                string name = element.Attribute("Name").Value;
                string value = element.Value;
                //nie odczytujemy typu, bo potem i tak będzie odczytany z typu pola T
                values.Add(name, value); 
            }
            return values;
        }

        private static Dictionary<string, object> loadFromJsonFile(List<string> memberNames, string path)
        {
            throw new NotImplementedException();
        }

        public static SettingsManager<T> Load(StoringMethod storingMethod, string path, IEnumerable<ISettingsValueConverter> memberConverters = null, bool useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue = false, bool useDefaultIfSettingNotExists = true, IFormatProvider formatProvider = null)
        {
            if (Exists(storingMethod, path))
            {
                try
                {
                    List<string> memberNames = extractMembers();
                    Dictionary<string, object> values = null;
                    switch (storingMethod)
                    {
                        case StoringMethod.SystemRegistry:
                            values = loadFromRegistry(memberNames, path);
                            break;
                        case StoringMethod.XmlFile:
                            values = loadFromXmlFile(memberNames, path);
                            break;
                        case StoringMethod.JsonFile:
                            values = loadFromJsonFile(memberNames, path);
                            break;
                    }
                    return new SettingsManager<T>(values, storingMethod, path, memberConverters, true, formatProvider);
                }
                catch (Exception exc)
                {
                    throw new SettingsException("Settings loading error", exc);
                }
            }
            else
            {
                if (useDefaultIfSettingNotExists)
                {
                    Dictionary<string, object> values = extractValues(new T(), buildConvertersDictionary(memberConverters), formatProvider);
                    return new SettingsManager<T>(values, storingMethod, path, memberConverters, true, formatProvider);
                }
                else throw new SettingsException("Incorrect settings path");
            }
        }

        //------------------------------------------------------------------------------

        public T GetSettingsObject() //dzięki temu, że T musi być class - zmiana będzie uwzględniania
        {
            return buildObject(values, memberConverters, useDefaultValueInsteadOfThrowingExceptionInCaseOfMissingStoredValue, formatProvider); //rozważyć zbuforowanie obiektu i zrobienie własności (flaga rebuildNecessary)
        }

        public void SetSettingsObject(T settingObject)
        {
            values = extractValues(settingObject, memberConverters, formatProvider);
        }
    }
}
