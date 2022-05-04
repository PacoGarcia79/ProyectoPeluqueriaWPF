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
    /// Converter usado para convertir la cantidad stock en int, a una Lista para llenar el combobox.
    /// </summary>
    class StockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<int> listaStock = new List<int>();

            if (value is int)
            {
                int valor = (int)value;
                for (int i = 0; i < valor; i++)
                {
                    listaStock.Add(i + 1);
                }
            }

            return listaStock;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
