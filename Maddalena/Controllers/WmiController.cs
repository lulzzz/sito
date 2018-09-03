using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WindowsMonitor.Hardware.Power;
using Maddalena.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Maddalena.Controllers
{
    public class WmiController : Controller
    {
        private static readonly Assembly Assembly = typeof(Win32Battery).Assembly;

        private static readonly string[] Names = Assembly
            .GetTypes()
            .Where(x =>
            {
                if (!x.Namespace.StartsWith("WindowsMonitor.Hardware") || x.Name.Contains("<Retrieve>")) return false;

                try
                {
                    var method = x.GetMethod("Retrieve", new Type[0]);
                    var res = (IEnumerable<object>)method.Invoke(null, new object[0]);

                    return res.Any();
                }
                catch (Exception)
                {
                    return false;
                }
            })
            .Select(x => x.FullName.Substring("WindowsMonitor.Hardware.".Length)).ToArray();



        public async Task<ActionResult> Index(string id)
        {
            var model = new WmiModel
            {
                Names = Names,
            };


           if (!string.IsNullOrWhiteSpace(id))
            {
                var tName = $"WindowsMonitor.Hardware.{id}";
                var type = Assembly.GetType(tName);
                    
                var method = type.GetMethod("Retrieve", new Type[0]);

                model.Instances = (IEnumerable<object>) method.Invoke(null, new object[0]);
                model.Properties = type.GetProperties();
            }


            return View(model);
        }
    }
}
