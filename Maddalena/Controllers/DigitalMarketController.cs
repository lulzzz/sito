using System.Linq;
using System.Threading.Tasks;
using Maddalena.Modules.DigitalMarket;
using Mongolino;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Maddalena.Controllers
{
    [Authorize]
    public class DigitalMarketController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public DigitalMarketController(UserManager<ApplicationUser> userManager,ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var items = DigitalItem.SortByDescending(x => x.Bought)
                .Take(20);
                
            return View(items);
        }

        public async Task<ActionResult> My()
        {
            var user = await _userManager.GetUserAsync(User);
            var items = await DigitalItem.WhereAsync(x => x.User.Id == user.Id);
            return View(items);
        }


        [AllowAnonymous]
        public async Task<ActionResult> Details(string id)
        {
            var item = await DigitalItem.FirstOrDefaultAsync(x => x.Id == id);

            return item != null ? (ActionResult) View(item) : NotFound();
        }

        public ActionResult Create() => View();

        // POST: DigitalItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DigitalItem item)
        {
            try
            {
                item.Id = null;
                item.Views = 0;
                item.Bought = 0;

                item.User = new ObjectRef<ApplicationUser>
                {
                    Value = await _userManager.GetUserAsync(User)
                };

                var content = Request.Form.Files.GetFile("content");
                if (content == null)
                {
                    ModelState.AddModelError("content","Content missing");
                    return View();
                }

                //item.Attachments.Add(await UploadFile.Create(content,User));

                var preview  = Request.Form.Files.GetFile("preview");
                item.HasPreview = preview != null;
                item.VideoPreview = preview?.FileName?.EndsWith(".mp4") ?? false;

                //if(preview != null) item.Attachments.Add(await UploadFile.Create(preview, User));

                await DigitalItem.CreateAsync(item);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            var item = await DigitalItem.FirstOrDefaultAsync(x => x.Id == id);

            return item != null ? (ActionResult)View(item) : NotFound();
        }

        // POST: DigitalItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DigitalItem item)
        {
            try
            {
                var old = await DigitalItem.FirstOrDefaultAsync(x => x.Id == item.Id);

                if (old == null) return NotFound();

                var user = await _userManager.GetUserAsync(User);
                if (old.User.Id != user.Id) return NotFound();

                item.Views = old.Views;
                item.Bought = old.Bought;

                var content = Request.Form.Files.GetFile("content");
                if (content != null)
                {
                    //var oldContent = item.Attachments.FirstOrDefault(x => x.Name == "content");

                    //if (oldContent == null) return NotFound();

                    //await oldContent.Delete();
                    //item.Attachments.Remove(oldContent);
                    //item.Attachments.Add(await UploadFile.Create(content, User));
                }

                var preview = Request.Form.Files.GetFile("preview");

                if (preview != null)
                {
                    item.HasPreview = true;
                    item.VideoPreview = preview.FileName?.EndsWith(".mp4") ?? false;

                    //var oldPreview = item.Attachments.FirstOrDefault(x => x.Name == "preview");

                    //if (oldPreview == null) return NotFound();

                    //await oldPreview.Delete();
                    //item.Attachments.Remove(oldPreview);
                    //item.Attachments.Add(await UploadFile.Create(preview, User));
                }

                await DigitalItem.UpdateAsync(item);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            var item = await DigitalItem.FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);

            if (item == null || item.User.Id != user.Id) return NotFound();

            return View(item);
        }

        // POST: DigitalItem/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("delete")]
        public async Task<ActionResult> DeletePost(string id)
        {
            try
            {
                var item = await DigitalItem.FirstOrDefaultAsync(x => x.Id == id);
                var user = await _userManager.GetUserAsync(User);

                if (item != null || item.User.Id != user.Id) return NotFound();

                await DigitalItem.DeleteAsync(item);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}