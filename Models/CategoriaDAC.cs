using System;

namespace ActividadesExtraPortal
{
    public class CategoriaDAC
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public CategoriaDAC() { }

        public CategoriaDAC(int idCategoria, string nombreCategoria, string? descripcion)
        {
            IdCategoria = idCategoria;
            NombreCategoria = nombreCategoria;
            Descripcion = descripcion;
        }
    }
}
