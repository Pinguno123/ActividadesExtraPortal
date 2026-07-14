using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ActividadesExtraPortal
{
    public partial class Deportes : Form
    {
        private Portal formPrincipal;
        public Deportes(Portal principal)
        {
            InitializeComponent();
            this.formPrincipal = principal;
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.formPrincipal.Show();
            this.Close();
        }

        private void Deportes_Load(object sender, EventArgs e)
        {

        }
        private void Forms_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !this.formPrincipal.Visible)
            {
                Application.Exit();
            }
        }
    }
}
