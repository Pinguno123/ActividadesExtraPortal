using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ActividadesExtraPortal.Views
{
    public partial class PlantillaAACD : Form
    {
        protected Portal formPrincipal = null!;

        public PlantillaAACD()
        {
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.formPrincipal.Show();
            this.Close();
        }

        private void PlantillaAACD_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !this.formPrincipal.Visible)
            {
                Application.Exit();
            }
        }
    }
}
