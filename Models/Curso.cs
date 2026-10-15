using System;

namespace ActividadesExtraPortal
{
    public class Curso : ActividadExtracurricularBase
    {
        public int IdCurso
        {
            get => Id;
            set => Id = value;
        }

        public string NombreCurso
        {
            get => Nombre;
            set => Nombre = value;
        }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Horario { get; set; } = string.Empty;
        public int CupoMaximo { get; set; }
        public string? CarnetInstructor { get; set; }

        // Propiedades auxiliares
        public string? NombreInstructor { get; set; }
        public int? CuposOcupados { get; set; }
        public int? CuposDisponibles { get; set; }

        public Curso() { }

        public Curso(int idCurso, string nombreCurso, string? descripcion, DateTime fechaInicio, DateTime fechaFin, string horario, int cupoMaximo, string estado, string? carnetInstructor)
            : base(idCurso, nombreCurso, descripcion, estado)
        {
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Horario = horario;
            CupoMaximo = cupoMaximo;
            CarnetInstructor = carnetInstructor;
        }

        /// <summary>
        /// Implementación polimórfica del tipo de actividad.
        /// </summary>
        public override string ObtenerTipoActividad() => "Curso Académico";

        /// <summary>
        /// Sobrescritura polimórfica: Un curso está disponible si su estado es activo y aún quedan cupos.
        /// </summary>
        public override bool EstaDisponible()
        {
            bool estadoValido = base.EstaDisponible();
            bool hayCupos = !CuposDisponibles.HasValue || CuposDisponibles.Value > 0;
            return estadoValido && hayCupos;
        }

        /// <summary>
        /// Sobrescritura polimórfica para resumen detallado del curso.
        /// </summary>
        public override string ObtenerResumen()
        {
            return $"{NombreCurso} [{Horario}] - Instructor: {NombreInstructor ?? "N/A"} - Cupos Disp: {CuposDisponibles?.ToString() ?? "N/D"}";
        }
    }
}
