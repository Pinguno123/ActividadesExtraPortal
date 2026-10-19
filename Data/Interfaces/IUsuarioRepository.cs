using System.Collections.Generic;

namespace ActividadesExtraPortal.Data
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        void AsegurarRolesPorDefecto();
        bool RegistrarUsuario(string carnet, string nombre, string carrera, string campus, string correo, string rol = "Estudiante");
        bool ActualizarUsuario(string carnet, string nombre, string carrera, string campus, bool activo, string correo, string rol);
        string? ObtenerRolActual(string carnet);
        bool RegistrarOActualizarUsuario(Usuario usuario);
        List<Usuario> ObtenerTodosLosUsuarios();
        List<Usuario> ObtenerInstructores();
    }
}
