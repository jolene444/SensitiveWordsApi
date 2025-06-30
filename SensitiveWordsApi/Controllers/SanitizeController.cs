using Microsoft.AspNetCore.Mvc;
using SensitiveWordsApi.Repositories;
using SensitiveWordsApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SensitiveWordsApi.Controllers
{
    /// <summary>
    /// Request body model for the sanitize endpoint.
    /// </summary>
    public class SanitizeRequest
    {
        /// <summary>
        /// The input text to sanitize.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// API controller for sanitizing input strings using sensitive words.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SanitizeController : ControllerBase
    {
        private readonly ISensitiveWordsRepository _repo;

        /// <summary>
        /// Gets repository via Dependency Injection.
        /// </summary>
        public SanitizeController(ISensitiveWordsRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Sanitizes the provided text by replacing all sensitive words
        /// (found in the database) with asterisks of the same length.
        /// </summary>
        /// <param name="req">Object containing the text to sanitize.</param>
        /// <returns>Sanitized text (with sensitive words masked).</returns>
        [HttpPost]
        public async Task<IActionResult> SanitizeText([FromBody] SanitizeRequest req)
        {
            // Get all sensitive words from the repository (with Ids and Words)
            var wordObjs = await _repo.GetAllAsync();

            // Extract just the words (string list) for sanitization
            var words = wordObjs.Select(x => x.Word).ToList();

            // Perform the sanitization using a helper method
            var sanitized = Sanitize(req.Text, words);

            // Return the result in a JSON object
            return Ok(new { sanitized });
        }

        /// <summary>
        /// Helper method that replaces each sensitive word in input
        /// with asterisks (****), preserving length, case-insensitive.
        /// </summary>
        /// <param name="input">Text to scan and sanitize.</param>
        /// <param name="words">List of sensitive words to match.</param>
        /// <returns>Sanitized text string.</returns>
        private string Sanitize(string input, List<string> words)
        {
            if (string.IsNullOrEmpty(input) || words == null)
                return input;

            string result = input;
            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    // Word-boundary match, case-insensitive, replace with ****
                    result = Regex.Replace(
                        result,
                        $@"\b{Regex.Escape(word)}\b",
                        new string('*', word.Length),
                        RegexOptions.IgnoreCase
                    );
                }
            }
            return result;
        }
    }
}
