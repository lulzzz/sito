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
    public class RoleController : BaseController
    {
        private readonly RoleManager<ApplicationRole> RoleManager;

        public RoleController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          RoleManager<ApplicationRole> roleManager,
          IEmailSender emailSender,
          ILogger<RoleController> logger) : base(userManager,signInManager,emailSender,logger)
        {
            RoleManager = roleManager;
        }


        // GET: Role
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        // GET: Role/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var role = await RoleManager.FindByNameAsync(id);
            return View(role);
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApplicationRole role)
        {
            try
            {
                await RoleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Role/Edit/5
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

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Role/Delete/5
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