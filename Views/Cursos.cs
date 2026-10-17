using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ActividadesExtraPortal.Data;
using ActividadesExtraPortal.Views;

namespace ActividadesExtraPortal
{
    /// <summary>
    /// Formulario que gestiona el catálogo y las inscripciones a Cursos Especializados.
    /// Hereda de PlantillaAACD sobreescribiendo sus métodos polimórficos de ciclo de vida.
    /// Sigue la misma arquitectura desacoplada y principios SOLID aplicados en ProgramasFormacion y VoluntareadoCultural.
    /// </summary>
    public partial class Cursos : PlantillaAACD
    {
        protected readonly ICursoRepository cursoRepo;

        public Cursos()
        {
            InitializeComponent();
            this.cursoRepo = new CursoRepository();
        }

        public Cursos(Portal principal, ICursoRepository? repo = null)
            : base(principal)
        {
            InitializeComponent();
            this.cursoRepo = repo ?? new CursoRepository();

            ConfigurarModulo();
            this.Load += (s, e) => CargarDatos();
        }

        /// <summary>
        /// Sobreescritura polimórfica para configurar la interfaz, estilos de la cuadrícula y vinculación de eventos.
        /// </summary>
        protected override void ConfigurarModulo()
        {
            this.lblNombreOpcion.Text = "Cursos";
            this.lblBanner.Text = "Impulsa tu potencial y sé parte de los cursos especializados en la Universidad Don Bosco.";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(13, 148, 136);

            ConfigurarEstiloDataGridView();

            // Vincular eventos de selección de vista
            rbOfertados.CheckedChanged += (s, e) => { if (rbOfertados.Checked) CargarCursosOfertados(); };
            rbInscritos.CheckedChanged += (s, e) => { if (rbInscritos.Checked) CargarCursosInscritos(); };

            // Vincular evento de selección de fila para actualizar estado de botones dinámicamente
            dgvCursos.SelectionChanged += (s, e) => ActualizarBotonesSegunSeleccion();

            // Vincular botones de acción
            btnInscribir.Click += BtnInscribir_Click;
            btnRetirar.Click += BtnRetirar_Click;

            // Selección inicial
            rbOfertados.Checked = true;
        }

        private void ConfigurarEstiloDataGridView()
        {
            dgvCursos.AllowUserToAddRows = false;
            dgvCursos.AllowUserToDeleteRows = false;
            dgvCursos.ReadOnly = true;
            dgvCursos.MultiSelect = false;
            dgvCursos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCursos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCursos.BackgroundColor = Color.White;
            dgvCursos.BorderStyle = BorderStyle.None;
            dgvCursos.RowHeadersVisible = false;
            dgvCursos.EnableHeadersVisualStyles = false;

            dgvCursos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 48, 135);
            dgvCursos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCursos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCursos.ColumnHeadersHeight = 35;

            dgvCursos.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvCursos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 251, 241);
            dgvCursos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCursos.RowTemplate.Height = 32;
        }

        /// <summary>
        /// Sobreescritura polimórfica para refrescar los datos según el filtro activo.
        /// </summary>
        protected override void CargarDatos()
        {
            if (rbInscritos.Checked)
            {
                CargarCursosInscritos();
            }
            else
            {
                CargarCursosOfertados();
            }
        }

        private void CargarCursosOfertados()
        {
            try
            {
                string? carnet = formPrincipal?.UsuarioActual?.Id;
                var cursos = cursoRepo.ObtenerTodosLosCursos();
                var inscripciones = !string.IsNullOrEmpty(carnet)
                    ? cursoRepo.ObtenerInscripcionesEstudiante(carnet)
                    : new List<InscripcionCurso>();

                var datos = cursos.Select(c =>
                {
                    var inscripcion = inscripciones.FirstOrDefault(i => i.IdCurso == c.IdCurso &&
                        (i.EstadoInscripcion == "Confirmada" || i.EstadoInscripcion == "En Espera" || i.EstadoInscripcion == "Pre-inscrito"));
                    bool estaInscrito = inscripcion != null;

                    string cuposTxt = c.CuposDisponibles.HasValue
                        ? $"{c.CuposDisponibles.Value} / {c.CupoMaximo}"
                        : $"{c.CupoMaximo}";

                    return new
                    {
                        IdCurso = c.IdCurso,
                        Curso = c.NombreCurso,
                        Horario = c.Horario,
                        Instructor = string.IsNullOrWhiteSpace(c.NombreInstructor) ? "Sin asignar" : c.NombreInstructor,
                        Cupos = cuposTxt,
                        Periodo = $"{c.FechaInicio.ToShortDateString()} - {c.FechaFin.ToShortDateString()}",
                        Estado = c.Estado,
                        Inscrito = estaInscrito ? $"Sí ({inscripcion!.EstadoInscripcion})" : "No"
                    };
                }).ToList();

                dgvCursos.DataSource = null;
                dgvCursos.DataSource = datos;

                if (dgvCursos.Columns.Contains("IdCurso"))
                    dgvCursos.Columns["IdCurso"]!.Visible = false;

                ActualizarBotonesSegunSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos ofertados: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarCursosInscritos()
        {
            try
            {
                string? carnet = formPrincipal?.UsuarioActual?.Id;
                if (string.IsNullOrEmpty(carnet))
                {
                    dgvCursos.DataSource = null;
                    ActualizarBotonesSegunSeleccion();
                    return;
                }

                var inscripciones = cursoRepo.ObtenerInscripcionesEstudiante(carnet)
                    .Where(i => i.EstadoInscripcion == "Confirmada" || i.EstadoInscripcion == "En Espera" || i.EstadoInscripcion == "Pre-inscrito")
                    .Select(i => new
                    {
                        IdCurso = i.IdCurso,
                        Curso = i.NombreCurso,
                        Instructor = string.IsNullOrWhiteSpace(i.NombreInstructor) ? "Sin asignar" : i.NombreInstructor,
                        FechaInscripcion = i.FechaInscripcion.ToShortDateString(),
                        Nota = i.Nota.HasValue ? i.Nota.Value.ToString("F1") : "Pendiente",
                        Estado = i.EstadoInscripcion
                    })
                    .ToList();

                dgvCursos.DataSource = null;
                dgvCursos.DataSource = inscripciones;

                if (dgvCursos.Columns.Contains("IdCurso"))
                    dgvCursos.Columns["IdCurso"]!.Visible = false;

                ActualizarBotonesSegunSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos inscritos: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarBotonesSegunSeleccion()
        {
            if (dgvCursos.CurrentRow == null || !dgvCursos.Columns.Contains("IdCurso") || dgvCursos.CurrentRow.Cells["IdCurso"]?.Value == null)
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

            int idCurso = Convert.ToInt32(dgvCursos.CurrentRow.Cells["IdCurso"].Value);
            var inscripciones = cursoRepo.ObtenerInscripcionesEstudiante(carnet);
            bool estaInscrito = inscripciones.Any(i => i.IdCurso == idCurso &&
                (i.EstadoInscripcion == "Confirmada" || i.EstadoInscripcion == "En Espera" || i.EstadoInscripcion == "Pre-inscrito"));

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
            if (dgvCursos.CurrentRow == null || !dgvCursos.Columns.Contains("IdCurso") || dgvCursos.CurrentRow.Cells["IdCurso"]?.Value == null)
            {
                MessageBox.Show("Por favor, selecciona un curso de la lista.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string? carnet = formPrincipal?.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes iniciar sesión para inscribirte en un curso.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idCurso = Convert.ToInt32(dgvCursos.CurrentRow.Cells["IdCurso"].Value);
            string nombre = dgvCursos.CurrentRow.Cells["Curso"]?.Value?.ToString() ?? "Curso";

            // Validar si ya está inscrito
            var inscripciones = cursoRepo.ObtenerInscripcionesEstudiante(carnet);
            if (inscripciones.Any(i => i.IdCurso == idCurso &&
                (i.EstadoInscripcion == "Confirmada" || i.EstadoInscripcion == "En Espera" || i.EstadoInscripcion == "Pre-inscrito")))
            {
                MessageBox.Show($"Ya cuentas con una inscripción activa en '{nombre}'.", "Inscripción Existente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Deseas inscribirte al curso '{nombre}'?",
                "Confirmar Inscripción",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                string resultado = cursoRepo.InscribirEstudianteEnCurso(carnet, idCurso);
                MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
        }

        private void BtnRetirar_Click(object? sender, EventArgs e)
        {
            if (dgvCursos.CurrentRow == null || !dgvCursos.Columns.Contains("IdCurso") || dgvCursos.CurrentRow.Cells["IdCurso"]?.Value == null)
            {
                MessageBox.Show("Por favor, selecciona un curso de la lista para retirar.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string? carnet = formPrincipal?.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes iniciar sesión para retirar una inscripción.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idCurso = Convert.ToInt32(dgvCursos.CurrentRow.Cells["IdCurso"].Value);
            string nombre = dgvCursos.CurrentRow.Cells["Curso"]?.Value?.ToString() ?? "Curso";

            // Validar si el estudiante está inscrito
            var inscripciones = cursoRepo.ObtenerInscripcionesEstudiante(carnet);
            bool estaInscrito = inscripciones.Any(i => i.IdCurso == idCurso &&
                (i.EstadoInscripcion == "Confirmada" || i.EstadoInscripcion == "En Espera" || i.EstadoInscripcion == "Pre-inscrito"));

            if (!estaInscrito)
            {
                MessageBox.Show($"No cuentas con una inscripción activa en '{nombre}'.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Estás seguro de que deseas retirar tu inscripción del curso '{nombre}'?",
                "Confirmar Retiro",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                string resultado = cursoRepo.CancelarInscripcionCurso(carnet, idCurso);
                MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
            }
        }
    }
}
