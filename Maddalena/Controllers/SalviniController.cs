using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Models.Salvini;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Mongolino;

namespace Maddalena.Controllers
{
    public class SalviniController : Controller
    {
        static IMongoCollection<MailMessage> collection;

        static SalviniController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}