using Microsoft.AspNetCore.Mvc;
using SensitiveWordsApi.Repositories;

namespace SensitiveWordsApi.Controllers
{
    /// <summary>
    /// Controller for managing sensitive words (CRUD operations).
    /// This is used to add, list, update, or delete sensitive words in the system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SensitiveWordsController : ControllerBase
    {
        private readonly ISensitiveWordsRepository _repo;

        /// <summary>
        /// Constructor that receives the repository via Dependency Injection.
        /// This allows us to change how words are stored without changing controller code.
        /// </summary>
        public SensitiveWordsController(ISensitiveWordsRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Adds a new sensitive word to the list.
        /// </summary>
        /// <param name="word">The word to add. Must be unique.</param>
        /// <returns>200 OK with the new word's ID, or 409 Conflict if the word exists.</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string word)
        {
            try
            {
                var id = await _repo.AddAsync(word);
                return Ok(id); // Success: return the new record's ID
            }
            catch (System.Data.SqlClient.SqlException ex) when (ex.Number == 2627) // Unique constraint violation
            {
                return Conflict("That word already exists in the sensitive words list.");
            }
            catch (Exception ex)
            {
                // General error: Return as server error
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of all sensitive words.
        /// </summary>
        /// <returns>200 OK with a list of all sensitive words.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var words = await _repo.GetAllAsync();
                return Ok(words);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates a sensitive word by ID.
        /// </summary>
        /// <param name="id">The unique ID of the word to update.</param>
        /// <param name="newWord">The new value for the word.</param>
        /// <returns>200 OK if successful, 404 if not found, or 409 if newWord is duplicate.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string newWord)
        {
            try
            {
                var updated = await _repo.UpdateAsync(id, newWord);
                return updated ? Ok() : NotFound();
            }
            catch (System.Data.SqlClient.SqlException ex) when (ex.Number == 2627)
            {
                return Conflict("That word already exists in the sensitive words list.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a sensitive word by ID.
        /// </summary>
        /// <param name="id">The unique ID of the word to delete.</param>
        /// <returns>200 OK if deleted, 404 if not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _repo.DeleteAsync(id);
                return deleted ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a sensitive word by the word itself.
        /// </summary>
        /// <param name="word">The word to delete.</param>
        /// <returns>200 OK if deleted, 404 if not found.</returns>
        [HttpDelete("ByWord/{word}")]
        public async Task<IActionResult> DeleteByWord(string word)
        {
            var deleted = await _repo.DeleteByWordAsync(word);
            return deleted ? Ok() : NotFound();
        }

    }
}
