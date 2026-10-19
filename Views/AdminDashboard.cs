using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using ActividadesExtraPortal.Data;
using ActividadesExtraPortal.Services;

namespace ActividadesExtraPortal
{
    public partial class AdminDashboard : Form
    {
        private readonly Portal formPrincipal;

        // Repositorios e interfaces (Inversión de Dependencias - DIP)
        private readonly ICursoRepository cursoRepo;
        private readonly IAsociacionRepository asoRepo;
        private readonly IDeporteRepository deporteRepo;
        private readonly IArteRepository arteRepo;

        // Servicio de imágenes desacoplado mediante interfaz
        private readonly IImageService imgService;

        // Estado de selección para edición de registros existentes
        private int? selectedAsoId;
        private string? selectedAsoImgUrl;
        private int? selectedDeporteId;
        private string? selectedDeporteImgUrl;
        private int? selectedArteId;

        public AdminDashboard(Portal principal,
            ICursoRepository? cursoR = null,
            IAsociacionRepository? asoR = null,
            IDeporteRepository? depR = null,
            IArteRepository? artR = null,
            IImageService? imgS = null)
        {
            InitializeComponent();
            this.formPrincipal = principal;
            this.cursoRepo = cursoR ?? new CursoRepository();
            this.asoRepo = asoR ?? new AsociacionRepository();
            this.deporteRepo = depR ?? new DeporteRepository();
            this.arteRepo = artR ?? new ArteRepository();
            this.imgService = imgS ?? new ImgBBService();
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            CargarGrids();
            CargarComboBoxes();
            txtCursoId.Enabled = false;

            // Permitir cargar datos para edición al hacer clic en cualquier celda o encabezado
            gridCursos.CellClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0) CargarCursoSeleccionado(gridCursos.Rows[ev.RowIndex]);
            };

            gridAsociaciones.CellClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0) CargarAsociacionSeleccionada(gridAsociaciones.Rows[ev.RowIndex]);
            };
            gridAsociaciones.RowHeaderMouseClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0) CargarAsociacionSeleccionada(gridAsociaciones.Rows[ev.RowIndex]);
            };

            gridDeportes.CellClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0) CargarDeporteSeleccionado(gridDeportes.Rows[ev.RowIndex]);
            };
            gridDeportes.RowHeaderMouseClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0) CargarDeporteSeleccionado(gridDeportes.Rows[ev.RowIndex]);
            };

            gridArte.CellClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0) CargarArteSeleccionado(gridArte.Rows[ev.RowIndex]);
            };
            gridArte.RowHeaderMouseClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0) CargarArteSeleccionado(gridArte.Rows[ev.RowIndex]);
            };
        }

        private void CargarGrids()
        {
            try
            {
                // Cargar cursos
                gridCursos.DataSource = null;
                gridCursos.DataSource = cursoRepo.ObtenerTodosLosCursos();

                // Cargar asociaciones
                gridAsociaciones.DataSource = null;
                gridAsociaciones.DataSource = asoRepo.ObtenerTodasLasAsociaciones();

                // Cargar deportes
                gridDeportes.DataSource = null;
                gridDeportes.DataSource = deporteRepo.ObtenerTodasLasSelecciones();

                // Cargar actividades de arte
                gridArte.DataSource = null;
                gridArte.DataSource = arteRepo.ObtenerTodasLasActividades();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la información: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarComboBoxes()
        {
            try
            {
                // Obtener instructores activos (regla fn_ValidarInstructor)
                var userRepo = new UsuarioRepository();
                var instructores = userRepo.ObtenerInstructores();

                // Asignar instructores para cursos
                var listaInstructoresCurso = new List<Usuario>();
                listaInstructoresCurso.Add(new Usuario { Id = "", Nombre = "-- Sin Instructor --" });
                listaInstructoresCurso.AddRange(instructores);

                cboCursoInstructor.DataSource = listaInstructoresCurso;
                cboCursoInstructor.DisplayMember = "Nombre";
                cboCursoInstructor.ValueMember = "Id";

                // Asignar instructores para arte
                var listaInstructoresArte = new List<Usuario>();
                listaInstructoresArte.Add(new Usuario { Id = "", Nombre = "-- Sin Instructor --" });
                listaInstructoresArte.AddRange(instructores);

                cboArteInstructor.DataSource = listaInstructoresArte;
                cboArteInstructor.DisplayMember = "Nombre";
                cboArteInstructor.ValueMember = "Id";

                // Obtener categorias
                var categorias = arteRepo.ObtenerTodasLasCategorias();
                cboArteCategoria.DataSource = categorias;
                cboArteCategoria.DisplayMember = "NombreCategoria";
                cboArteCategoria.ValueMember = "IdCategoria";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar catálogos en desplegables: {ex.Message}", "Error de Catálogos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // =========================================================================
        // MÉTODOS DE VALIDACIÓN DE ENTRADA Y NOTIFICACIÓN
        // =========================================================================

        private bool MostrarErroresValidacion(List<string> errores, Control? primerControlConError)
        {
            if (errores.Count == 0) return true;

            string mensaje = "Por favor corrija los siguientes errores antes de continuar:\n\n• "
                + string.Join("\n• ", errores);

            MessageBox.Show(mensaje, "Validación de Campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            primerControlConError?.Focus();
            return false;
        }

        private bool ValidarCurso(bool esActualizacion, out Curso? cursoValido, out Control? controlConError)
        {
            cursoValido = null;
            controlConError = null;
            var errores = new List<string>();

            int idCurso = 0;
            if (esActualizacion)
            {
                if (!int.TryParse(txtCursoId.Text?.Trim(), out idCurso) || idCurso <= 0)
                {
                    errores.Add("Debe seleccionar un curso válido de la tabla para poder actualizarlo.");
                    controlConError ??= gridCursos;
                }
            }

            string nombre = txtCursoNombre.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 3)
            {
                errores.Add("El nombre del curso es obligatorio y debe tener al menos 3 caracteres (máx. 150).");
                controlConError ??= txtCursoNombre;
            }
            else if (nombre.Length > 150)
            {
                errores.Add("El nombre del curso no puede exceder los 150 caracteres.");
                controlConError ??= txtCursoNombre;
            }

            string desc = txtCursoDesc.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(desc) || desc.Length < 5)
            {
                errores.Add("La descripción del curso es obligatoria y debe tener al menos 5 caracteres (máx. 500).");
                controlConError ??= txtCursoDesc;
            }
            else if (desc.Length > 500)
            {
                errores.Add("La descripción del curso no puede exceder los 500 caracteres.");
                controlConError ??= txtCursoDesc;
            }

            DateTime inicio = dtpCursoInicio.Value;
            DateTime fin = dtpCursoFin.Value;
            if (fin.Date < inicio.Date)
            {
                errores.Add("La fecha de finalización debe ser igual o posterior a la fecha de inicio.");
                controlConError ??= dtpCursoFin;
            }

            string horario = txtCursoHorario.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(horario) || horario.Length < 3)
            {
                errores.Add("El horario es obligatorio y debe tener al menos 3 caracteres (máx. 100).");
                controlConError ??= txtCursoHorario;
            }
            else if (horario.Length > 100)
            {
                errores.Add("El horario no puede exceder los 100 caracteres.");
                controlConError ??= txtCursoHorario;
            }

            string cupoTxt = txtCursoCupo.Text?.Trim() ?? "";
            if (!int.TryParse(cupoTxt, out int cupo) || cupo <= 0)
            {
                errores.Add("El cupo máximo debe ser un número entero positivo mayor a 0.");
                controlConError ??= txtCursoCupo;
            }

            string estadoRaw = txtCursoEstado.Text?.Trim() ?? "";
            string[] estadosPermitidos = { "Activo", "Inactivo", "Finalizado" };
            string? estadoEncontrado = estadosPermitidos.FirstOrDefault(e => e.Equals(estadoRaw, StringComparison.OrdinalIgnoreCase));
            if (estadoEncontrado == null)
            {
                errores.Add("El estado del curso debe ser 'Activo', 'Inactivo' o 'Finalizado'.");
                controlConError ??= txtCursoEstado;
            }

            if (!MostrarErroresValidacion(errores, controlConError))
            {
                return false;
            }

            string? instructor = cboCursoInstructor.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(instructor)) instructor = null;

            cursoValido = new Curso
            {
                IdCurso = idCurso,
                NombreCurso = nombre,
                Descripcion = desc,
                FechaInicio = inicio,
                FechaFin = fin,
                Horario = horario,
                CupoMaximo = cupo,
                Estado = estadoEncontrado!,
                CarnetInstructor = instructor
            };

            return true;
        }

        private bool ValidarAsociacion(bool esActualizacion, out Asociacion? asoValida, out Control? controlConError)
        {
            asoValida = null;
            controlConError = null;
            var errores = new List<string>();

            if (esActualizacion)
            {
                if (!selectedAsoId.HasValue || selectedAsoId.Value <= 0)
                {
                    errores.Add("Debe seleccionar una asociación de la tabla para poder actualizarla.");
                    controlConError ??= gridAsociaciones;
                }
            }

            string acronimo = txtAsoAcronimo.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(acronimo) || acronimo.Length < 2)
            {
                errores.Add("El acrónimo de la asociación es obligatorio y debe tener al menos 2 caracteres (máx. 20).");
                controlConError ??= txtAsoAcronimo;
            }
            else if (acronimo.Length > 20)
            {
                errores.Add("El acrónimo de la asociación no puede exceder los 20 caracteres.");
                controlConError ??= txtAsoAcronimo;
            }

            string nombre = txtAsoNombre.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 3)
            {
                errores.Add("El nombre de la asociación es obligatorio y debe tener al menos 3 caracteres (máx. 200).");
                controlConError ??= txtAsoNombre;
            }
            else if (nombre.Length > 200)
            {
                errores.Add("El nombre de la asociación no puede exceder los 200 caracteres.");
                controlConError ??= txtAsoNombre;
            }

            string anioTxt = txtAsoAnio.Text?.Trim() ?? "";
            int anioActual = DateTime.Now.Year;
            if (!int.TryParse(anioTxt, out int anio) || anio < 1900 || anio > anioActual)
            {
                errores.Add($"El año de fundación debe ser un número válido entre 1900 y {anioActual}.");
                controlConError ??= txtAsoAnio;
            }

            string desc = txtAsoDesc.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(desc) || desc.Length < 10)
            {
                errores.Add("La descripción de la asociación es obligatoria y debe tener al menos 10 caracteres.");
                controlConError ??= txtAsoDesc;
            }

            string formulario = txtAsoFormulario.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(formulario) || formulario.Length < 5)
            {
                errores.Add("El enlace o información del formulario de inscripción es obligatorio y debe tener al menos 5 caracteres.");
                controlConError ??= txtAsoFormulario;
            }

            string localImgPath = txtAsoRutaImg.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(localImgPath) && localImgPath != "(Imagen actual guardada)" && !File.Exists(localImgPath))
            {
                errores.Add("La ruta de imagen local especificada para la asociación no existe en el equipo.");
                controlConError ??= txtAsoRutaImg;
            }

            if (!MostrarErroresValidacion(errores, controlConError))
            {
                return false;
            }

            asoValida = new Asociacion
            {
                IdAsociacion = selectedAsoId ?? 0,
                Acronimo = acronimo,
                Nombre = nombre,
                AnioFundacion = anio,
                Descripcion = desc,
                Formulario = formulario
            };

            return true;
        }

        private bool ValidarDeporte(bool esActualizacion, out SeleccionDeportiva? depValido, out Control? controlConError)
        {
            depValido = null;
            controlConError = null;
            var errores = new List<string>();

            if (esActualizacion)
            {
                if (!selectedDeporteId.HasValue || selectedDeporteId.Value <= 0)
                {
                    errores.Add("Debe seleccionar un deporte de la tabla para poder actualizarlo.");
                    controlConError ??= gridDeportes;
                }
            }

            string disciplina = txtDepDisciplina.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(disciplina) || disciplina.Length < 3)
            {
                errores.Add("La disciplina deportiva es obligatoria y debe tener al menos 3 caracteres (máx. 50).");
                controlConError ??= txtDepDisciplina;
            }
            else if (disciplina.Length > 50)
            {
                errores.Add("La disciplina deportiva no puede exceder los 50 caracteres.");
                controlConError ??= txtDepDisciplina;
            }

            string rama = txtDepRama.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(rama) || rama.Length < 3)
            {
                errores.Add("La rama deportiva es obligatoria y debe tener al menos 3 caracteres (máx. 20).");
                controlConError ??= txtDepRama;
            }
            else if (rama.Length > 20)
            {
                errores.Add("La rama deportiva no puede exceder los 20 caracteres.");
                controlConError ??= txtDepRama;
            }

            string equipo = txtDepEquipo.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(equipo) || equipo.Length < 3)
            {
                errores.Add("El nombre del equipo/selección es obligatorio y debe tener al menos 3 caracteres (máx. 100).");
                controlConError ??= txtDepEquipo;
            }
            else if (equipo.Length > 100)
            {
                errores.Add("El nombre del equipo no puede exceder los 100 caracteres.");
                controlConError ??= txtDepEquipo;
            }

            string localImgPath = txtDepRutaImg.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(localImgPath) && localImgPath != "(Imagen actual guardada)" && !File.Exists(localImgPath))
            {
                errores.Add("La ruta de imagen local especificada para el deporte no existe en el equipo.");
                controlConError ??= txtDepRutaImg;
            }

            if (!MostrarErroresValidacion(errores, controlConError))
            {
                return false;
            }

            depValido = new SeleccionDeportiva
            {
                IdSeleccion = selectedDeporteId ?? 0,
                Disciplina = disciplina,
                Rama = rama,
                NombreEquipo = equipo
            };

            return true;
        }

        private bool ValidarArte(bool esActualizacion, out ActividadDAC? arteValido, out Control? controlConError)
        {
            arteValido = null;
            controlConError = null;
            var errores = new List<string>();

            if (esActualizacion)
            {
                if (!selectedArteId.HasValue || selectedArteId.Value <= 0)
                {
                    errores.Add("Debe seleccionar una actividad artística de la tabla para poder actualizarla.");
                    controlConError ??= gridArte;
                }
            }

            if (cboArteCategoria.SelectedValue == null ||
                !int.TryParse(cboArteCategoria.SelectedValue.ToString(), out int idCat) ||
                idCat <= 0)
            {
                errores.Add("Debe seleccionar una categoría válida para la actividad artística.");
                controlConError ??= cboArteCategoria;
            }

            string nombre = txtArteNombre.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 3)
            {
                errores.Add("El nombre de la actividad artística es obligatorio y debe tener al menos 3 caracteres (máx. 150).");
                controlConError ??= txtArteNombre;
            }
            else if (nombre.Length > 150)
            {
                errores.Add("El nombre de la actividad artística no puede exceder los 150 caracteres.");
                controlConError ??= txtArteNombre;
            }

            string desc = txtArteDesc.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(desc) || desc.Length < 5)
            {
                errores.Add("La descripción de la actividad artística es obligatoria y debe tener al menos 5 caracteres.");
                controlConError ??= txtArteDesc;
            }

            if (!MostrarErroresValidacion(errores, controlConError))
            {
                return false;
            }

            string? instructor = cboArteInstructor.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(instructor)) instructor = null;

            arteValido = new ActividadDAC
            {
                IdActividad = selectedArteId ?? 0,
                IdCategoria = Convert.ToInt32(cboArteCategoria.SelectedValue),
                NombreActividad = nombre,
                Descripcion = desc,
                CarnetInstructor = instructor
            };

            return true;
        }

        // =========================================================================
        // ACCIONES DE CURSOS
        // =========================================================================

        private void btnCrearCurso_Click(object sender, EventArgs e)
        {
            if (!ValidarCurso(esActualizacion: false, out Curso? cursoAgregado, out _))
            {
                return;
            }

            try
            {
                bool exito = cursoRepo.AgregarCurso(cursoAgregado!);
                if (exito)
                {
                    MessageBox.Show("Curso creado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    LimpiarCamposCurso();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear curso: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnActualizarCurso_Click(object sender, EventArgs e)
        {
            if (!ValidarCurso(esActualizacion: true, out Curso? cursoModificado, out _))
            {
                return;
            }

            try
            {
                bool exito = cursoRepo.ActualizarCurso(cursoModificado!);
                if (exito)
                {
                    MessageBox.Show("Curso actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    LimpiarCamposCurso();
                }
                else
                {
                    MessageBox.Show("No se realizaron cambios en el curso.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar curso: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LimpiarCamposCurso()
        {
            txtCursoId.Clear();
            txtCursoId.Enabled = false;
            txtCursoNombre.Clear();
            txtCursoDesc.Clear();
            txtCursoHorario.Clear();
            txtCursoCupo.Clear();
            cboCursoInstructor.SelectedIndex = 0;
            txtCursoEstado.Text = "Activo";
        }

        private void gridCursos_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                CargarCursoSeleccionado(gridCursos.Rows[e.RowIndex]);
            }
        }

        private void CargarCursoSeleccionado(DataGridViewRow curso)
        {
            txtCursoId.Text = curso.Cells["IdCurso"].Value?.ToString();
            txtCursoId.Enabled = false;

            txtCursoNombre.Text = curso.Cells["NombreCurso"].Value?.ToString();
            txtCursoDesc.Text = curso.Cells["Descripcion"].Value?.ToString();

            // Configurar campos de fecha
            if (curso.Cells["FechaInicio"].Value != null)
                dtpCursoInicio.Value = Convert.ToDateTime(curso.Cells["FechaInicio"].Value);

            if (curso.Cells["FechaFin"].Value != null)
                dtpCursoFin.Value = Convert.ToDateTime(curso.Cells["FechaFin"].Value);

            txtCursoHorario.Text = curso.Cells["Horario"].Value?.ToString();

            if (curso.Cells["CupoMaximo"].Value != null)
                txtCursoCupo.Text = curso.Cells["CupoMaximo"].Value?.ToString();

            txtCursoEstado.Text = curso.Cells["Estado"].Value?.ToString();
            string? nombreInstructor = curso.Cells["NombreInstructor"].Value?.ToString();
            if (!string.IsNullOrEmpty(nombreInstructor))
            {
                cboCursoInstructor.Text = nombreInstructor;
            }
            else
            {
                cboCursoInstructor.SelectedIndex = 0;
            }
        }

        // =========================================================================
        // ACCIONES DE ASOCIACIONES
        // =========================================================================

        private void CargarAsociacionSeleccionada(DataGridViewRow row)
        {
            if (row.Cells["IdAsociacion"].Value != null)
            {
                selectedAsoId = Convert.ToInt32(row.Cells["IdAsociacion"].Value);
            }
            txtAsoAcronimo.Text = row.Cells["Acronimo"].Value?.ToString();
            txtAsoNombre.Text = row.Cells["Nombre"].Value?.ToString();
            txtAsoAnio.Text = row.Cells["AnioFundacion"].Value?.ToString();
            txtAsoDesc.Text = row.Cells["Descripcion"].Value?.ToString();
            txtAsoFormulario.Text = row.Cells["Formulario"].Value?.ToString();
            selectedAsoImgUrl = row.Cells["ImgUrl"].Value?.ToString();
            txtAsoRutaImg.Text = string.IsNullOrEmpty(selectedAsoImgUrl) ? "" : "(Imagen actual guardada)";
        }

        private void LimpiarCamposAsociacion()
        {
            selectedAsoId = null;
            selectedAsoImgUrl = null;
            txtAsoAcronimo.Clear();
            txtAsoNombre.Clear();
            txtAsoAnio.Clear();
            txtAsoDesc.Clear();
            txtAsoFormulario.Clear();
            txtAsoRutaImg.Clear();
        }

        private async void btnCrearAso_Click(object sender, EventArgs e)
        {
            if (!ValidarAsociacion(esActualizacion: false, out Asociacion? nuevaAso, out _))
            {
                return;
            }

            try
            {
                string localImgPath = txtAsoRutaImg.Text?.Trim() ?? "";
                string? uploadedUrl = null;

                if (!string.IsNullOrWhiteSpace(localImgPath))
                {
                    this.Cursor = Cursors.WaitCursor;
                    btnCrearAso.Enabled = false;

                    try
                    {
                        uploadedUrl = await imgService.SubirImagenAsync(localImgPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al subir imagen a imgBB: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        btnCrearAso.Enabled = true;
                    }
                }

                nuevaAso!.ImgUrl = uploadedUrl;

                bool exito = asoRepo.CrearAsociacion(nuevaAso);
                if (exito)
                {
                    MessageBox.Show("Asociación creada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    LimpiarCamposAsociacion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear asociación: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnActualizarAso_Click(object sender, EventArgs e)
        {
            if (!ValidarAsociacion(esActualizacion: true, out Asociacion? asoModificada, out _))
            {
                return;
            }

            try
            {
                string localImgPath = txtAsoRutaImg.Text?.Trim() ?? "";
                string? uploadedUrl = null;

                if (!string.IsNullOrWhiteSpace(localImgPath) && localImgPath != "(Imagen actual guardada)" && File.Exists(localImgPath))
                {
                    this.Cursor = Cursors.WaitCursor;
                    btnActualizarAso.Enabled = false;

                    try
                    {
                        uploadedUrl = await imgService.SubirImagenAsync(localImgPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al subir imagen a imgBB: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        btnActualizarAso.Enabled = true;
                    }
                }
                else
                {
                    uploadedUrl = selectedAsoImgUrl;
                }

                asoModificada!.IdAsociacion = selectedAsoId!.Value;
                asoModificada.ImgUrl = uploadedUrl;

                bool exito = asoRepo.ActualizarAsociacion(asoModificada);
                if (exito)
                {
                    MessageBox.Show("Asociación actualizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    LimpiarCamposAsociacion();
                }
                else
                {
                    MessageBox.Show("No se realizaron cambios en la asociación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar asociación: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // =========================================================================
        // ACCIONES DE DEPORTES
        // =========================================================================

        private void CargarDeporteSeleccionado(DataGridViewRow row)
        {
            if (row.Cells["IdSeleccion"].Value != null)
            {
                selectedDeporteId = Convert.ToInt32(row.Cells["IdSeleccion"].Value);
            }
            txtDepDisciplina.Text = row.Cells["Disciplina"].Value?.ToString();
            txtDepRama.Text = row.Cells["Rama"].Value?.ToString();
            txtDepEquipo.Text = row.Cells["NombreEquipo"].Value?.ToString();
            selectedDeporteImgUrl = row.Cells["ImgUrl"].Value?.ToString();
            txtDepRutaImg.Text = string.IsNullOrEmpty(selectedDeporteImgUrl) ? "" : "(Imagen actual guardada)";
        }

        private void LimpiarCamposDeporte()
        {
            selectedDeporteId = null;
            selectedDeporteImgUrl = null;
            txtDepDisciplina.Clear();
            txtDepRama.Clear();
            txtDepEquipo.Clear();
            txtDepRutaImg.Clear();
        }

        private async void btnCrearDeporte_Click(object sender, EventArgs e)
        {
            if (!ValidarDeporte(esActualizacion: false, out SeleccionDeportiva? nuevoDeporte, out _))
            {
                return;
            }

            try
            {
                string localImgPath = txtDepRutaImg.Text?.Trim() ?? "";
                string? uploadedUrl = null;

                if (!string.IsNullOrWhiteSpace(localImgPath))
                {
                    this.Cursor = Cursors.WaitCursor;
                    btnCrearDeporte.Enabled = false;

                    try
                    {
                        uploadedUrl = await imgService.SubirImagenAsync(localImgPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al subir imagen a imgBB: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        btnCrearDeporte.Enabled = true;
                    }
                }

                nuevoDeporte!.ImgUrl = uploadedUrl;

                bool exito = deporteRepo.CrearSeleccionDeportiva(nuevoDeporte);
                if (exito)
                {
                    MessageBox.Show("Selección deportiva creada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    LimpiarCamposDeporte();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear selección: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnActualizarDeporte_Click(object sender, EventArgs e)
        {
            if (!ValidarDeporte(esActualizacion: true, out SeleccionDeportiva? depModificado, out _))
            {
                return;
            }

            try
            {
                string localImgPath = txtDepRutaImg.Text?.Trim() ?? "";
                string? uploadedUrl = null;

                if (!string.IsNullOrWhiteSpace(localImgPath) && localImgPath != "(Imagen actual guardada)" && File.Exists(localImgPath))
                {
                    this.Cursor = Cursors.WaitCursor;
                    btnActualizarDeporte.Enabled = false;

                    try
                    {
                        uploadedUrl = await imgService.SubirImagenAsync(localImgPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al subir imagen a imgBB: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        btnActualizarDeporte.Enabled = true;
                    }
                }
                else
                {
                    uploadedUrl = selectedDeporteImgUrl;
                }

                depModificado!.IdSeleccion = selectedDeporteId!.Value;
                depModificado.ImgUrl = uploadedUrl;

                bool exito = deporteRepo.ActualizarSeleccionDeportiva(depModificado);
                if (exito)
                {
                    MessageBox.Show("Selección deportiva actualizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    LimpiarCamposDeporte();
                }
                else
                {
                    MessageBox.Show("No se realizaron cambios en la selección deportiva.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar selección deportiva: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAsoBuscarImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Todos los archivos (*.*)|*.*";
                ofd.Title = "Seleccionar Imagen de la Asociación";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtAsoRutaImg.Text = ofd.FileName;
                }
            }
        }

        private void btnDepBuscarImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Todos los archivos (*.*)|*.*";
                ofd.Title = "Seleccionar Imagen del Deporte";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtDepRutaImg.Text = ofd.FileName;
                }
            }
        }

        // =========================================================================
        // ACCIONES DE ARTE Y CULTURA (DAC)
        // =========================================================================

        private void CargarArteSeleccionado(DataGridViewRow row)
        {
            if (row.Cells["IdActividad"].Value != null)
            {
                selectedArteId = Convert.ToInt32(row.Cells["IdActividad"].Value);
            }
            txtArteNombre.Text = row.Cells["NombreActividad"].Value?.ToString();
            txtArteDesc.Text = row.Cells["Descripcion"].Value?.ToString();
            if (row.Cells["IdCategoria"].Value != null)
            {
                cboArteCategoria.SelectedValue = Convert.ToInt32(row.Cells["IdCategoria"].Value);
            }
            string? instructor = row.Cells["CarnetInstructor"].Value?.ToString();
            if (!string.IsNullOrEmpty(instructor))
            {
                cboArteInstructor.SelectedValue = instructor;
            }
            else
            {
                cboArteInstructor.SelectedIndex = 0;
            }
        }

        private void LimpiarCamposArte()
        {
            selectedArteId = null;
            txtArteNombre.Clear();
            txtArteDesc.Clear();
            cboArteInstructor.SelectedIndex = 0;
        }

        private void btnCrearArte_Click(object sender, EventArgs e)
        {
            if (!ValidarArte(esActualizacion: false, out ActividadDAC? nuevaActividad, out _))
            {
                return;
            }

            try
            {
                bool exito = arteRepo.CrearActividadDAC(nuevaActividad!);
                if (exito)
                {
                    MessageBox.Show("Actividad DAC creada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    LimpiarCamposArte();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear actividad DAC: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnActualizarArte_Click(object sender, EventArgs e)
        {
            if (!ValidarArte(esActualizacion: true, out ActividadDAC? arteModificado, out _))
            {
                return;
            }

            try
            {
                arteModificado!.IdActividad = selectedArteId!.Value;
                bool exito = arteRepo.ActualizarActividadDAC(arteModificado);
                if (exito)
                {
                    MessageBox.Show("Actividad artística actualizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    LimpiarCamposArte();
                }
                else
                {
                    MessageBox.Show("No se realizaron cambios en la actividad artística.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar actividad DAC: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Navegacion y cierre
        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.formPrincipal.Show();
            this.Close();
        }

        private void AdminDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !this.formPrincipal.Visible)
            {
                Application.Exit();
            }
        }
    }
}
