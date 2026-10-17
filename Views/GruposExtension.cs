using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ActividadesExtraPortal.Data;

namespace ActividadesExtraPortal.Views
{
    /// <summary>
    /// Formulario que gestiona el catálogo y la inscripción a Grupos de Extensión artística (DAC).
    /// Hereda de PlantillaAACD sobreescribiendo sus métodos polimórficos de ciclo de vida.
    /// Utiliza el UserControl GrupoExtensionTarjeta y el patrón Factory (TarjetaFactory).
    /// </summary>
    public partial class GruposExtension : PlantillaAACD
    {
        protected readonly IArteRepository arteRepo;

        public GruposExtension()
        {
            InitializeComponent();
            this.arteRepo = new ArteRepository();
        }

        public GruposExtension(Portal principal, Form? padre = null, IArteRepository? repo = null)
            : base(principal, padre)
        {
            InitializeComponent();
            this.arteRepo = repo ?? new ArteRepository();

            ConfigurarModulo();
            this.Load += (s, e) => CargarDatos();
        }

        /// <summary>
        /// Sobreescritura polimórfica para configurar banners, títulos y propiedades del FlowLayoutPanel.
        /// </summary>
        protected override void ConfigurarModulo()
        {
            this.lblNombreOpcion.Text = "Grupos de Extensión";
            this.lblBanner.Text = "Espacio para el desarrollo de tus habilidades y talentos artísticos. ¡Inscríbete y sé parte de la formación integral UDB!";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(138, 53, 252);

            this.flpExtension.AutoScroll = true;
            this.flpExtension.WrapContents = false;
            this.flpExtension.FlowDirection = FlowDirection.TopDown;
        }

        /// <summary>
        /// Sobreescritura polimórfica para cargar el catálogo de grupos de extensión y las inscripciones del estudiante.
        /// </summary>
        protected override void CargarDatos()
        {
            try
            {
                flpExtension.Controls.Clear();
                arteRepo.AsegurarCategoriasBase();

                string? carnet = formPrincipal?.UsuarioActual?.Id;
                if (string.IsNullOrEmpty(carnet))
                {
                    MostrarMensajeEnFlp("No se pudo identificar la sesión del estudiante actual.");
                    return;
                }

                // Obtener catálogo de actividades de la categoría 'Grupos de Extensión'
                List<ActividadDAC> actividades = arteRepo.ObtenerActividadesPorCategoria("Extensi");

                // Obtener inscripciones vigentes del estudiante en DAC
                List<InscripcionDAC> inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);

                if (actividades == null || actividades.Count == 0)
                {
                    MostrarMensajeEnFlp("Actualmente no hay grupos de extensión disponibles.");
                    return;
                }

                foreach (var actividad in actividades)
                {
                    bool estaInscrito = inscripciones.Any(i => i.IdActividad == actividad.IdActividad && i.EstadoInscripcion == "Activa");

                    // Construcción desacoplada de la tarjeta mediante el patrón creacional Factory
                    GrupoExtensionTarjeta tarjeta = TarjetaFactory.CrearGrupoExtensionTarjeta(actividad, estaInscrito, Tarjeta_BotonInscribirClick);
                    flpExtension.Controls.Add(tarjeta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los grupos de extensión: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Tarjeta_BotonInscribirClick(object? sender, EventArgs e)
        {
            if (sender is not GrupoExtensionTarjeta tarjeta || tarjeta.Tag is not ActividadDAC actividad)
                return;

            string? carnet = formPrincipal?.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes contar con una sesión activa para inscribirte.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Verificar si ya está inscrito
                var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);
                bool estaInscrito = inscripciones.Any(i => i.IdActividad == actividad.IdActividad && i.EstadoInscripcion == "Activa");

                if (estaInscrito)
                {
                    var confirm = MessageBox.Show(
                        $"¿Deseas retirarte del grupo '{actividad.NombreActividad}'?",
                        "Confirmar Retiro",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        string resultado = arteRepo.CancelarInscripcionDAC(carnet, actividad.IdActividad);
                        MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();
                    }
                }
                else
                {
                    var confirm = MessageBox.Show(
                        $"¿Deseas inscribirte al grupo '{actividad.NombreActividad}'?",
                        "Confirmar Inscripción",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        string resultado = arteRepo.InscribirActividadDAC(carnet, actividad.IdActividad);
                        MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al procesar la solicitud: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarMensajeEnFlp(string mensaje)
        {
            flpExtension.Controls.Clear();
            Label lbl = new Label
            {
                Text = mensaje,
                Font = new Font("Segoe UI", 12F, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Margin = new Padding(20, 20, 0, 0)
            };
            flpExtension.Controls.Add(lbl);
        }
    }
}
