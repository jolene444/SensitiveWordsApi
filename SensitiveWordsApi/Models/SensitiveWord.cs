namespace SensitiveWordsApi.Models
{
    /// <summary>
    /// Represents a sensitive word record as stored in the database.
    /// </summary>
    public class SensitiveWord
    {
        /// <summary>
        /// The unique identifier for the sensitive word.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The sensitive word itself.
        /// </summary>
        public string Word { get; set; }
    }
}
