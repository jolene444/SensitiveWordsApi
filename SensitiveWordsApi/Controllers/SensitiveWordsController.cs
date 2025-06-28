using Microsoft.AspNetCore.Mvc;
using SensitiveWordsApi.Repositories;
using System.Threading.Tasks;

namespace SensitiveWordsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensitiveWordsController : ControllerBase
    {
        private readonly SensitiveWordsRepository _repo;

        public SensitiveWordsController(SensitiveWordsRepository repo)
        {
            _repo = repo;
        }

        // GET: api/sensitivewords
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var words = await _repo.GetAllAsync();
            return Ok(words);
        }

        // POST: api/sensitivewords
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string word)
        {
            var id = await _repo.AddAsync(word);
            return Ok(new { id });
        }

        // PUT: api/sensitivewords/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string newWord)
        {
            var updated = await _repo.UpdateAsync(id, newWord);
            if (!updated) return NotFound();
            return Ok();
        }

        // DELETE: api/sensitivewords/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }
    }
}
