namespace MovieBookRecommendation.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public string Genre { get; set; }  // Nova propriedade para gênero
    }
}

