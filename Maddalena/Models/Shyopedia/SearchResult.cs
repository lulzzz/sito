using System;
using System.Collections.Generic;

namespace Maddalena.Models.Shyopedia
{
    public class SearchResult
    {
        public SubVideo Video { get; set; }

        public List<VideoText> Texts { get; set; } = new List<VideoText>();
    }
}
