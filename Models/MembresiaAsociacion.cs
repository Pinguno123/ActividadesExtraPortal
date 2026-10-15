using System;

namespace ActividadesExtraPortal
{
    public class MembresiaAsociacion : InscripcionBase
    {
        public int IdMembresia
        {
            get => IdInscripcionGeneral;
            set => IdInscripcionGeneral = value;
        }

        public int IdAsociacion { get; set; }
        public string Rol { get; set; } = "Miembro";

        public DateTime FechaSolicitud
        {
            get => FechaRegistro;
            set => FechaRegistro = value;
        }

        public string EstadoValidacion
        {
            get => EstadoGeneral;
            set => EstadoGeneral = value;
        }

        // Propiedades auxiliares
        public string? NombreAsociacion { get; set; }
        public string? AcronimoAsociacion { get; set; }

        public MembresiaAsociacion() { }

        public MembresiaAsociacion(int idMembresia, string carnet, int idAsociacion, string rol, DateTime fechaSolicitud, string estadoValidacion)
            : base(idMembresia, carnet, fechaSolicitud, estadoValidacion)
        {
            IdAsociacion = idAsociacion;
            Rol = rol;
        }

        public override string ObtenerTitulo() => NombreAsociacion ?? "Asociación Estudiantil";

        public override string ObtenerDetalle()
        {
            return $"Acrónimo: {AcronimoAsociacion ?? "N/A"} | Solicitado: {FechaSolicitud.ToShortDateString()}";
        }
    }
}
