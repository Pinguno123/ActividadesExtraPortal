using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ActividadesExtraPortal.Data
{
    public class UsuarioRepository
    {
        private readonly DatabaseConnection db = new DatabaseConnection();

        // Asegurar roles por defecto
        public void AsegurarRolesPorDefecto()
        {
            string[] roles = { "Estudiante", "Instructor", "Administrador" };
            foreach (var rol in roles)
            {
                try
                {
                    using (var conn = db.GetConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand("dbo.sp_CrearRol", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@NombreRol", rol);
                            cmd.Parameters.AddWithValue("@Descripcion", $"Rol de {rol} para el sistema");
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number != 50007 && ex.Number != 2627)
                    {
                        Console.WriteLine($"Error de base de datos al asegurar rol {rol}: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error general al asegurar rol {rol}: {ex.Message}");
                }
            }
        }

        // Registrar usuario
        public bool RegistrarUsuario(string carnet, string nombre, string carrera, string campus, string correo, string rol = "Estudiante")
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_RegistrarUsuario", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@NombreCompleto", nombre);
                        cmd.Parameters.AddWithValue("@Carrera", carrera);
                        cmd.Parameters.AddWithValue("@Campus", campus);
                        cmd.Parameters.AddWithValue("@CorreoElectronico", correo);
                        cmd.Parameters.AddWithValue("@NombreRol", rol);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50006 || ex.Number == 2627)
                {
                    Console.WriteLine("El usuario ya se encuentra registrado.");
                }
                else
                {
                    Console.WriteLine($"Error SQL al registrar usuario: {ex.Message}");
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                return false;
            }
        }
        // Actualizar usuario
        public bool ActualizarUsuario(string carnet, string nombre, string carrera, string campus, bool activo, string correo, string rol = "Estudiante")
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_ActualizarUsuario", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@NombreCompleto", nombre);
                        cmd.Parameters.AddWithValue("@Carrera", carrera);
                        cmd.Parameters.AddWithValue("@Campus", campus);
                        cmd.Parameters.AddWithValue("@Activo", activo);
                        cmd.Parameters.AddWithValue("@CorreoElectronico", correo);
                        cmd.Parameters.AddWithValue("@NombreRol", rol);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene el nombre del rol actual que tiene asignado el usuario en la base de datos.
        /// </summary>
        public string? ObtenerRolActual(string carnet)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = @"
                        SELECT r.NombreRol 
                        FROM Usuarios u 
                        JOIN Roles r ON u.IdRol = r.IdRol 
                        WHERE u.Carnet = @Carnet";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        conn.Open();
                        object? result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el rol del usuario {carnet}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Coordina el registro o actualización automática del estudiante preservando roles previos.
        /// </summary>
        public bool RegistrarOActualizarUsuario(Usuario usuario)
        {
            AsegurarRolesPorDefecto();

            // Verificar existencia y rol del usuario
            string? rolExistente = ObtenerRolActual(usuario.Id);

            if (rolExistente != null)
            {
                // Actualizar conservando el rol
                Console.WriteLine($"Usuario {usuario.Id} ya existe en BD con el rol '{rolExistente}'. Sincronizando datos del portal...");
                return ActualizarUsuario(
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Carrera,
                    usuario.Campus,
                    usuario.Activo,
                    usuario.CorreoElectronico,
                    rolExistente
                );
            }
            else
            {
                // Registrar nuevo usuario
                string rolDB = usuario.IdRol == 2 ? "Instructor" : "Estudiante";
                Console.WriteLine($"Registrando nuevo usuario {usuario.Id} con el rol '{rolDB}'...");
                return RegistrarUsuario(
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Carrera,
                    usuario.Campus,
                    usuario.CorreoElectronico,
                    rolDB
                );
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema.
        /// </summary>
        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = "SELECT Carnet, NombreCompleto, Carrera, Campus, Activo, CorreoElectronico, IdRol FROM Usuarios";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new Usuario
                                {
                                    Id = r["Carnet"].ToString() ?? "",
                                    Nombre = r["NombreCompleto"].ToString() ?? "",
                                    Carrera = r["Carrera"]?.ToString() ?? "",
                                    Campus = r["Campus"]?.ToString() ?? "",
                                    Activo = Convert.ToBoolean(r["Activo"]),
                                    CorreoElectronico = r["CorreoElectronico"]?.ToString() ?? "",
                                    IdRol = Convert.ToInt32(r["IdRol"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
            }
            return lista;
        }
    }
}
