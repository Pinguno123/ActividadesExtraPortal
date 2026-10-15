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

        /// <summary>
        /// Método virtual del ciclo de vida para configurar títulos, banners y estilos polimórficamente.
        /// Las vistas hijas (Deportes, Asociaciones, Arte, Cursos) lo sobreescriben (override).
        /// </summary>
        protected virtual void ConfigurarModulo() { }

        /// <summary>
        /// Método virtual del ciclo de vida para cargar catálogos y datos específicos del módulo polimórficamente.
        /// </summary>
        protected virtual void CargarDatos() { }

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
