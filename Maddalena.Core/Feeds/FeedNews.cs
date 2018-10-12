using Maddalena.Core.Mongo;
using System;

namespace Maddalena.Core.Feeds
{
    public class FeedNews : MongoObject
    {
        public string Title { get; set; }

        public DateTimeOffset Published { get; set; }

        public string Description { get; set; }

        public string[] Categories { get; set; }

        public string[] Contributors { get; set; }

        public string FeedId { get; set; }

        public string FullText
        {
            get => $"{Title} {Description}";
            set { }
        }
    }
}