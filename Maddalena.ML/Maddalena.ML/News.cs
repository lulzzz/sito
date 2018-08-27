using System;

namespace Maddalena.ML.DataProvider.Maddalena.Grains.DataProvider
{
    public class News
    {
        public string Source { get; set; }
        public SourceType SourceType { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }

        public DateTime Timestamp { get; set; }
       
        public string[] Categories { get; set; }
    }
}