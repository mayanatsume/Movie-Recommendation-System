using OpenCvSharp;
using System;
using System.IO;
// Se quiser evitar ambiguidade com System.Windows:
using CvRect = OpenCvSharp.Rect;
using CvPoint = OpenCvSharp.Point;

namespace MovieBookRecommendation.Services
{
    public static class FaceSwapService
    {
        // Arquivo Haar Cascade para DETECÇÃO de rostos (usuário e pôster)
        // Ex: "posters\\haar.xml" ou "posters\\haarcascade_frontalface_default.xml"
        private const string HaarCascadePath = "haar.xml";

        /// <summary>
        /// Expande um Rect (rosto detectado) por um fator, mantendo-o dentro dos limites.
        /// Útil para evitar que o rosto fique muito pequeno.
        /// </summary>
        private static Rect ExpandRect(Rect faceRect, double scaleFactor, int maxWidth, int maxHeight)
        {
            // Aumenta a largura e altura pela porcentagem "scaleFactor"
            int newWidth = (int)(faceRect.Width * scaleFactor);
            int newHeight = (int)(faceRect.Height * scaleFactor);

            // Move X e Y para manter o retângulo centralizado
            int newX = faceRect.X - (newWidth - faceRect.Width) / 2;
            int newY = faceRect.Y - (newHeight - faceRect.Height) / 2;

            // Garante que não saia da imagem
            if (newX < 0) newX = 0;
            if (newY < 0) newY = 0;
            if (newX + newWidth > maxWidth) newWidth = maxWidth - newX;
            if (newY + newHeight > maxHeight) newHeight = maxHeight - newY;

            return new Rect(newX, newY, newWidth, newHeight);
        }

        /// <summary>
        /// Faz o blending do rosto oval do usuário sobre o pôster,
        /// usando máscara para evitar quadrados pretos.
        /// </summary>
        private static void OverlayWithMask(Mat poster, Mat userOvalFace, Rect targetRect)
        {
            // Ajusta o tamanho do userOvalFace para targetRect
            using (Mat resizedFace = new Mat())
            {
                Cv2.Resize(userOvalFace, resizedFace, new OpenCvSharp.Size(targetRect.Width, targetRect.Height));

                // Cria a máscara a partir do que não é preto no resizedFace
                using (Mat faceGray = new Mat())
                using (Mat mask = new Mat())
                using (Mat maskInv = new Mat())
                {
                    // Converte para escala de cinza
                    Cv2.CvtColor(resizedFace, faceGray, ColorConversionCodes.BGR2GRAY);
                    // Tudo que for > 10 de intensidade vira branco (255)
                    Cv2.Threshold(faceGray, mask, 10, 255, ThresholdTypes.Binary);

                    // Inverte a máscara pra preservar o fundo do pôster
                    Cv2.BitwiseNot(mask, maskInv);

                    // Cria ROI no pôster
                    using (Mat posterROI = new Mat(poster, targetRect))
                    {
                        // Copia o fundo do pôster onde maskInv é branco
                        using (Mat background = new Mat())
                        {
                            posterROI.CopyTo(background, maskInv);

                            // Copia o rosto do usuário onde mask é branco
                            using (Mat foreground = new Mat())
                            {
                                resizedFace.CopyTo(foreground, mask);

                                // Combina background + foreground
                                using (Mat blended = new Mat())
                                {
                                    Cv2.Add(background, foreground, blended);
                                    blended.CopyTo(posterROI);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Detecta o rosto do usuário e recorta em formato oval (máscara elíptica).
        /// </summary>
        public static Mat CreateOvalFace(Mat userImage)
        {
            // Monta o caminho completo do arquivo Haar Cascade
            string fullCascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HaarCascadePath);
            if (!File.Exists(fullCascadePath))
                throw new Exception("Arquivo Haar Cascade não encontrado: " + fullCascadePath);

            // Instancia o cascade
            var cascade = new CascadeClassifier(fullCascadePath);

            using (var gray = new Mat())
            {
                // Converte o userImage para cinza
                Cv2.CvtColor(userImage, gray, ColorConversionCodes.BGR2GRAY);
                // Detecta rostos
                CvRect[] faces = cascade.DetectMultiScale(gray, 1.1, 6);
                if (faces.Length == 0)
                    throw new Exception("Nenhum rosto detectado na imagem do usuário.");

                // Pega o primeiro rosto
                CvRect faceRect = faces[0];
                Mat face = new Mat(userImage, faceRect);

                // Cria uma máscara oval do tamanho do rosto
                Mat mask = new Mat(face.Size(), MatType.CV_8UC1, Scalar.Black);
                Cv2.Ellipse(
                    mask,
                    new CvPoint(face.Width / 2, face.Height / 2),
                    new Size(face.Width / 2, face.Height / 2),
                    0, 0, 360, Scalar.White, -1
                );

                // Aplica a máscara
                Mat faceOval = new Mat();
                face.CopyTo(faceOval, mask);
                return faceOval;
            }
        }

        /// <summary>
        /// Detecta o rosto no pôster e sobrepõe o rosto oval do usuário na posição detectada.
        /// </summary>
        public static Mat PerformFaceSwap(Mat userImage, string category)
        {
            // 1) Escolhe o arquivo do pôster
            string posterFile;
            switch (category.ToLower())
            {
                case "terror":
                    posterFile = "poster_terror.jpg";
                    break;
                case "ação":
                case "acao":
                    posterFile = "poster_acao.jpg";
                    break;
                case "suspense":
                    posterFile = "poster_suspense.jpg";
                    break;
                case "drama":
                    posterFile = "poster_drama.jpg";
                    break;
                case "romance":
                    posterFile = "poster_romance.jpg";
                    break;
                case "comedia":
                    posterFile = "poster_comedia.jpg";
                    break;
                default:
                    throw new Exception("Categoria inválida.");
            }

            // 2) Carrega o pôster
            string posterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "posters", posterFile);
            Mat poster = Cv2.ImRead(posterPath);
            if (poster.Empty())
                throw new Exception("Poster não encontrado: " + posterPath);

            // 3) Cria o rosto oval do usuário
            Mat userFaceOval = CreateOvalFace(userImage);

            // 4) Detecta o rosto no pôster (usando o mesmo Haar Cascade)
            string fullCascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HaarCascadePath);
            if (!File.Exists(fullCascadePath))
                throw new Exception("Arquivo HaarCascade não encontrado: " + fullCascadePath);

            CascadeClassifier posterCascade = new CascadeClassifier(fullCascadePath);

            Rect[] posterFaces;
            using (Mat posterGray = new Mat())
            {
                Cv2.CvtColor(poster, posterGray, ColorConversionCodes.BGR2GRAY);
                Cv2.EqualizeHist(posterGray, posterGray);

                posterFaces = posterCascade.DetectMultiScale(
                    posterGray,
                    1.1,
                    3,
                    HaarDetectionTypes.ScaleImage,
                    new Size(30, 30)
                );
            }

            // 5) Define a região do rosto no pôster (expandindo se quiser)
            Rect targetFaceRect;
            if (posterFaces.Length > 0)
            {
                targetFaceRect = posterFaces[0];
                // Expande 20% para não ficar pequeno
                targetFaceRect = ExpandRect(targetFaceRect, 1.2, poster.Width, poster.Height);
            }
            else
            {
                // Se não detectar, define uma ROI padrão (exemplo, centro)
                int w = poster.Width / 4;
                int h = poster.Height / 4;
                targetFaceRect = new Rect((poster.Width - w) / 2, (poster.Height - h) / 2, w, h);
            }

            // 6) Faz a sobreposição usando máscara (para não ficar quadrado preto)
            OverlayWithMask(poster, userFaceOval, targetFaceRect);

            // Retorna o pôster modificado
            return poster;
        }
    }
}
