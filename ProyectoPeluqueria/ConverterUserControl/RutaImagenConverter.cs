using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProyectoPeluqueria.UserControlMenu.ConverterUserControl
{
    /// <summary>
    /// Comprueba la ruta de la imagen. Si solo contiene el nombre de la imagen, añade el resto de la ruta
    /// </summary>
    class RutaImagenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagen = "";
            string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);


            string[] paths = { path, "img" };
            string folderName = Path.Combine(paths);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }


            if (value is string)
            {
                if (!((string)value).Contains("http"))
                {
                    path = string.Format(path + "\\{0}\\" + value, "img");

                    imagen = path.Replace("\\", "/");
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
