using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ActividadesExtraPortal.Data
{
    public class DeporteRepository
    {
        private readonly DatabaseConnection db = new DatabaseConnection();

        /// <summary>
        /// Obtiene todas las selecciones deportivas desde la tabla SeleccionesDeportivas.
        /// </summary>
        public List<SeleccionDeportiva> ObtenerTodasLasSelecciones()
        {
            List<SeleccionDeportiva> lista = new List<SeleccionDeportiva>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = "SELECT IdSeleccion, Disciplina, Rama, NombreEquipo, ImgUrl FROM SeleccionesDeportivas";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new SeleccionDeportiva
                                {
                                    IdSeleccion = Convert.ToInt32(r["IdSeleccion"]),
                                    Disciplina = r["Disciplina"].ToString() ?? "",
                                    Rama = r["Rama"].ToString() ?? "",
                                    NombreEquipo = r["NombreEquipo"].ToString() ?? "",
                                    ImgUrl = r["ImgUrl"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener selecciones deportivas: {ex.Message}");
            }
            return lista;
        }

        /// <summary>
        /// Obtiene las participaciones de un estudiante por su carnet desde la vista uv_DetalleParticipacionDeportiva.
        /// </summary>
        public List<ParticipacionDeportiva> ObtenerParticipacionesEstudiante(string carnet)
        {
            List<ParticipacionDeportiva> lista = new List<ParticipacionDeportiva>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = @"
                        SELECT IdParticipacion, Carnet, NombreEstudiante, Carrera, IdSeleccion, Disciplina, Rama, NombreEquipo, Modalidad, FechaIngreso 
                        FROM uv_DetalleParticipacionDeportiva 
                        WHERE Carnet = @Carnet";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new ParticipacionDeportiva
                                {
                                    IdParticipacion = Convert.ToInt32(r["IdParticipacion"]),
                                    Carnet = r["Carnet"].ToString() ?? "",
                                    IdSeleccion = Convert.ToInt32(r["IdSeleccion"]),
                                    Modalidad = r["Modalidad"]?.ToString(),
                                    FechaIngreso = Convert.ToDateTime(r["FechaIngreso"]),
                                    NombreEstudiante = r["NombreEstudiante"].ToString(),
                                    CarreraEstudiante = r["Carrera"].ToString(),
                                    Disciplina = r["Disciplina"].ToString(),
                                    Rama = r["Rama"].ToString(),
                                    NombreEquipo = r["NombreEquipo"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener participaciones deportivas: {ex.Message}");
            }
            return lista;
        }

        /// <summary>
        /// Registra la participación deportiva invocando el procedimiento sp_InscribirParticipacionDeportiva.
        /// </summary>
        public string InscribirParticipacionDeportiva(string carnet, int idSeleccion, string modalidad)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_InscribirParticipacionDeportiva", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdSeleccion", idSeleccion);
                        cmd.Parameters.AddWithValue("@Modalidad", modalidad);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Participación deportiva registrada con éxito.";
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
        /// Retira la participación deportiva invocando el procedimiento sp_CancelarParticipacionDeportiva.
        /// </summary>
        public string CancelarParticipacionDeportiva(string carnet, int idSeleccion)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_CancelarParticipacionDeportiva", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdSeleccion", idSeleccion);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Participación deportiva cancelada con éxito.";
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
        /// Registra una nueva selección deportiva invocando el procedimiento sp_CrearSeleccionDeportiva.
        /// </summary>
        public bool CrearSeleccionDeportiva(SeleccionDeportiva deporte)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_CrearSeleccionDeportiva", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Disciplina", deporte.Disciplina);
                        cmd.Parameters.AddWithValue("@Rama", deporte.Rama);
                        cmd.Parameters.AddWithValue("@NombreEquipo", deporte.NombreEquipo);
                        cmd.Parameters.AddWithValue("@ImgUrl", (object?)deporte.ImgUrl ?? DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear selección deportiva: {ex.Message}");
                throw;
            }
        }
    }
}
