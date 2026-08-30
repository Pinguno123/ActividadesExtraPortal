using System;
using System.Drawing;
using System.Windows.Forms;
using ActividadesExtraPortal.Services;

namespace ActividadesExtraPortal.Views
{
    public partial class DeportesTarjeta : UserControl
    {
        public event EventHandler? BotonInscribirClick;

        public DeportesTarjeta()
        {
            InitializeComponent();
            btnInscribir.Click += (s, e) => BotonInscribirClick?.Invoke(this, e);
        }

        public void CargarDatos(SeleccionDeportiva deporte, bool estaInscrito = false)
        {
            this.Tag = deporte;
            lblDeporteTitulo.Text = deporte.NombreEquipo;
            lblDeporte.Text = "Deporte: " + deporte.Disciplina;
            lblRama.Text = "Rama: " + deporte.Rama;

            if (estaInscrito)
            {
                btnInscribir.Text = "Retirarse";
                btnInscribir.BackColor = Color.FromArgb(220, 53, 69); // Rojo (danger)
            }
            else
            {
                btnInscribir.Text = "Inscribirse";
                btnInscribir.BackColor = Color.FromArgb(13, 110, 253); // Azul (primary)
            }

            // Uso del servicio unificado de imágenes con caché
            ImageCacheService.CargarImagenConCache(pcDeporte, deporte.ImgUrl, "DeportesCache");
        }
    }
}
