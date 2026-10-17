using ActividadesExtraPortal.Data;
using ActividadesExtraPortal.Views;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ActividadesExtraPortal
{
    /// <summary>
    /// Menú principal del Departamento de Arte y Cultura (DAC).
    /// Coordina la navegación polimórfica hacia los tres submódulos especializados:
    /// Grupos de Extensión, Programas de Formación y Voluntariado Cultural.
    /// Actualiza el estado de las inscripciones del estudiante (Principio SRP y DIP).
    /// </summary>
    public partial class Arte : PlantillaAACD
    {
        protected readonly IArteRepository arteRepo;

        public Arte(Portal? principal = null, IArteRepository? repo = null)
        {
            InitializeComponent();
            this.formPrincipal = principal!;
            this.arteRepo = repo ?? new ArteRepository();

            ConfigurarModulo();

            // Vincular botones de navegación hacia los tres submódulos
            btnVerGruposExtension.Click += BtnVerGruposExtension_Click;
            btnVerProgramasFormacion.Click += BtnVerProgramasFormacion_Click;
            btnVoluntariadoCultural.Click += BtnVoluntariadoCultural_Click;

            // Recargar datos al cargar o al volver a hacerse visible
            this.Load += (s, e) => CargarDatos();
            this.VisibleChanged += (s, e) => { if (this.Visible) CargarDatos(); };
        }

        /// <summary>
        /// Configuración polimórfica de títulos, banners y colores distintivos del DAC.
        /// </summary>
        protected override void ConfigurarModulo()
        {
            this.lblNombreOpcion.Text = "Departamento de Arte y Cultura";
            this.lblBanner.Text = "Inscríbete aquí y participa en las diversas actividades, cursos y programas que el Departamento de Arte y Cultura (DAC) tiene para ti.";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(138, 53, 252);
        }

        /// <summary>
        /// Sobreescritura polimórfica para cargar el estado de inscripciones activas del estudiante.
        /// </summary>
        protected override void CargarDatos()
        {
            try
            {
                arteRepo.AsegurarCategoriasBase();

                string? carnet = formPrincipal?.UsuarioActual?.Id;
                if (string.IsNullOrEmpty(carnet))
                {
                    label2.Text = "Sesión no iniciada";
                    return;
                }

                var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);
                var activas = inscripciones?.Where(i => i.EstadoInscripcion == "Activa").ToList();

                if (activas != null && activas.Count > 0)
                {
                    label2.Text = $"Posees {activas.Count} inscripción(es) activa(s) en DAC";
                    label2.BackColor = Color.FromArgb(222, 247, 236);
                    label2.ForeColor = Color.FromArgb(3, 84, 63);
                }
                else
                {
                    label2.Text = "Aún no posee una inscripción";
                    label2.BackColor = Color.FromArgb(207, 244, 252);
                    label2.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar panel de inscripciones en Arte: {ex.Message}");
            }
        }

        private void BtnVerGruposExtension_Click(object? sender, EventArgs e)
        {
            this.Hide();
            GruposExtension gruposForm = new GruposExtension(this.formPrincipal, this, this.arteRepo);
            gruposForm.Show();
        }

        private void BtnVerProgramasFormacion_Click(object? sender, EventArgs e)
        {
            this.Hide();
            ProgramasFormacion formacionForm = new ProgramasFormacion(this.formPrincipal, this, this.arteRepo);
            formacionForm.Show();
        }

        private void BtnVoluntariadoCultural_Click(object? sender, EventArgs e)
        {
            this.Hide();
            VoluntareadoCultural voluntariadoForm = new VoluntareadoCultural(this.formPrincipal, this, this.arteRepo);
            voluntariadoForm.Show();
        }

        private void label13_Click(object sender, EventArgs e)
        {
        }

        private void label8_Click(object sender, EventArgs e)
        {
        }
    }
}
