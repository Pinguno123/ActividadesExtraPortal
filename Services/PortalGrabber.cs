using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ActividadesExtraPortal
{
    public class PortalGrabber
    {
        public async Task<Usuario?> IniciarCapturaAsync()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-blink-features=AutomationControlled");

            return await Task.Run(async () =>
            {
                using var driver = new ChromeDriver(options);
                try
                {
                    driver.Navigate().GoToUrl("https://admacad.udb.edu.sv/PortalWeb/");

                    string usuarioDetectado = "Estudiante";
                    bool loginExitoso = false;

                    while (!loginExitoso)
                    {
                        await Task.Delay(400);

                        string urlActual;
                        try { urlActual = driver.Url; }
                        catch (WebDriverException) { break; }

                        // Fase A: Captura del carnet
                        if (urlActual.Contains("admacad.udb.edu.sv"))
                        {
                            driver.SwitchTo().DefaultContent();
                            string carnetEncontrado = BuscarUsuarioEnDocumento(driver);

                            if (string.IsNullOrEmpty(carnetEncontrado))
                            {
                                var iframes = driver.FindElements(By.TagName("iframe"));
                                foreach (var iframe in iframes)
                                {
                                    try
                                    {
                                        driver.SwitchTo().Frame(iframe);
                                        carnetEncontrado = BuscarUsuarioEnDocumento(driver);
                                        if (!string.IsNullOrEmpty(carnetEncontrado)) break;
                                    }
                                    catch (WebDriverException) { }
                                    finally { driver.SwitchTo().DefaultContent(); }
                                }
                            }

                            if (!string.IsNullOrEmpty(carnetEncontrado))
                            {
                                usuarioDetectado = carnetEncontrado;
                            }
                        }

                        // Fase B: Extraccion de datos
                        if (urlActual.Contains("portal.udb.edu.sv"))
                        {
                            loginExitoso = true;

                            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                            wait.Until(d => ((IJavaScriptExecutor)d!).ExecuteScript("return document.readyState")?.ToString() == "complete");

                            var js = (IJavaScriptExecutor)driver;

                            // Inyeccion de JavaScript
                            string scriptSimplificado = @"
                                var datos = { nombre: '', carrera: '', campus: '', estado: '', srcFoto: '', foto: 'No detectada' };

                                var cardContent = document.querySelector('.card-content');
                                if (cardContent) {
                                    var h5s = cardContent.querySelectorAll('h5');
                                    if (h5s.length >= 2) {
                                        datos.nombre = (h5s[0].innerText.trim() + ' ' + h5s[1].innerText.trim()).replace(/\s+/g, ' ');
                                    }
                                }

                                var headers = document.querySelectorAll('.card-header');
                                headers.forEach(function(header) {
                                    var small = header.querySelector('small');
                                    var h5 = header.querySelector('h5');
                                    if (small && h5) {
                                        var txtSmall = small.innerText.trim();
                                        var txtH5 = h5.innerText.trim();

                                        if (txtSmall.toLowerCase().includes('campus')) {
                                            datos.campus = txtH5;
                                        } else if (txtSmall.toLowerCase().includes('estudiante')) {
                                            datos.estado = txtSmall;
                                            datos.carrera = txtH5;
                                        }
                                    }
                                });

                                var img = document.querySelector('img.profile, img[src*=""ObtenerImagenP""]');
                                if (img && img.src) {
                                    datos.srcFoto = img.src;
                                    var partes = img.src.split('/');
                                    datos.foto = partes[partes.length - 1].split('?')[0] + '.jpg';
                                }

                                return JSON.stringify(datos);
                            ";

                            string? jsonString = js.ExecuteScript(scriptSimplificado) as string;

                            // Mapear datos y descargar foto
                            if (jsonString != null)
                            {
                                using (JsonDocument doc = JsonDocument.Parse(jsonString))
                                {
                                    var root = doc.RootElement;

                                    string nombreCompleto = root.GetProperty("nombre").GetString() ?? "Estudiante";
                                    string carreraActual = root.GetProperty("carrera").GetString() ?? "No detectada";
                                    string campus = root.GetProperty("campus").GetString() ?? "No detectado";
                                    string estadoEstudiante = root.GetProperty("estado").GetString() ?? "Estudiante Activo";
                                    string archivoFoto = root.GetProperty("foto").GetString() ?? "No detectada";
                                    string srcFoto = root.GetProperty("srcFoto").GetString() ?? "";

                                    string rutaDestinoFinal = archivoFoto; // Si falla la descarga, dejamos el nombre relativo de respaldo

                                    // Descargar imagen usando cookies de la sesion
                                    if (!string.IsNullOrEmpty(srcFoto))
                                    {
                                        try
                                        {
                                            var cookieContainer = new CookieContainer();
                                            var cookiesSelenium = driver.Manage().Cookies.AllCookies;
                                            foreach (var cookie in cookiesSelenium)
                                            {
                                                cookieContainer.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
                                            }

                                            var handler = new HttpClientHandler { CookieContainer = cookieContainer };
                                            using (var client = new HttpClient(handler))
                                            {
                                                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                                                // Obtener bytes de la imagen
                                                byte[] bytesImagen = await client.GetByteArrayAsync(srcFoto);

                                                // Guardar archivo temporal
                                                string rutaAbsoluta = Path.Combine(Path.GetTempPath(), archivoFoto);
                                                File.WriteAllBytes(rutaAbsoluta, bytesImagen);

                                                // Asignar ruta absoluta
                                                rutaDestinoFinal = rutaAbsoluta;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            // Manejo de error de red
                                            rutaDestinoFinal = archivoFoto;
                                        }
                                    }

                                    // Extraccion de correo electronico
                                    string correoScrapeado = $"{usuarioDetectado.Trim()}@udb.edu.sv"; // Valor por defecto
                                    try
                                    {
                                        driver.Navigate().GoToUrl("https://portal.udb.edu.sv/EstudiantesPlus/Perfil");
                                        
                                        var waitPerfil = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
                                        waitPerfil.Until(d => ((IJavaScriptExecutor)d!).ExecuteScript("return document.readyState")?.ToString() == "complete");

                                        string scriptCorreo = @"
                                            var divs = document.querySelectorAll('div.col-lg-7');
                                            var correo = '';
                                            for (var i = 0; i < divs.length; i++) {
                                                var text = divs[i].innerText;
                                                if (text.toLowerCase().includes('correo:')) {
                                                    var match = text.match(/Correo:\s*([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})/i);
                                                    if (match) {
                                                        correo = match[1].trim();
                                                        break;
                                                    }
                                                }
                                            }
                                            return correo;
                                        ";

                                        string? resCorreo = js.ExecuteScript(scriptCorreo) as string;
                                        if (!string.IsNullOrEmpty(resCorreo))
                                        {
                                            correoScrapeado = resCorreo;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        // Manejo de error silencioso
                                    }

                                    // Crear objeto de usuario
                                    var nuevoUsuario = new Usuario(usuarioDetectado, nombreCompleto, carreraActual, campus, estadoEstudiante, rutaDestinoFinal);
                                    nuevoUsuario.CorreoElectronico = correoScrapeado;
                                    return nuevoUsuario;
                                }
                            }
                        }
                    }
                }
                catch (Exception) { }

                return null;
            });
        }

        private static string BuscarUsuarioEnDocumento(IWebDriver d)
        {
            try
            {
                var inputs = d.FindElements(By.TagName("input"));
                foreach (var input in inputs)
                {
                    try
                    {
                        string type = input.GetAttribute("type")?.ToLower() ?? "text";
                        if (type != "text" && type != "number" && !string.IsNullOrEmpty(type)) continue;

                        string id = input.GetAttribute("id")?.ToLower() ?? "";
                        string name = input.GetAttribute("name")?.ToLower() ?? "";
                        string placeholder = input.GetAttribute("placeholder")?.ToLower() ?? "";

                        bool coincide = id.Contains("usuario") || id.Contains("carnet") || id.Contains("user") || id.Contains("login") || id.Contains("codigo") ||
                                       name.Contains("usuario") || name.Contains("carnet") || name.Contains("user") || name.Contains("login") ||
                                       placeholder.Contains("usuario") || placeholder.Contains("carnet") || placeholder.Contains("user");

                        if (coincide)
                        {
                            string? value = input.GetAttribute("value");
                            if (!string.IsNullOrEmpty(value) && value.Trim().Length >= 4)
                            {
                                return value.Trim();
                            }
                        }
                    }
                    catch (WebDriverException) { }
                }
            }
            catch (WebDriverException) { }
            return string.Empty;
        }
    }
}