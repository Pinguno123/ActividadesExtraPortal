using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ActividadesExtraPortal
{
    public partial class Arte : Form
    {
        private Portal? formPrincipal;
        public Arte(Portal? principal = null)
        {
            InitializeComponent();
            this.formPrincipal = principal;
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.formPrincipal?.Show();
            this.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !(this.formPrincipal?.Visible ?? false))
            {
                Application.Exit();
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
