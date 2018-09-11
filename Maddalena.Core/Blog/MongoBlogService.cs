using System.Collections.Generic;
using System.Threading.Tasks;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Blog
{
    public class MongoBlogService : IBlogService
    {
        private readonly MongoObjectCollection<BlogPost> _post;

        public MongoBlogService(string connectionString)
        {
            _post = new MongoObjectCollection<BlogPost>(connectionString, "blog");
        }

        public Task<IEnumerable<BlogPost>> GetPosts(int count, int skip = 0) => _post.TakeAsync(count, skip);

        public Task<IEnumerable<BlogPost>> GetPostsByCategory(string category) => _post.WhereAsync(x => x.Category == category);

        public Task<BlogPost> GetPostBySlug(string slug) => _post.FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<BlogPost> GetPostById(string id) => _post.FirstOrDefaultAsync(x => x.Id == id);

        public Task<IEnumerable<string>> GetCategories() => Task.FromResult(new[] {"Code"} as IEnumerable<string>);

        public Task SavePost(BlogPost post) => _post.CreateAsync(post);

        public Task DeletePost(BlogPost post) => _post.DeleteAsync(post);
    }
}