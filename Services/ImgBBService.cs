using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ActividadesExtraPortal.Services
{
    public class ImgBBService
    {
        private static readonly HttpClient client = new HttpClient();

        private const string ApiKey = "d337f4a9e81270b23748335276417b6d";

        /// <summary>
        /// Sube una imagen local a la API de imgBB.
        /// </summary>
        /// <param name="filePath">Ruta física de la imagen local.</param>
        /// <returns>URL de la imagen hospedada, o null si falla.</returns>
        public async Task<string?> SubirImagenAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ApiKey))
                {
                    throw new InvalidOperationException("Debe configurar una API Key válida en el archivo ImgBBService.cs.");
                }

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("El archivo de imagen no se encuentra localmente.", filePath);
                }

                using var form = new MultipartFormDataContent();
                byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                var fileContent = new ByteArrayContent(fileBytes);
                
                // Enviar archivo como parametro "image"
                form.Add(fileContent, "image", Path.GetFileName(filePath));

                string url = $"https://api.imgbb.com/1/upload?key={ApiKey}";
                var response = await client.PostAsync(url, form);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMsg = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error de API de imgBB: {response.StatusCode} - {errorMsg}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                
                if (doc.RootElement.TryGetProperty("data", out JsonElement dataElement) &&
                    dataElement.TryGetProperty("url", out JsonElement urlElement))
                {
                    return urlElement.GetString();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al subir imagen a imgBB: {ex.Message}");
                throw;
            }
        }
    }
}
