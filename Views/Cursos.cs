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
    public partial class Cursos : Form
    {
        private Portal formPrincipal;
        public Cursos(Portal principal)
        {
            InitializeComponent();
            this.formPrincipal = principal;
        }

        private void Cursos_Load(object sender, EventArgs e)
        {

        }
        private void label1_Click_1(object sender, EventArgs e)
        {
            this.formPrincipal.Show();
            this.Close();
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
