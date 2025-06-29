using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace SensitiveWordsApi.Repositories
{
    /// <summary>
    /// Repository for managing sensitive words in the SQL database using ADO.NET.
    /// Implements all CRUD operations required by ISensitiveWordsRepository.
    /// </summary>
    public class SensitiveWordsRepository : ISensitiveWordsRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructs the repository, reading the DB connection string from configuration.
        /// </summary>
        public SensitiveWordsRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Adds a new sensitive word to the database.
        /// </summary>
        /// <param name="word">The word to add (must be unique).</param>
        /// <returns>The ID of the new word record.</returns>
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
        /// Gets a list of all sensitive words in the database.
        /// </summary>
        /// <returns>List of all sensitive words (strings).</returns>
        public async Task<List<string>> GetAllAsync()
        {
            var words = new List<string>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT Word FROM SensitiveWords", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        words.Add(reader.GetString(0));
                }
            }
            return words;
        }

        /// <summary>
        /// Updates the word for a specific record (by ID).
        /// </summary>
        /// <param name="id">ID of the record to update.</param>
        /// <param name="newWord">The new word value.</param>
        /// <returns>True if updated, false if not found.</returns>
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
        /// Deletes a word from the database by its ID.
        /// </summary>
        /// <param name="id">ID of the word to delete.</param>
        /// <returns>True if deleted, false if not found.</returns>
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
        /// Deletes a word from the database by its word value (case-insensitive).
        /// Useful for cleaning up test data or removing specific words.
        /// </summary>
        /// <param name="word">The word value to delete.</param>
        /// <returns>True if deleted, false if not found.</returns>
        public async Task<bool> DeleteByWordAsync(string word)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM SensitiveWords WHERE Word = @Word", conn))
            {
                cmd.Parameters.AddWithValue("@Word", word);
                await conn.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0; // true if any rows were deleted
            }
        }
    }
}
