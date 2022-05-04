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
    /// Usado para asignar una foto por defecto (person.png) al empleado si no dispone de una.
    /// </summary>
    class ImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagen = "";

            if (value is string)
            {
                if (string.IsNullOrEmpty((string)value) || (string)value == "null")
                {
                    imagen = "../Img/person.png";
                }
                else
                {
                    imagen = (string)value;
                }
            }

            return imagen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
