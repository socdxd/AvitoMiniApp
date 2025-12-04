using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using AvitoMiniApp.Models;

namespace AvitoMiniApp.Services
{
    public class CategoryService
    {
        private readonly DatabaseService dbService;

        public CategoryService()
        {
            dbService = new DatabaseService();
        }

        public ObservableCollection<Category> GetAllCategories()
        {
            var categories = new ObservableCollection<Category>();
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT CategoryId, Name FROM Categories", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return categories;
        }
    }
}
