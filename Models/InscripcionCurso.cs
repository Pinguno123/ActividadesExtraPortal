using System;

namespace ActividadesExtraPortal
{
    public class InscripcionCurso
    {
        public int IdInscripcion { get; set; }
        public string Carnet { get; set; } = string.Empty;
        public int IdCurso { get; set; }
        public decimal? Nota { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string EstadoInscripcion { get; set; } = "Confirmada";

        // Propiedades auxiliares
        public string? NombreEstudiante { get; set; }
        public string? CarreraEstudiante { get; set; }
        public string? CampusEstudiante { get; set; }
        public string? NombreCurso { get; set; }
        public string? NombreInstructor { get; set; }

        public InscripcionCurso() { }

        public InscripcionCurso(int idInscripcion, string carnet, int idCurso, decimal? nota, DateTime fechaInscripcion, string estadoInscripcion)
        {
            IdInscripcion = idInscripcion;
            Carnet = carnet;
            IdCurso = idCurso;
            Nota = nota;
            FechaInscripcion = fechaInscripcion;
            EstadoInscripcion = estadoInscripcion;
        }
    }
}
