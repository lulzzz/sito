using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Datastorage;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class NewsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await Datastore.News.AllAsync());
        }
    }
}