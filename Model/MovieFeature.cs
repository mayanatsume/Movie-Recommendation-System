using Microsoft.ML.Data;

namespace MovieBookRecommendation.Models
{
    // Herda de MovieData para manter os dados originais e adiciona o vetor de features
    public class MovieFeature : MovieData
    {
        public float[] Features { get; set; }
    }
}
