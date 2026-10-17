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
        protected Form? formPadre = null;
        private bool navegandoRegreso = false;

        public PlantillaAACD()
        {
            InitializeComponent();
        }

        public PlantillaAACD(Portal principal, Form? padre = null) : this()
        {
            this.formPrincipal = principal;
            this.formPadre = padre;
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
            navegandoRegreso = true;
            if (this.formPadre != null && !this.formPadre.IsDisposed)
            {
                this.formPadre.Show();
                this.Close();
            }
            else if (this.formPrincipal != null && !this.formPrincipal.IsDisposed)
            {
                this.formPrincipal.Show();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void PlantillaAACD_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si el cierre fue provocado por navegación de regreso, evitar cerrar la aplicación
            if (navegandoRegreso) return;

            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (this.formPadre != null && !this.formPadre.IsDisposed)
                {
                    this.formPadre.Show();
                }
                else if (this.formPrincipal != null && !this.formPrincipal.IsDisposed && !this.formPrincipal.Visible)
                {
                    Application.Exit();
                }
            }
        }
    }
}
