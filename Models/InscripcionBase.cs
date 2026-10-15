using System;

namespace ActividadesExtraPortal
{
    /// <summary>
    /// Clase base abstracta que unifica cualquier registro, afiliación o inscripción estudiantil
    /// en los diferentes módulos del portal. Permite el tratamiento polimórfico en vistas y reportes.
    /// </summary>
    public abstract class InscripcionBase
    {
        public int IdInscripcionGeneral { get; set; }
        public string Carnet { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string EstadoGeneral { get; set; } = "Activa";

        // Propiedades auxiliares de perfil estudiantil
        public string? NombreEstudiante { get; set; }
        public string? CarreraEstudiante { get; set; }

        protected InscripcionBase() { }

        protected InscripcionBase(int idInscripcion, string carnet, DateTime fechaRegistro, string estadoGeneral)
        {
            IdInscripcionGeneral = idInscripcion;
            Carnet = carnet;
            FechaRegistro = fechaRegistro;
            EstadoGeneral = estadoGeneral;
        }

        /// <summary>
        /// Método abstracto que retorna el título legible de la inscripción para la UI.
        /// </summary>
        public abstract string ObtenerTitulo();

        /// <summary>
        /// Método abstracto que retorna la línea descriptiva detallada formateada para la tarjeta UI.
        /// </summary>
        public abstract string ObtenerDetalle();

        /// <summary>
        /// Método virtual que evalúa si la inscripción se encuentra en un estado activo o confirmado.
        /// </summary>
        public virtual bool EsActiva()
        {
            return string.Equals(EstadoGeneral, "Aprobada", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(EstadoGeneral, "Confirmada", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(EstadoGeneral, "Activa", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(EstadoGeneral, "Participando", StringComparison.OrdinalIgnoreCase);
        }
    }
}
