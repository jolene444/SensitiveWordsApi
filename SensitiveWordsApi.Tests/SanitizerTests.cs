using Xunit;
using System.Collections.Generic;
using SensitiveWordsApi.Services;

namespace SensitiveWordsApi.Tests
{
    public class SanitizerTests
    {
        [Fact]
        public void SanitizeText_ReplacesSensitiveWordWithAsterisks()
        {
            // Arrange
            var sensitiveWords = new List<string> { "secret" };
            var input = "This is a secret message";

            // Act
            var sanitizer = new SensitiveWordSanitizer();
            var result = sanitizer.SanitizeText(input, sensitiveWords);

            // Assert
            Assert.Equal("This is a ****** message", result);
        }
    }
}
