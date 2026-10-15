using System.Threading.Tasks;

namespace ActividadesExtraPortal.Services
{
    /// <summary>
    /// Interfaz para servicios de captura y extracción de credenciales y perfiles académicos.
    /// Aplica el principio de Inversión de Dependencias (DIP).
    /// </summary>
    public interface IScrapingService
    {
        /// <summary>
        /// Inicia el flujo de automatización y devuelve el perfil extraído del usuario.
        /// </summary>
        Task<Usuario?> IniciarCapturaAsync();
    }
}
