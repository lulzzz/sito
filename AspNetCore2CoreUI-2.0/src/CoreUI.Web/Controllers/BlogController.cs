using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Core.Blog.Models;
using Maddalena.Core.Blog.Services;
using Maddalena.Core.GridFs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreUI.Web.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogService _blog;
        private readonly IGridFileSystem _gridFs;

        public BlogController(IBlogService blog, IGridFileSystem gridFs, IBlogSettings settings)
        {
            _blog = blog;
            _gridFs = gridFs;
        }

        [Route("/blog/upload")]
        public async Task<ActionResult> BlogUpload()
        {
            var file = Request.Form.Files[0];
            var gridName = await _gridFs.Upload(file.FileName, file.OpenReadStream());

            await _gridFs.SetOwner(gridName,User.Identity.Name);
            await _gridFs.MakePublic(gridName);

            return Json(new { location = gridName });
        }

        [AllowAnonymous]
        [Route("/read/{slug?}")]
        public async Task<IActionResult> Read(string slug)
        {
            var post = await _blog.GetPostBySlug(slug);

            if (post != null)
            {
                return View(post);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/blog/edit/{id?}")]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["AllCats"] = (await _blog.GetCategories()).ToList();

            if (string.IsNullOrEmpty(id))
            {
                return View(new Post());
            }

            var post = await _blog.GetPostById(id);

            if (post != null)
            {
                return View(post);
            }

            return NotFound();
        }

        [Route("/blog/{slug?}")]
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdatePost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", post);
            }

            var existing = await _blog.GetPostById(post.Id) ?? post;
            string categories = Request.Form["categories"];

            existing.Categories = categories.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToLowerInvariant()).ToList();
            existing.Title = post.Title.Trim();
            existing.Slug = post.Slug;
            existing.IsPublished = post.IsPublished;
            existing.Content = post.Content.Trim();
            existing.Excerpt = post.Excerpt.Trim();

            await _blog.SavePost(existing);

            return Redirect($"/read/{post.Slug}");
        }

        [Route("/blog/deletepost/{id}")]
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(string id)
        {
            var existing = await _blog.GetPostById(id);

            if (existing != null)
            {
                await _blog.DeletePost(existing);
                return Redirect("/");
            }

            return NotFound();
        }
    }
}
