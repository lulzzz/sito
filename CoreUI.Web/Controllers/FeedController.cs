using System.Threading.Tasks;
using Maddalena.Core.Feeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreUI.Web.Controllers
{
    [Authorize(Roles = "feed")]
    public class FeedController : Controller
    {
        private readonly IFeedService _feed;

        public FeedController(IFeedService feed)
        {
            _feed = feed;
        }

        // GET: Feed
        public async Task<ActionResult> Index() => View(await _feed.All());

        // GET: Feed/Create
        public ActionResult Create() => View();

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new Feed());
            }

            var post = await _feed.FeedById(id);

            if (post != null)
            {
                return View(post);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Feed feed)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", feed);
            }

            if (string.IsNullOrEmpty(feed.Id))
            {
                await _feed.Create(feed);
            }
            else
            {
                await _feed.Update(feed);
            }

            return Redirect("/feed");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            await _feed.Delete(await _feed.FeedById(id));
            return Redirect("/feed");
        }
    }
}