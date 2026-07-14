using System;

namespace ActividadesExtraPortal
{
    public class Curso
    {
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Horario { get; set; } = string.Empty;
        public int CupoMaximo { get; set; }
        public string Estado { get; set; } = "Activo";
        public string? CarnetInstructor { get; set; }

        // Propiedades auxiliares
        public string? NombreInstructor { get; set; }
        public int? CuposOcupados { get; set; }
        public int? CuposDisponibles { get; set; }

        public Curso() { }

        public Curso(int idCurso, string nombreCurso, string? descripcion, DateTime fechaInicio, DateTime fechaFin, string horario, int cupoMaximo, string estado, string? carnetInstructor)
        {
            IdCurso = idCurso;
            NombreCurso = nombreCurso;
            Descripcion = descripcion;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Horario = horario;
            CupoMaximo = cupoMaximo;
            Estado = estado;
            CarnetInstructor = carnetInstructor;
        }
    }
}
