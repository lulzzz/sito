using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using numl.Serialization;
using numl.Supervised;
using numl.Utils;
using System.Threading.Tasks;
using Maddalena.News.Grains.Libraries.Mongolino;

namespace AccountTransfer.Grains
{
    class Settings
    {
        static Settings()
        {
            Ject.AddAssembly(typeof(Settings).Assembly);
        }
    }
}
