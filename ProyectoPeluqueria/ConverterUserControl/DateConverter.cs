using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProyectoPeluqueria.UserControlMenu.ConverterUserControl
{
    /// <summary>
    /// Converte DateTime a cadena string con formato determinado
    /// </summary>
    class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string cadenaFecha = "";

            if(value is DateTime)
            {
                cadenaFecha = $"{value:dd - MM - yyyy}";
            }

            return cadenaFecha;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
