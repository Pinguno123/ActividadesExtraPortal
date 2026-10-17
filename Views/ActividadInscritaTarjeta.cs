using System;
using System.Drawing;
using System.Windows.Forms;
using ActividadesExtraPortal;

namespace ActividadesExtraPortal.Views
{
    public class ActividadInscritaTarjeta : UserControl
    {
        private Label lblNombre;
        private Label lblDetalle;
        private Label lblEstado;

        public ActividadInscritaTarjeta()
        {
            this.Size = new Size(600, 75);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            lblNombre = new Label();
            lblNombre.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(0, 48, 135); // Azul UDB
            lblNombre.Location = new Point(14, 10);
            lblNombre.AutoSize = true;

            lblDetalle = new Label();
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblDetalle.ForeColor = Color.DimGray;
            lblDetalle.Location = new Point(14, 38);
            lblDetalle.AutoSize = true;

            lblEstado = new Label();
            lblEstado.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblEstado.Padding = new Padding(6, 3, 6, 3);
            lblEstado.TextAlign = ContentAlignment.MiddleCenter;
            lblEstado.AutoSize = true;

            this.Controls.Add(lblNombre);
            this.Controls.Add(lblDetalle);
            this.Controls.Add(lblEstado);

            // Reenviar eventos de rueda del mouse para permitir desplazamiento fluido
            this.MouseWheel += ReenviarMouseWheel;
            lblNombre.MouseWheel += ReenviarMouseWheel;
            lblDetalle.MouseWheel += ReenviarMouseWheel;
            lblEstado.MouseWheel += ReenviarMouseWheel;

            this.MouseEnter += AsegurarFocoPadre;
            lblNombre.MouseEnter += AsegurarFocoPadre;
            lblDetalle.MouseEnter += AsegurarFocoPadre;
            lblEstado.MouseEnter += AsegurarFocoPadre;

            this.Resize += (s, e) =>
            {
                if (lblEstado != null)
                {
                    lblEstado.Location = new Point(this.Width - lblEstado.Width - 15, (this.Height - lblEstado.Height) / 2);
                }
            };
        }

        private void AsegurarFocoPadre(object? sender, EventArgs e)
        {
            if (this.Parent is FlowLayoutPanel flp && !flp.Focused)
            {
                flp.Focus();
            }
        }

        private void ReenviarMouseWheel(object? sender, MouseEventArgs e)
        {
            if (this.Parent is FlowLayoutPanel flp)
            {
                int nuevoY = -flp.AutoScrollPosition.Y - (e.Delta / 2);
                if (nuevoY < 0) nuevoY = 0;
                flp.AutoScrollPosition = new Point(0, nuevoY);
            }
        }

        /// <summary>
        /// Carga los datos de visualización aprovechando el polimorfismo de InscripcionBase (Principio LSP).
        /// </summary>
        public void CargarDatos(InscripcionBase inscripcion)
        {
            if (inscripcion == null) return;
            CargarDatos(inscripcion.ObtenerTitulo(), inscripcion.ObtenerDetalle(), inscripcion.EstadoGeneral);
        }

        public void CargarDatos(string nombre, string detalle, string estado)
        {
            lblNombre.Text = nombre;
            lblDetalle.Text = detalle;
            lblEstado.Text = estado;

            // Colores del badge según el estado
            if (estado == "Aprobada" || estado == "Confirmada" || estado == "Activa" || estado == "Participando")
            {
                lblEstado.ForeColor = Color.White;
                lblEstado.BackColor = Color.FromArgb(40, 167, 69); // Verde
            }
            else if (estado == "Pendiente" || estado == "Pre-inscrito")
            {
                lblEstado.ForeColor = Color.Black;
                lblEstado.BackColor = Color.FromArgb(255, 193, 7); // Amarillo
            }
            else if (estado == "Rechazada")
            {
                lblEstado.ForeColor = Color.White;
                lblEstado.BackColor = Color.FromArgb(220, 53, 69); // Rojo
            }
            else
            {
                lblEstado.ForeColor = Color.White;
                lblEstado.BackColor = Color.FromArgb(108, 117, 125); // Gris
            }

            this.PerformLayout();
            lblEstado.Location = new Point(this.Width - lblEstado.Width - 15, (this.Height - lblEstado.Height) / 2);
        }
    }
}
