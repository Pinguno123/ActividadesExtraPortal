using System;

namespace ActividadesExtraPortal
{
    public class ActividadDAC : ActividadExtracurricularBase
    {
        public int IdActividad
        {
            get => Id;
            set => Id = value;
        }

        public string NombreActividad
        {
            get => Nombre;
            set => Nombre = value;
        }

        public int IdCategoria { get; set; }
        public string? CarnetInstructor { get; set; }

        // Propiedades auxiliares
        public string? NombreCategoria { get; set; }
        public string? NombreInstructor { get; set; }

        public ActividadDAC() { }

        public ActividadDAC(int idActividad, int idCategoria, string nombreActividad, string? descripcion, string estado, string? carnetInstructor)
            : base(idActividad, nombreActividad, descripcion, estado)
        {
            IdCategoria = idCategoria;
            CarnetInstructor = carnetInstructor;
        }

        public override string ObtenerTipoActividad() => "Arte y Cultura (DAC)";

        public override string ObtenerResumen()
        {
            return $"{NombreActividad} (Categoría: {NombreCategoria ?? "General"}) - Instructor: {NombreInstructor ?? "N/A"}";
        }
    }
}
