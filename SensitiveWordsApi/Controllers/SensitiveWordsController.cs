using Microsoft.AspNetCore.Mvc;
using SensitiveWordsApi.Repositories;
using SensitiveWordsApi.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SensitiveWordsApi.Controllers
{
    /// <summary>
    /// API controller for managing sensitive words (CRUD).
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SensitiveWordsController : ControllerBase
    {
        private readonly ISensitiveWordsRepository _repo;

        /// <summary>
        /// Controller uses dependency injection to get the repository.
        /// </summary>
        public SensitiveWordsController(ISensitiveWordsRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Add a new sensitive word (string body, must be unique).
        /// </summary>
        /// <param name="word">The word to add.</param>
        /// <returns>Returns 200 with new ID, or 409 if duplicate.</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string word)
        {
            try
            {
                var id = await _repo.AddAsync(word);
                return Ok(id);
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                // Unique key violation
                return Conflict("That word already exists in the sensitive words list.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get all sensitive words (now returns list of objects with ID and Word).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var words = await _repo.GetAllAsync();
                return Ok(words);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update an existing sensitive word by ID.
        /// </summary>
        /// <param name="id">The ID to update.</param>
        /// <param name="newWord">The new word value.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string newWord)
        {
            try
            {
                var updated = await _repo.UpdateAsync(id, newWord);
                return updated ? Ok() : NotFound();
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                return Conflict("That word already exists in the sensitive words list.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete a sensitive word by ID.
        /// </summary>
        /// <param name="id">The ID to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _repo.DeleteAsync(id);
                return deleted ? Ok() : NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
