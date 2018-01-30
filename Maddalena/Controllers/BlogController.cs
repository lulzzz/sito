using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Identity;
using Maddalena.Modules.Blog;
using Markdig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class BlogController : Controller
    {
        private static readonly MarkdownPipeline pipeline;

        static BlogController()
        {
            BlogArticle.DescendingIndex(x => x.Link);
            BlogArticle.DescendingIndex(x=>x.DateTime);

            pipeline = (new MarkdownPipelineBuilder()).UseAbbreviations()
                .UseAdvancedExtensions()
                .UseAutoIdentifiers()
                .UseBootstrap()
                .UseMathematics()
                .UseMediaLinks()
                .UseNoFollowLinks()
                .UsePragmaLines()
                .UseSmartyPants()
                .Build();
        }

        // GET: Blog
        public ActionResult Index() => View(BlogArticle.Queryable().OrderByDescending(x=>x.DateTime).Take(20));

        // GET: Blog/Edit/5
        public ActionResult Edit(int id) => View();

        // GET: Blog/Create
        public ActionResult Create() => View();

        // GET: Blog/Delete/5
        public ActionResult Delete(int id) => View();


        // GET: Blog/Details/5
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
                article.RenderedBody = Markdown.ToHtml(article.Body, pipeline);

                await BlogArticle.CreateAsync(article);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

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