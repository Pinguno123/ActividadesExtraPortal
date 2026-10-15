using System;
using ActividadesExtraPortal;

namespace ActividadesExtraPortal.Views
{
    /// <summary>
    /// Fábrica (Patrón de diseño creacional Factory) para componentes visuales de interfaz gráfica.
    /// Encapsula la instanciación y enlace polimórfico de controles visuales a partir de entidades
    /// derivadas de InscripcionBase, desacoplando la capa de presentación.
    /// </summary>
    public static class TarjetaFactory
    {
        /// <summary>
        /// Crea e inicializa una tarjeta ActividadInscritaTarjeta a partir de cualquier
        /// especialización polimórfica de InscripcionBase (Principio LSP y OCP).
        /// </summary>
        public static ActividadInscritaTarjeta CrearTarjeta(InscripcionBase inscripcion)
        {
            if (inscripcion == null)
            {
                throw new ArgumentNullException(nameof(inscripcion), "La inscripción no puede ser nula.");
            }

            var tarjeta = new ActividadInscritaTarjeta();
            tarjeta.CargarDatos(inscripcion);
            return tarjeta;
        }

        /// <summary>
        /// Sobrecarga auxiliar para construir tarjetas con parámetros explícitos.
        /// </summary>
        public static ActividadInscritaTarjeta CrearTarjeta(string titulo, string detalle, string estado)
        {
            var tarjeta = new ActividadInscritaTarjeta();
            tarjeta.CargarDatos(titulo, detalle, estado);
            return tarjeta;
        }
    }
}
