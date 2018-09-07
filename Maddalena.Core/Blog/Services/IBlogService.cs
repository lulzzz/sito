using System.Collections.Generic;
using System.Threading.Tasks;
using Maddalena.Core.Blog.Models;

namespace Maddalena.Core.Blog.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<Post>> GetPosts(int count, int skip = 0);

        Task<IEnumerable<Post>> GetPostsByCategory(string category);

        Task<Post> GetPostBySlug(string slug);

        Task<Post> GetPostById(string id);

        Task<IEnumerable<string>> GetCategories();

        Task SavePost(Post post);

        Task DeletePost(Post post);
        Task<string> SaveFile(byte[] bytes, string value);
    }
}
