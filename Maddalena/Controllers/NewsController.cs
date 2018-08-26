using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Client;
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

        public async Task<IActionResult> Delete(string id)
        {
            await Datastore.News.DeleteAsync(id);
            return Redirect("/news");
        }

        public async Task<IActionResult> Label(string id, string label, LabelValue value)
        {
            var news = await Datastore.News.Get(id);
            await Datastore.News.LabelAsync(news,label,value);
            return Redirect("/news");
        }

        public async Task<IActionResult> Update(string id)
        {
            await Datastore.News.DeleteAsync(id);
            return Redirect("/news");
        }
    }
}