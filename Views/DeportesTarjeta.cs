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

            // Asegurar foco y reenvío de la rueda del mouse para un desplazamiento fluido
            this.MouseEnter += AsegurarFocoPadre;
            pcDeporte.MouseEnter += AsegurarFocoPadre;
            lblDeporteTitulo.MouseEnter += AsegurarFocoPadre;
            lblRama.MouseEnter += AsegurarFocoPadre;
            lblDeporte.MouseEnter += AsegurarFocoPadre;

            this.MouseWheel += ReenviarMouseWheel;
            pcDeporte.MouseWheel += ReenviarMouseWheel;
            lblDeporteTitulo.MouseWheel += ReenviarMouseWheel;
            lblRama.MouseWheel += ReenviarMouseWheel;
            lblDeporte.MouseWheel += ReenviarMouseWheel;
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
