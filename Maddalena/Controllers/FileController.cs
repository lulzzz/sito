using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Mongo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Maddalena.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upload(string onSuccess, List<IFormFile> files)
        {
            var list = new List<UploadFile>();
            foreach(var x in files)
            {
                list.Add(await UploadFile.Create(x, User));
            }
            return Json(new { OnSuccess = onSuccess, Data = list });
        }

        public async Task<IActionResult> Download(string id)
        {
            var file = await UploadFile.FirstOrDefaultAsync(x => x.GridName == id);

            if (file == null || !file.ACL.IsAllowed(User)) return NotFound();

            return File(await file.Download(), file.ContentType, file.FileName, file.DateTime, new EntityTagHeaderValue($"\"{file.GetHashCode()}\""));
        }

        // GET: File/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: File/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: File/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: File/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: File/Edit/5
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

        // GET: File/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: File/Delete/5
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