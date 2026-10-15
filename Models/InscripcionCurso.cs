using System;

namespace ActividadesExtraPortal
{
    public class InscripcionCurso : InscripcionBase
    {
        public int IdInscripcion
        {
            get => IdInscripcionGeneral;
            set => IdInscripcionGeneral = value;
        }

        public int IdCurso { get; set; }
        public decimal? Nota { get; set; }

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
        public string? CampusEstudiante { get; set; }
        public string? NombreCurso { get; set; }
        public string? NombreInstructor { get; set; }

        public InscripcionCurso() { }

        public InscripcionCurso(int idInscripcion, string carnet, int idCurso, decimal? nota, DateTime fechaInscripcion, string estadoInscripcion)
            : base(idInscripcion, carnet, fechaInscripcion, estadoInscripcion)
        {
            IdCurso = idCurso;
            Nota = nota;
        }

        public override string ObtenerTitulo() => NombreCurso ?? "Curso Universitario";

        public override string ObtenerDetalle()
        {
            string notaTxt = Nota.HasValue ? Nota.Value.ToString("F1") : "Pendiente";
            return $"Instructor: {NombreInstructor ?? "N/A"} | Nota: {notaTxt}";
        }
    }
}
