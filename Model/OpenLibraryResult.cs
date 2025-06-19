using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MovieBookRecommendation.Models
{
    public class OpenLibraryResult
    {
        [JsonPropertyName("docs")]
        public List<OpenLibraryBook> Docs { get; set; }
    }

    public class OpenLibraryBook
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author_name")]
        public List<string> AuthorName { get; set; }

        [JsonPropertyName("cover_i")]
        public int? CoverId { get; set; }
    }
}
