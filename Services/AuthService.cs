using Microsoft.Data.SqlClient;
using AvitoMiniApp.Models;

namespace AvitoMiniApp.Services
{
    public class AuthService
    {
        private readonly DatabaseService dbService;

        public AuthService()
        {
            dbService = new DatabaseService();
        }

        public User? Authenticate(string login, string password)
        {
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT UserId, Login, Email, CreatedAt FROM Users WHERE Login = @Login AND Password = @Password",
                    connection
                );
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            Email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3)
                        };
                    }
                }
            }
            return null;
        }
    }
}
