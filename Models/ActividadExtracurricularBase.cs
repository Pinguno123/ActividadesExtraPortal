using System;

namespace ActividadesExtraPortal
{
    /// <summary>
    /// Clase base abstracta que modela cualquier actividad u oferta extracurricular
    /// en la institución (Cursos, Asociaciones, Deportes, Arte y Cultura).
    /// Aplica principios de Abstracción, Herencia y Polimorfismo.
    /// </summary>
    public abstract class ActividadExtracurricularBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = "Activo";

        protected ActividadExtracurricularBase() { }

        protected ActividadExtracurricularBase(int id, string nombre, string? descripcion, string estado = "Activo")
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Estado = estado;
        }

        /// <summary>
        /// Método abstracto que obliga a cada clase derivada a identificar el tipo específico de actividad.
        /// </summary>
        public abstract string ObtenerTipoActividad();

        /// <summary>
        /// Método virtual con implementación por defecto que genera un resumen legible.
        /// Puede ser sobreescrito (override) por clases derivadas para dar mayor detalle.
        /// </summary>
        public virtual string ObtenerResumen()
        {
            return $"{Nombre} [{ObtenerTipoActividad()}] - Estado: {Estado}";
        }

        /// <summary>
        /// Determina polimórficamente si la actividad se encuentra disponible para inscripción.
        /// </summary>
        public virtual bool EstaDisponible()
        {
            return string.Equals(Estado, "Activo", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(Estado, "Abierto", StringComparison.OrdinalIgnoreCase);
        }
    }
}
