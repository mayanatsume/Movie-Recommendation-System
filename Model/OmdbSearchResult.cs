using System.Collections.Generic;

namespace MovieBookRecommendation.Models
{
    public class OmdbSearchResult
    {
        public List<Movie> Search { get; set; }
        public string TotalResults { get; set; }
        public string Response { get; set; }
    }
}