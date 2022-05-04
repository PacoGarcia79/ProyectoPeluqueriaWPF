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
    /// Converter usado para el diálogo de añadir producto a una cita. Muestra "Sin productos" si la cita no tiene asignado/s.
    /// </summary>
    class ProductosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string cadenaProductos = "";

            if (value is string)
            {
                if (string.IsNullOrEmpty((string)value))
                {
                    cadenaProductos = "Sin productos";
                }
                else
                {
                    cadenaProductos = (string)value;
                }
            }

            return cadenaProductos;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
