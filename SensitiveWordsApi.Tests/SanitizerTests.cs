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
            // Arrange: Prepare a list of sensitive words and an input string
            var sensitiveWords = new List<string> { "secret" };
            var sanitizer = new SensitiveWordSanitizer(sensitiveWords);

            // Act: Call the method under test
            var input = "This is a secret message";
            var result = sanitizer.SanitizeText(input);

            // Assert: Check that the output is as expected
            Assert.Equal("This is a ****** message", result);
        }
    }
}
