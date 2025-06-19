using System.Windows;
using MovieBookRecommendation.Controllers; // Importa o MainController
using System.Threading.Tasks;

namespace MovieBookRecommendation
{
    public partial class MainWindow : Window
    {
        private MainController _controller;

        public MainWindow()
        {
            InitializeComponent();
            // Instancia o controller, passando "this" para que o controller possa atualizar a UI quando necessário
            _controller = new MainController(this);

            // Registra o evento Loaded da janela
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _controller.OnWindowLoaded();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Assume que há um controle chamado SearchTextBox no XAML
            await _controller.OnSearchButtonClicked(SearchTextBox.Text);
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            await _controller.OnStarButtonClicked(sender, e);
        }

        private async void BookStarButton_Click(object sender, RoutedEventArgs e)
        {
            await _controller.OnBookStarButtonClicked(sender, e);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.OnLoginButtonClicked();
        }

        private void SendPreferencesButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.OnSendPreferencesButtonClicked();
        }

        private void ChatBotButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.OnChatBotButtonClicked();
        }

        // Se houver um botão para abrir a FaceSwap, também delegamos:
        private void FaceSwapButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.OnFaceSwapButtonClicked();
        }
    }
}
