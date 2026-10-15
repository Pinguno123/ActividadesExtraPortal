using System;

namespace ActividadesExtraPortal
{
    public class InscripcionDAC : InscripcionBase
    {
        public int IdInscripcionDAC
        {
            get => IdInscripcionGeneral;
            set => IdInscripcionGeneral = value;
        }

        public int IdActividad { get; set; }

        public DateTime FechaInscripcion
        {
            get => FechaRegistro;
            set => FechaRegistro = value;
        }

        public string EstadoInscripcion
        {
            get => EstadoGeneral;
            set => EstadoGeneral = value;
        }

        // Propiedades auxiliares
        public string? NombreActividad { get; set; }
        public string? NombreCategoria { get; set; }
        public string? NombreInstructor { get; set; }

        public InscripcionDAC() { }

        public InscripcionDAC(int idInscripcionDAC, string carnet, int idActividad, DateTime fechaInscripcion, string estadoInscripcion)
            : base(idInscripcionDAC, carnet, fechaInscripcion, estadoInscripcion)
        {
            IdActividad = idActividad;
        }

        public override string ObtenerTitulo() => NombreActividad ?? "Actividad DAC";

        public override string ObtenerDetalle()
        {
            return $"Categoría: {NombreCategoria ?? "N/A"} | Instructor: {NombreInstructor ?? "N/A"}";
        }
    }
}
