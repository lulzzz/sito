using System.Threading.Tasks;
using Maddalena.Core.Feeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreUI.Web.Controllers
{
    [Authorize(Roles = "feed")]
    public class FeedsController : Controller
    {
        private readonly IFeedService _feed;

        public FeedsController(IFeedService feed)
        {
            _feed = feed;
        }

        // GET: Feed
        public async Task<ActionResult> Index() => View(await _feed.AllFeed());

        // GET: Feed/Create
        [HttpGet]
        public ActionResult Create() => View("Edit",new Feed());

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

            return Redirect("/feeds");
        }

        public async Task<ActionResult> Delete(string id)
        {
            await _feed.Delete(await _feed.FeedById(id));
            return Redirect("/feeds");
        }
    }
}