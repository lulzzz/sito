using System.Threading.Tasks;
using Maddalena.Core;
using Maddalena.Core.Identity;
using Maddalena.Core.Identity.Model;
using Maddalena.Core.Identity.Stores;
using Maddalena.Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    [Authorize()]
    public class UserController : Controller
    {
        private readonly ISettingsService _settings;

        private readonly UserManager<MaddalenaUser> _userManager;
        private readonly RoleManager<MongoRole> _roleManager;
        readonly IIdentityUserCollection<MaddalenaUser> _userUserCollection;

        public UserController(
            ISettingsService settings,
            UserManager<MaddalenaUser> userManager,
            RoleManager<MongoRole> roleManager,
            IIdentityUserCollection<MaddalenaUser> userCollection)
        {
            _settings = settings;

            _roleManager = roleManager;
            _userManager = userManager;
            _userUserCollection = userCollection;
        }

        public ActionResult Index(string id) => View(_userManager.Users);

        public async Task<ActionResult> AddToRole(string roleName, string userId)
        {
            var u = await _userManager.FindByNameAsync(userId);

            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new MongoRole(roleName));

            if (u == null) return NotFound();

            await _userManager.AddToRoleAsync(u, roleName);

            return Redirect($"/user/edit/{userId}");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByNameAsync(id);

            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MaddalenaUser user)
        {
            await _userUserCollection.UpdateAsync(user);
            return Redirect("/user");
        }

        [HttpGet]
        public ActionResult Settings() => View(_settings.Get<SiteSettings>());

        [HttpPost]
        public ActionResult Settings(SiteSettings settings)
        {
            _settings.Save(settings);
            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userUserCollection.FindByIdAsync(id);
            await _userUserCollection.DeleteAsync(user);
            return Redirect("/user");
        }
    }
}