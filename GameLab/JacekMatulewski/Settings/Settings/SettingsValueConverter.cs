using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Uwaga! Ściągnięte z CsvValueConverter

namespace JacekMatulewski.Settings
{
    public interface ISettingsValueConverter
    {
        Type ValueType { get; }
        string ConvertValueToString(object value, IFormatProvider formatProvider);
        object ConvertValueFromString(string s, IFormatProvider formatProvider);
    }
}
