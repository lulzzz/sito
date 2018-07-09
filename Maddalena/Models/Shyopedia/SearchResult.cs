using System;

namespace Maddalena.Models.Shyopedia
{
    public class SearchResult
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string Thumbnail { get; set; }

        public DateTime Published { get; set; }

        public TimeSpan OffSet { get; set; }

        public string Text { get; set; }
    }
}
