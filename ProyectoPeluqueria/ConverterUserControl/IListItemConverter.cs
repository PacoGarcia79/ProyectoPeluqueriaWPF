using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.ConverterUserControl
{
    /// <summary>
    /// Interfaz usada para implementar la selección múltiple en un DataGrid 
    /// 
    /// <href>https://github.com/itsChris/WpfMvvmDataGridMultiselect</href>
    /// </summary>
    public interface IListItemConverter
    {
        /// <summary>
        /// Convierte el elemento especificado de la lista principal.
        /// </summary>
        /// <param name="masterListItem">Elemento especificado de la lista principal.</param>
        /// <returns>Resultado de la conversión.</returns>
        object Convert(object masterListItem);

        /// <summary>
        /// Convierte el elemento especificado de la lista de destino.
        /// </summary>
        /// <param name="targetListItem">Elemento especificado de la lista destino.</param>
        /// <returns>Resultado de la conversión.</returns>
        object ConvertBack(object targetListItem);
    }
}
