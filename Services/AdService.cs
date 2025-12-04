using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using AvitoMiniApp.Models;

namespace AvitoMiniApp.Services
{
    public class AdService
    {
        private readonly DatabaseService dbService;

        public AdService()
        {
            dbService = new DatabaseService();
        }

        public ObservableCollection<Ad> GetAllAds()
        {
            var ads = new ObservableCollection<Ad>();
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT a.AdId, a.UserId, a.CategoryId, a.Title, a.Description, 
                           a.Price, a.City, a.ImagePath, a.Status, a.CreatedAt, c.Name
                    FROM Ads a
                    INNER JOIN Categories c ON a.CategoryId = c.CategoryId
                    WHERE a.Status = 'Active'
                    ORDER BY a.CreatedAt DESC", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ads.Add(new Ad
                        {
                            AdId = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            CategoryId = reader.GetInt32(2),
                            Title = reader.GetString(3),
                            Description = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Price = reader.GetDecimal(5),
                            City = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            ImagePath = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            Status = reader.GetString(8),
                            CreatedAt = reader.GetDateTime(9),
                            CategoryName = reader.GetString(10)
                        });
                    }
                }
            }
            return ads;
        }

        public ObservableCollection<Ad> GetUserAds(int userId)
        {
            var ads = new ObservableCollection<Ad>();
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT a.AdId, a.UserId, a.CategoryId, a.Title, a.Description, 
                           a.Price, a.City, a.ImagePath, a.Status, a.CreatedAt, c.Name
                    FROM Ads a
                    INNER JOIN Categories c ON a.CategoryId = c.CategoryId
                    WHERE a.UserId = @UserId AND a.Status = 'Active'
                    ORDER BY a.CreatedAt DESC", connection);
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ads.Add(new Ad
                        {
                            AdId = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            CategoryId = reader.GetInt32(2),
                            Title = reader.GetString(3),
                            Description = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Price = reader.GetDecimal(5),
                            City = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            ImagePath = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            Status = reader.GetString(8),
                            CreatedAt = reader.GetDateTime(9),
                            CategoryName = reader.GetString(10)
                        });
                    }
                }
            }
            return ads;
        }

        public ObservableCollection<Ad> SearchAds(string searchText, int? categoryId = null)
        {
            var ads = new ObservableCollection<Ad>();
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT a.AdId, a.UserId, a.CategoryId, a.Title, a.Description, 
                           a.Price, a.City, a.ImagePath, a.Status, a.CreatedAt, c.Name
                    FROM Ads a
                    INNER JOIN Categories c ON a.CategoryId = c.CategoryId
                    WHERE a.Status = 'Active' 
                    AND (a.Title LIKE @Search OR a.Description LIKE @Search)";

                if (categoryId.HasValue)
                {
                    query += " AND a.CategoryId = @CategoryId";
                }

                query += " ORDER BY a.CreatedAt DESC";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Search", "%" + searchText + "%");
                if (categoryId.HasValue)
                {
                    command.Parameters.AddWithValue("@CategoryId", categoryId.Value);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ads.Add(new Ad
                        {
                            AdId = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            CategoryId = reader.GetInt32(2),
                            Title = reader.GetString(3),
                            Description = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Price = reader.GetDecimal(5),
                            City = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            ImagePath = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            Status = reader.GetString(8),
                            CreatedAt = reader.GetDateTime(9),
                            CategoryName = reader.GetString(10)
                        });
                    }
                }
            }
            return ads;
        }

        public void AddAd(Ad ad)
        {
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    INSERT INTO Ads (UserId, CategoryId, Title, Description, Price, City, ImagePath, Status)
                    VALUES (@UserId, @CategoryId, @Title, @Description, @Price, @City, @ImagePath, 'Active')",
                    connection);

                command.Parameters.AddWithValue("@UserId", ad.UserId);
                command.Parameters.AddWithValue("@CategoryId", ad.CategoryId);
                command.Parameters.AddWithValue("@Title", ad.Title);
                command.Parameters.AddWithValue("@Description", ad.Description ?? string.Empty);
                command.Parameters.AddWithValue("@Price", ad.Price);
                command.Parameters.AddWithValue("@City", ad.City ?? string.Empty);
                command.Parameters.AddWithValue("@ImagePath", ad.ImagePath ?? string.Empty);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateAd(Ad ad)
        {
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    UPDATE Ads 
                    SET CategoryId = @CategoryId, Title = @Title, Description = @Description, 
                        Price = @Price, City = @City, ImagePath = @ImagePath
                    WHERE AdId = @AdId",
                    connection);

                command.Parameters.AddWithValue("@AdId", ad.AdId);
                command.Parameters.AddWithValue("@CategoryId", ad.CategoryId);
                command.Parameters.AddWithValue("@Title", ad.Title);
                command.Parameters.AddWithValue("@Description", ad.Description ?? string.Empty);
                command.Parameters.AddWithValue("@Price", ad.Price);
                command.Parameters.AddWithValue("@City", ad.City ?? string.Empty);
                command.Parameters.AddWithValue("@ImagePath", ad.ImagePath ?? string.Empty);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteAd(int adId)
        {
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Ads WHERE AdId = @AdId", connection);
                command.Parameters.AddWithValue("@AdId", adId);
                command.ExecuteNonQuery();
            }
        }

        public void CompleteAd(int adId, decimal finalPrice)
        {
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updateCommand = new SqlCommand(
                            "UPDATE Ads SET Status = 'Completed' WHERE AdId = @AdId",
                            connection, transaction);
                        updateCommand.Parameters.AddWithValue("@AdId", adId);
                        updateCommand.ExecuteNonQuery();

                        var insertCommand = new SqlCommand(@"
                            INSERT INTO CompletedAds (AdId, FinalPrice)
                            VALUES (@AdId, @FinalPrice)",
                            connection, transaction);
                        insertCommand.Parameters.AddWithValue("@AdId", adId);
                        insertCommand.Parameters.AddWithValue("@FinalPrice", finalPrice);
                        insertCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public ObservableCollection<CompletedAd> GetCompletedAds(int userId)
        {
            var completedAds = new ObservableCollection<CompletedAd>();
            using (var connection = dbService.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT ca.CompletedAdId, ca.AdId, ca.FinalPrice, ca.CompletedAt, 
                           a.Title, a.Price
                    FROM CompletedAds ca
                    INNER JOIN Ads a ON ca.AdId = a.AdId
                    WHERE a.UserId = @UserId
                    ORDER BY ca.CompletedAt DESC", connection);
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var originalPrice = reader.GetDecimal(5);
                        var finalPrice = reader.GetDecimal(2);
                        completedAds.Add(new CompletedAd
                        {
                            CompletedAdId = reader.GetInt32(0),
                            AdId = reader.GetInt32(1),
                            FinalPrice = finalPrice,
                            CompletedAt = reader.GetDateTime(3),
                            Title = reader.GetString(4),
                            OriginalPrice = originalPrice,
                            Profit = finalPrice - originalPrice
                        });
                    }
                }
            }
            return completedAds;
        }
    }
}
