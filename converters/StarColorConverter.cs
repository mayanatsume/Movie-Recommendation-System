using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MovieBookRecommendation.Converters
{
    public class StarColorConverter : IValueConverter
    {
        // Retorna Gold se a classificação for maior ou igual ao valor da estrela; caso contrário, Gray.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating = (int)value;
            int starValue = int.Parse(parameter.ToString());
            return rating >= starValue ? Brushes.Gold : Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
