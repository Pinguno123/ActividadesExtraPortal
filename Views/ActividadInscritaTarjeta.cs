using System;
using System.Drawing;
using System.Windows.Forms;

namespace ActividadesExtraPortal.Views
{
    public class ActividadInscritaTarjeta : UserControl
    {
        private Label lblNombre;
        private Label lblDetalle;
        private Label lblEstado;

        public ActividadInscritaTarjeta()
        {
            this.Size = new Size(635, 80);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            lblNombre = new Label();
            lblNombre.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(0, 48, 135); // Azul UDB
            lblNombre.Location = new Point(15, 12);
            lblNombre.AutoSize = true;

            lblDetalle = new Label();
            lblDetalle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            lblDetalle.ForeColor = Color.DimGray;
            lblDetalle.Location = new Point(15, 42);
            lblDetalle.AutoSize = true;

            lblEstado = new Label();
            lblEstado.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblEstado.Padding = new Padding(6, 3, 6, 3);
            lblEstado.TextAlign = ContentAlignment.MiddleCenter;
            lblEstado.AutoSize = true;

            this.Controls.Add(lblNombre);
            this.Controls.Add(lblDetalle);
            this.Controls.Add(lblEstado);
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
            lblEstado.Location = new Point(this.Width - lblEstado.Width - 15, 25);
        }
    }
}
