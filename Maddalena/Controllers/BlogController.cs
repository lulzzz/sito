using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Identity;
using Maddalena.Modules.Blog;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    [Authorize(Roles = "admin")]
    public class BlogController : Controller
    {
        private static readonly MarkdownPipeline pipeline;

        static BlogController()
        {
            BlogArticle.DescendingIndex(x => x.Link);
            BlogArticle.DescendingIndex(x=>x.DateTime);

            pipeline = (new MarkdownPipelineBuilder()).UseAbbreviations()
                .UseAbbreviations()
                .UseAutoIdentifiers()
                .UseCitations()
                .UseCustomContainers()
                .UseDefinitionLists()
                .UseEmphasisExtras()
                .UseFigures()
                .UseFooters()
                .UseFootnotes()
                .UseGridTables()
                .UseMathematics()
                .UsePipeTables()
                .UseListExtras()
                .UseTaskLists()
                .UseDiagrams()
                .UseAutoLinks()
                .UseGenericAttributes()
                .UseBootstrap()
                .UseMediaLinks(new Markdig.Extensions.MediaLinks.MediaOptions
                {
                    Width = "",
                    Class = "",
                    Height = "embed-responsive-item"
                })
                .UseNoFollowLinks()
                .UsePragmaLines()
                .UseSmartyPants()
                .Build();
        }

        // GET: Blog
        public ActionResult Index() => View(BlogArticle.Queryable().OrderByDescending(x=>x.DateTime).Take(20));

        // GET: Blog/Create
        public ActionResult Create() => View();

        // GET: Blog/Delete/5
        public ActionResult Delete(int id) => View();


        // GET: Blog/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Read(string id)
        {
            var article = await BlogArticle.FirstOrDefaultAsync(x => x.Link == id);
            if (article == null) return NotFound();

            //await BlogArticle.IncreaseAsync(article, x => x.Views, 1);

            return View(article);
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BlogArticle article)
        {
            try
            {
                article.Id = string.Empty;
                article.DateTime = DateTime.Now;
                article.Author = await ApplicationUser.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                await BlogArticle.CreateAsync(article);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Blog/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var article = await BlogArticle.FirstOrDefaultAsync(x => x.Link == id);
            if (article == null) return NotFound();

            //await BlogArticle.IncreaseAsync(article, x => x.Views, 1);

            return View(article);
        }


        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BlogArticle article)
        {
            try
            {
                await BlogArticle.ReplaceAsync(article);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Blog/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await BlogArticle.DeleteAsync(x => x.Id == id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}