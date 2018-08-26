using Maddalena.Client;
using Maddalena.Datastorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Maddalena.Controllers
{
    public class FeedController : Controller
    {
        // GET: Feed
        public ActionResult Index()
        {
            return View(Datastore.Feed.Feeds);
        }

        // GET: Feed/Details/5
        public async Task<ActionResult> Details(string id)
        {
            return View(await Datastore.Feed.Get(id));
        }

        // GET: Feed/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Feed/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Feed feed)
        {
            try
            {
                await Datastore.Feed.Create(feed);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Feed/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            return View(await Datastore.Feed.Get(id));
        }

        // POST: Feed/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Feed feed)
        {
            try
            {
                await Datastore.Feed.Update(feed);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Feed/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            return View(await Datastore.Feed.Get(id));
        }

        // POST: Feed/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Feed feed)
        {
            try
            {
                await Datastore.Feed.Delete(feed);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}