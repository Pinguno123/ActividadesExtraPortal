using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ActividadesExtraPortal.Data
{
    public class DatabaseConnection
    {       
        private readonly string connectionString = @"Server=localhost;Database=GestionExtracurricular;Trusted_Connection=True;TrustServerCertificate=True;";

        // Obtener conexion SQL
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Probar conexion con base de datos
        public bool ProbarConexion()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión a la base de datos: {ex.Message}");
                return false;
            }
        }
    }
}
