using Maddalena.Client;
using System;
using System.Collections.Generic;

namespace Maddalena.Datastorage.Data
{
    [Serializable]
    class MongoNews : MongoBaseObject
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        public string[] Categories { get; set; }

        public string Link { get; set; }

        public string Source { get; set; }

        public SourceType SourceType { get; set; }

        public List<string> Good { get; set; }

        public List<string> Bad { get; set; }
    }
}