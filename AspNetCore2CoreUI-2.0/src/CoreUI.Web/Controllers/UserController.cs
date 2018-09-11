using System.Threading.Tasks;
using Maddalena.Core.Identity;
using Maddalena.Core.Identity.Model;
using Maddalena.Core.Identity.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    [Authorize()]
    public class UserController : Controller
    {
        readonly UserManager<MaddalenaUser> _userManager;
        readonly RoleManager<MongoRole> _roleManager;
        readonly IIdentityUserCollection<MaddalenaUser> _userCollection;

        public UserController(
            UserManager<MaddalenaUser> userManager,
            RoleManager<MongoRole> roleManager,
            IIdentityUserCollection<MaddalenaUser> collection)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userCollection = collection;
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
            await _userCollection.UpdateAsync(user);
            return Redirect("/user");
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userCollection.FindByIdAsync(id);
            await _userCollection.DeleteAsync(user);
            return Redirect("/user");
        }
    }
}