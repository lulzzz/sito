using Maddalena.Core.Scripts;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class ScriptController : Controller
    {
        private readonly IScriptService _service;

        public ScriptController(IScriptService service)
        {
            _service = service;
        }

        public IActionResult Index() => View(_service.All());

        public IActionResult Create() => View();

        public IActionResult Edit(string id) => View(_service.ById(id));

        public IActionResult Delete(string id)
        {
            _service.Delete(id);
            return Redirect("/script");
        }
    }
}
