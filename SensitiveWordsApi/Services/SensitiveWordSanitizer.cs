using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SensitiveWordsApi.Services
{
    /// <summary>
    /// Service to sanitize input text by masking sensitive words using regex for word boundaries.
    /// This design is stateless: the sensitive words list is passed in when needed,
    /// ensuring the latest DB values are always used.
    /// </summary>
    public class SensitiveWordSanitizer
    {
        /// <summary>
        /// Replaces each sensitive word in the input with asterisks, matching whole words only.
        /// 
        /// Why this design?
        /// - Passing sensitiveWords as a parameter ensures the up-to-date list from the DB.
        /// - Stateless design (no member fields) makes this class safe for use across requests/services.
        /// - Regex with word boundaries (\b) only replaces whole words, not substrings inside other words.
        /// - RegexOptions.IgnoreCase ensures that replacements are case-insensitive ("Secret", "secret", "SECRET" all match).
        /// 
        /// Example:
        /// Input:  "You need to create a string"
        /// Words:  [ "create" ]
        /// Output: "You need to ****** a string"
        /// </summary>
        /// <param name="input">The original input string to be sanitized.</param>
        /// <param name="sensitiveWords">A list of words to search for and mask.</param>
        /// <returns>
        /// The sanitized string, with sensitive words replaced by asterisks of the same length.
        /// Returns the original input if no sensitive words found or if input is empty.
        /// </returns>
        public string SanitizeText(string input, IEnumerable<string> sensitiveWords)
        {
            // Guard clause: return the input unchanged if it's empty or if no words to sanitize.
            if (string.IsNullOrEmpty(input) || sensitiveWords == null)
                return input;

            string result = input;

            // Loop through each sensitive word.
            foreach (var word in sensitiveWords)
            {
                // Only replace non-empty words (skip blanks/nulls for safety).
                if (!string.IsNullOrWhiteSpace(word))
                {
                    // Use Regex to replace only exact matches of the word (word boundary),
                    // ignoring case, and replacing with same-length asterisks.
                    result = Regex.Replace(
                        result,
                        $@"\b{Regex.Escape(word)}\b",        // \b ensures word boundaries.
                        new string('*', word.Length),        // Replace with asterisks of same length.
                        RegexOptions.IgnoreCase              // Ignore case in matching.
                    );
                }
            }
            return result;
        }
    }
}
