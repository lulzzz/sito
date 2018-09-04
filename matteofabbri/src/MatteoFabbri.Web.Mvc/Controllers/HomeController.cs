using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using MatteoFabbri.Controllers;

namespace MatteoFabbri.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : MatteoFabbriControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
