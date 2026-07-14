using System;

namespace ActividadesExtraPortal
{
    public class InscripcionDAC
    {
        public int IdInscripcionDAC { get; set; }
        public string Carnet { get; set; } = string.Empty;
        public int IdActividad { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string EstadoInscripcion { get; set; } = "Activa";

        // Propiedades auxiliares
        public string? NombreEstudiante { get; set; }
        public string? CarreraEstudiante { get; set; }
        public string? NombreActividad { get; set; }
        public string? NombreCategoria { get; set; }
        public string? NombreInstructor { get; set; }

        public InscripcionDAC() { }

        public InscripcionDAC(int idInscripcionDAC, string carnet, int idActividad, DateTime fechaInscripcion, string estadoInscripcion)
        {
            IdInscripcionDAC = idInscripcionDAC;
            Carnet = carnet;
            IdActividad = idActividad;
            FechaInscripcion = fechaInscripcion;
            EstadoInscripcion = estadoInscripcion;
        }
    }
}
