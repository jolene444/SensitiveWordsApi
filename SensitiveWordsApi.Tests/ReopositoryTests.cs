using Xunit;
using SensitiveWordsApi.Repositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SensitiveWordsApi.Models;

namespace SensitiveWordsApi.Tests
{
    public class RepositoryTests
    {
        private readonly SensitiveWordsRepository _repo;

        public RepositoryTests()
        {
            // In-memory settings for configuration
            var inMemorySettings = new Dictionary<string, string> {
                {"ConnectionStrings:DefaultConnection", "Server=localhost\\SQLEXPRESS;Database=SENSITIVE_WORDS_DB;Trusted_Connection=True;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _repo = new SensitiveWordsRepository(config);
        }

        [Fact]
        public async Task AddAndGetAllAsync_ShouldAddWordAndRetrieveIt()
        {
            // Arrange
            var word = "testword";

            // Get all sensitive words from the DB (list of SensitiveWord objects)
            var wordsBefore = await _repo.GetAllAsync();

            // Extract just the word strings for comparison
            var wordStringsBefore = wordsBefore.Select(w => w.Word).ToList();

            // Make sure testword is not already present (repeatable test)
            if (wordStringsBefore.Contains(word))
            {
                // If found, delete all occurrences (optional safety for tests)
                foreach (var sw in wordsBefore.Where(w => w.Word == word))
                {
                    await _repo.DeleteAsync(sw.Id);
                }
            }

            // Act: Add the word
            var id = await _repo.AddAsync(word);

            // Assert: It should be in the list after add
            var wordsAfter = await _repo.GetAllAsync();
            var wordStringsAfter = wordsAfter.Select(w => w.Word).ToList();
            Assert.Contains(word, wordStringsAfter);

            // Clean up: Remove the test word so it doesn't persist
            await _repo.DeleteAsync(id);
        }
    }
}
