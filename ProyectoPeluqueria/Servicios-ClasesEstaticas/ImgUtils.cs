using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Servicios_ClasesEstaticas
{
    /// <summary>
    /// Clase de utilidad para el tratamiento de las imágenes
    /// </summary>
    public static class ImgUtils
    {
        /// <summary>
        /// Método estático para reducir el tamaño de la imagen
        /// </summary>
        /// <param name="imgToResize">Imagen a reducir</param>
        /// <param name="size">Tamaño deseado</param>
        /// <returns>Imagen reducida</returns>
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        /// <summary>
        /// Método estático para obtener el array de bytes de la imagen
        /// </summary>
        /// <param name="x">Imagen de interés</param>
        /// <returns>Array de bytes</returns>
        public static byte[] imgToByteArray(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        /// <summary>
        /// Método estático para obtener la imagen en formato Base64, previa reducción
        /// </summary>
        /// <param name="path">Ruta de la imagen</param>
        /// <returns>Imagen en formato Base64</returns>
        public static string imgToBase64(string path)
        {
            try
            {
                Image img = Image.FromFile(path);
                Bitmap imgbitmap = new Bitmap(img);
                int ancho = imgbitmap.Width;
                int alto = imgbitmap.Height;
                Image resizedImage = resizeImage(imgbitmap, new System.Drawing.Size(ancho / 4, alto / 4));
                byte[] bytes = imgToByteArray(resizedImage);
                return Convert.ToBase64String(bytes);
            }
            catch(Exception)
            {
                return "";
            }
            
        }
    }
}
