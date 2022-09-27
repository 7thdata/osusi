using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;
using wppBacklog.Handlers.Interfaces;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class MeController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IBlobHandlers _blobHandlers;
        private readonly IFileServices _fileServices;

        public MeController(UserManager<UserModel> userManager,
            IBlobHandlers blobHandlers, IFileServices fileServices)
        {
            _userManager = userManager;
            _blobHandlers = blobHandlers;
            _fileServices = fileServices;
        }

        [Route("/{culture}/me")]
        public async Task<IActionResult> Index(string culture, int rcode=0)
        {
            var user = await _userManager.GetUserAsync(User);

            var view = new UsrMeIndexViewModel()
            {
                Title = "Me",
                Culture = culture,
                RCode = rcode,
                User = new CurrentUserModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    OrganizationId = user.OrganizationId,
                    ProjectId = user.LastProjectId,
                    ProfileImageUrl = user.ProfileImage
                }
            };

            return View(view);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("/{culture}/me/upload-profile-image")]
        public async Task<IActionResult> UploadProfileImage(string culture, [FromForm] IFormFileCollection images)
        {
            var user = await _userManager.GetUserAsync(User);

            var listOfUrls = await _blobHandlers.UploadImageAsync(user.Id, images);

            if (listOfUrls != null)
            {
                if(listOfUrls.Count > 0)
                {
                    user.ProfileImage = listOfUrls.First();

                    // Update
                    await _userManager.UpdateAsync(user);
                }
            }

            return RedirectToAction("Index", new { @culture = culture, @rcode = 201 });

        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("/{culture}/me/update")]
        public async Task<IActionResult> UpdateMyProfile(string culture, string name)
        {
            var user = await _userManager.GetUserAsync(User);

            // Set image location
            user.Name = name;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", new { @culture = culture, @rcode = 202 });
        }
    }
}
