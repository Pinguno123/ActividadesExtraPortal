using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ActividadesExtraPortal.Data;
using ActividadesExtraPortal.Views;
using ActividadesExtraPortal.Services;

namespace ActividadesExtraPortal
{
    public partial class Asociaciones : Form
    {
        private Portal formPrincipal;
        private readonly AsociacionRepository asoRepo = new AsociacionRepository();

        public Asociaciones(Portal principal)
        {
            InitializeComponent();
            this.formPrincipal = principal;
            this.Load += Asociaciones_Load;
        }

        private void Asociaciones_Load(object? sender, EventArgs e)
        {
            CargarAsociaciones();
        }

        private void CargarAsociaciones()
        {
            try
            {
                flpAsociacionTarjeta.Controls.Clear();

                // Obtener carnet del estudiante
                string? carnet = formPrincipal.UsuarioActual?.Id;

                // Obtener todas las asociaciones
                List<Asociacion> listaAsociaciones = asoRepo.ObtenerTodasLasAsociaciones();

                // Obtener membresías activas
                List<MembresiaAsociacion> membresias = new List<MembresiaAsociacion>();
                if (!string.IsNullOrEmpty(carnet))
                {
                    membresias = asoRepo.ObtenerMembresiasEstudiante(carnet);
                }

                foreach (var aso in listaAsociaciones)
                {
                    AsociacionTarjeta tarjeta = new AsociacionTarjeta();

                    // Verificar membresía
                    var membresia = membresias.FirstOrDefault(m => m.IdAsociacion == aso.IdAsociacion);
                    string? estado = membresia?.EstadoValidacion;

                    tarjeta.CargarDatos(aso, estado);
                    tarjeta.Click += Tarjeta_Click;

                    flpAsociacionTarjeta.Controls.Add(tarjeta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las asociaciones: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Tarjeta_Click(object? sender, EventArgs e)
        {
            if (sender is not AsociacionTarjeta tarjeta || tarjeta.Tag is not Asociacion aso)
                return;

            string? carnet = formPrincipal.UsuarioActual?.Id;
            if (string.IsNullOrEmpty(carnet))
            {
                MessageBox.Show("Debes iniciar sesión para inscribirte a una asociación.", "Sesión Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Actualizar estado de membresías
                var membresias = asoRepo.ObtenerMembresiasEstudiante(carnet);
                var membresia = membresias.FirstOrDefault(m => m.IdAsociacion == aso.IdAsociacion);

                if (membresia != null)
                {
                    string estado = membresia.EstadoValidacion;
                    if (estado == "Pendiente")
                    {
                        var result = MessageBox.Show(
                            $"Tu solicitud de membresía para la asociación '{aso.Nombre}' está PENDIENTE de aprobación.\n\n¿Deseas cancelar esta solicitud?",
                            "Solicitud Pendiente",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            string res = asoRepo.CancelarMembresiaAsociacion(carnet, aso.IdAsociacion);
                            MessageBox.Show(res, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarAsociaciones();
                        }
                    }
                    else if (estado == "Aprobada")
                    {
                        var result = MessageBox.Show(
                            $"Actualmente eres miembro ACTIVO de '{aso.Nombre}' ({aso.Acronimo}).\n\n¿Deseas darte de baja de esta asociación?",
                            "Miembro Activo",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            string res = asoRepo.CancelarMembresiaAsociacion(carnet, aso.IdAsociacion);
                            MessageBox.Show(res, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarAsociaciones();
                        }
                    }
                    else if (estado == "Rechazada")
                    {
                        var result = MessageBox.Show(
                            $"Tu solicitud previa para la asociación '{aso.Nombre}' fue RECHAZADA.\n\n¿Deseas solicitar la inscripción nuevamente?",
                            "Solicitud Rechazada",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            string res = asoRepo.InscribirMembresiaAsociacion(carnet, aso.IdAsociacion);
                            MessageBox.Show(res, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Abrir formulario si está disponible
                            if (!string.IsNullOrEmpty(aso.Formulario))
                            {
                                var openForm = MessageBox.Show(
                                    "Esta asociación requiere completar un formulario externo.\n¿Deseas abrir el formulario ahora?",
                                    "Formulario Requerido",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information);

                                if (openForm == DialogResult.Yes)
                                {
                                    string correo = formPrincipal.UsuarioActual?.CorreoElectronico ?? $"{carnet}@udb.edu.sv";
                                    var interactor = new InteractWithForm();
                                    await interactor.ProcesoAdmision(aso.Formulario, correo, carnet, aso.IdAsociacion);
                                }
                            }
                            CargarAsociaciones();
                        }
                    }
                }
                else
                {
                    // Sin membresía registrada
                    var result = MessageBox.Show(
                        $"¿Deseas solicitar la afiliación a la asociación '{aso.Nombre}' ({aso.Acronimo})?",
                        "Solicitar Afiliación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        string res = asoRepo.InscribirMembresiaAsociacion(carnet, aso.IdAsociacion);
                        MessageBox.Show(res, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Abrir formulario si está disponible
                        if (!string.IsNullOrEmpty(aso.Formulario))
                        {
                            var openForm = MessageBox.Show(
                                "Esta asociación requiere completar un formulario externo.\n¿Deseas abrir el formulario ahora?",
                                "Formulario Requerido",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (openForm == DialogResult.Yes)
                            {
                                string correo = formPrincipal.UsuarioActual?.CorreoElectronico ?? $"{carnet}@udb.edu.sv";
                                var interactor = new InteractWithForm();
                                await interactor.ProcesoAdmision(aso.Formulario, correo, carnet, aso.IdAsociacion);
                            }
                        }
                        CargarAsociaciones();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al procesar la acción: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
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
