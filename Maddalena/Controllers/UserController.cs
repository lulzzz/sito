using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Security;
using Maddalena.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Maddalena.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : BaseController
    {
        RoleManager<ApplicationRole> _roleManager;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> SignInManager,
            IEmailSender emailSender,
            ILogger logger) : base(userManager, SignInManager, emailSender, logger)
        {
            _roleManager = roleManager;
        }


        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddToRole(string role, string user)
        {
            var u = await UserManager.FindByNameAsync(user);

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new ApplicationRole(role));

            if (u == null) return NotFound();

            await UserManager.AddToRoleAsync(u, role);

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByNameAsync(id);

            if (user == null) return NotFound();

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ApplicationUser user)
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
            var user = await UserManager.FindByNameAsync(id);

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