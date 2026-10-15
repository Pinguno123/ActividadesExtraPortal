using System.Collections.Generic;

namespace ActividadesExtraPortal.Data
{
    public interface ICursoRepository : IRepository<Curso>
    {
        List<Curso> ObtenerTodosLosCursos();
        List<InscripcionCurso> ObtenerInscripcionesEstudiante(string carnet);
        string InscribirEstudiante(string carnet, int idCurso);
        string InscribirEstudianteEnCurso(string carnet, int idCurso);
        string CancelarInscripcion(string carnet, int idCurso);
        string CancelarInscripcionCurso(string carnet, int idCurso);
        bool ActualizarCurso(Curso cursoModificado);
        bool AgregarCurso(Curso cursoAgregado);
    }
}
