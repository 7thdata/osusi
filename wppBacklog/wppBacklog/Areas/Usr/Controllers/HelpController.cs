using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class HelpController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        public HelpController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        [Route("/{culture}/helps")]
        public IActionResult Index(string culture)
        {
            var view = new UsrHelpIndexViewModel()
            {
                Title = "Help",
                Culture = culture
            };
            return View(view);
        }
    }
}
