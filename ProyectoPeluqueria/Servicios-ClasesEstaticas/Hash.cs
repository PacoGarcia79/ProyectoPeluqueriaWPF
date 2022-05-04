using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria
{
    /// <summary>
    /// Clase usada para el cifrado de las contraseñas
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Obtiene la cadena cifrada a partir de la original mediante función SHA-256
        /// </summary>
        /// <param name="rawData">cadena original</param>
        /// <returns>cadena cifrada</returns>
        public static string ComputeSha256Hash(string rawData)
        { 
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));                
  
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
