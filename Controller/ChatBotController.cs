using MovieBookRecommendation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace MovieBookRecommendation.Controllers
{
    public class ChatBotController
    {
        private readonly ChatBotWindow _view;
        private readonly HttpClient _httpClient = new HttpClient();

        // Constantes para OMDb (para obter sinopse)
        private const string OmdbApiKey = "7257bf9f";
        private const string OmdbApiUrl = "http://www.omdbapi.com/";

        // Constantes para TMDb (para buscar filmes), usando pt-PT
        private const string TMDbApiKey = "f743d2f2259e90a115c5f47be669ecc4";
        private const string TMDbBaseUrl = "https://api.themoviedb.org/3/";

        // Filme avaliado anteriormente (passado, por exemplo, pelo MainWindow)
        public Movie LastRatedMovie { get; set; }

        public ChatBotController(ChatBotWindow view)
        {
            _view = view;
        }

        // Exibe a sinopse completa do último filme avaliado
        public async Task ShowSynopsisAsync()
        {
            _view.PanelSinopse.Visibility = Visibility.Visible;
            _view.PanelBusca.Visibility = Visibility.Collapsed;
            _view.TxtSinopse.Text = "";

            if (LastRatedMovie == null)
            {
                _view.TxtSinopse.Text = "Nenhum filme avaliado ainda.";
                return;
            }

            string synopsis = await GetMovieSynopsisAsync(LastRatedMovie.imdbID);
            _view.TxtSinopse.Text = $"{LastRatedMovie.Title}:\n\n{synopsis}";
        }

        // Exibe o painel de busca e oculta o de sinopse
        public void ShowSearchPanel()
        {
            _view.PanelBusca.Visibility = Visibility.Visible;
            _view.PanelSinopse.Visibility = Visibility.Collapsed;
            _view.TxtResultados.Text = "";
        }

        // Processa a busca de filmes pela descrição
        public async Task SearchMoviesByDescriptionAsync()
        {
            string descricao = _view.TxtDescricao.Text;
            if (string.IsNullOrWhiteSpace(descricao))
            {
                _view.TxtResultados.Text = "Por favor, digite uma descrição.";
                return;
            }
            _view.TxtResultados.Text = "Buscando filmes, aguarde...\n";
            var movies = await SearchMoviesAsync(descricao);
            if (movies.Any())
            {
                _view.TxtResultados.Text = "Filmes encontrados:\n";
                foreach (var movie in movies)
                {
                    _view.TxtResultados.Text += $"{movie.Title} ({movie.Year})\n";
                }
            }
            else
            {
                _view.TxtResultados.Text = "Nenhum filme encontrado para essa descrição.";
            }
        }

        // Busca filmes na TMDb utilizando o idioma pt-PT
        private async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            try
            {
                string url = $"{TMDbBaseUrl}search/movie?api_key={TMDbApiKey}&language=pt-PT&query={Uri.EscapeDataString(query)}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var searchResult = JsonSerializer.Deserialize<TMDbSearchResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (searchResult != null && searchResult.Results != null && searchResult.Results.Count > 0)
                    {
                        List<Movie> movies = new List<Movie>();
                        foreach (var result in searchResult.Results.Take(10))
                        {
                            movies.Add(new Movie
                            {
                                Title = result.Title,
                                Year = !string.IsNullOrEmpty(result.ReleaseDate) ? result.ReleaseDate.Split('-')[0] : "",
                                imdbID = result.Id.ToString(),
                                Poster = string.IsNullOrEmpty(result.PosterPath) ? "" : "https://image.tmdb.org/t/p/w500" + result.PosterPath,
                                Genre = "Desconhecido",
                                Rating = 0
                            });
                        }
                        return movies;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca (TMDb): " + ex.Message);
            }
            // Se não houver resultados, utiliza o fallback por palavras-chave
            return await SearchMoviesByKeywordsAsync(query);
        }

        // Fallback: busca filmes por palavras-chave extraídas da descrição
        private async Task<List<Movie>> SearchMoviesByKeywordsAsync(string query)
        {
            List<Movie> allMovies = new List<Movie>();
            string[] stopWords = new string[] { "filme", "com", "de", "e", "a", "o", "os", "as" };
            var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                             .Where(w => !stopWords.Contains(w.ToLower()))
                             .ToArray();

            foreach (var word in words)
            {
                try
                {
                    string url = $"{TMDbBaseUrl}search/movie?api_key={TMDbApiKey}&language=pt-PT&query={Uri.EscapeDataString(word)}";
                    var response = await _httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var searchResult = JsonSerializer.Deserialize<TMDbSearchResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (searchResult != null && searchResult.Results != null)
                        {
                            foreach (var result in searchResult.Results)
                            {
                                if (!allMovies.Any(m => m.Title.Equals(result.Title, StringComparison.OrdinalIgnoreCase)))
                                {
                                    allMovies.Add(new Movie
                                    {
                                        Title = result.Title,
                                        Year = !string.IsNullOrEmpty(result.ReleaseDate) ? result.ReleaseDate.Split('-')[0] : "",
                                        imdbID = result.Id.ToString(),
                                        Poster = string.IsNullOrEmpty(result.PosterPath) ? "" : "https://image.tmdb.org/t/p/w500" + result.PosterPath,
                                        Genre = "Desconhecido",
                                        Rating = 0
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Erro na busca por palavra-chave: " + ex.Message);
                }
            }
            return allMovies;
        }

        // Busca a sinopse completa do filme via OMDb
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
    }

    // Classes auxiliares para mapear a resposta da TMDb
    public class TMDbSearchResult
    {
        public int Page { get; set; }
        public List<TMDbMovie> Results { get; set; }
        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
    }

    public class TMDbMovie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ReleaseDate { get; set; }
        public string PosterPath { get; set; }
    }
}
