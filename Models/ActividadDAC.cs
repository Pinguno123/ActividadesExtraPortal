using System;

namespace ActividadesExtraPortal
{
    public class ActividadDAC
    {
        public int IdActividad { get; set; }
        public int IdCategoria { get; set; }
        public string NombreActividad { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = "Activo";
        public string? CarnetInstructor { get; set; }

        // Propiedades auxiliares
        public string? NombreCategoria { get; set; }
        public string? NombreInstructor { get; set; }

        public ActividadDAC() { }

        public ActividadDAC(int idActividad, int idCategoria, string nombreActividad, string? descripcion, string estado, string? carnetInstructor)
        {
            IdActividad = idActividad;
            IdCategoria = idCategoria;
            NombreActividad = nombreActividad;
            Descripcion = descripcion;
            Estado = estado;
            CarnetInstructor = carnetInstructor;
        }
    }
}
