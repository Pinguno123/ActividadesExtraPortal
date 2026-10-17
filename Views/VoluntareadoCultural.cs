using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ActividadesExtraPortal.Data;

namespace ActividadesExtraPortal.Views
{
    /// <summary>
    /// Formulario que gestiona el Voluntariado Cultural del Departamento de Arte y Cultura (DAC).
    /// Permite a los estudiantes participar como promotores culturales en actividades universitarias,
    /// consultar convocatorias ofertadas y administrar sus participaciones activas.
    /// </summary>
    public partial class VoluntareadoCultural : PlantillaAACD
    {
        protected readonly IArteRepository arteRepo;

        public VoluntareadoCultural()
        {
            InitializeComponent();
            this.arteRepo = new ArteRepository();
        }

        public VoluntareadoCultural(Portal principal, Form? padre = null, IArteRepository? repo = null)
            : base(principal, padre)
        {
            InitializeComponent();
            this.arteRepo = repo ?? new ArteRepository();

            ConfigurarModulo();
            this.Load += (s, e) => CargarDatos();
        }

        /// <summary>
        /// Sobreescritura polimórfica para configurar la interfaz, estilos y eventos del módulo de voluntariado.
        /// </summary>
        protected override void ConfigurarModulo()
        {
            this.lblNombreOpcion.Text = "Voluntariado Cultural";
            this.lblBanner.Text = "Colabora como promotor cultural y sé protagonista en la organización de eventos y difusión artística de la UDB.";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(13, 202, 240);

            ConfigurarEstiloDataGridView();

            // Vincular eventos de selección de vista
            rbOfertados.CheckedChanged += (s, e) => { if (rbOfertados.Checked) CargarVoluntariadosOfertados(); };
            rbInscritos.CheckedChanged += (s, e) => { if (rbInscritos.Checked) CargarVoluntariadosInscritos(); };

            // Vincular evento de selección de fila para actualizar estado de botones dinámicamente
            dgvVoluntareado.SelectionChanged += (s, e) => ActualizarBotonesSegunSeleccion();

            // Vincular botones de acción
            btnInscribir.Click += BtnInscribir_Click;
            btnRetirar.Click += BtnRetirar_Click;

            // Selección inicial
            rbOfertados.Checked = true;
        }

        private void ConfigurarEstiloDataGridView()
        {
            dgvVoluntareado.AllowUserToAddRows = false;
            dgvVoluntareado.AllowUserToDeleteRows = false;
            dgvVoluntareado.ReadOnly = true;
            dgvVoluntareado.MultiSelect = false;
            dgvVoluntareado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVoluntareado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVoluntareado.BackgroundColor = Color.White;
            dgvVoluntareado.BorderStyle = BorderStyle.None;
            dgvVoluntareado.RowHeadersVisible = false;
            dgvVoluntareado.EnableHeadersVisualStyles = false;

            dgvVoluntareado.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 48, 135);
            dgvVoluntareado.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVoluntareado.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvVoluntareado.ColumnHeadersHeight = 35;

            dgvVoluntareado.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvVoluntareado.DefaultCellStyle.SelectionBackColor = Color.FromArgb(186, 234, 250);
            dgvVoluntareado.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvVoluntareado.RowTemplate.Height = 32;
        }

        /// <summary>
        /// Sobreescritura polimórfica para refrescar los datos según el estado del filtro seleccionado.
        /// </summary>
        protected override void CargarDatos()
        {
            arteRepo.AsegurarCategoriasBase();
            if (rbInscritos.Checked)
            {
                CargarVoluntariadosInscritos();
            }
            else
            {
                CargarVoluntariadosOfertados();
            }
        }

        private void CargarVoluntariadosOfertados()
        {
            try
            {
                string? carnet = formPrincipal?.UsuarioActual?.Id;
                var actividades = arteRepo.ObtenerActividadesPorCategoria("Voluntar");
                var inscripciones = !string.IsNullOrEmpty(carnet)
                    ? arteRepo.ObtenerInscripcionesDACEstudiante(carnet)
                    : new List<InscripcionDAC>();

                var datos = actividades.Select(a =>
                {
                    bool estaInscrito = inscripciones.Any(i => i.IdActividad == a.IdActividad && i.EstadoInscripcion == "Activa");
                    return new
                    {
                        IdActividad = a.IdActividad,
                        Voluntariado = a.NombreActividad,
                        Coordinador = string.IsNullOrWhiteSpace(a.NombreInstructor) ? "DAC General" : a.NombreInstructor,
                        Descripcion = string.IsNullOrWhiteSpace(a.Descripcion) ? "Sin descripción" : a.Descripcion,
                        Estado = a.Estado,
                        Inscrito = estaInscrito ? "Sí (Inscrito)" : "No"
                    };
                }).ToList();

                dgvVoluntareado.DataSource = null;
                dgvVoluntareado.DataSource = datos;

                if (dgvVoluntareado.Columns.Contains("IdActividad"))
                    dgvVoluntareado.Columns["IdActividad"]!.Visible = false;

                ActualizarBotonesSegunSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar voluntariados ofertados: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarVoluntariadosInscritos()
        {
            try
            {
                string? carnet = formPrincipal?.UsuarioActual?.Id;
                if (string.IsNullOrEmpty(carnet))
                {
                    dgvVoluntareado.DataSource = null;
                    ActualizarBotonesSegunSeleccion();
                    return;
                }

                // Identificar los IDs de actividades correspondientes a voluntariado
                var actividadesVoluntariado = arteRepo.ObtenerActividadesPorCategoria("Voluntar");
                var idsVoluntariado = actividadesVoluntariado.Select(a => a.IdActividad).ToHashSet();

                var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet)
                    .Where(i => (idsVoluntariado.Contains(i.IdActividad) ||
                                (!string.IsNullOrEmpty(i.NombreCategoria) && i.NombreCategoria.IndexOf("Voluntar", StringComparison.OrdinalIgnoreCase) >= 0))
                                && i.EstadoInscripcion == "Activa")
                    .Select(i => new
                    {
                        IdActividad = i.IdActividad,
                        Voluntariado = i.NombreActividad,
                        Coordinador = string.IsNullOrWhiteSpace(i.NombreInstructor) ? "DAC General" : i.NombreInstructor,
                        FechaInscripcion = i.FechaInscripcion.ToShortDateString(),
                        Estado = i.EstadoInscripcion
                    })
                    .ToList();

                dgvVoluntareado.DataSource = null;
                dgvVoluntareado.DataSource = inscripciones;

                if (dgvVoluntareado.Columns.Contains("IdActividad"))
                    dgvVoluntareado.Columns["IdActividad"]!.Visible = false;

                ActualizarBotonesSegunSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar voluntariados inscritos: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarBotonesSegunSeleccion()
        {
            if (dgvVoluntareado.CurrentRow == null || !dgvVoluntareado.Columns.Contains("IdActividad") || dgvVoluntareado.CurrentRow.Cells["IdActividad"]?.Value == null)
            {
                btnInscribir.Enabled = false;
                btnRetirar.Enabled = false;
                return;
            }

            string? carnet = formPrincipal?.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                btnInscribir.Enabled = false;
                btnRetirar.Enabled = false;
                return;
            }

            int idActividad = Convert.ToInt32(dgvVoluntareado.CurrentRow.Cells["IdActividad"].Value);
            var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);
            bool estaInscrito = inscripciones.Any(i => i.IdActividad == idActividad && i.EstadoInscripcion == "Activa");

            if (rbInscritos.Checked)
            {
                btnInscribir.Enabled = false;
                btnInscribir.Text = "Inscribirse";
                btnRetirar.Enabled = true;
                btnRetirar.Text = "Retirar";
            }
            else
            {
                // En ofertados: si ya está inscrito, se habilita "Retirar" y se deshabilita "Inscribirse"
                btnInscribir.Enabled = !estaInscrito;
                btnInscribir.Text = estaInscrito ? "Inscrito" : "Inscribirse";
                btnRetirar.Enabled = estaInscrito;
                btnRetirar.Text = "Retirar";
            }
        }

        private void BtnInscribir_Click(object? sender, EventArgs e)
        {
            if (dgvVoluntareado.CurrentRow == null || !dgvVoluntareado.Columns.Contains("IdActividad") || dgvVoluntareado.CurrentRow.Cells["IdActividad"]?.Value == null)
            {
                MessageBox.Show("Por favor, selecciona una actividad de voluntariado de la lista.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string? carnet = formPrincipal?.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes iniciar sesión para postularte como voluntario.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idActividad = Convert.ToInt32(dgvVoluntareado.CurrentRow.Cells["IdActividad"].Value);
            string nombre = dgvVoluntareado.CurrentRow.Cells["Voluntariado"]?.Value?.ToString() ?? "Actividad";

            // Validar si ya está inscrito
            var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);
            if (inscripciones.Any(i => i.IdActividad == idActividad && i.EstadoInscripcion == "Activa"))
            {
                MessageBox.Show($"Ya cuentas con una inscripción activa en '{nombre}'.", "Inscripción Existente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Deseas inscribirte a la actividad de voluntariado '{nombre}'?",
                "Confirmar Inscripción",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                string resultado = arteRepo.InscribirActividadDAC(carnet, idActividad);
                MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
        }

        private void BtnRetirar_Click(object? sender, EventArgs e)
        {
            if (dgvVoluntareado.CurrentRow == null || !dgvVoluntareado.Columns.Contains("IdActividad") || dgvVoluntareado.CurrentRow.Cells["IdActividad"]?.Value == null)
            {
                MessageBox.Show("Por favor, selecciona una actividad de voluntariado para retirar.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string? carnet = formPrincipal?.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes iniciar sesión para retirar una inscripción.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idActividad = Convert.ToInt32(dgvVoluntareado.CurrentRow.Cells["IdActividad"].Value);
            string nombre = dgvVoluntareado.CurrentRow.Cells["Voluntariado"]?.Value?.ToString() ?? "Actividad";

            // Validar si el estudiante está inscrito
            var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);
            bool estaInscrito = inscripciones.Any(i => i.IdActividad == idActividad && i.EstadoInscripcion == "Activa");

            if (!estaInscrito)
            {
                MessageBox.Show($"No cuentas con una inscripción activa en '{nombre}'.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Estás seguro de que deseas retirar tu participación en '{nombre}'?",
                "Confirmar Retiro",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                string resultado = arteRepo.CancelarInscripcionDAC(carnet, idActividad);
                MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
        }
    }
}
