using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Maddalena.Modules.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    [Authorize(Roles = "blog")]
    public class BlogController : Controller
    {
        static BlogController()
        {
            BlogArticle.DescendingIndex(x => x.Link);
            BlogArticle.DescendingIndex(x => x.DateTime);
        }

        public ActionResult Index()
        {
            return View(BlogArticle.Queryable().OrderByDescending(x => x.DateTime).Take(20));
        }

        public ActionResult Create()
        {
            return View();
        }

        // GET: Blog/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Read(string link)
        {
            var article = await BlogArticle.FirstOrDefaultAsync(x => x.Link == link);
            if (article == null) return NotFound();

            //await BlogArticle.IncreaseAsync(article, x => x.Views, 1);

            return View(article);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Category(string id)
        {
            var article = await BlogArticle.WhereAsync(x => x.Category == id);
            if (article == null) return NotFound();

            ViewData["Title"] = $"Articles in {id}";

            return View("List", article);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Search(string q)
        {
            var articleList = await BlogArticle.FullTextSearchAsync(q);
            if (articleList == null) return NotFound();

            @ViewData["Title"] = $"Search results for {q}";
            return View("List", articleList);
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BlogArticle article)
        {
            article.Id = string.Empty;
            article.DateTime = DateTime.Now;
            article.Author = User.Identity.Name;

            // From String
            var doc = new HtmlDocument();
            doc.LoadHtml(article.Body);

            article.TextPreview = doc.DocumentNode.InnerText;

            await BlogArticle.CreateAsync(article);

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Edit(string id)
        {
            var article = await BlogArticle.FirstOrDefaultAsync(x => x.Link == id);
            if (article == null) return NotFound();

            await BlogArticle.UpdateAsync(article, x => x.Views, article.Views + 1);

            return View(article);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BlogArticle article)
        {
            article.DateTime = DateTime.Now;
            article.Author = User.Identity.Name;

            // From String
            var doc = new HtmlDocument();
            doc.LoadHtml(article.Body);

            article.TextPreview = doc.DocumentNode.InnerText;

            await BlogArticle.ReplaceAsync(article);

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Delete(string id)
        {
            var article = await BlogArticle.FirstOrDefaultAsync(x => x.Id == id);
            if (article == null) return NotFound();

            return View(article);
        }

        // POST: Blog/Delete/5
        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePost(string id)
        {
            try
            {
                await BlogArticle.DeleteAsync(x => x.Id == id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}