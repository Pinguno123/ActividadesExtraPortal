using ActividadesExtraPortal.Views;
using ActividadesExtraPortal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace ActividadesExtraPortal
{
    public partial class Deportes : PlantillaAACD
    {
        private readonly DeporteRepository deporteRepo = new DeporteRepository();

        public Deportes(Portal principal)
        {
            InitializeComponent();

            this.lblNombreOpcion.Text = "Deportes";
            this.lblBanner.Text = "Forma parte de las actividades deportivas que te ofrece la UDB";
            this.lblBanner.ForeColor = Color.White;
            this.pnBanner.BackColor = Color.FromArgb(26, 86, 219);

            this.formPrincipal = principal;

            // Vincular evento del botón "Mis Participaciones"
            button1.Click += (s, e) => MostrarParticipaciones();
        }

        private void Deportes_Load(object sender, EventArgs e)
        {
            CargarDeportes();
        }

        private void CargarDeportes()
        {
            try
            {
                flpDeportesTarjeta.Controls.Clear();

                string? carnet = formPrincipal.UsuarioActual?.Id;
                if (string.IsNullOrEmpty(carnet))
                {
                    MessageBox.Show("Error al obtener la sesión del estudiante actual.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Obtener selecciones de la base de datos
                List<SeleccionDeportiva> selecciones = deporteRepo.ObtenerTodasLasSelecciones();

                // Obtener participaciones del estudiante
                List<ParticipacionDeportiva> participaciones = deporteRepo.ObtenerParticipacionesEstudiante(carnet);

                foreach (var deporte in selecciones)
                {
                    DeportesTarjeta tarjeta = new DeportesTarjeta();

                    // Comprobar si el estudiante ya participa en esta selección
                    bool estaInscrito = participaciones.Any(p => p.IdSeleccion == deporte.IdSeleccion);

                    tarjeta.CargarDatos(deporte, estaInscrito);
                    tarjeta.BotonInscribirClick += Tarjeta_BotonInscribirClick;

                    flpDeportesTarjeta.Controls.Add(tarjeta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los deportes: " + ex.Message, "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Tarjeta_BotonInscribirClick(object? sender, EventArgs e)
        {
            if (sender is not DeportesTarjeta tarjeta || tarjeta.Tag is not SeleccionDeportiva deporte)
                return;

            string? carnet = formPrincipal.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes iniciar sesión para inscribirte a una selección deportiva.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Consultar si está inscrito actualmente
                var participaciones = deporteRepo.ObtenerParticipacionesEstudiante(carnet);
                bool estaInscrito = participaciones.Any(p => p.IdSeleccion == deporte.IdSeleccion);

                if (estaInscrito)
                {
                    var confirm = MessageBox.Show(
                        "¿Deseas retirarte de la selección de '" + deporte.NombreEquipo + "'?",
                        "Confirmar Retiro",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        string resultado = deporteRepo.CancelarParticipacionDeportiva(carnet, deporte.IdSeleccion);
                        MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDeportes();
                    }
                }
                else
                {
                    // Solicitar modalidad
                    using (Form prompt = new Form())
                    {
                        prompt.Width = 320;
                        prompt.Height = 180;
                        prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                        prompt.Text = "Modalidad de Participación";
                        prompt.StartPosition = FormStartPosition.CenterParent;
                        prompt.MaximizeBox = false;
                        prompt.MinimizeBox = false;

                        Label textLabel = new Label() { Left = 20, Top = 15, Text = "Seleccione la modalidad de participación:", Width = 260 };
                        ComboBox comboBox = new ComboBox() { Left = 20, Top = 45, Width = 260 };
                        comboBox.Items.AddRange(new object[] { "Recreativo", "Competitivo", "Entrenamiento" });
                        comboBox.SelectedIndex = 0;
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                        Button confirmation = new Button() { Text = "Aceptar", Left = 180, Width = 100, Top = 90, DialogResult = DialogResult.OK };
                        confirmation.Click += (senderPrompt, ePrompt) => { prompt.Close(); };
                        
                        prompt.Controls.Add(comboBox);
                        prompt.Controls.Add(textLabel);
                        prompt.Controls.Add(confirmation);
                        prompt.AcceptButton = confirmation;

                        if (prompt.ShowDialog() == DialogResult.OK)
                        {
                            string modalidad = comboBox.SelectedItem?.ToString() ?? "Entrenamiento";
                            string resultado = deporteRepo.InscribirParticipacionDeportiva(carnet, deporte.IdSeleccion, modalidad);
                            MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarDeportes();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al procesar la acción: " + ex.Message, "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarParticipaciones()
        {
            string? carnet = formPrincipal.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet)) return;

            try
            {
                var participaciones = deporteRepo.ObtenerParticipacionesEstudiante(carnet);
                if (participaciones == null || participaciones.Count == 0)
                {
                    MessageBox.Show("Actualmente no estás inscrito en ninguna selección deportiva.", "Mis Participaciones", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Estás inscrito en las siguientes selecciones deportivas:\n");
                foreach (var p in participaciones)
                {
                    sb.AppendLine("• " + p.NombreEquipo + " (" + p.Disciplina + ") - Rama: " + p.Rama);
                    sb.AppendLine("  Modalidad: " + p.Modalidad + " | Fecha Ingreso: " + p.FechaIngreso.ToShortDateString());
                    sb.AppendLine();
                }

                MessageBox.Show(sb.ToString(), "Mis Participaciones", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar participaciones: " + ex.Message, "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
