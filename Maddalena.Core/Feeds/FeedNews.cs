using Maddalena.Core.Mongo;
using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Maddalena.Core.Feeds
{
    public class FeedNews : MongoObject
    {
        public string Title { get; internal set; }
        public DateTimeOffset Published { get; internal set; }
        public string Description { get; internal set; }
        public IEnumerable<ISyndicationCategory> Categories { get; internal set; }
        public IEnumerable<ISyndicationPerson> Contributors { get; internal set; }
    }
}