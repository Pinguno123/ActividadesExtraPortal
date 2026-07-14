using System;

namespace ActividadesExtraPortal
{
    public class Usuario
    {
        public string Id { get; set; } = string.Empty; // ID de usuario
        public string Nombre { get; set; } = string.Empty; // Nombre completo
        public string Carrera { get; set; } = string.Empty;
        public string Campus { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty; // Estado de actividad
        public string RutaArchivo { get; set; } = string.Empty; // Ruta del archivo

        // Propiedades de base de datos
        public bool Activo { get; set; } = true;
        public string CorreoElectronico { get; set; } = string.Empty;
        public int IdRol { get; set; } = 1; // Rol de usuario

        public Usuario() { }

        // Constructor para scraping
        public Usuario(string id, string nombre, string carrera, string campus, string estado, string rutaArchivo)
        {
            Id = id;
            Nombre = nombre;
            Carrera = carrera;
            Campus = campus;
            Estado = estado;
            RutaArchivo = rutaArchivo;
            Activo = estado.ToLower().Contains("activo") || estado.ToLower().Contains("estudiante");
            CorreoElectronico = $"{id.Trim()}@udb.edu.sv";
            IdRol = 1; // Rol predeterminado
        }

        // Constructor para base de datos
        public Usuario(string id, string nombre, string carrera, string campus, bool activo, string correoElectronico, int idRol, string rutaArchivo = "")
        {
            Id = id;
            Nombre = nombre;
            Carrera = carrera;
            Campus = campus;
            Activo = activo;
            Estado = activo ? "Activo" : "Inactivo";
            CorreoElectronico = correoElectronico;
            IdRol = idRol;
            RutaArchivo = rutaArchivo;
        }
    }
}