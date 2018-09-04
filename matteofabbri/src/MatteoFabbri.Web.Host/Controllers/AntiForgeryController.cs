using Microsoft.AspNetCore.Antiforgery;
using MatteoFabbri.Controllers;

namespace MatteoFabbri.Web.Host.Controllers
{
    public class AntiForgeryController : MatteoFabbriControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
