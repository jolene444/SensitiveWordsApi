using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensitiveWordsApi.Repositories
{
    /// <summary>
    /// Defines methods for CRUD operations on sensitive words.
    /// </summary>
    public interface ISensitiveWordsRepository
    {
        /// <summary>
        /// Adds a new sensitive word.
        /// </summary>
        /// <param name="word">The word to add (must be unique).</param>
        /// <returns>The new record's ID.</returns>
        Task<int> AddAsync(string word);

        /// <summary>
        /// Gets all sensitive words.
        /// </summary>
        /// <returns>List of all words.</returns>
        Task<List<string>> GetAllAsync();

        /// <summary>
        /// Updates a sensitive word by its ID.
        /// </summary>
        /// <param name="id">ID of the word to update.</param>
        /// <param name="newWord">The new word value.</param>
        /// <returns>True if updated, false if not found.</returns>
        Task<bool> UpdateAsync(int id, string newWord);

        /// <summary>
        /// Deletes a word by its ID.
        /// </summary>
        /// <param name="id">The ID to delete.</param>
        /// <returns>True if deleted, false if not found.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Deletes a word by the word value.
        /// </summary>
        /// <param name="word">The word value to delete.</param>
        /// <returns>True if deleted, false if not found.</returns>
        Task<bool> DeleteByWordAsync(string word);
    }
}
