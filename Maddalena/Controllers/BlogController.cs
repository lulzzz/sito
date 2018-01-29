using System.Threading.Tasks;
using Maddalena.Modules.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index() => View();

        // GET: Blog/Edit/5
        public ActionResult Edit(int id) => View();

        // GET: Blog/Create
        public ActionResult Create() => View();

        // GET: Blog/Delete/5
        public ActionResult Delete(int id) => View();


        // GET: Blog/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var article = await BlogArticle.FirstOrDefaultAsync(x => x.Link == id);
            if (article == null) return NotFound();

            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

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