using System;

namespace ActividadesExtraPortal
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public Rol() { }

        public Rol(int idRol, string nombreRol, string? descripcion)
        {
            IdRol = idRol;
            NombreRol = nombreRol;
            Descripcion = descripcion;
        }
    }
}
