namespace MovieBookRecommendation.Models
{
    public class MovieDetailResult
    {
        public string Title { get; set; }
        public string Year { get; set; }
        // Outras propriedades que você já possui...

        // Adicione a propriedade Genre para armazenar o gênero do filme:
        public string Genre { get; set; }

        // Se ainda não existir, também adicione a propriedade Plot:
        public string Plot { get; set; }
    }
}
