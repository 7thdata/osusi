using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wppBacklog.Areas.Usr.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class WikiController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IProjectServices _projectServices;
        private readonly IOrganizationServices _organizationServices;
        private readonly IWikiServices _wikiServices;

        public WikiController(UserManager<UserModel> userManager, IProjectServices projectServices,
            IOrganizationServices organizationServices, IWikiServices wikiServices)
        {
            _userManager = userManager;
            _projectServices = projectServices;
            _organizationServices = organizationServices;
            _wikiServices = wikiServices;
        }

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/wikis")]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/{parentId}/wikis")]
        public async Task<IActionResult> Index(string culture, string organizationId, string projectId, string parentId, string keyword,
            string sort, int currentPage = 1, int itemsPerPage = 50, int rcode = 0)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Make sure you are ready to be here.
            var organization = _organizationServices.GetOrganization(organizationId);

            if (organization == null)
            {
                return NotFound();
            }

            // Project
            var project = _projectServices.GetProject(organizationId, projectId);

            if (project == null)
            {
                // 
                return NotFound();
            }

            // Make sure you are in this.
            var member = _projectServices.GetProjectMembersView(organizationId, projectId, currentUser.Id, "", 1, 1);
            if (member.TotalItems == 0)
            {
                return NotFound();
            }

            // Get project
            var wikis = _wikiServices.GetWikis(project.Id, parentId, keyword, sort, currentPage, itemsPerPage);

            var view = new UsrWikiIndexViewModel(project, organization, wikis)
            {
                Title = "Wiki",
                Culture = culture,
                Project = project,
                RCode = rcode
            };

            return View(view);
        }

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/wikis/{parentId}/wiki/{id}/edit")]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/wikis/{parentId}/wiki/create")]
        public async Task<IActionResult> Edit(string culture, string organizationId, string projectId, string parentId, string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Make sure you are ready to be here.
            var organization = _organizationServices.GetOrganization(organizationId);

            if (organization == null)
            {
                return NotFound();
            }

            // Project
            var project = _projectServices.GetProject(organizationId, projectId);

            if (project == null)
            {
                // 
                return NotFound();
            }

            // Make sure you are in this.
            var member = _projectServices.GetProjectMembersView(organizationId, projectId, currentUser.Id, "", 1, 1);
            if (member.TotalItems == 0)
            {
                return NotFound();
            }

            var wikiId = Guid.NewGuid().ToString();
            var wiki = new WikiModel(projectId, wikiId, "", "", parentId);

            if (string.IsNullOrEmpty(id))
            {
                // Must be new then.
                wiki.Name = "New Wiki Title";
                wiki.Description = "";
                wiki.ParentWikiId = parentId;
            }
            else
            {
                // Get wiki
                wiki = _wikiServices.GetWik(id);
                wiki.ParentWikiId = parentId;

                if (wiki == null)
                {
                    return NotFound();
                }
            }

            var view = new UsrWikiEditViewModel(project, organization, wiki)
            {
                Title = wiki.Name,
                Culture = culture,
                Project = project
            };

            return View(view);
        }

        [Route("/{culture}/organization/{organizationId}/project/{projectId}/wiki/{id}")]
        public async Task<IActionResult> Details(string culture, string organizationId, string projectId, string id,
            int rcode = 0)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Make sure you are ready to be here.
            var organization = _organizationServices.GetOrganization(organizationId);

            if (organization == null)
            {
                return NotFound();
            }

            // Project
            var project = _projectServices.GetProject(organizationId, projectId);

            if (project == null)
            {
                // 
                return NotFound();
            }

            // Make sure you are in this.
            var member = _projectServices.GetProjectMembersView(organizationId, projectId, currentUser.Id, "", 1, 1);
            if (member.TotalItems == 0)
            {
                return NotFound();
            }

            var wiki = _wikiServices.GetWik(id);

            if (wiki == null)
            {
                return NotFound();
            }

            var view = new UsrWikiDetailsViewModel(project, organization, wiki)
            {
                Title = wiki.Name,
                Culture = culture,
                Project = project,
                RCode = rcode
            };

            return View(view);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/wiki/upsert")]
        public IActionResult Update(string culture, string organizationId, string projectId, WikiModel wiki)
        {
            var result = _wikiServices.UpsertWikiAsync(wiki);

            if (result == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new
            {
                @culture = culture,
                @organizationId = organizationId,
                @projectId = projectId,
                @id = result.Id,
                @rcode = 201
            });
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/project/{projectId}/wiki/{id}/delete")]
        public IActionResult Delete(string culture, string organizationId, string projectId, string id)
        {
            var result = _wikiServices.DeleteWikiAsync(id);

            if (result == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", new
            {
                @culture = culture,
                @organizationId = organizationId,
                @projectId = projectId,
                @rcode = 201
            });
        }
    }
}
