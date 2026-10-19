using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ActividadesExtraPortal.Data
{
    public class ArteRepository : BaseRepository, IArteRepository
    {
        public List<ActividadDAC> ObtenerTodos() => ObtenerTodasLasActividades();

        /// <summary>
        /// Asegura que existan las 3 categorías canónicas de Arte y Cultura en la base de datos.
        /// </summary>
        public void AsegurarCategoriasBase()
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    string script = @"
                        IF NOT EXISTS (SELECT 1 FROM CategoriasDAC WHERE NombreCategoria LIKE '%Extensi%')
                            INSERT INTO CategoriasDAC (NombreCategoria, Descripcion) VALUES ('Grupos de Extensión', 'Espacio para el desarrollo de habilidades y talentos artísticos extracurriculares.');
                        IF NOT EXISTS (SELECT 1 FROM CategoriasDAC WHERE NombreCategoria LIKE '%Formaci%')
                            INSERT INTO CategoriasDAC (NombreCategoria, Descripcion) VALUES ('Programas de Formación', 'Cursos, talleres y diplomados en diferentes disciplinas artísticas.');
                        IF NOT EXISTS (SELECT 1 FROM CategoriasDAC WHERE NombreCategoria LIKE '%Voluntari%')
                            INSERT INTO CategoriasDAC (NombreCategoria, Descripcion) VALUES ('Voluntariado Cultural', 'Promoción y apoyo en eventos culturales y artísticos universitarios.');";

                    using (SqlCommand cmd = new SqlCommand(script, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al asegurar categorías base DAC: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene las actividades que corresponden a una categoría específica por coincidencia de nombre.
        /// </summary>
        public List<ActividadDAC> ObtenerActividadesPorCategoria(string nombreCategoria)
        {
            var todas = ObtenerTodasLasActividades();
            if (string.IsNullOrWhiteSpace(nombreCategoria)) return todas;

            return todas.FindAll(a =>
                !string.IsNullOrEmpty(a.NombreCategoria) &&
                a.NombreCategoria.IndexOf(nombreCategoria, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Obtiene todas las actividades artísticas disponibles (DAC) con sus categorías e instructores.
        /// </summary>
        public List<ActividadDAC> ObtenerTodasLasActividades()
        {
            List<ActividadDAC> lista = new List<ActividadDAC>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = @"
                        SELECT a.IdActividad, a.IdCategoria, a.NombreActividad, a.Descripcion, a.Estado, a.CarnetInstructor, 
                               c.NombreCategoria, u.NombreCompleto AS NombreInstructor
                        FROM ActividadesDAC a
                        JOIN CategoriasDAC c ON a.IdCategoria = c.IdCategoria
                        LEFT JOIN Usuarios u ON a.CarnetInstructor = u.Carnet";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new ActividadDAC
                                {
                                    IdActividad = Convert.ToInt32(r["IdActividad"]),
                                    IdCategoria = Convert.ToInt32(r["IdCategoria"]),
                                    NombreActividad = r["NombreActividad"].ToString() ?? "",
                                    Descripcion = r["Descripcion"]?.ToString(),
                                    Estado = r["Estado"].ToString() ?? "Activo",
                                    CarnetInstructor = r["CarnetInstructor"]?.ToString(),
                                    NombreCategoria = r["NombreCategoria"]?.ToString(),
                                    NombreInstructor = r["NombreInstructor"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener catálogo de actividades DAC: {ex.Message}");
            }
            return lista;
        }

        /// <summary>
        /// Obtiene las inscripciones a actividades DAC de un estudiante por su carnet desde la vista uv_DetalleInscripcionesDAC.
        /// </summary>
        public List<InscripcionDAC> ObtenerInscripcionesDACEstudiante(string carnet)
        {
            List<InscripcionDAC> lista = new List<InscripcionDAC>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = @"
                        SELECT IdInscripcionDAC, Carnet, NombreEstudiante, Carrera, IdActividad, NombreActividad, 
                               NombreCategoria, FechaInscripcion, EstadoInscripcion, NombreInstructor
                        FROM uv_DetalleInscripcionesDAC
                        WHERE Carnet = @Carnet";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new InscripcionDAC
                                {
                                    IdInscripcionDAC = Convert.ToInt32(r["IdInscripcionDAC"]),
                                    Carnet = r["Carnet"].ToString() ?? "",
                                    IdActividad = Convert.ToInt32(r["IdActividad"]),
                                    FechaInscripcion = Convert.ToDateTime(r["FechaInscripcion"]),
                                    EstadoInscripcion = r["EstadoInscripcion"].ToString() ?? "Activa",
                                    NombreEstudiante = r["NombreEstudiante"].ToString(),
                                    CarreraEstudiante = r["Carrera"].ToString(),
                                    NombreActividad = r["NombreActividad"].ToString(),
                                    NombreCategoria = r["NombreCategoria"].ToString(),
                                    NombreInstructor = r["NombreInstructor"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener inscripciones DAC: {ex.Message}");
            }
            return lista;
        }

        /// <summary>
        /// Inscribe a un estudiante en una actividad artística (DAC) llamando al procedimiento sp_InscribirActividadDAC.
        /// </summary>
        public string InscribirActividadDAC(string carnet, int idActividad)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_InscribirActividadDAC", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdActividad", idActividad);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Inscripción en actividad artística completada con éxito.";
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

        public List<InscripcionDAC> ObtenerInscripcionesEstudiante(string carnet) => ObtenerInscripcionesDACEstudiante(carnet);

        public string InscribirEstudianteDAC(string carnet, int idActividad) => InscribirActividadDAC(carnet, idActividad);

        /// <summary>
        /// Cancela la inscripción a una actividad artística (DAC) llamando al procedimiento sp_CancelarInscripcionDAC.
        /// </summary>
        public string CancelarInscripcionDAC(string carnet, int idActividad)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_CancelarInscripcionDAC", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdActividad", idActividad);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Inscripción en actividad artística cancelada con éxito.";
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
        /// Obtiene todas las categorías de actividades artísticas y culturales (DAC).
        /// </summary>
        public List<CategoriaDAC> ObtenerTodasLasCategorias()
        {
            List<CategoriaDAC> lista = new List<CategoriaDAC>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = "SELECT IdCategoria, NombreCategoria FROM CategoriasDAC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new CategoriaDAC
                                {
                                    IdCategoria = Convert.ToInt32(r["IdCategoria"]),
                                    NombreCategoria = r["NombreCategoria"].ToString() ?? ""
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categorías DAC: {ex.Message}");
            }
            return lista;
        }

        /// <summary>
        /// Crea una nueva actividad de arte y cultura (DAC) invocando el procedimiento sp_CrearActividadDAC.
        /// </summary>
        public bool CrearActividadDAC(ActividadDAC arte)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_CrearActividadDAC", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCategoria", arte.IdCategoria);
                        cmd.Parameters.AddWithValue("@NombreActividad", arte.NombreActividad);
                        cmd.Parameters.AddWithValue("@Descripcion", (object?)arte.Descripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CarnetInstructor", (object?)arte.CarnetInstructor ?? DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear actividad DAC: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza una actividad artística (DAC) existente en la tabla ActividadesDAC.
        /// </summary>
        public bool ActualizarActividadDAC(ActividadDAC arte)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = @"
                        UPDATE ActividadesDAC 
                        SET IdCategoria = @IdCategoria,
                            NombreActividad = @NombreActividad,
                            Descripcion = @Descripcion,
                            CarnetInstructor = @CarnetInstructor
                        WHERE IdActividad = @IdActividad";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdActividad", arte.IdActividad);
                        cmd.Parameters.AddWithValue("@IdCategoria", arte.IdCategoria);
                        cmd.Parameters.AddWithValue("@NombreActividad", arte.NombreActividad);
                        cmd.Parameters.AddWithValue("@Descripcion", (object?)arte.Descripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CarnetInstructor", (object?)arte.CarnetInstructor ?? DBNull.Value);

                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar actividad DAC: {ex.Message}");
                throw;
            }
        }
    }
}
