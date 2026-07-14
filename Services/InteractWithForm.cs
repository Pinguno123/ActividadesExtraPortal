using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ActividadesExtraPortal.Services
{
    public class InteractWithForm
    {
        private IWebDriver? _driver;

        public async Task ProcesoAdmision(string urlFormulario, string correo, string carnet, int idAsociacion)
        {
            // Cerrar navegador anterior
            CerrarNavegadorActivo();

            double? notaFinal = null;

            try
            {
                _driver = ConfigurarNavegador();
                _driver.Navigate().GoToUrl(urlFormulario);
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                await RellenarCampoCorreoAsync(_driver, wait, correo);

                notaFinal = await MonitorearResultadosAsync(_driver, wait);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general en el flujo de Selenium: {ex.Message}", "Error de Automatización");
            }
            finally
            {
                CerrarNavegadorActivo();
            }

            bool paso = notaFinal.HasValue && notaFinal.Value >= 6.0;
            string nuevoEstado = paso ? "Aprobada" : "Rechazada";

            try
            {
                var repo = new ActividadesExtraPortal.Data.AsociacionRepository();
                repo.ActualizarEstadoMembresia(carnet, idAsociacion, nuevoEstado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el estado de membresía en la base de datos: {ex.Message}", "Error de Base de Datos");
            }

            if (notaFinal.HasValue)
            {
                string mensaje = paso
                    ? $"¡Pasaste la prueba de ingreso! Nota: {notaFinal.Value}. Tu afiliación ha sido aprobada."
                    : $"No pasaste la prueba de ingreso. Nota: {notaFinal.Value}. Tu afiliación ha sido rechazada.";

                MessageBox.Show(mensaje, "Resultado");
            }
            else
            {
                MessageBox.Show("No se pudo determinar el resultado del examen. La afiliación se mantendrá como rechazada.", "Resultado No Obtenido");
            }
        }

        private IWebDriver ConfigurarNavegador()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddUserProfilePreference("detach", true);
            return new ChromeDriver(options);
        }

        private async Task RellenarCampoCorreoAsync(IWebDriver driver, WebDriverWait wait, string correo)
        {
            IWebElement campoEmail = wait.Until(d =>
            {
                try
                {
                    var elemento = d.FindElement(By.CssSelector("input[jsname='YPqjbf']"));
                    return elemento.Displayed ? elemento : null;
                }
                catch (NoSuchElementException) { return null; }
            });

            if (campoEmail == null) return;

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            
            // Validar estado del campo
            bool esEditable = false;
            for (int i = 0; i < 10; i++)
            {
                string? roAttr = campoEmail.GetAttribute("readonly");
                bool isReadOnly = !string.IsNullOrEmpty(roAttr) && roAttr != "false";

                string? dsAttr = campoEmail.GetAttribute("disabled");
                bool isDisabled = !campoEmail.Enabled || (!string.IsNullOrEmpty(dsAttr) && dsAttr != "false");

                if (!isReadOnly && !isDisabled)
                {
                    esEditable = true;
                    break;
                }
                await Task.Delay(200);
            }

            if (!esEditable)
            {
                // Salir si el campo no es editable
                return;
            }

            try
            {
                campoEmail.Clear();
                campoEmail.Click();

                // Simular escritura
                foreach (char letra in correo)
                {
                    campoEmail.SendKeys(letra.ToString());
                    await Task.Delay(80);
                }
                await Task.Delay(300);

                // Bloquear campo con JavaScript
                jsExecutor.ExecuteScript("arguments[0].readOnly = true; arguments[0].style.backgroundColor = '#f0f0f0';", campoEmail);
            }
            catch (InvalidElementStateException ex)
            {
                // Obtener HTML en caso de error
                string html = jsExecutor.ExecuteScript("return arguments[0].outerHTML;", campoEmail)?.ToString() ?? "";
                throw new Exception($"El campo no es interactuable. HTML: {html}. Detalle: {ex.Message}");
            }
        }

        private async Task<double?> MonitorearResultadosAsync(IWebDriver driver, WebDriverWait wait)
        {
            string urlTerminado = "/formResponse";
            string urlNota = "/viewscore";
            DateTime tiempoLimite = DateTime.Now.AddMinutes(3);

            while (DateTime.Now < tiempoLimite)
            {
                try
                {
                    // Redirigir a los resultados
                    if (driver.Url.Contains(urlTerminado))
                    {
                        var campoEnlace = IntentarBuscarElemento(wait, By.CssSelector("a.IrxBzb.TpQm9d"));
                        if (campoEnlace != null)
                        {
                            string? href = campoEnlace.GetAttribute("href");
                            if (!string.IsNullOrEmpty(href))
                            {
                                driver.Navigate().GoToUrl(href);
                            }
                        }
                    }

                    // Calcular nota
                    if (driver.Url.Contains(urlNota))
                    {
                        var campoNota = IntentarBuscarElemento(wait, By.CssSelector("span.nkOYNd.RVEQke"));
                        if (campoNota != null)
                        {
                            double? nota = EvaluarNota(campoNota.Text);
                            if (nota.HasValue)
                            {
                                return nota;
                            }
                        }
                    }
                }
                catch (WebDriverException)
                {
                    break; // Salir si el navegador fue cerrado
                }

                await Task.Delay(500);
            }

            return null;
        }

        private IWebElement? IntentarBuscarElemento(WebDriverWait wait, By locator)
        {
            try
            {
                return wait.Until(d => {
                    var el = d.FindElement(locator);
                    return el.Displayed ? el : null;
                });
            }
            catch (WebDriverTimeoutException) { return null; }
            catch (NoSuchElementException) { return null; }
        }        

        private double? EvaluarNota(string textoScore)
        {
            string[] partes = textoScore.Replace(" ", "").Split('/');

            if (partes.Length == 2 &&
                int.TryParse(partes[0], out int conseguidos) &&
                int.TryParse(partes[1], out int totales) && totales > 0)
            {
                double notaFinal = ((double)conseguidos / totales) * 10;
                return Math.Round(notaFinal, 2);
            }

            return null;
        }

        private void CerrarNavegadorActivo()
        {
            if (_driver != null)
            {
                try
                {
                    _driver.Quit();
                }
                catch { }
                finally
                {
                    _driver = null;
                }
            }
        }
    }
}
