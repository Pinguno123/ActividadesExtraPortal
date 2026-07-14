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

            // Suscribir al evento Shown
            this.Shown += Form1_Shown;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Finalizar aplicacion
            Application.Exit();
        }
    }
}