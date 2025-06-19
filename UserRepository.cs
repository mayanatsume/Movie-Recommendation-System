using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace MovieBookRecommendation.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Atualiza a preferência do utilizador para filmes (BestMovie)
        public async Task UpdateUserPreferenceAsync(string email, string preferredGenre)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Users SET BestMovie = @Genre WHERE Email = @Email";
                command.Parameters.AddWithValue("@Genre", preferredGenre ?? "");
                command.Parameters.AddWithValue("@Email", email);
                await command.ExecuteNonQueryAsync();
            }
        }

        // Atualiza a preferência do utilizador para livros (FavoriteBooks)
        public async Task UpdateBookPreferenceAsync(string email, string preferredAuthor)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Users SET FavoriteBooks = @Author WHERE Email = @Email";
                command.Parameters.AddWithValue("@Author", preferredAuthor ?? "");
                command.Parameters.AddWithValue("@Email", email);
                await command.ExecuteNonQueryAsync();
            }
        }

        // Verifica se o utilizador existe, buscando a senha
        public async Task<string> GetUserPasswordAsync(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Password FROM Users WHERE Email = @Email";
                command.Parameters.AddWithValue("@Email", email);
                object result = await command.ExecuteScalarAsync();
                return result?.ToString();
            }
        }

        // Cria um novo utilizador com o e-mail e a senha informados
        public async Task<bool> CreateUserAsync(string email, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Users (Email, Password, BestMovie, FavoriteBooks) VALUES (@Email, @Password, NULL, NULL)";
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                int affected = await command.ExecuteNonQueryAsync();
                return affected > 0;
            }
        }

        // Retorna as preferências do utilizador: BestMovie (gênero) e FavoriteBooks (autor)
        public async Task<(string movieGenre, string bookAuthor)> GetUserPreferencesAsync(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT BestMovie, FavoriteBooks FROM Users WHERE Email = @Email";
                command.Parameters.AddWithValue("@Email", email);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string movieGenre = reader["BestMovie"]?.ToString();
                        string bookAuthor = reader["FavoriteBooks"]?.ToString();
                        return (movieGenre, bookAuthor);
                    }
                }
            }
            return (null, null);
        }
    }
}
