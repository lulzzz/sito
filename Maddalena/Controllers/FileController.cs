using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Mongo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Maddalena.Controllers
{
    [Authorize(Roles = "admin")]
    public class FileController : Controller
    {
        #region ANON ZONE

        [AllowAnonymous]
        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files.ToArray();

            var list = new List<UploadFile>();
            foreach (var x in files)
            {
                list.Add(await UploadFile.Create(x, User));
            }
            return Json(new
            {
                success = true,
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                data = new
                {
                    baseurl = @"http://mercati.news/file/download/",
                    messages = list.Select(x => $"File {x.FileName} was uploaded"),
                    files = list.Select(x => x.GridName),
                    code = 220
                }
            });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Download(string id)
        {
            var file = await UploadFile.FirstOrDefaultAsync(x => x.GridName == id);

            if (file == null || !file.ACL.IsAllowed(User)) return NotFound();

            return File(await file.Download(), file.ContentType, file.FileName, file.DateTime, new EntityTagHeaderValue($"\"{file.GetHashCode()}\""));
        }

        [AllowAnonymous]
        public IActionResult Dropbox() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dropbox(List<IFormFile> files)
        {
            foreach (var x in files)
            {
                await UploadFile.Create(x, User, new ACL
                {
                    AllowUsers = new List<string>(new[] { "matteo" })
                });
            }
            return View();
        }

        #endregion

        // GET: File
        public ActionResult Index() => View();

        // GET: File/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var file = await UploadFile.FirstOrDefaultAsync(x => x.GridName == id);
            return file != null ? (ActionResult)View(file) : NotFound();
        }

        // GET: File/Create
        public ActionResult Create() => View();

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