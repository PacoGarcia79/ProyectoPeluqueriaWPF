using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria
{
    public static class PathUtils
    {
        /// <summary>
        /// Obtiene el directorio en el que se guardará la foto seleccionada
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo a guardar</param>
        /// <param name="nombreCarpeta">Nombre de la carpeta en la que se guardará el archivo</param>
        /// <returns>Ruta completa</returns>
        public static string GetDestinationPath(string nombreArchivo, string nombreCarpeta)
        {
            string appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string[] paths = { appPath, "img" };
            string folderName = Path.Combine(paths);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string path = string.Format(appPath + "\\{0}\\" + nombreArchivo, nombreCarpeta);

            return path;
        }
    }
}
