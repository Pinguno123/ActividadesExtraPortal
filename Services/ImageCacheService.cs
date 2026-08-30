using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ActividadesExtraPortal.Services
{
    public static class ImageCacheService
    {
        /// <summary>
        /// Obtiene la ruta del archivo local en la caché para una URL dada.
        /// </summary>
        public static string GetLocalCachePath(string url, string cacheSubfolder)
        {
            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(url));
                    string hashStr = Convert.ToHexString(hash);
                    string extension = Path.GetExtension(new Uri(url).AbsolutePath);
                    if (string.IsNullOrEmpty(extension)) extension = ".png";

                    string cacheFolder = Path.Combine(Path.GetTempPath(), "ActividadesExtraPortal", cacheSubfolder);
                    return Path.Combine(cacheFolder, hashStr + extension);
                }
            }
            catch
            {
                return Path.Combine(Path.GetTempPath(), "ActividadesExtraPortal", cacheSubfolder, "default.png");
            }
        }

        /// <summary>
        /// Carga una imagen en un PictureBox desde la caché local si existe; de lo contrario, la descarga 
        /// de internet asíncronamente y guarda una copia local automáticamente.
        /// </summary>
        public static void CargarImagenConCache(PictureBox pictureBox, string? url, string cacheSubfolder)
        {
            if (pictureBox == null) return;

            if (string.IsNullOrEmpty(url))
            {
                pictureBox.Image = null;
                pictureBox.ImageLocation = null;
                return;
            }

            string localPath = GetLocalCachePath(url, cacheSubfolder);
            if (File.Exists(localPath))
            {
                pictureBox.ImageLocation = localPath;
            }
            else
            {
                AsyncCompletedEventHandler? handler = null;
                handler = (sender, e) =>
                {
                    // Desuscribir el manejador para evitar fugas de memoria
                    pictureBox.LoadCompleted -= handler;

                    if (e.Error == null && !e.Cancelled && pictureBox.Image != null)
                    {
                        string? currentUrl = pictureBox.ImageLocation;
                        if (currentUrl != null && (currentUrl.StartsWith("http://") || currentUrl.StartsWith("https://")))
                        {
                            try
                            {
                                string savePath = GetLocalCachePath(currentUrl, cacheSubfolder);
                                string? directory = Path.GetDirectoryName(savePath);
                                if (directory != null && !Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }

                                if (!File.Exists(savePath))
                                {
                                    pictureBox.Image.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error al guardar imagen en caché de servicio: " + ex.Message);
                            }
                        }
                    }
                };

                pictureBox.LoadCompleted += handler;
                pictureBox.ImageLocation = url;
            }
        }
    }
}
