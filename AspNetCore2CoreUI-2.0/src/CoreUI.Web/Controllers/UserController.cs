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

        public async Task<ActionResult> AddToRole(string role, string user)
        {
            var u = await _userManager.FindByNameAsync(user);

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new MongoRole(role));

            if (u == null) return NotFound();

            await _userManager.AddToRoleAsync(u, role);

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByNameAsync(id);

            if (user == null) return NotFound();

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, MaddalenaUser user)
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

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByNameAsync(id);

            if (user == null) return NotFound();

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}