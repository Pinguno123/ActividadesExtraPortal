using System;

namespace ActividadesExtraPortal
{
    public class ParticipacionDeportiva : InscripcionBase
    {
        public int IdParticipacion
        {
            get => IdInscripcionGeneral;
            set => IdInscripcionGeneral = value;
        }

        public int IdSeleccion { get; set; }
        public string? Modalidad { get; set; }

        public DateTime FechaIngreso
        {
            get => FechaRegistro;
            set => FechaRegistro = value;
        }

        // Propiedades auxiliares
        public string? Disciplina { get; set; }
        public string? Rama { get; set; }
        public string? NombreEquipo { get; set; }

        public ParticipacionDeportiva()
        {
            EstadoGeneral = "Participando";
        }

        public ParticipacionDeportiva(int idParticipacion, string carnet, int idSeleccion, string? modalidad, DateTime fechaIngreso, string estado = "Participando")
            : base(idParticipacion, carnet, fechaIngreso, estado)
        {
            IdSeleccion = idSeleccion;
            Modalidad = modalidad;
        }

        public override string ObtenerTitulo() => NombreEquipo ?? Disciplina ?? "Selección Deportiva";

        public override string ObtenerDetalle()
        {
            return $"Disciplina: {Disciplina ?? "N/A"} | Rama: {Rama ?? "N/A"} | Mod: {Modalidad ?? "General"}";
        }
    }
}
