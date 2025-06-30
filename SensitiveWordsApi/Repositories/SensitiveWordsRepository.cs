using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using SensitiveWordsApi.Models;

namespace SensitiveWordsApi.Repositories
{
    /// <summary>
    /// Implementation of the ISensitiveWordsRepository using ADO.NET for direct SQL access.
    /// </summary>
    public class SensitiveWordsRepository : ISensitiveWordsRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Gets the DB connection string from configuration (Dependency Injection).
        /// </summary>
        public SensitiveWordsRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Adds a new sensitive word to the database and returns the new record's ID.
        /// </summary>
        public async Task<int> AddAsync(string word)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "INSERT INTO SensitiveWords (Word) VALUES (@Word); SELECT SCOPE_IDENTITY();", conn))
            {
                cmd.Parameters.AddWithValue("@Word", word);
                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
        }

        /// <summary>
        /// Gets all sensitive words from the database, including their IDs.
        /// </summary>
        public async Task<List<SensitiveWord>> GetAllAsync()
        {
            var words = new List<SensitiveWord>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT Id, Word FROM SensitiveWords", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        words.Add(new SensitiveWord
                        {
                            Id = reader.GetInt32(0),
                            Word = reader.GetString(1)
                        });
                    }
                }
            }
            return words;
        }

        /// <summary>
        /// Updates a sensitive word by its ID.
        /// </summary>
        public async Task<bool> UpdateAsync(int id, string newWord)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "UPDATE SensitiveWords SET Word = @Word WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Word", newWord);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }

        /// <summary>
        /// Deletes a sensitive word by its ID.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "DELETE FROM SensitiveWords WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }

        /// <summary>
        /// Deletes a sensitive word by its text value.
        /// </summary>
        public async Task<bool> DeleteByWordAsync(string word)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM SensitiveWords WHERE Word = @Word", conn))
            {
                cmd.Parameters.AddWithValue("@Word", word);
                await conn.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
        }
    }
}
