using System.Windows;

namespace MovieBookRecommendation
{
    public partial class MLNetRecommendationWindow : Window
    {
        public MLNetRecommendationWindow(string recommendations)
        {
            InitializeComponent();
            RecommendationsTextBlock.Text = recommendations;
        }
    }
}
