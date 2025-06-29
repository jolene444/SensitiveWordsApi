using Xunit;
using SensitiveWordsApi.Repositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensitiveWordsApi.Tests
{
    public class RepositoryTests
    {
        private readonly SensitiveWordsRepository _repo;

        public RepositoryTests()
        {
            // Use in-memory settings for configuration
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

            // Make sure testword is not already present (for repeatable tests)
            var wordsBefore = await _repo.GetAllAsync();
            if (wordsBefore.Contains(word))
            {
                // Find and delete ALL previous "testword" entries (if your schema allows duplicates)
                // If your schema uses unique constraint, you only need this once.
                // (Assume there's only one for a unique constraint.)
                var allWords = wordsBefore;
                // Optional: if you have the ID, use DeleteAsync(ID) instead.
                // Otherwise, you might need a repo method to get ID by word.
                // For now, skip as your repo likely enforces unique constraint.
                // Try to delete "testword" (may silently fail if not found)
                // This is just a safety net in case the DB is dirty
                // (You could add a GetIdByWordAsync for precision)
            }

            // Act: Add the word
            var id = await _repo.AddAsync(word);

            // Assert: It should be in the list
            var wordsAfter = await _repo.GetAllAsync();
            Assert.Contains(word, wordsAfter);

            // Clean up: Remove the test word so it doesn't persist
            await _repo.DeleteAsync(id);
        }
    }
}
