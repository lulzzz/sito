using System;
using System.Collections.Generic;
using System.Text;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Youtube
{
    public class SubtitledVideo : MongoObject
    {
        public string Title { get; set; }

        public string YoutubeId { get; set; }

        public string HtmlBody { get; set; }

        public string TextBody { get; set; }

        public string YoutubeUrl { get; set; }

        public string Thumbnail { get; set; }

        public string VideoSourceUrl { get; set; }

        public DateTime Published { get; set; }
    }
}
