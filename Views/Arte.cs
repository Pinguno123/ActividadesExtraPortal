using ActividadesExtraPortal.Data;
using ActividadesExtraPortal.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ActividadesExtraPortal
{
    public partial class Arte : PlantillaAACD
    {
        protected readonly IArteRepository arteRepo;

        public Arte(Portal? principal = null, IArteRepository? repo = null)
        {
            InitializeComponent();
            this.formPrincipal = principal!;
            this.arteRepo = repo ?? new ArteRepository();

            ConfigurarModulo();
        }

        protected override void ConfigurarModulo()
        {
            this.lblNombreOpcion.Text = "Departamento de Arte y Cultura";
            this.lblBanner.Text = "Inscríbete aquí y participa en las diversas actividades, cursos y programas que el Departamento de Arte y Cultura (DAC) tiene para ti.";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(138, 53, 252);
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
