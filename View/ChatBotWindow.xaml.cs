using MovieBookRecommendation.Controllers;
using MovieBookRecommendation.Models; // ← Adicione isso para reconhecer "Movie"
using System.Windows;

namespace MovieBookRecommendation
{
    public partial class ChatBotWindow : Window
    {
        private readonly ChatBotController _chatBotController;

        // Aqui está a propriedade que o MainController precisa acessar
        public Movie LastRatedMovie { get; set; }

        public ChatBotWindow()
        {
            InitializeComponent();

            _chatBotController = new ChatBotController(this);

            // Envia o filme avaliado para o controller
            _chatBotController.LastRatedMovie = this.LastRatedMovie;
        }

        private async void BtnSinopse_Click(object sender, RoutedEventArgs e)
        {
            await _chatBotController.ShowSynopsisAsync();
        }

        private void BtnBuscarDescricao_Click(object sender, RoutedEventArgs e)
        {
            _chatBotController.ShowSearchPanel();
        }

        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await _chatBotController.SearchMoviesByDescriptionAsync();
        }
    }
}



