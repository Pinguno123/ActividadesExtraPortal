using ActividadesExtraPortal.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace ActividadesExtraPortal
{
    public partial class Cursos : PlantillaAACD
    {
        public Cursos(Portal principal)
        {
            InitializeComponent();

            this.lblNombreOpcion.Text = "Cursos";
            this.lblBanner.Text = "Impulsa tu potencial y sé parte de los cursos especializados en la Universidad Don Bosco.";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(13, 148, 136);

            this.formPrincipal = principal;
        }
    }
}
