using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ActividadesExtraPortal.Data
{
    public class CursoRepository
    {
        private readonly DatabaseConnection db = new DatabaseConnection();
        
        // Obtener catalogo de cursos
        
        public List<Curso> ObtenerTodosLosCursos()
        {
            List<Curso> lista = new List<Curso>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = @"
                        SELECT IdCurso, NombreCurso, Descripcion, FechaInicio, FechaFin, Horario, CupoMaximo, Estado, NombreInstructor, CuposOcupados, CuposDisponibles 
                        FROM uv_ListarCursosConCupos";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new Curso
                                {
                                    IdCurso = Convert.ToInt32(r["IdCurso"]),
                                    NombreCurso = r["NombreCurso"].ToString() ?? "",
                                    Descripcion = r["Descripcion"]?.ToString(),
                                    FechaInicio = Convert.ToDateTime(r["FechaInicio"]),
                                    FechaFin = Convert.ToDateTime(r["FechaFin"]),
                                    Horario = r["Horario"].ToString() ?? "",
                                    CupoMaximo = Convert.ToInt32(r["CupoMaximo"]),
                                    Estado = r["Estado"].ToString() ?? "Activo",
                                    NombreInstructor = r["NombreInstructor"]?.ToString(),
                                    CuposOcupados = r["CuposOcupados"] != DBNull.Value ? Convert.ToInt32(r["CuposOcupados"]) : 0,
                                    CuposDisponibles = r["CuposDisponibles"] != DBNull.Value ? Convert.ToInt32(r["CuposDisponibles"]) : 0
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener catálogo de cursos: {ex.Message}");
            }
            return lista;
        }
        
        // Obtener inscripciones de estudiante
        
        public List<InscripcionCurso> ObtenerInscripcionesEstudiante(string carnet)
        {
            List<InscripcionCurso> lista = new List<InscripcionCurso>();
            try
            {
                using (var conn = db.GetConnection())
                {
                    string query = @"
                        SELECT IdInscripcion, Carnet, NombreEstudiante, Carrera, Campus, IdCurso, NombreCurso, FechaInicio, FechaFin, Horario, CupoMaximo, Nota, FechaInscripcion, EstadoInscripcion, NombreInstructor 
                        FROM uv_DetalleInscripcionesCursos 
                        WHERE Carnet = @Carnet";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                lista.Add(new InscripcionCurso
                                {
                                    IdInscripcion = Convert.ToInt32(r["IdInscripcion"]),
                                    Carnet = r["Carnet"].ToString() ?? "",
                                    IdCurso = Convert.ToInt32(r["IdCurso"]),
                                    Nota = r["Nota"] != DBNull.Value ? Convert.ToDecimal(r["Nota"]) : null,
                                    FechaInscripcion = Convert.ToDateTime(r["FechaInscripcion"]),
                                    EstadoInscripcion = r["EstadoInscripcion"].ToString() ?? "Confirmada",
                                    NombreEstudiante = r["NombreEstudiante"].ToString(),
                                    CarreraEstudiante = r["Carrera"].ToString(),
                                    CampusEstudiante = r["Campus"].ToString(),
                                    NombreCurso = r["NombreCurso"].ToString(),
                                    NombreInstructor = r["NombreInstructor"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener inscripciones de cursos: {ex.Message}");
            }
            return lista;
        }

        // Inscribir estudiante en curso
        public string InscribirEstudianteEnCurso(string carnet, int idCurso)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_InscribirEstudianteCurso", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdCurso", idCurso);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Inscripción procesada con éxito.";
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

        // Cancelar inscripcion de curso
        public string CancelarInscripcionCurso(string carnet, int idCurso)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_CancelarInscripcionCurso", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@IdCurso", idCurso);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Inscripción cancelada con éxito.";
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

        // Actualizar informacion del curso
        public bool ActualizarCurso(Curso cursoModificado)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_ActualizarCurso", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCurso", cursoModificado.IdCurso);
                        cmd.Parameters.AddWithValue("@NombreCurso", cursoModificado.NombreCurso);
                        cmd.Parameters.AddWithValue("@Descripcion", (object?)cursoModificado.Descripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaInicio", cursoModificado.FechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFin", cursoModificado.FechaFin);
                        cmd.Parameters.AddWithValue("@Horario", cursoModificado.Horario);
                        cmd.Parameters.AddWithValue("@CupoMaximo", cursoModificado.CupoMaximo);
                        cmd.Parameters.AddWithValue("@Estado", cursoModificado.Estado);
                        cmd.Parameters.AddWithValue("@CarnetInstructor", (object?)cursoModificado.CarnetInstructor ?? DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
                throw;
            }
        }

        // Agregar curso
        public bool AgregarCurso(Curso cursoAgregado)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    using (var cmd = new SqlCommand("dbo.sp_CrearCurso", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NombreCurso", cursoAgregado.NombreCurso);
                        cmd.Parameters.AddWithValue("@Descripcion", cursoAgregado.Descripcion);
                        cmd.Parameters.AddWithValue("@FechaInicio", cursoAgregado.FechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFin", cursoAgregado.FechaFin);
                        cmd.Parameters.AddWithValue("@Horario", cursoAgregado.Horario);
                        cmd.Parameters.AddWithValue("@CupoMaximo", cursoAgregado.CupoMaximo);
                        cmd.Parameters.AddWithValue("@CarnetInstructor", (object?)cursoAgregado.CarnetInstructor ?? DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }

            }
            catch(Exception ex) {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
                throw;
            }
        }
    }
}
