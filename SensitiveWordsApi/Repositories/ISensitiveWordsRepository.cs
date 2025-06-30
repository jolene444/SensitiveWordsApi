using System.Collections.Generic;
using System.Threading.Tasks;
using SensitiveWordsApi.Models; 

namespace SensitiveWordsApi.Repositories
{
    /// <summary>
    /// Interface for managing sensitive words in the database.
    /// Defines all CRUD operations (Create, Read, Update, Delete).
    /// </summary>
    public interface ISensitiveWordsRepository
    {
        /// <summary>
        /// Adds a new sensitive word to the database.
        /// </summary>
        /// <param name="word">The word to add. Must be unique, enforced by the DB.</param>
        /// <returns>The new record's ID as an integer.</returns>
        Task<int> AddAsync(string word);

        /// <summary>
        /// Retrieves all sensitive words from the database.
        /// </summary>
        /// <returns>
        /// A list of SensitiveWord objects, each containing both the unique ID and the word.
        /// Useful for UI, auditing, or for referencing by ID for update/delete.
        /// </returns>
        Task<List<SensitiveWord>> GetAllAsync();  

        /// <summary>
        /// Updates an existing sensitive word by its database ID.
        /// </summary>
        /// <param name="id">The unique ID of the word to update.</param>
        /// <param name="newWord">The new value for the word.</param>
        /// <returns>True if an update occurred, false if no matching record was found.</returns>
        Task<bool> UpdateAsync(int id, string newWord);

        /// <summary>
        /// Deletes a sensitive word by its database ID.
        /// </summary>
        /// <param name="id">The unique ID of the word to delete.</param>
        /// <returns>True if deleted, false if not found.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Deletes a sensitive word by its text value.
        /// </summary>
        /// <param name="word">The word to delete (case-insensitive, must match exactly).</param>
        /// <returns>True if deleted, false if not found.</returns>
        Task<bool> DeleteByWordAsync(string word);
    }
}
