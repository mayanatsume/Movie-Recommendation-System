using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MovieBookRecommendation.Models
{
    // Resposta principal da TMDb para busca de filmes
    public class TMDbSearchResult
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<TMDbMovie> Results { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }

    // Objeto que representa cada filme retornado pela TMDb
    public class TMDbMovie
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        // Estes dois campos são usados no fallback do MainWindow.xaml.cs
        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }
    }

    // Resposta principal da TMDb para busca de vídeos (trailers)
    public class TMDbVideosResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("results")]
        public List<TMDbVideo> Results { get; set; }
    }

    // Objeto que representa cada vídeo (trailer) retornado pela TMDb
    public class TMDbVideo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("iso_639_1")]
        public string Iso_639_1 { get; set; }

        [JsonPropertyName("iso_3166_1")]
        public string Iso_3166_1 { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("site")]
        public string Site { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
