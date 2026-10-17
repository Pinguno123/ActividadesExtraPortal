using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ActividadesExtraPortal.Data;

namespace ActividadesExtraPortal.Views
{
    /// <summary>
    /// Formulario que gestiona los Programas de Formación artística y cultural (DAC).
    /// Permite al estudiante consultar programas ofertados, visualizar sus inscripciones activas,
    /// e inscribirse o retirarse utilizando la capa desacoplada de repositorio (IArteRepository).
    /// </summary>
    public partial class ProgramasFormacion : PlantillaAACD
    {
        protected readonly IArteRepository arteRepo;

        public ProgramasFormacion()
        {
            InitializeComponent();
            this.arteRepo = new ArteRepository();
        }

        public ProgramasFormacion(Portal principal, Form? padre = null, IArteRepository? repo = null)
            : base(principal, padre)
        {
            InitializeComponent();
            this.arteRepo = repo ?? new ArteRepository();

            ConfigurarModulo();
            this.Load += (s, e) => CargarDatos();
        }

        /// <summary>
        /// Sobreescritura polimórfica para configurar la interfaz, estilos de cuadrícula y eventos de control.
        /// </summary>
        protected override void ConfigurarModulo()
        {
            this.lblNombreOpcion.Text = "Programas de Formación";
            this.lblBanner.Text = "Cursos, talleres y diplomados en diferentes disciplinas artísticas diseñados para impulsar tu creatividad.";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(13, 110, 253);

            ConfigurarEstiloDataGridView();

            // Vincular eventos de selección de vista
            rbOfertados.CheckedChanged += (s, e) => { if (rbOfertados.Checked) CargarProgramasOfertados(); };
            rbInscritos.CheckedChanged += (s, e) => { if (rbInscritos.Checked) CargarProgramasInscritos(); };

            // Vincular evento de selección de fila para actualizar estado de botones dinámicamente
            dgvFormacion.SelectionChanged += (s, e) => ActualizarBotonesSegunSeleccion();

            // Vincular botones de acción
            btnInscribir.Click += BtnInscribir_Click;
            btnRetirar.Click += BtnRetirar_Click;

            // Selección inicial
            rbOfertados.Checked = true;
        }

        private void ConfigurarEstiloDataGridView()
        {
            dgvFormacion.AllowUserToAddRows = false;
            dgvFormacion.AllowUserToDeleteRows = false;
            dgvFormacion.ReadOnly = true;
            dgvFormacion.MultiSelect = false;
            dgvFormacion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFormacion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFormacion.BackgroundColor = Color.White;
            dgvFormacion.BorderStyle = BorderStyle.None;
            dgvFormacion.RowHeadersVisible = false;
            dgvFormacion.EnableHeadersVisualStyles = false;

            dgvFormacion.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 48, 135);
            dgvFormacion.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFormacion.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvFormacion.ColumnHeadersHeight = 35;

            dgvFormacion.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvFormacion.DefaultCellStyle.SelectionBackColor = Color.FromArgb(207, 226, 255);
            dgvFormacion.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvFormacion.RowTemplate.Height = 32;
        }

        /// <summary>
        /// Sobreescritura polimórfica para refrescar los datos según el filtro activo.
        /// </summary>
        protected override void CargarDatos()
        {
            arteRepo.AsegurarCategoriasBase();
            if (rbInscritos.Checked)
            {
                CargarProgramasInscritos();
            }
            else
            {
                CargarProgramasOfertados();
            }
        }

        private void CargarProgramasOfertados()
        {
            try
            {
                string? carnet = formPrincipal?.UsuarioActual?.Id;
                var actividades = arteRepo.ObtenerActividadesPorCategoria("Formaci");
                var inscripciones = !string.IsNullOrEmpty(carnet)
                    ? arteRepo.ObtenerInscripcionesDACEstudiante(carnet)
                    : new List<InscripcionDAC>();

                var datos = actividades.Select(a =>
                {
                    bool estaInscrito = inscripciones.Any(i => i.IdActividad == a.IdActividad && i.EstadoInscripcion == "Activa");
                    return new
                    {
                        IdActividad = a.IdActividad,
                        Programa = a.NombreActividad,
                        Instructor = string.IsNullOrWhiteSpace(a.NombreInstructor) ? "Sin asignar" : a.NombreInstructor,
                        Descripcion = string.IsNullOrWhiteSpace(a.Descripcion) ? "Sin descripción" : a.Descripcion,
                        Estado = a.Estado,
                        Inscrito = estaInscrito ? "Sí (Inscrito)" : "No"
                    };
                }).ToList();

                dgvFormacion.DataSource = null;
                dgvFormacion.DataSource = datos;

                if (dgvFormacion.Columns.Contains("IdActividad"))
                    dgvFormacion.Columns["IdActividad"]!.Visible = false;

                ActualizarBotonesSegunSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar programas ofertados: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProgramasInscritos()
        {
            try
            {
                string? carnet = formPrincipal?.UsuarioActual?.Id;
                if (string.IsNullOrEmpty(carnet))
                {
                    dgvFormacion.DataSource = null;
                    ActualizarBotonesSegunSeleccion();
                    return;
                }

                // Identificar los IDs de actividades correspondientes a formación
                var actividadesFormacion = arteRepo.ObtenerActividadesPorCategoria("Formaci");
                var idsFormacion = actividadesFormacion.Select(a => a.IdActividad).ToHashSet();

                var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet)
                    .Where(i => (idsFormacion.Contains(i.IdActividad) ||
                                (!string.IsNullOrEmpty(i.NombreCategoria) && i.NombreCategoria.IndexOf("Formaci", StringComparison.OrdinalIgnoreCase) >= 0))
                                && i.EstadoInscripcion == "Activa")
                    .Select(i => new
                    {
                        IdActividad = i.IdActividad,
                        Programa = i.NombreActividad,
                        Instructor = string.IsNullOrWhiteSpace(i.NombreInstructor) ? "Sin asignar" : i.NombreInstructor,
                        FechaInscripcion = i.FechaInscripcion.ToShortDateString(),
                        Estado = i.EstadoInscripcion
                    })
                    .ToList();

                dgvFormacion.DataSource = null;
                dgvFormacion.DataSource = inscripciones;

                if (dgvFormacion.Columns.Contains("IdActividad"))
                    dgvFormacion.Columns["IdActividad"]!.Visible = false;

                ActualizarBotonesSegunSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar inscripciones: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarBotonesSegunSeleccion()
        {
            if (dgvFormacion.CurrentRow == null || !dgvFormacion.Columns.Contains("IdActividad") || dgvFormacion.CurrentRow.Cells["IdActividad"]?.Value == null)
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

            int idActividad = Convert.ToInt32(dgvFormacion.CurrentRow.Cells["IdActividad"].Value);
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
            if (dgvFormacion.CurrentRow == null || !dgvFormacion.Columns.Contains("IdActividad") || dgvFormacion.CurrentRow.Cells["IdActividad"]?.Value == null)
            {
                MessageBox.Show("Por favor, selecciona un programa de formación de la lista.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string? carnet = formPrincipal?.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes iniciar sesión para inscribirte en un programa.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idActividad = Convert.ToInt32(dgvFormacion.CurrentRow.Cells["IdActividad"].Value);
            string nombre = dgvFormacion.CurrentRow.Cells["Programa"]?.Value?.ToString() ?? "Programa";

            // Validar si ya está inscrito
            var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);
            if (inscripciones.Any(i => i.IdActividad == idActividad && i.EstadoInscripcion == "Activa"))
            {
                MessageBox.Show($"Ya cuentas con una inscripción activa en '{nombre}'.", "Inscripción Existente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Deseas inscribirte al programa '{nombre}'?",
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
            if (dgvFormacion.CurrentRow == null || !dgvFormacion.Columns.Contains("IdActividad") || dgvFormacion.CurrentRow.Cells["IdActividad"]?.Value == null)
            {
                MessageBox.Show("Por favor, selecciona un programa de la lista para retirar.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string? carnet = formPrincipal?.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes iniciar sesión para retirar una inscripción.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idActividad = Convert.ToInt32(dgvFormacion.CurrentRow.Cells["IdActividad"].Value);
            string nombre = dgvFormacion.CurrentRow.Cells["Programa"]?.Value?.ToString() ?? "Programa";

            // Validar si el estudiante está inscrito
            var inscripciones = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);
            bool estaInscrito = inscripciones.Any(i => i.IdActividad == idActividad && i.EstadoInscripcion == "Activa");

            if (!estaInscrito)
            {
                MessageBox.Show($"No cuentas con una inscripción activa en '{nombre}'.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Estás seguro de que deseas retirar tu inscripción del programa '{nombre}'?",
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
