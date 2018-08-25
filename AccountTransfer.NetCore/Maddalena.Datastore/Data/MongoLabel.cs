using Maddalena.Client;

namespace Maddalena.Datastorage.Data
{
    class MongoLabel : MongoBaseObject
    {
        public string Label { get; set; }

        public LabelValue LabelValue { get; set; }

        public string NewsId { get; set; }
    }
}
