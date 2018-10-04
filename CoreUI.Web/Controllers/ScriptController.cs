using Maddalena.Core.Scripts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Maddalena.Controllers
{
    public class ScriptController : Controller
    {
        private readonly IScriptService _service;

        public ScriptController(IScriptService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index() => View(await _service.All());

        public IActionResult Create() => View();

        public async Task<IActionResult> Edit(string id) => View(await _service.ById(id));

        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return Redirect("/script");
        }
    }
}
