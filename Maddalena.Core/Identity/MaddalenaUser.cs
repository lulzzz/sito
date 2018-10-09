using AspNetCore.Identity.Mongo.Model;

namespace Maddalena.Core.Identity
{
    public class MaddalenaUser  : MongoUser
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
