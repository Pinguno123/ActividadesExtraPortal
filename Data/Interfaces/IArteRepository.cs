using System.Collections.Generic;

namespace ActividadesExtraPortal.Data
{
    public interface IArteRepository : IRepository<ActividadDAC>
    {
        List<ActividadDAC> ObtenerTodasLasActividades();
        List<ActividadDAC> ObtenerActividadesPorCategoria(string nombreCategoria);
        List<CategoriaDAC> ObtenerTodasLasCategorias();
        void AsegurarCategoriasBase();
        List<InscripcionDAC> ObtenerInscripcionesEstudiante(string carnet);
        List<InscripcionDAC> ObtenerInscripcionesDACEstudiante(string carnet);
        string InscribirEstudianteDAC(string carnet, int idActividad);
        string InscribirActividadDAC(string carnet, int idActividad);
        string CancelarInscripcionDAC(string carnet, int idActividad);
        bool CrearActividadDAC(ActividadDAC arte);
        bool ActualizarActividadDAC(ActividadDAC arte);
    }
}
