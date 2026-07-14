using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActividadesExtraPortal
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            /*
            Usuario usuarioFicticio = new Usuario(
                            id: "00112233",
                            nombre: "Usuario de Prueba",
                            carrera: "Ingeniería en Desarrollo de Software",
                            campus: "Campus Central",
                            estado: "Activo",
                            rutaArchivo: @"C:\Temp\datos_prueba.json" // Opcional: usa una ruta real si tu form lee este archivo al cargar
                        );
            Application.Run(new Portal(usuarioFicticio)); */

            
            // Instanciar capturador
            PortalGrabber grabber = new PortalGrabber();

            // Esperar la extraccion de datos de forma sincrona
            Usuario? estudianteDetectado = Task.Run(async () => await grabber.IniciarCapturaAsync()).GetAwaiter().GetResult();

            // Evaluar resultado
            if (estudianteDetectado != null)
            {
                // Registro y actualizacion de datos del usuario en base de datos
                try
                {
                    var repo = new ActividadesExtraPortal.Data.UsuarioRepository();
                    repo.RegistrarOActualizarUsuario(estudianteDetectado);
                }
                catch (Exception ex)
                {
                    // Registrar error de sincronizacion en consola
                    Console.WriteLine($"[Advertencia BD] No se pudo sincronizar el usuario en SQL Server: {ex.Message}");
                }

                // Iniciar formulario principal con los datos del usuario
                Application.Run(new Portal(estudianteDetectado));
            }
            else
            {
                // Salir si no se pudieron extraer los datos o se cerro el navegador
                MessageBox.Show(
                    "No se pudieron extraer los datos de acceso del portal. El programa se cerrará.",
                    "Captura Interrumpida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                Application.Exit();
            } 

        }
    }
}