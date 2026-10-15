using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ActividadesExtraPortal.Data
{
    /// <summary>
    /// Gestiona la conexión con SQL Server implementando el patrón creacional Singleton.
    /// Garantiza una única instancia centralizada y segura para subprocesos (thread-safe)
    /// evitando instancias redundantes y fugas de recursos de conexión.
    /// </summary>
    public sealed class DatabaseConnection
    {
        // Instancia perezosa (Lazy) segura para subprocesos (thread-safe)
        private static readonly Lazy<DatabaseConnection> _instancia =
            new Lazy<DatabaseConnection>(() => new DatabaseConnection());

        /// <summary>
        /// Acceso global a la instancia única del Singleton.
        /// </summary>
        public static DatabaseConnection Instance => _instancia.Value;

        private readonly string connectionString = @"Server=localhost;Database=GestionExtracurricular;Trusted_Connection=True;TrustServerCertificate=True;";

        // Constructor privado para impedir la instanciación externa directa (regla canónica de Singleton)
        private DatabaseConnection() { }

        /// <summary>
        /// Obtiene y retorna una nueva conexión SQL configurada con la cadena centralizada.
        /// </summary>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Prueba si el motor de base de datos responde correctamente.
        /// </summary>
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
