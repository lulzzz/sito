using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Maddalena.Core.Blog.Models;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Blog.Services
{
    public class MongoBlogService : IBlogService
    {
        private MongoObjectCollection<Post> _post;

        public MongoBlogService(string connectionString)
        {
            _post = new MongoObjectCollection<Post>(connectionString, "blog");
        }

        public Task<IEnumerable<Post>> GetPosts(int count, int skip = 0) => _post.TakeAsync(count, skip);

        public Task<IEnumerable<Post>> GetPostsByCategory(string category) => _post.AnyEqualAsync(x => x.Categories, category);

        public Task<Post> GetPostBySlug(string slug) => _post.FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<Post> GetPostById(string id) => _post.FirstOrDefaultAsync(x => x.Id == id);

        public Task<IEnumerable<string>> GetCategories() => Task.FromResult(new[] {"Code"} as IEnumerable<string>);

        public Task SavePost(Post post) => _post.CreateAsync(post);

        public Task DeletePost(Post post) => _post.DeleteAsync(post);

        public Task<string> SaveFile(byte[] bytes, string value)
        {
            return Task.FromResult(value);
        }
    }
}
