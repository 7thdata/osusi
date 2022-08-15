using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class MeController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        public MeController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        [Route("/{culture}/me")]
        public IActionResult Index(string culture)
        {
            var view = new UsrMeIndexViewModel()
            {
                Title = "Me",
                Culture = culture
            };

            return View(view);
        }
    }
}
