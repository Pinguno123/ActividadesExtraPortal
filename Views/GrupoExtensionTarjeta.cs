using System;
using System.Drawing;
using System.Windows.Forms;

namespace ActividadesExtraPortal.Views
{
    /// <summary>
    /// User Control que representa una tarjeta interactiva para un Grupo de Extensión artística (DAC).
    /// Desacopla la lógica de renderizado y expone eventos para el botón de inscripción/retiro.
    /// </summary>
    public partial class GrupoExtensionTarjeta : UserControl
    {
        /// <summary>
        /// Evento desacoplado que notifica cuando el usuario interactúa con el botón de inscripción/retiro.
        /// </summary>
        public event EventHandler? BotonInscribirClick;

        private Label lblHorarioDetalle = null!;

        public GrupoExtensionTarjeta()
        {
            InitializeComponent();
            InicializarDetallesVisuales();
            button1.Click += (s, e) => BotonInscribirClick?.Invoke(this, e);
        }

        private void InicializarDetallesVisuales()
        {
            lblHorarioDetalle = new Label
            {
                Location = new Point(15, 45),
                Size = new Size(300, 120),
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(105, 81, 9),
                Text = "A coordinar con el instructor.",
                TextAlign = ContentAlignment.TopLeft
            };
            panel2.Controls.Add(lblHorarioDetalle);
        }

        /// <summary>
        /// Carga los datos de la actividad DAC en los controles visuales de la tarjeta.
        /// </summary>
        /// <param name="actividad">Entidad de la actividad artística.</param>
        /// <param name="estaInscrito">Indica si el estudiante en sesión ya está inscrito.</param>
        public void CargarDatos(ActividadDAC actividad, bool estaInscrito = false)
        {
            if (actividad == null) return;

            this.Tag = actividad;
            lblNombre.Text = actividad.NombreActividad;
            lblInstructor.Text = string.IsNullOrWhiteSpace(actividad.NombreInstructor) ? "Sin asignar" : actividad.NombreInstructor;
            lblDescripcion.Text = "Descripción:\n" + (string.IsNullOrWhiteSpace(actividad.Descripcion) ? "Sin descripción disponible." : actividad.Descripcion);

            if (estaInscrito)
            {
                button1.Text = "Retirarse";
                button1.BackColor = Color.FromArgb(220, 53, 69); // Rojo peligro / retirada
                button1.ForeColor = Color.White;
            }
            else
            {
                button1.Text = "Inscribir";
                button1.BackColor = Color.FromArgb(13, 110, 253); // Azul primario
                button1.ForeColor = Color.White;
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}
