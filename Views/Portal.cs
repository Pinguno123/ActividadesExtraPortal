using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActividadesExtraPortal.Views;

namespace ActividadesExtraPortal
{
    public partial class Portal : Form
    {
        private Usuario? usuarioActual;

        public Usuario? UsuarioActual => usuarioActual;

        public Portal(Usuario? usuario = null)
        {
            InitializeComponent();
            usuarioActual = usuario;

            // Suscribir al evento Shown y VisibleChanged
            this.Shown += Form1_Shown;
            this.VisibleChanged += Portal_VisibleChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (usuarioActual == null) return;

            // Cargar datos en los controles
            txtId.Text = usuarioActual.Id;
            txtCarrera.Text = usuarioActual.Carrera;
            txtCampus.Text = usuarioActual.Campus;
            txtEstado.Text = usuarioActual.Estado;

            // Validar rol de administrador
            try
            {
                var repo = new ActividadesExtraPortal.Data.UsuarioRepository();
                string? rolActual = repo.ObtenerRolActual(usuarioActual.Id);
                if (rolActual == "Administrador")
                {
                    btnAdmin.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al validar rol de administrador: {ex.Message}");
            }

            // Dividir nombre completo
            string[] partesNombre = usuarioActual.Nombre.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Ajustar texto segun cantidad de palabras
            if (partesNombre.Length >= 4)
            {
                // Primeros dos nombres
                string nombresArriba = $"{partesNombre[0]} {partesNombre[1]}";

                // Apellidos
                string apellidosAbajo = string.Join(" ", partesNombre.Skip(2));

                // Asignar texto con salto de linea
                txtNombre.Text = $"{nombresArriba}{Environment.NewLine}{apellidosAbajo}";
            }
            else if (partesNombre.Length == 3)
            {
                // Caso especial con tres palabras
                txtNombre.Text = $"{partesNombre[0]}{Environment.NewLine}{partesNombre[1]} {partesNombre[2]}";
            }
            else
            {
                // Caso por defecto
                txtNombre.Text = usuarioActual.Nombre;
            }

            ActualizarEstadosPaneles();
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            if (usuarioActual == null) return;

            // Cargar foto de perfil
            try
            {
                // Obtener ruta de la imagen
                string rutaAbsolutaReal = usuarioActual.RutaArchivo;

                if (!string.IsNullOrEmpty(rutaAbsolutaReal) && !rutaAbsolutaReal.Contains(Path.DirectorySeparatorChar))
                {
                    rutaAbsolutaReal = Path.Combine(Path.GetTempPath(), usuarioActual.RutaArchivo);
                }

                // Verificar existencia del archivo
                if (!string.IsNullOrEmpty(rutaAbsolutaReal) && File.Exists(rutaAbsolutaReal))
                {
                    // Limpiar imagen anterior
                    if (pBpfp.Image != null)
                    {
                        pBpfp.Image.Dispose();
                        pBpfp.Image = null;
                    }

                    // Cargar imagen en memoria y liberar archivo
                    using (var stream = new FileStream(rutaAbsolutaReal, FileMode.Open, FileAccess.Read))
                    {
                        using (var imgOriginal = Image.FromStream(stream))
                        {
                            // Asignar copia del bitmap
                            pBpfp.Image = new Bitmap(imgOriginal);
                        }
                    }

                    // Ajustar escala de la imagen
                    pBpfp.SizeMode = PictureBoxSizeMode.Zoom;

                    // Redibujar el control
                    pBpfp.Refresh();
                    HacerOvalado(pBpfp);
                }
                else
                {
                    MessageBox.Show($"La foto no se cargó porque el archivo no existe en:\n{rutaAbsolutaReal}",
                                    "Error de Ubicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error físico al renderizar en pBpfp: {ex.Message}",
                                "Error de Gráficos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HacerOvalado(PictureBox picBox)
        {
            // Validar dimensiones antes de recortar
            if (picBox.Width > 0 && picBox.Height > 0)
            {
                // Crear ruta de recorte
                using (System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    // Definir elipse para contorno
                    gp.AddEllipse(0, 0, picBox.Width, picBox.Height);

                    // Liberar region previa
                    if (picBox.Region != null)
                    {
                        picBox.Region.Dispose();
                    }

                    // Aplicar recorte ovalado
                    picBox.Region = new Region(gp);
                }
            }
        }

        private void Abrir_ArteCultura(object sender, EventArgs e)
        {
            this.Hide();
            Arte arte = new Arte(this);
            arte.Show();
        }
        private void Abrir_Asociaciones(object sender, EventArgs e)
        {
            this.Hide();
            Asociaciones aso = new Asociaciones(this);
            aso.Show();
        }
        private void Abrir_Deportes(object sender, EventArgs e)
        {
            this.Hide();
            Deportes deportes = new Deportes(this);
            deportes.Show();
        }
        private void Abrir_Cursos(object sender, EventArgs e)
        {
            this.Hide();
            Cursos cursos = new Cursos(this);
            cursos.Show();
        }
        private void btnAdmin_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminDashboard adminForm = new AdminDashboard(this);
            adminForm.Show();
        }

        private void Portal_VisibleChanged(object? sender, EventArgs e)
        {
            if (this.Visible)
            {
                ActualizarEstadosPaneles();
            }
        }

        private void ActualizarEstadosPaneles()
        {
            if (usuarioActual == null) return;

            string carnet = usuarioActual.Id;

            try
            {
                // 1. Obtener registros y contar para los paneles superiores
                var arteRepo = new ActividadesExtraPortal.Data.ArteRepository();
                var inscripcionesArte = arteRepo.ObtenerInscripcionesDACEstudiante(carnet);
                bool tieneArte = inscripcionesArte != null && inscripcionesArte.Count > 0;
                ActualizarVisualizacionPanel(panel8, label5, "Arte y Cultura", tieneArte, inscripcionesArte?.Count ?? 0);

                var asoRepo = new ActividadesExtraPortal.Data.AsociacionRepository();
                var membresias = asoRepo.ObtenerMembresiasEstudiante(carnet);
                var activas = membresias.Where(m => m.EstadoValidacion == "Aprobada" || m.EstadoValidacion == "Pendiente").ToList();
                bool tieneAsociaciones = activas.Count > 0;
                ActualizarVisualizacionPanel(panel9, label6, "Asociaciones", tieneAsociaciones, tieneAsociaciones ? activas.Count : 0);

                var deporteRepo = new ActividadesExtraPortal.Data.DeporteRepository();
                var participaciones = deporteRepo.ObtenerParticipacionesEstudiante(carnet);
                bool tieneDeportes = participaciones != null && participaciones.Count > 0;
                ActualizarVisualizacionPanel(panel10, label7, "Deportes", tieneDeportes, participaciones?.Count ?? 0);

                var cursoRepo = new ActividadesExtraPortal.Data.CursoRepository();
                var inscripcionesCurso = cursoRepo.ObtenerInscripcionesEstudiante(carnet);
                var cursosActivos = inscripcionesCurso.Where(c => c.EstadoInscripcion == "Confirmada" || c.EstadoInscripcion == "Pre-inscrito").ToList();
                bool tieneCursos = cursosActivos.Count > 0;
                ActualizarVisualizacionPanel(panel11, label8, "Cursos", tieneCursos, tieneCursos ? cursosActivos.Count : 0);

                // 2. Llenar los paneles horizontales de abajo según su categoría correspondiente

                // A. Arte y Cultura (pnArte)
                var flpArte = GetFlowLayoutPanel(pnArte);
                if (tieneArte && inscripcionesArte != null && inscripcionesArte.Count > 0)
                {
                    foreach (var a in inscripcionesArte.Where(x => x.EstadoInscripcion == "Activa"))
                    {
                        var tarjeta = new ActividadInscritaTarjeta();
                        tarjeta.CargarDatos(a.NombreActividad ?? "", 
                            "Categoría: " + a.NombreCategoria + " | Instructor: " + (a.NombreInstructor ?? "N/A"), 
                            a.EstadoInscripcion);
                        flpArte.Controls.Add(tarjeta);
                    }
                }
                else
                {
                    MostrarMensajeVacioEnFlp(flpArte, "Arte y Cultura", "No estás inscrito en actividades de Arte y Cultura.");
                }

                // B. Panel de Asociaciones (pnAsoc)
                var flpAsoc = GetFlowLayoutPanel(pnAsoc);
                if (tieneAsociaciones && activas.Count > 0)
                {
                    foreach (var m in activas)
                    {
                        var tarjeta = new ActividadInscritaTarjeta();
                        tarjeta.CargarDatos(m.NombreAsociacion ?? "", 
                            "Acrónimo: " + m.AcronimoAsociacion + " | Solicitado: " + m.FechaSolicitud.ToShortDateString(), 
                            m.EstadoValidacion);
                        flpAsoc.Controls.Add(tarjeta);
                    }
                }
                else
                {
                    MostrarMensajeVacioEnFlp(flpAsoc, "Asociaciones", "No eres miembro de ninguna asociación estudiantil.");
                }

                // C. Panel de Deportes (pnDeportes)
                var flpDeportes = GetFlowLayoutPanel(pnDeportes);
                if (tieneDeportes && participaciones != null && participaciones.Count > 0)
                {
                    foreach (var p in participaciones)
                    {
                        var tarjeta = new ActividadInscritaTarjeta();
                        tarjeta.CargarDatos(p.NombreEquipo ?? "", 
                            "Disciplina: " + p.Disciplina + " | Rama: " + p.Rama + " | Mod: " + p.Modalidad, 
                            "Participando");
                        flpDeportes.Controls.Add(tarjeta);
                    }
                }
                else
                {
                    MostrarMensajeVacioEnFlp(flpDeportes, "Deportes", "No participas en selecciones deportivas.");
                }

                // D. Panel de Cursos (pnCursos)
                var flpCursos = GetFlowLayoutPanel(pnCursos);
                if (tieneCursos && cursosActivos.Count > 0)
                {
                    foreach (var c in cursosActivos)
                    {
                        var tarjeta = new ActividadInscritaTarjeta();
                        tarjeta.CargarDatos(c.NombreCurso ?? "", 
                            "Instructor: " + (c.NombreInstructor ?? "N/A") + " | Nota: " + (c.Nota.HasValue ? c.Nota.Value.ToString("F1") : "Pendiente"), 
                            c.EstadoInscripcion);
                        flpCursos.Controls.Add(tarjeta);
                    }
                }
                else
                {
                    MostrarMensajeVacioEnFlp(flpCursos, "Cursos", "No estás inscrito en ningún curso extra-académico.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar estados de los paneles: " + ex.Message);
            }
        }

        private FlowLayoutPanel GetFlowLayoutPanel(Panel parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is FlowLayoutPanel flp)
                {
                    flp.Controls.Clear();
                    return flp;
                }
            }

            FlowLayoutPanel newFlp = new FlowLayoutPanel();
            newFlp.Dock = DockStyle.Fill;
            newFlp.FlowDirection = FlowDirection.TopDown;
            newFlp.WrapContents = false;
            newFlp.AutoScroll = true;
            newFlp.BackColor = Color.Transparent;

            parent.Controls.Clear();
            parent.Controls.Add(newFlp);
            return newFlp;
        }

        private void MostrarMensajeVacioEnFlp(FlowLayoutPanel flp, string categoria, string mensaje)
        {
            flp.Controls.Clear();
            
            Label lblMsg = new Label();
            lblMsg.Text = categoria + " - Sin Registros: " + mensaje;
            lblMsg.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblMsg.ForeColor = Color.Gray;
            lblMsg.Margin = new Padding(15, 15, 0, 0);
            lblMsg.AutoSize = true;

            flp.Controls.Add(lblMsg);
        }

        private void ActualizarVisualizacionPanel(Panel panel, Label label, string nombreBase, bool estaRegistrado, int cantidad)
        {
            if (estaRegistrado)
            {
                panel.BackColor = Color.FromArgb(222, 247, 236); // Verde éxito claro
                label.ForeColor = Color.FromArgb(3, 84, 63);      // Verde éxito oscuro para texto
                label.Font = new Font(label.Font.FontFamily, 7.5F, FontStyle.Bold);
                label.Text = nombreBase + " (" + cantidad + ")";
            }
            else
            {
                panel.BackColor = SystemColors.Control;
                label.ForeColor = SystemColors.ControlText;
                label.Font = new Font(label.Font.FontFamily, 7.5F, FontStyle.Regular);
                label.Text = nombreBase;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Finalizar aplicacion
            Application.Exit();
        }
    }
}