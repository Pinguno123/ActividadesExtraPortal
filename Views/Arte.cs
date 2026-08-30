using ActividadesExtraPortal.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ActividadesExtraPortal
{
    public partial class Arte : PlantillaAACD
    {
        public Arte(Portal? principal = null)
        {
            InitializeComponent();

            this.lblNombreOpcion.Text = "Departamento de Arte y Cultura";
            this.lblBanner.Text = "Inscríbete aquí y participa en las diversas actividades, cursos y programas que el Departamento de Arte y Cultura (DAC) tiene para ti.";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(138, 53, 252);

            this.formPrincipal = principal!;
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
