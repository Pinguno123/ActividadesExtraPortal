using ActividadesExtraPortal.Data;
using ActividadesExtraPortal.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ActividadesExtraPortal
{
    public partial class Cursos : PlantillaAACD
    {
        protected readonly ICursoRepository cursoRepo;

        public Cursos(Portal principal, ICursoRepository? repo = null)
        {
            InitializeComponent();
            this.formPrincipal = principal;
            this.cursoRepo = repo ?? new CursoRepository();

            ConfigurarModulo();
        }

        protected override void ConfigurarModulo()
        {
            this.lblNombreOpcion.Text = "Cursos";
            this.lblBanner.Text = "Impulsa tu potencial y sé parte de los cursos especializados en la Universidad Don Bosco.";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(13, 148, 136);
        }
    }
}
