using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace MovieBookRecommendation.Controllers
{
    public class FaceSwapController
    {
        private Mat userImage;
        private List<FaceSwapWindow.Poster> posters;

        public FaceSwapController()
        {
            // Construtor – não precisa de lógica especial aqui, a lista de posters será carregada pelo método abaixo.
        }

        public List<FaceSwapWindow.Poster> LoadPosters()
        {
            // Define o caminho para a pasta "posters" relativa à pasta de saída do app
            string postersDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "posters");

            posters = new List<FaceSwapWindow.Poster>
            {
                new FaceSwapWindow.Poster { Category = "terror",   ImagePath = Path.Combine(postersDir, "poster_terror.jpg") },
                new FaceSwapWindow.Poster { Category = "acao",     ImagePath = Path.Combine(postersDir, "poster_acao.jpg") },
                new FaceSwapWindow.Poster { Category = "suspense", ImagePath = Path.Combine(postersDir, "poster_suspense.jpg") },
                new FaceSwapWindow.Poster { Category = "drama",    ImagePath = Path.Combine(postersDir, "poster_drama.jpg") },
                new FaceSwapWindow.Poster { Category = "romance",  ImagePath = Path.Combine(postersDir, "poster_romance.jpg") },
                new FaceSwapWindow.Poster { Category = "comedia",  ImagePath = Path.Combine(postersDir, "poster_comedia.jpg") }
            };

            return posters;
        }

        public void SetUserImage(Mat image)
        {
            userImage = image;
        }

        public Mat PerformFaceSwap(string posterCategory)
        {
            if (userImage == null)
            {
                throw new Exception("Nenhuma imagem de usuário definida.");
            }

            // Chama o serviço de face swap utilizando a imagem do usuário e a categoria do poster
            return Services.FaceSwapService.PerformFaceSwap(userImage, posterCategory);
        }
    }
}
