using MovieBookRecommendation.Controllers;
using System.Windows;

namespace MovieBookRecommendation
{
    public partial class LoginWindow : Window
    {
        // Essa propriedade continua para passar o e-mail logado para outras partes
        public string LoggedInUserEmail { get; set; }

        // Instância do LoginController
        private readonly LoginController _loginController;

        public LoginWindow()
        {
            InitializeComponent();
            // Cria o controller e injeta a referência da View (esta janela)
            _loginController = new LoginController(this);
        }

        // Handler para o botão de login
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await _loginController.ProcessLoginAsync();
        }

        // Ao fechar, define DialogResult=false se não houve login
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult == null)
            {
                this.DialogResult = false;
            }
            base.OnClosing(e);
        }
    }
}

