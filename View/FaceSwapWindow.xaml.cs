using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

// Cria um alias para evitar ambiguidade: queremos a Window do WPF
using WpfWindow = System.Windows.Window;

namespace MovieBookRecommendation
{
    public partial class FaceSwapWindow : WpfWindow
    {
        private Mat userImageMat = null;

        // Classe simples para representar um poster
        public class Poster
        {
            public string Category { get; set; }
            public string ImagePath { get; set; }
        }

        private List<Poster> posters;

        public FaceSwapWindow()
        {
            InitializeComponent();
            LoadPosters();
        }

        private void LoadPosters()
        {
            // Caminho para a pasta "posters" na pasta de saída
            string postersDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "posters");
            // Cria uma lista de posters com suas categorias correspondentes
            posters = new List<Poster>
            {
                new Poster { Category = "terror", ImagePath = Path.Combine(postersDir, "poster_terror.jpg") },
                new Poster { Category = "acao", ImagePath = Path.Combine(postersDir, "poster_acao.jpg") },
                new Poster { Category = "suspense", ImagePath = Path.Combine(postersDir, "poster_suspense.jpg") },
                new Poster { Category = "drama", ImagePath = Path.Combine(postersDir, "poster_drama.jpg") },
                new Poster { Category = "romance", ImagePath = Path.Combine(postersDir, "poster_romance.jpg") },
                new Poster { Category = "comedia", ImagePath = Path.Combine(postersDir, "poster_comedia.jpg") }
            };
            PosterListBox.ItemsSource = posters;
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Imagens (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    Mat src = Cv2.ImRead(dlg.FileName);
                    if (src.Empty())
                    {
                        MessageBox.Show("Não foi possível carregar a imagem.");
                        return;
                    }
                    userImageMat = src;
                    ImgUser.Source = src.ToBitmapSource();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar a imagem: " + ex.Message);
                }
            }
        }

        private void PosterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userImageMat == null)
            {
                MessageBox.Show("Por favor, faça upload de uma imagem primeiro.");
                return;
            }

            if (PosterListBox.SelectedItem is Poster selectedPoster)
            {
                try
                {
                    // Chama o FaceSwapService usando a categoria do poster selecionado
                    Mat result = Services.FaceSwapService.PerformFaceSwap(userImageMat, selectedPoster.Category);
                    ImgResult.Source = result.ToBitmapSource();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao aplicar FaceSwap: " + ex.Message);
                }
            }
        }
    }
}
