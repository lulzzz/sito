using Maddalena.Datastore.Mongolino;

namespace Maddalena.Grains.Learning
{
    class MongoModel : CollectionItem
    {
        public string Name { get; set; }
        public string Json { get; set; }
    }
}
