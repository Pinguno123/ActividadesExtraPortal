using System.Collections.Generic;

namespace ActividadesExtraPortal.Data
{
    public interface IAsociacionRepository : IRepository<Asociacion>
    {
        List<Asociacion> ObtenerTodasLasAsociaciones();
        List<MembresiaAsociacion> ObtenerMembresiasEstudiante(string carnet);
        string InscribirMembresiaAsociacion(string carnet, int idAsociacion, string rol = "Miembro");
        string InscribirOActualizarMembresia(string carnet, int idAsociacion, string rol, string nuevoEstado);
        string CancelarMembresia(string carnet, int idAsociacion);
        string CancelarMembresiaAsociacion(string carnet, int idAsociacion);
        bool CrearAsociacion(Asociacion aso);
        bool ActualizarAsociacion(Asociacion aso);
        bool ActualizarEstadoMembresia(string carnet, int idAsociacion, string nuevoEstado);
    }
}
