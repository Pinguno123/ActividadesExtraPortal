using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using ActividadesExtraPortal.Data;
using ActividadesExtraPortal.Services;

namespace ActividadesExtraPortal
{
    public partial class AdminDashboard : Form
    {
        private readonly Portal formPrincipal;

        // Repositorios
        private readonly CursoRepository cursoRepo = new CursoRepository();
        private readonly AsociacionRepository asoRepo = new AsociacionRepository();
        private readonly DeporteRepository deporteRepo = new DeporteRepository();
        private readonly ArteRepository arteRepo = new ArteRepository();

        // Servicio de imagenes
        private readonly ImgBBService imgService = new ImgBBService();

        public AdminDashboard(Portal principal)
        {
            InitializeComponent();
            this.formPrincipal = principal;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            CargarGrids();
            CargarComboBoxes();
            txtCursoId.Enabled = false;
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
                // Obtener usuarios
                var userRepo = new UsuarioRepository();
                var usuarios = userRepo.ObtenerTodosLosUsuarios();

                // Asignar instructores para cursos
                var listaInstructoresCurso = new List<Usuario>();
                listaInstructoresCurso.Add(new Usuario { Id = "", Nombre = "-- Sin Instructor --" });
                listaInstructoresCurso.AddRange(usuarios);

                cboCursoInstructor.DataSource = listaInstructoresCurso;
                cboCursoInstructor.DisplayMember = "Nombre";
                cboCursoInstructor.ValueMember = "Id";

                // Asignar instructores para arte
                var listaInstructoresArte = new List<Usuario>();
                listaInstructoresArte.Add(new Usuario { Id = "", Nombre = "-- Sin Instructor --" });
                listaInstructoresArte.AddRange(usuarios);

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

        // Acciones de cursos
        private void btnCrearCurso_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtCursoNombre.Text;
                string desc = txtCursoDesc.Text;
                DateTime inicio = dtpCursoInicio.Value;
                DateTime fin = dtpCursoFin.Value;
                string horario = txtCursoHorario.Text;
                int cupo = int.Parse(txtCursoCupo.Text);

                // Obtener instructor
                string? instructor = cboCursoInstructor.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(instructor)) instructor = null;

                string estado = txtCursoEstado.Text;

                Curso cursoAgregado = new Curso
                {
                    NombreCurso = nombre,
                    Descripcion = desc,
                    FechaInicio = inicio,
                    FechaFin = fin,
                    Horario = horario,
                    CupoMaximo = cupo,
                    Estado = estado,
                    CarnetInstructor = instructor
                };

                bool exito = cursoRepo.AgregarCurso(cursoAgregado);
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
            try
            {
                int id = int.Parse(txtCursoId.Text);
                string nombre = txtCursoNombre.Text;
                string desc = txtCursoDesc.Text;
                DateTime inicio = dtpCursoInicio.Value;
                DateTime fin = dtpCursoFin.Value;
                string horario = txtCursoHorario.Text;
                int cupo = int.Parse(txtCursoCupo.Text);
                string estado = txtCursoEstado.Text;

                // Obtener instructor
                string? instructor = cboCursoInstructor.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(instructor)) instructor = null;

                Curso cursoModificado = new Curso
                {
                    IdCurso = id,
                    NombreCurso = nombre,
                    Descripcion = desc,
                    FechaInicio = inicio,
                    FechaFin = fin,
                    Horario = horario,
                    CupoMaximo = cupo,
                    Estado = estado,
                    CarnetInstructor = instructor
                };

                bool exito = cursoRepo.ActualizarCurso(cursoModificado);
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
                DataGridViewRow curso = gridCursos.Rows[e.RowIndex];

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
                cboCursoInstructor.Text = curso.Cells["NombreInstructor"].Value?.ToString();
            }
        }

        // Acciones de asociaciones
        private async void btnCrearAso_Click(object sender, EventArgs e)
        {
            try
            {
                string acronimo = txtAsoAcronimo.Text;
                string nombre = txtAsoNombre.Text;
                int anio = int.Parse(txtAsoAnio.Text);
                string desc = txtAsoDesc.Text;
                string formulario = txtAsoFormulario.Text;
                string localImgPath = txtAsoRutaImg.Text;
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

                Asociacion nuevaAso = new Asociacion
                {
                    Acronimo = acronimo,
                    Nombre = nombre,
                    AnioFundacion = anio,
                    Descripcion = desc,
                    Formulario = formulario,
                    ImgUrl = uploadedUrl
                };

                bool exito = asoRepo.CrearAsociacion(nuevaAso);
                if (exito)
                {
                    MessageBox.Show("Asociación creada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    txtAsoAcronimo.Clear();
                    txtAsoNombre.Clear();
                    txtAsoAnio.Clear();
                    txtAsoDesc.Clear();
                    txtAsoFormulario.Clear();
                    txtAsoRutaImg.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear asociación: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Acciones de deportes
        private async void btnCrearDeporte_Click(object sender, EventArgs e)
        {
            try
            {
                string disciplina = txtDepDisciplina.Text;
                string rama = txtDepRama.Text;
                string equipo = txtDepEquipo.Text;
                string localImgPath = txtDepRutaImg.Text;
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

                SeleccionDeportiva nuevoDeporte = new SeleccionDeportiva
                {
                    Disciplina = disciplina,
                    Rama = rama,
                    NombreEquipo = equipo,
                    ImgUrl = uploadedUrl
                };

                bool exito = deporteRepo.CrearSeleccionDeportiva(nuevoDeporte);
                if (exito)
                {
                    MessageBox.Show("Selección deportiva creada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    txtDepDisciplina.Clear();
                    txtDepRama.Clear();
                    txtDepEquipo.Clear();
                    txtDepRutaImg.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear selección: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // Acciones de arte y cultura (DAC)
        private void btnCrearArte_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener datos de controles
                int idCat = Convert.ToInt32(cboArteCategoria.SelectedValue);
                string nombre = txtArteNombre.Text;
                string desc = txtArteDesc.Text;

                string? instructor = cboArteInstructor.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(instructor)) instructor = null;

                ActividadDAC nuevaActividad = new ActividadDAC
                {
                    IdCategoria = idCat,
                    NombreActividad = nombre,
                    Descripcion = desc,
                    CarnetInstructor = instructor
                };

                bool exito = arteRepo.CrearActividadDAC(nuevaActividad);
                if (exito)
                {
                    MessageBox.Show("Actividad DAC creada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrids();
                    txtArteNombre.Clear();
                    txtArteDesc.Clear();
                    cboArteInstructor.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear actividad DAC: {ex.Message}", "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
