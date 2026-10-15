using System.Collections.Generic;

namespace ActividadesExtraPortal.Data
{
    /// <summary>
    /// Interfaz genérica base del patrón Repository que establece los contratos fundamentales
    /// de lectura y gestión para cualquier entidad de datos.
    /// Aplica el principio de Inversión de Dependencias (DIP) y Segregación de Interfaces (ISP).
    /// </summary>
    /// <typeparam name="T">Tipo de la entidad del modelo</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene la colección completa de elementos disponibles de la entidad.
        /// </summary>
        List<T> ObtenerTodos();
    }
}
