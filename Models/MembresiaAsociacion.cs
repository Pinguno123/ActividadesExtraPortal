using System;

namespace ActividadesExtraPortal
{
    public class MembresiaAsociacion
    {
        public int IdMembresia { get; set; }
        public string Carnet { get; set; } = string.Empty;
        public int IdAsociacion { get; set; }
        public string Rol { get; set; } = "Miembro";
        public DateTime FechaSolicitud { get; set; }
        public string EstadoValidacion { get; set; } = "Pendiente";

        // Propiedades auxiliares
        public string? NombreEstudiante { get; set; }
        public string? CarreraEstudiante { get; set; }
        public string? NombreAsociacion { get; set; }
        public string? AcronimoAsociacion { get; set; }

        public MembresiaAsociacion() { }

        public MembresiaAsociacion(int idMembresia, string carnet, int idAsociacion, string rol, DateTime fechaSolicitud, string estadoValidacion)
        {
            IdMembresia = idMembresia;
            Carnet = carnet;
            IdAsociacion = idAsociacion;
            Rol = rol;
            FechaSolicitud = fechaSolicitud;
            EstadoValidacion = estadoValidacion;
        }
    }
}
