using System;

namespace ActividadesExtraPortal.Data
{
    /// <summary>
    /// Clase base abstracta para la capa de acceso a datos (Patrón Repository).
    /// Centraliza la referencia al Singleton de DatabaseConnection y métodos comunes
    /// protegidos para todas las implementaciones de repositorios.
    /// </summary>
    public abstract class BaseRepository
    {
        /// <summary>
        /// Acceso protegido al Singleton de conexión para clases derivadas.
        /// </summary>
        protected readonly DatabaseConnection db = DatabaseConnection.Instance;

        protected BaseRepository() { }
    }
}
