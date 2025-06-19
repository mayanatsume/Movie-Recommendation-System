using Microsoft.ML.Data;

namespace MovieBookRecommendation.Models
{
    public class MovieData
    {
        [LoadColumn(0)]
        public int MovieId { get; set; }

        [LoadColumn(1)]
        public string Title { get; set; }

        [LoadColumn(2)]
        public string Genres { get; set; }
    }
}
