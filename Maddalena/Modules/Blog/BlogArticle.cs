using DnsClient.Protocol;
using Maddalena.Identity;
using Maddalena.Mongo;

namespace Maddalena.Modules.Blog
{
    public class BlogArticle : DBObject<BlogArticle>
    {
        public ObjectRef<ApplicationUser> User { get; set; }

        public string Name { get; set; }

        public string Link { get; internal set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string[] Tags { get; set; }

        public bool Visible { get; set; }

        public decimal Views { get; set; }
        
    }
}
