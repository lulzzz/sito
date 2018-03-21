using Maddalena.Security;
using Maddalena.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Maddalena.Controllers
{
    public class BaseController : Controller
    {
        public readonly UserManager<ApplicationUser> UserManager;
        public readonly SignInManager<ApplicationUser> SignInManager;
        public readonly IEmailSender EmailSender;
        public readonly ILogger Logger;

        public BaseController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            EmailSender = emailSender;
            Logger = logger;
        }
    }
}
