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
    /// Converter usado para convertir el formato de hora y eliminar los segundos.
    /// </summary>
    class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string cadenaHora = "";

            if (value is string)
            {
                int valor = ((string)value).LastIndexOf(":");
                if (valor != -1)
                    cadenaHora = ((string)value).Substring(0, valor);
            }

            return cadenaHora;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
