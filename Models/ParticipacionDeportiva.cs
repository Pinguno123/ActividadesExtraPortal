using System;

namespace ActividadesExtraPortal
{
    public class ParticipacionDeportiva
    {
        public int IdParticipacion { get; set; }
        public string Carnet { get; set; } = string.Empty;
        public int IdSeleccion { get; set; }
        public string? Modalidad { get; set; }
        public DateTime FechaIngreso { get; set; }

        // Propiedades auxiliares
        public string? NombreEstudiante { get; set; }
        public string? CarreraEstudiante { get; set; }
        public string? Disciplina { get; set; }
        public string? Rama { get; set; }
        public string? NombreEquipo { get; set; }

        public ParticipacionDeportiva() { }

        public ParticipacionDeportiva(int idParticipacion, string carnet, int idSeleccion, string? modalidad, DateTime fechaIngreso)
        {
            IdParticipacion = idParticipacion;
            Carnet = carnet;
            IdSeleccion = idSeleccion;
            Modalidad = modalidad;
            FechaIngreso = fechaIngreso;
        }
    }
}
