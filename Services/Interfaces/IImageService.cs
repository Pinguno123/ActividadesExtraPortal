using System.Threading.Tasks;

namespace ActividadesExtraPortal.Services
{
    /// <summary>
    /// Interfaz para servicios de procesamiento y almacenamiento de imágenes en la nube.
    /// Aplica el principio de Inversión de Dependencias (DIP).
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Sube una imagen local al servidor y retorna su URL pública accesible.
        /// </summary>
        Task<string?> SubirImagenAsync(string filePath);
    }
}
