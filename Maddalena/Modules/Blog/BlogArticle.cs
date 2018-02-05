using System;
using Maddalena.Identity;
using Maddalena.Mongo;

namespace Maddalena.Modules.Blog
{
    public class BlogArticle : DBObject<BlogArticle>
    {
        static BlogArticle()
        {
            DescendingIndex(x => x.Link);
            DescendingIndex(x => x.DateTime);
            DescendingIndex(x => x.Tags);
        }

        public ObjectRef<ApplicationUser> Author { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public DateTime DateTime { get; set; }

        public string Category { get; set; }

        public string Body { get; set; }

        public string[] Tags { get; set; }

        public bool Visible { get; set; }

        public double Views { get; set; }
    }
}
