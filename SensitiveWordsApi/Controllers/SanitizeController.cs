using Microsoft.AspNetCore.Mvc;
using SensitiveWordsApi.Repositories;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SensitiveWordsApi.Controllers
{
    public class SanitizeRequest
    {
        public string Text { get; set; }
    }

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SanitizeController : ControllerBase
    {
        private readonly SensitiveWordsRepository _repo;
        public SanitizeController(SensitiveWordsRepository repo) { _repo = repo; }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> SanitizeText([FromBody] SanitizeRequest req)
        {
            var words = await _repo.GetAllAsync();
            var sanitized = Sanitize(req.Text, words);
            return Ok(new { sanitized });
        }

        private string Sanitize(string input, List<string> words)
        {
            foreach (var word in words)
                input = Regex.Replace(input, word, new string('*', word.Length), RegexOptions.IgnoreCase);
            return input;
        }
    }
}
