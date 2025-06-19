using System.Collections.Generic;

namespace MovieBookRecommendation.Models
{
    public class GoogleBooksResult
    {
        public List<GoogleBookItem> Items { get; set; }
    }

    public class GoogleBookItem
    {
        public VolumeInfo VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public ImageLinks ImageLinks { get; set; }
    }

    public class ImageLinks
    {
        public string Thumbnail { get; set; }
    }
}
