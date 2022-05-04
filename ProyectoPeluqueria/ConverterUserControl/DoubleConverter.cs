using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ProyectoPeluqueria.UserControlMenu.ConverterUserControl
{
    /// <summary>
    /// Converter double-string
    /// </summary>
    class DoubleConverter : IValueConverter    
    {
        string cantidadString;
        double cantidad;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            if (cantidad == (double)value)
                return cantidadString;
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            cantidadString = (string)value;

            if (string.IsNullOrEmpty(cantidadString) || !cantidadString.All(char.IsDigit))
                cantidad = 0.0;
            else
                cantidad = double.Parse(cantidadString);
            
            return cantidad;
        }
    }
}
