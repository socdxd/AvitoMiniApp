using Microsoft.Data.SqlClient;

namespace AvitoMiniApp.Services
{
    public class DatabaseService
    {
        private readonly string connectionString;

        public DatabaseService()
        {
            // Try SQLEXPRESS instance first, fallback to localhost
            connectionString = "Server=localhost\\SQLEXPRESS;Database=AvitoMiniDB;Integrated Security=true;TrustServerCertificate=true;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
