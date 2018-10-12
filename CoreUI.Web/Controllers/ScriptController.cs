using System;
using Maddalena.Core.Scripts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Maddalena.Controllers
{
    [Authorize(Roles ="script")]
    public class ScriptController : Controller
    {
        private readonly IScriptService _service;

        public ScriptController(IScriptService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index() => View(await _service.All());

        [HttpPost, Route("/script/edit")]
        public IActionResult Edit(Script script)
        {
            script.Author = User?.Identity?.Name;
            script.LastModified = DateTime.Now;

            if(script.Id == null) _service.Create(script); else _service.Update(script);

            return Redirect("/script");
        }

        public async Task<IActionResult> Edit(string id) => id == null ? View("Edit", new Script()) : View(await _service.ById(id));

        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return Redirect("/script");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Run(string id)
        {
            var script = await _service.ById(id);

            if (script == null) return NotFound();

            var console = await _service.Run(script);
            return View(console);
        }
    }
}
