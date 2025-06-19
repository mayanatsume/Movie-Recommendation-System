using MovieBookRecommendation.Data;
using System.Threading.Tasks;
using System.Windows;

namespace MovieBookRecommendation.Controllers
{
    public class LoginController
    {
        private readonly LoginWindow _view;
        private readonly UserRepository _userRepository;

        public LoginController(LoginWindow view)
        {
            _view = view;
            // A mesma connection string usada no MainWindow
            string connectionString = "server=localhost;port=3306;database=MovieBookDB;user=root;password=Geraldo00;";
            _userRepository = new UserRepository(connectionString);
        }

        public async Task ProcessLoginAsync()
        {
            // Obtém o e-mail e a senha diretamente da View
            string email = _view.EmailTextBox.Text.Trim();
            string password = _view.PasswordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, preencha e-mail e senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Tenta obter a senha armazenada para o e-mail informado
                string storedPassword = await _userRepository.GetUserPasswordAsync(email);

                if (string.IsNullOrEmpty(storedPassword))
                {
                    // Se o utilizador não existe, cria um novo
                    bool created = await _userRepository.CreateUserAsync(email, password);
                    if (created)
                    {
                        _view.LoggedInUserEmail = email;
                        _view.DialogResult = true;
                        _view.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao criar o utilizador. Tente novamente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Se o utilizador existe, verifica a senha
                    if (storedPassword == password)
                    {
                        _view.LoggedInUserEmail = email;
                        _view.DialogResult = true;
                        _view.Close();
                    }
                    else
                    {
                        MessageBox.Show("Senha inválida. Tente novamente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Erro no processo de login: " + ex.Message);
            }
        }
    }
}
