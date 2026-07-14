using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActividadesExtraPortal.Data;

namespace ActividadesExtraPortal.Views
{
    public partial class AsociacionTarjeta : UserControl
    {
        private static readonly HttpClient _httpClient;

        static AsociacionTarjeta()
        {
            _httpClient = new HttpClient();
            // Configurar User-Agent
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }

        public AsociacionTarjeta()
        {
            InitializeComponent();
            
            // Propagar evento click
            pictureBox1.Click += child_Click;
            lblNombre.Click += child_Click;
            lblFunda.Click += child_Click;
            lblDesc.Click += child_Click;
        }

        public void CargarDatos(Asociacion aso, string? estadoMembresia = null)
        {
            this.Tag = aso;
            lblNombre.Text = aso.Nombre;
            lblDesc.Text = aso.Descripcion;

            string fundacionTxt = aso.AnioFundacion.HasValue ? aso.AnioFundacion.Value.ToString() : "N/A";

            if (string.IsNullOrEmpty(estadoMembresia))
            {
                lblFunda.Text = $"Fundación: {fundacionTxt}";
                this.BackColor = Color.FromArgb(248, 249, 250);
            }
            else
            {
                lblFunda.Text = $"Fundación: {fundacionTxt} ({estadoMembresia})";
                if (estadoMembresia == "Aprobada")
                {
                    this.BackColor = Color.FromArgb(220, 245, 230); // Color estado aprobado
                }
                else if (estadoMembresia == "Pendiente")
                {
                    this.BackColor = Color.FromArgb(255, 243, 205); // Color estado pendiente
                }
                else if (estadoMembresia == "Rechazada")
                {
                    this.BackColor = Color.FromArgb(248, 215, 218); // Color estado rechazado
                }
                else
                {
                    this.BackColor = Color.FromArgb(248, 249, 250);
                }
            }

            if (!string.IsNullOrEmpty(aso.ImgUrl))
            {
                _ = CargarImagenDesdeUrlAsync(aso.ImgUrl);
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        private string GetLocalCachePath(string url)
        {
            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(url);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convertir hash a hexadecimal
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    string hashStr = sb.ToString();

                    string extension = ".png";
                    try
                    {
                        string urlExt = Path.GetExtension(new Uri(url).AbsolutePath);
                        if (!string.IsNullOrEmpty(urlExt))
                        {
                            extension = urlExt;
                        }
                    }
                    catch { }

                    string cacheFolder = Path.Combine(Path.GetTempPath(), "ActividadesExtraPortal", "AsociacionesCache");
                    if (!Directory.Exists(cacheFolder))
                    {
                        Directory.CreateDirectory(cacheFolder);
                    }

                    return Path.Combine(cacheFolder, hashStr + extension);
                }
            }
            catch
            {
                string cacheFolder = Path.Combine(Path.GetTempPath(), "ActividadesExtraPortal", "AsociacionesCache");
                if (!Directory.Exists(cacheFolder))
                {
                    Directory.CreateDirectory(cacheFolder);
                }
                return Path.Combine(cacheFolder, "default_image.png");
            }
        }

        private async Task CargarImagenDesdeUrlAsync(string url)
        {
            string localPath = GetLocalCachePath(url);

            try
            {
                // Cargar desde cache local
                if (File.Exists(localPath))
                {
                    byte[] fileData = await File.ReadAllBytesAsync(localPath);
                    if (fileData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(fileData))
                        {
                            using (Image img = Image.FromStream(ms))
                            {
                                Bitmap bitmap = new Bitmap(img.Width, img.Height);
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    g.DrawImage(img, 0, 0, img.Width, img.Height);
                                }
                                SetPictureBoxImage(bitmap);
                                return;
                            }
                        }
                    }
                }

                // Descargar de internet
                byte[] downloadedData = await _httpClient.GetByteArrayAsync(url);

                // Guardar en cache local
                await File.WriteAllBytesAsync(localPath, downloadedData);

                // Cargar en el PictureBox
                using (MemoryStream ms = new MemoryStream(downloadedData))
                {
                    using (Image img = Image.FromStream(ms))
                    {
                        Bitmap bitmap = new Bitmap(img.Width, img.Height);
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.DrawImage(img, 0, 0, img.Width, img.Height);
                        }
                        SetPictureBoxImage(bitmap);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image_errors.log");
                    File.AppendAllText(logPath, $"[{DateTime.Now}] Error al procesar imagen desde {url} (Ruta: {localPath}): {ex.Message}\n{ex.StackTrace}\n\n");
                }
                catch { }

                SetPictureBoxImage(null);
            }
        }

        private void SetPictureBoxImage(Image? img)
        {
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new Action(() =>
                {
                    var oldImage = pictureBox1.Image;
                    pictureBox1.Image = img;
                    oldImage?.Dispose();
                }));
            }
            else
            {
                var oldImage = pictureBox1.Image;
                pictureBox1.Image = img;
                oldImage?.Dispose();
            }
        }

        private void child_Click(object? sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }
}
