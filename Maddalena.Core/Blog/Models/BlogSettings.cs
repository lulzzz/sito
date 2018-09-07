using Maddalena.Core.Mongo;

namespace Maddalena.Core.Blog.Services
{
    public class BlogSettings : MongoObject, IBlogSettings
    {
        public int CommentsCloseAfterDays { get; set; }
        public int PostsPerPage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
    }
}
