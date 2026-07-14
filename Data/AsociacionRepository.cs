using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ActividadesExtraPortal.Data
{
    public class AsociacionRepository
    {
        private readonly DatabaseConnection db = new DatabaseConnection();

        /// <summary>
        /// Obtiene todas las asociaciones estudiantiles desde la tabla Asociaciones.
        /// </summary>
        public List<Asociacion> ObtenerTodasLasAsociaciones()
        {
            List<Asociacion> lista = new List<Asociacion>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = "SELECT IdAsociacion, Acronimo, Nombre, AnioFundacion, Descripcion, Formulario, ImgUrl FROM Asociaciones";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new Asociacion
                                {
                                    IdAsociacion = Convert.ToInt32(r["IdAsociacion"]),
                                    Acronimo = r["Acronimo"].ToString() ?? "",
                                    Nombre = r["Nombre"].ToString() ?? "",
                                    AnioFundacion = r["AnioFundacion"] != DBNull.Value ? Convert.ToInt32(r["AnioFundacion"]) : null,
                                    Descripcion = r["Descripcion"]?.ToString(),
                                    Formulario = r["Formulario"]?.ToString(),
                                    ImgUrl = r["ImgUrl"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener asociaciones: {ex.Message}");
            }
            return lista;
        }

        /// <summary>
        /// Obtiene las membresías (solicitudes/activas) de un estudiante por su carnet desde la vista uv_DetalleMembresiasAsociaciones.
        /// </summary>
        public List<MembresiaAsociacion> ObtenerMembresiasEstudiante(string carnet)
        {
            List<MembresiaAsociacion> lista = new List<MembresiaAsociacion>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = @"
                        SELECT IdMembresia, Carnet, NombreEstudiante, Carrera, IdAsociacion, Acronimo, NombreAsociacion, Rol, FechaSolicitud, EstadoValidacion 
                        FROM uv_DetalleMembresiasAsociaciones 
                        WHERE Carnet = @Carnet";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new MembresiaAsociacion
                                {
                                    IdMembresia = Convert.ToInt32(r["IdMembresia"]),
                                    Carnet = r["Carnet"].ToString() ?? "",
                                    IdAsociacion = Convert.ToInt32(r["IdAsociacion"]),
                                    Rol = r["Rol"].ToString() ?? "Miembro",
                                    FechaSolicitud = Convert.ToDateTime(r["FechaSolicitud"]),
                                    EstadoValidacion = r["EstadoValidacion"].ToString() ?? "Pendiente",
                                    NombreEstudiante = r["NombreEstudiante"].ToString(),
                                    CarreraEstudiante = r["Carrera"].ToString(),
                                    NombreAsociacion = r["NombreAsociacion"].ToString(),
                                    AcronimoAsociacion = r["Acronimo"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener membresías de asociaciones: {ex.Message}");
            }
            return lista;
        }

        /// <summary>
        /// Afilia a un estudiante a una asociación invocando el procedimiento sp_InscribirMembresiaAsociacion.
        /// </summary>
        public string InscribirMembresiaAsociacion(string carnet, int idAsociacion, string rol = "Miembro")
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_InscribirMembresiaAsociacion", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdAsociacion", idAsociacion);
                        cmd.Parameters.AddWithValue("@Rol", rol);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Inscripción en asociación solicitada con éxito.";
                    }
                }
            }
            catch (SqlException ex)
            {
                return $"Error de negocio: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error de sistema: {ex.Message}";
            }
        }

        /// <summary>
        /// Cancela la membresía o solicitud de un estudiante a una asociación invocando el procedimiento sp_CancelarMembresiaAsociacion.
        /// </summary>
        public string CancelarMembresiaAsociacion(string carnet, int idAsociacion)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_CancelarMembresiaAsociacion", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdAsociacion", idAsociacion);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Membresía cancelada con éxito.";
                    }
                }
            }
            catch (SqlException ex)
            {
                return $"Error de negocio: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error de sistema: {ex.Message}";
            }
        }

        /// <summary>
        /// Crea una nueva asociación estudiantil invocando el procedimiento sp_CrearAsociacion.
        /// </summary>
        public bool CrearAsociacion(Asociacion aso)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_CrearAsociacion", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Acronimo", aso.Acronimo);
                        cmd.Parameters.AddWithValue("@Nombre", aso.Nombre);
                        cmd.Parameters.AddWithValue("@AnioFundacion", (object?)aso.AnioFundacion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Descripcion", (object?)aso.Descripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Formulario", (object?)aso.Formulario ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ImgUrl", (object?)aso.ImgUrl ?? DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear asociación: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza el estado de validación de la membresía de un estudiante.
        /// </summary>
        public bool ActualizarEstadoMembresia(string carnet, int idAsociacion, string nuevoEstado)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = "UPDATE MembresiaAsociaciones SET EstadoValidacion = @Estado WHERE Carnet = @Carnet AND IdAsociacion = @IdAsociacion";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdAsociacion", idAsociacion);
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estado de membresía: {ex.Message}");
                return false;
            }
        }
    }
}
