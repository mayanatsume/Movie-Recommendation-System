using MovieBookRecommendation.Models;
using MovieBookRecommendation.Data;
using MovieBookRecommendation.Services;
using Microsoft.ML;
using Microsoft.ML.Data;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

// Aliases para evitar ambiguidade com System.Windows
using CvRect = OpenCvSharp.Rect;
using CvPoint = OpenCvSharp.Point;
using CvSize = OpenCvSharp.Size;

namespace MovieBookRecommendation.Controllers
{
    public class MainController
    {
        private readonly MainWindow _view;
        private readonly HttpClient _httpClient = new HttpClient();

        // Constantes de API
        private const string OmdbApiKey = "7257bf9f";
        private const string OmdbApiUrl = "http://www.omdbapi.com/";
        private const string TMDbApiKey = "f743d2f2259e90a115c5f47be669ecc4";
        private const string TMDbBaseUrl = "https://api.themoviedb.org/3/";

        // Usuário logado
        private string _loggedInUserEmail = null;

        // Repositório (exemplo: MySQL)
        private UserRepository _userRepository = new UserRepository("server=localhost;port=3306;database=MovieBookDB;user=root;password=Geraldo00;");

        // Último filme avaliado (para o ChatBot)
        public Movie LastRatedMovie { get; set; }

        // Lista das últimas 3 avaliações e contador para ML.NET
        private List<Movie> _lastRatedMovies = new List<Movie>();
        private int _ratingsSinceLastML = 0;

        // Serviço de recomendação via ML.NET (baseado no movie.csv)
        private MovieService _movieService;

        public MainController(MainWindow view)
        {
            _view = view;
            _movieService = new MovieService();
        }

        // Chamado no Loaded da janela
        public async Task OnWindowLoaded()
        {
            if (!string.IsNullOrEmpty(_loggedInUserEmail))
            {
                await RefreshUserRecommendations();
            }
            else
            {
                _view.StatusTextBlock.Text = "Buscando filmes e livros, aguarde...";
                _view.MoviesItemsControl.ItemsSource = await SearchMoviesAsync("Star Wars");
                _view.RecommendationsItemsControl.ItemsSource = null;
                _view.BooksItemsControl.ItemsSource = await SearchBooksAsync("Rich Dad Poor Dad");
                await UpdateTrailerForMovieAsync("Star Wars Episode 8");
                _view.StatusTextBlock.Text = "";
            }
        }

        // Atualiza as recomendações do usuário se logado
        private async Task RefreshUserRecommendations()
        {
            if (!string.IsNullOrEmpty(_loggedInUserEmail))
            {
                var preferences = await _userRepository.GetUserPreferencesAsync(_loggedInUserEmail);

                if (string.IsNullOrEmpty(preferences.movieGenre))
                {
                    await _userRepository.UpdateUserPreferenceAsync(_loggedInUserEmail, "Action");
                    preferences.movieGenre = "Action";
                }
                if (string.IsNullOrEmpty(preferences.bookAuthor))
                {
                    await _userRepository.UpdateBookPreferenceAsync(_loggedInUserEmail, "Tolkien");
                    preferences.bookAuthor = "Tolkien";
                }

                string firstGenre = preferences.movieGenre.Split(',')[0].Trim();
                var moviesList = await DiscoverMoviesByGenreAsync(firstGenre);
                _view.MoviesItemsControl.ItemsSource = moviesList;
                _view.RecommendationsItemsControl.ItemsSource = null;

                if (moviesList.Any())
                    await UpdateTrailerForMovieAsync(moviesList.First().Title);
            }
        }

        public async Task OnSearchButtonClicked(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                _view.StatusTextBlock.Text = "Buscando filmes e livros, aguarde...";
                _view.MoviesItemsControl.ItemsSource = await SearchMoviesAsync(query);
                _view.BooksItemsControl.ItemsSource = await SearchBooksAsync(query);
                _view.StatusTextBlock.Text = "";
            }
        }

        public async Task OnStarButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Movie movie &&
                int.TryParse(btn.CommandParameter.ToString(), out int rating))
            {
                movie.Rating = rating;
                _view.MoviesItemsControl.Items.Refresh();
                MessageBox.Show($"Você classificou \"{movie.Title}\" com {rating} estrela(s).",
                                "Classificação", MessageBoxButton.OK, MessageBoxImage.Information);
                // Atualiza as recomendações de filmes
                await UpdateRecommendations(movie);
                await UpdateTrailerForMovieAsync(movie.Title);
                LastRatedMovie = movie;
                _lastRatedMovies.Add(movie);
                if (_lastRatedMovies.Count > 3)
                    _lastRatedMovies.RemoveAt(0);
                _ratingsSinceLastML++;
                if (_ratingsSinceLastML >= 3)
                {
                    ShowContentBasedRecommendations();
                    _ratingsSinceLastML = 0;
                }
                if (!string.IsNullOrEmpty(_loggedInUserEmail) && rating >= 3)
                {
                    string firstGenre = movie.Genre.Split(',')[0].Trim();
                    await _userRepository.UpdateUserPreferenceAsync(_loggedInUserEmail, firstGenre);
                }
            }
        }

        public async Task OnBookStarButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Book book &&
                int.TryParse(btn.CommandParameter.ToString(), out int rating))
            {
                book.Rating = rating;
                _view.BooksItemsControl.Items.Refresh();
                MessageBox.Show($"Você classificou \"{book.Title}\" com {rating} estrela(s).",
                                "Classificação", MessageBoxButton.OK, MessageBoxImage.Information);
                // Atualiza as recomendações de livros
                await UpdateBookRecommendations(book);
                if (!string.IsNullOrEmpty(_loggedInUserEmail) && rating >= 3)
                {
                    string firstAuthor = book.Authors.Split(',')[0].Trim();
                    if (string.IsNullOrWhiteSpace(firstAuthor))
                        firstAuthor = "Desconhecido";
                    await _userRepository.UpdateBookPreferenceAsync(_loggedInUserEmail, firstAuthor);
                }
            }
        }

        public void OnChatBotButtonClicked()
        {
            var chatBotWindow = new ChatBotWindow { LastRatedMovie = this.LastRatedMovie };
            chatBotWindow.Show();
        }

        public void OnFaceSwapButtonClicked()
        {
            FaceSwapWindow fsWindow = new FaceSwapWindow();
            fsWindow.ShowDialog();
        }

        // Método de login atualizado para usar async/await e evitar bloqueio da UI
        public async void OnLoginButtonClicked()
        {
            if (!string.IsNullOrEmpty(_loggedInUserEmail))
            {
                // Logout
                _loggedInUserEmail = null;
                _view.LoginButton.Content = "Login";
                _view.SendPreferencesButton.Visibility = Visibility.Collapsed;
                _view.StatusTextBlock.Text = "Utilizador deslogado.";
                _view.MoviesItemsControl.ItemsSource = await SearchMoviesAsync("Star Wars");
                _view.BooksItemsControl.ItemsSource = await SearchBooksAsync("Rich Dad Poor Dad");
                _view.RecommendationsItemsControl.ItemsSource = null;
            }
            else
            {
                try
                {
                    LoginWindow loginWindow = new LoginWindow();
                    bool? result = loginWindow.ShowDialog();
                    if (result == true)
                    {
                        _loggedInUserEmail = loginWindow.LoggedInUserEmail;
                        _view.LoginButton.Content = "Logout";
                        _view.SendPreferencesButton.Visibility = Visibility.Visible;
                        _view.StatusTextBlock.Text = $"Utilizador logado: {_loggedInUserEmail}";
                        await RefreshUserRecommendations();
                    }
                    else
                    {
                        MessageBox.Show("Login não efetuado.", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro no processo de login: " + ex.Message);
                }
            }
        }

        // Método de envio de preferências (Email + PDF) atualizado para async/await
        public async void OnSendPreferencesButtonClicked()
        {
            try
            {
                if (string.IsNullOrEmpty(_loggedInUserEmail))
                {
                    MessageBox.Show("Você precisa estar logado para enviar as preferências.",
                                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var recommendedMovies = _view.RecommendationsItemsControl.ItemsSource as IEnumerable<Movie>;
                var recommendedBooks = _view.BooksRecommendationsItemsControl.ItemsSource as IEnumerable<Book>;
                var topMovies = recommendedMovies?.Take(5).ToList() ?? new List<Movie>();
                var topBooks = recommendedBooks?.Take(5).ToList() ?? new List<Book>();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<h1>Suas Recomendações</h1>");
                sb.AppendLine("<h2>Filmes Recomendados</h2>");
                if (topMovies.Count > 0)
                {
                    sb.AppendLine("<ul>");
                    foreach (var movie in topMovies)
                    {
                        sb.AppendLine($"<li>{movie.Title} ({movie.Year}) - {movie.Genre}</li>");
                    }
                    sb.AppendLine("</ul>");
                }
                else
                {
                    sb.AppendLine("<p>Nenhum filme recomendado disponível.</p>");
                }

                sb.AppendLine("<h2>Livros Recomendados</h2>");
                if (topBooks.Count > 0)
                {
                    sb.AppendLine("<ul>");
                    foreach (var book in topBooks)
                    {
                        sb.AppendLine($"<li>{book.Title} - {book.Authors}</li>");
                    }
                    sb.AppendLine("</ul>");
                }
                else
                {
                    sb.AppendLine("<p>Nenhum livro recomendado disponível.</p>");
                }

                var emailService = new EmailService("smtp.gmail.com", 587,
                    "projetofaculdadecsharp@gmail.com", "vbuv wffz hytm mcut");
                emailService.SendSuggestionsEmail(_loggedInUserEmail, "Suas Recomendações", sb.ToString());
                MessageBox.Show("Email enviado com sucesso!", "Email", MessageBoxButton.OK, MessageBoxImage.Information);

                await GeneratePdfReport(topMovies, topBooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar email e gerar PDF: " + ex.Message,
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- Novas Implementações para Correção dos Erros ---

        // Atualiza as recomendações de filmes com base na avaliação do filme.
        private async Task UpdateRecommendations(Movie movie)
        {
            // Exemplo: utiliza o serviço de recomendação para obter sugestões com base no título e gênero.
            var recommendedMovies = _movieService.RecommendMoviesByContent(movie.Title, movie.Genre, topN: 5);
            _view.RecommendationsItemsControl.ItemsSource = recommendedMovies;
            await Task.CompletedTask;
        }

        // Atualiza as recomendações de livros com base na avaliação do livro.
        private async Task UpdateBookRecommendations(Book book)
        {
            // Exemplo: utiliza a busca por livros baseada no autor do livro.
            var recommendedBooks = await SearchBooksByAuthorAsync(book.Authors);
            _view.BooksRecommendationsItemsControl.ItemsSource = recommendedBooks;
        }

        // --- Fim das Novas Implementações ---

        #region Métodos de Busca de Filmes (OMDb/TMDb)
        private async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                query = "Star Wars";
            try
            {
                var url = $"{OmdbApiUrl}?apikey={OmdbApiKey}&s={Uri.EscapeDataString(query)}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var omdbResult = JsonSerializer.Deserialize<OmdbSearchResult>(json, options);
                    if (omdbResult != null && omdbResult.Response == "True" && omdbResult.Search != null)
                    {
                        var moviesList = omdbResult.Search.Take(16).ToList();
                        var tasks = moviesList.Select(async m =>
                        {
                            m.Rating = 0;
                            m.Genre = await GetMovieGenreAsync(m.imdbID);
                        });
                        await Task.WhenAll(tasks);
                        return moviesList;
                    }
                    else
                    {
                        return await SearchMoviesTmdbAsync(query);
                    }
                }
                else
                {
                    MessageBox.Show("Erro na conexão com a API. Código: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar filmes (OMDb): " + ex.Message);
            }
            return new List<Movie>();
        }

        private async Task<List<Movie>> SearchMoviesTmdbAsync(string query)
        {
            try
            {
                string url = $"{TMDbBaseUrl}search/movie?api_key={TMDbApiKey}&query={Uri.EscapeDataString(query)}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var tmdbResult = JsonSerializer.Deserialize<TMDbSearchResult>(json, options);
                    if (tmdbResult != null && tmdbResult.Results != null)
                    {
                        List<Movie> movies = new List<Movie>();
                        foreach (var result in tmdbResult.Results.Take(16))
                        {
                            movies.Add(new Movie
                            {
                                Title = result.Title,
                                Year = !string.IsNullOrEmpty(result.ReleaseDate) ? result.ReleaseDate.Split('-')[0] : "",
                                imdbID = result.Id.ToString(),
                                Poster = string.IsNullOrEmpty(result.PosterPath) ? "" : "https://image.tmdb.org/t/p/w500" + result.PosterPath,
                                Genre = "Não definido",
                                Rating = 0
                            });
                        }
                        return movies;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar filmes (TMDb): " + ex.Message);
            }
            return new List<Movie>();
        }

        private async Task<List<Movie>> DiscoverMoviesByGenreAsync(string genre)
        {
            var genreMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Action", "28" },
                { "Adventure", "12" },
                { "Animation", "16" },
                { "Comedy", "35" },
                { "Crime", "80" },
                { "Documentary", "99" },
                { "Drama", "18" },
                { "Family", "10751" },
                { "Fantasy", "14" },
                { "History", "36" },
                { "Horror", "27" },
                { "Music", "10402" },
                { "Mystery", "9648" },
                { "Romance", "10749" },
                { "Science Fiction", "878" },
                { "TV Movie", "10770" },
                { "Thriller", "53" },
                { "War", "10752" },
                { "Western", "37" }
            };

            if (!genreMap.ContainsKey(genre))
            {
                return await SearchMoviesTmdbAsync(genre);
            }

            string genreId = genreMap[genre];
            try
            {
                string url = $"{TMDbBaseUrl}discover/movie?api_key={TMDbApiKey}&with_genres={genreId}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var tmdbResult = JsonSerializer.Deserialize<TMDbSearchResult>(json, options);
                    if (tmdbResult != null && tmdbResult.Results != null)
                    {
                        List<Movie> movies = new List<Movie>();
                        foreach (var result in tmdbResult.Results.Take(16))
                        {
                            movies.Add(new Movie
                            {
                                Title = result.Title,
                                Year = !string.IsNullOrEmpty(result.ReleaseDate) ? result.ReleaseDate.Split('-')[0] : "",
                                imdbID = result.Id.ToString(),
                                Poster = string.IsNullOrEmpty(result.PosterPath) ? "" : "https://image.tmdb.org/t/p/w500" + result.PosterPath,
                                Genre = genre,
                                Rating = 0
                            });
                        }
                        return movies;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao descobrir filmes por gênero (TMDb): " + ex.Message);
            }
            return new List<Movie>();
        }

        private async Task<string> GetMovieGenreAsync(string imdbID)
        {
            try
            {
                var url = $"{OmdbApiUrl}?apikey={OmdbApiKey}&i={imdbID}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var detail = JsonSerializer.Deserialize<MovieDetailResult>(json, options);
                    if (detail != null)
                        return detail.Genre;
                }
            }
            catch { }
            return string.Empty;
        }

        private async Task<string> GetMovieSynopsisAsync(string imdbID)
        {
            try
            {
                string url = $"{OmdbApiUrl}?apikey={OmdbApiKey}&i={imdbID}&plot=full";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var detail = JsonSerializer.Deserialize<MovieDetailResult>(json);
                    if (detail != null)
                        return detail.Plot;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar sinopse: " + ex.Message);
            }
            return "Sinopse não disponível.";
        }
        #endregion

        #region Métodos de Busca de Livros
        private async Task<List<Book>> SearchBooksAsync(string query)
        {
            string url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}";
            var books = new List<Book>();
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<OpenLibraryResult>(json, options);
                    if (result != null && result.Docs != null)
                    {
                        foreach (var item in result.Docs.Take(16))
                        {
                            string coverUrl = item.CoverId.HasValue
                                ? $"https://covers.openlibrary.org/b/id/{item.CoverId.Value}-M.jpg"
                                : "";
                            string defaultGenre = "Não definido";
                            if (!string.IsNullOrWhiteSpace(item.Title))
                            {
                                if (item.Title.IndexOf("Rich Dad", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    item.Title.IndexOf("Pai Rico", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    defaultGenre = "Financeiro";
                                }
                            }
                            books.Add(new Book
                            {
                                Title = item.Title,
                                Authors = item.AuthorName != null ? string.Join(", ", item.AuthorName) : "Desconhecido",
                                ImageUrl = coverUrl,
                                Rating = 0,
                                Genre = defaultGenre
                            });
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Erro na conexão com a API da Open Library. Código: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar livros: " + ex.Message);
            }
            return books;
        }

        private async Task<List<Book>> SearchBooksByAuthorAsync(string author)
        {
            string url = $"https://openlibrary.org/search.json?author={Uri.EscapeDataString(author)}";
            var books = new List<Book>();
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<OpenLibraryResult>(json, options);
                    if (result != null && result.Docs != null)
                    {
                        foreach (var item in result.Docs.Take(16))
                        {
                            string coverUrl = item.CoverId.HasValue
                                ? $"https://covers.openlibrary.org/b/id/{item.CoverId.Value}-M.jpg"
                                : "";
                            books.Add(new Book
                            {
                                Title = item.Title,
                                Authors = item.AuthorName != null ? string.Join(", ", item.AuthorName) : "Desconhecido",
                                ImageUrl = coverUrl,
                                Rating = 0,
                                Genre = "Não definido"
                            });
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Erro na conexão com a API da Open Library (por autor). Código: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar livros por autor: " + ex.Message);
            }
            return books;
        }
        #endregion

        #region Atualizar Trailer
        private async Task UpdateTrailerForMovieAsync(string title)
        {
            string tmdbId = await GetTMDbMovieIdAsync(title);
            if (!string.IsNullOrEmpty(tmdbId))
            {
                string trailerKey = await GetTMDbTrailerKeyAsync(tmdbId);
                if (!string.IsNullOrEmpty(trailerKey))
                {
                    await _view.TrailerWebView2.EnsureCoreWebView2Async(null);
                    _view.TrailerWebView2.CoreWebView2.Navigate($"https://www.youtube.com/embed/{trailerKey}?autoplay=1");
                }
            }
        }

        private async Task<string> GetTMDbMovieIdAsync(string title)
        {
            try
            {
                string url = $"{TMDbBaseUrl}search/movie?api_key={TMDbApiKey}&query={Uri.EscapeDataString(title)}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var searchResult = JsonSerializer.Deserialize<TMDbSearchResult>(json, options);
                    if (searchResult != null && searchResult.Results != null && searchResult.Results.Count > 0)
                        return searchResult.Results[0].Id.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar TMDb ID: " + ex.Message);
            }
            return null;
        }

        private async Task<string> GetTMDbTrailerKeyAsync(string tmdbId)
        {
            try
            {
                string url = $"{TMDbBaseUrl}movie/{tmdbId}/videos?api_key={TMDbApiKey}&language=en-US";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var videosResult = JsonSerializer.Deserialize<TMDbVideosResult>(json, options);
                    if (videosResult != null && videosResult.Results != null)
                    {
                        var trailer = videosResult.Results.FirstOrDefault(v =>
                            v.Type.Equals("Trailer", StringComparison.OrdinalIgnoreCase) &&
                            v.Site.Equals("YouTube", StringComparison.OrdinalIgnoreCase));
                        if (trailer != null)
                            return trailer.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar trailer do TMDb: " + ex.Message);
            }
            return null;
        }
        #endregion

        #region Recomendações Baseadas em Conteúdo (ML.NET)
        private void ShowContentBasedRecommendations()
        {
            var recommendedMovies = _movieService.RecommendMoviesByContent("star", "action", topN: 3);
            if (recommendedMovies.Any())
            {
                StringBuilder recommendationMessage = new StringBuilder();
                recommendationMessage.AppendLine("ML.NET Avisa: Você também pode gostar destes filmes:\n");
                foreach (var movie in recommendedMovies)
                {
                    recommendationMessage.AppendLine($"{movie.Title} - {movie.Genres}");
                }
                MLNetRecommendationWindow recommendationWindow = new MLNetRecommendationWindow(recommendationMessage.ToString());
                recommendationWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nenhum filme recomendado encontrado com os critérios.", "Recomendação");
            }
        }
        #endregion

        #region Geração de PDF
        // Adaptado para ser assíncrono, usando await para baixar as imagens
        private async Task GeneratePdfReport(List<Movie> movies, List<Book> books)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Relatório de Recomendações - Grupo 27";

            XFont titleFont = new XFont("Arial", 20);
            XFont subTitleFont = new XFont("Arial", 16);
            XFont textFont = new XFont("Arial", 14);

            XColor bgColor = XColor.FromArgb(222, 184, 135);
            string filename = $"RelatorioRecomendacoes_{DateTime.Now.Ticks}.pdf";

            // Página para Filmes
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawRectangle(new XSolidBrush(bgColor), new XRect(0, 0, page.Width, page.Height));

            double yPoint = 40;
            gfx.DrawString("Grupo 27", titleFont, XBrushes.Black,
                new XRect(40, yPoint, page.Width - 80, 30), XStringFormats.TopCenter);
            yPoint += 40;

            gfx.DrawString("Filmes Recomendados", subTitleFont, XBrushes.Black,
                new XRect(40, yPoint, page.Width - 80, 30), XStringFormats.TopLeft);
            yPoint += 40;

            foreach (var movie in movies)
            {
                string info = $"{movie.Title} ({movie.Year}) - {movie.Genre}";
                XImage img = null;
                if (!string.IsNullOrEmpty(movie.Poster))
                {
                    try
                    {
                        byte[] imageData = await _httpClient.GetByteArrayAsync(movie.Poster);
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            img = XImage.FromStream(ms);
                        }
                    }
                    catch
                    {
                        try { img = XImage.FromFile("Resources/poster_lds.png"); }
                        catch { }
                    }
                }
                else
                {
                    try { img = XImage.FromFile("Resources/poster_lds.png"); }
                    catch { }
                }

                float offsetXImage = 40f;
                float offsetYImage = (float)yPoint;
                float offsetXText = offsetXImage + 110f;
                float offsetYText = offsetYImage;

                if (img != null)
                {
                    gfx.DrawImage(img, offsetXImage, offsetYImage, 100, 150);
                }
                else
                {
                    gfx.DrawRectangle(XPens.Black, offsetXImage, offsetYImage, 100, 150);
                    gfx.DrawString("Imagem indisponível", textFont, XBrushes.Black,
                        new XRect(offsetXImage, offsetYImage + 60, 100, 30), XStringFormats.Center);
                }
                gfx.DrawString(info, textFont, XBrushes.Black,
                    new XRect(offsetXText, offsetYText, page.Width - offsetXText - 40, 150),
                    XStringFormats.TopLeft);
                yPoint += 160;
                if (yPoint > page.Height - 100)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawRectangle(new XSolidBrush(bgColor), new XRect(0, 0, page.Width, page.Height));
                    yPoint = 40;
                }
            }

            // Página para Livros
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            gfx.DrawRectangle(new XSolidBrush(bgColor), new XRect(0, 0, page.Width, page.Height));
            yPoint = 40;
            gfx.DrawString("Livros Recomendados", subTitleFont, XBrushes.Black,
                new XRect(40, yPoint, page.Width - 80, 30), XStringFormats.TopLeft);
            yPoint += 40;

            foreach (var book in books)
            {
                string info = $"{book.Title} - {book.Authors}";
                XImage img = null;
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    try
                    {
                        byte[] imageData = await _httpClient.GetByteArrayAsync(book.ImageUrl);
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            img = XImage.FromStream(ms);
                        }
                    }
                    catch
                    {
                        try { img = XImage.FromFile("Resources/poster_lds.png"); }
                        catch { }
                    }
                }
                else
                {
                    try { img = XImage.FromFile("Resources/poster_lds.png"); }
                    catch { }
                }

                float offsetXImage = 40f;
                float offsetYImage = (float)yPoint;
                float offsetXText = offsetXImage + 110f;
                float offsetYText = offsetYImage;

                if (img != null)
                {
                    gfx.DrawImage(img, offsetXImage, offsetYImage, 100, 150);
                }
                else
                {
                    gfx.DrawRectangle(XPens.Black, offsetXImage, offsetYImage, 100, 150);
                    gfx.DrawString("Imagem indisponível", textFont, XBrushes.Black,
                        new XRect(offsetXImage, offsetYImage + 60, 100, 30), XStringFormats.Center);
                }
                gfx.DrawString(info, textFont, XBrushes.Black,
                    new XRect(offsetXText, offsetYText, page.Width - offsetXText - 40, 150),
                    XStringFormats.TopLeft);
                yPoint += 160;
                if (yPoint > page.Height - 100)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawRectangle(new XSolidBrush(bgColor), new XRect(0, 0, page.Width, page.Height));
                    yPoint = 40;
                }
            }

            document.Save(filename);
            Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
        }
        #endregion
    }

    // Classes de suporte para ML.NET
    public class MovieRatingData
    {
        public uint UserId { get; set; }
        public uint MovieId { get; set; }
        public float Label { get; set; }
    }
    public class MovieRatingPrediction
    {
        [ColumnName("Score")]
        public float PredictedRating { get; set; }
    }
}
