using System;
using System.Drawing;
using System.Windows.Forms;
using ActividadesExtraPortal.Services;

namespace ActividadesExtraPortal.Views
{
    public partial class AsociacionTarjeta : UserControl
    {
        public AsociacionTarjeta()
        {
            InitializeComponent();
            
            // Propagar evento click
            pictureBox1.Click += child_Click;
            lblNombre.Click += child_Click;
            lblFunda.Click += child_Click;
            lblDesc.Click += child_Click;
        }

        public void CargarDatos(Asociacion aso, string? estadoMembresia = null)
        {
            this.Tag = aso;
            lblNombre.Text = aso.Nombre;
            lblDesc.Text = aso.Descripcion;

            string fundacionTxt = aso.AnioFundacion.HasValue ? aso.AnioFundacion.Value.ToString() : "N/A";

            if (string.IsNullOrEmpty(estadoMembresia))
            {
                lblFunda.Text = "Fundación: " + fundacionTxt;
                this.BackColor = Color.FromArgb(248, 249, 250);
            }
            else
            {
                lblFunda.Text = "Fundación: " + fundacionTxt + " (" + estadoMembresia + ")";
                if (estadoMembresia == "Aprobada")
                {
                    this.BackColor = Color.FromArgb(220, 245, 230); // Color estado aprobado
                }
                else if (estadoMembresia == "Pendiente")
                {
                    this.BackColor = Color.FromArgb(255, 243, 205); // Color estado pendiente
                }
                else if (estadoMembresia == "Rechazada")
                {
                    this.BackColor = Color.FromArgb(248, 215, 218); // Color estado rechazado
                }
                else
                {
                    this.BackColor = Color.FromArgb(248, 249, 250);
                }
            }

            // Uso del servicio unificado de imágenes con caché
            ImageCacheService.CargarImagenConCache(pictureBox1, aso.ImgUrl, "AsociacionesCache");
        }

        private void child_Click(object? sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }
}
