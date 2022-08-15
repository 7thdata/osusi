using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using wppBacklog.Models;

namespace wppBacklog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [Route("/")]
        [Route("/{culture}")]
        public IActionResult Index(string culture)
        {
            var languages = Request.GetTypedHeaders()
                      .AcceptLanguage
                      ?.OrderByDescending(x => x.Quality ?? 1)
                      .Select(x => x.Value.ToString())
                      .ToArray() ?? Array.Empty<string>();

            var view = new HomeIndexViewModel()
            {
                Culture = culture
            };

            return View(view);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}