using System.Collections.Generic;

namespace ActividadesExtraPortal.Data
{
    public interface IDeporteRepository : IRepository<SeleccionDeportiva>
    {
        List<SeleccionDeportiva> ObtenerTodasLasSelecciones();
        List<ParticipacionDeportiva> ObtenerParticipacionesEstudiante(string carnet);
        string InscribirParticipacionDeportiva(string carnet, int idSeleccion, string modalidad);
        string InscribirOActualizarParticipacion(string carnet, int idSeleccion, string? modalidad, string nuevoEstado);
        string CancelarParticipacionDeportiva(string carnet, int idSeleccion);
        bool CrearSeleccionDeportiva(SeleccionDeportiva deporte);
    }
}
