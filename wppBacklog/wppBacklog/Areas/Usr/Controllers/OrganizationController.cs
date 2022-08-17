using clsBacklog.Interfaces;
using clsBacklog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;
using wppBacklog.Areas.Usr.Models;
using wppBacklog.Handlers.Interfaces;
using wppBacklog.Models;

namespace wppBacklog.Areas.Usr.Controllers
{
    [Area("Usr"), Authorize()]
    public class OrganizationController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IOrganizationServices _organizationService;
        private readonly Random _random = new Random();
        private readonly IUserStore<UserModel> _userStore;
        private readonly IUserEmailStore<UserModel> _emailStore;
        private readonly IOptions<AppConfigModel> _config;
        private readonly INotificationHandlers _notificationHandlers;

        public OrganizationController(UserManager<UserModel> userManager,
            IUserStore<UserModel> userStore,
            IOrganizationServices organizationService,
            IOptions<AppConfigModel> config,
            INotificationHandlers notificationHandlers)
        {
            _userManager = userManager;
            _organizationService = organizationService;
            _emailStore = GetEmailStore();
            _userStore = userStore;
            _config = config;
            _notificationHandlers = notificationHandlers;
        }
        private IUserEmailStore<UserModel> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<UserModel>)_userStore;
        }

        [Route("/{culture}/organizations")]
        public async Task<IActionResult> Index(string culture, string keyword, string sort, int currentPage = 1, int itemsPerPage = 50, int rcode = 0)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var organizations = _organizationService.GetMyOrganizations(currentUser.Id, keyword, sort, currentPage, itemsPerPage);

            var view = new UsrOrganizationIndexViewModel(organizations)
            {
                Culture = culture
            };

            return View(view);
        }

        /// <summary>
        /// Details of the organzation.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        [Route("/{culture}/organization/{id}")]
        public async Task<IActionResult> Details(string culture, string id, string keyword, string sort, int currentPage = 1, int itemsPerPage = 50, int rcode = 0)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var isActiveOrganization = false;

            if (currentUser.OrganizationId == id)
            {
                isActiveOrganization = true;
            }

            // Get org.
            var organization = _organizationService.GetOrganization(id);

            if (organization == null)
            {
                return NotFound();
            }

            // Make sure you belong to the organization
            var membership = _organizationService.GetMembershipInformationByOrganizationId(id, currentUser.Id, "", 1, 1).Items?.FirstOrDefault();

            if (membership == null)
            {
                // You are not member here.
                return NotFound();
            }

            var members = _organizationService.GetMembershipInformationByOrganizationIdView(id, keyword, sort, currentPage, itemsPerPage);

            var view = new UsrOrganizationDetailsViewModel(organization)
            {
                Title = "Organization",
                Culture = culture,
                Members = members,
                IsActiveOrganization = isActiveOrganization,
                RCode = rcode
            };

            // Show your organization here.
            return View(view);
        }

        /// <summary>
        /// Call when creating the organization.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/create")]
        public async Task<IActionResult> Create(string culture, string name)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var organizationId = Guid.NewGuid().ToString();

            // Create
            var organization = await _organizationService.CreateOrganizationAsync(new OrganizationModel(organizationId, name)
            {
                OwnerId = currentUser.Id
            });

            // Add as 

            // Update user 
            currentUser.OrganizationId = organizationId;

            await _userManager.UpdateAsync(currentUser);

            return RedirectToAction("Details", new { @culture = culture, @id = organizationId, @rcode = 200 });
        }

        /// <summary>
        /// Update organization information.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="billingName"></param>
        /// <param name="billingAddressCountry"></param>
        /// <param name="billingAddressPostalCode"></param>
        /// <param name="billingAddressRegion"></param>
        /// <param name="billingAddressLocality"></param>
        /// <param name="billingAddressStreet"></param>
        /// <param name="billingAddressUnit"></param>
        /// <returns></returns>
        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{id}/update")]
        public async Task<IActionResult> Update(string culture, string id, string billingName,
            string billingAddressCountry, string billingAddressPostalCode, string billingAddressRegion,
            string billingAddressLocality, string billingAddressStreet, string billingAddressUnit)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Later: You want to make sure you have permision to do this.

            // Get my organization.
            var organization = _organizationService.GetOrganization(id: id);

            if (organization == null)
            {
                return BadRequest();
            }

            organization.BillingName = billingName;
            organization.BillingAddressCountry = billingAddressCountry;
            organization.BillingAddressPostalCode = billingAddressPostalCode;
            organization.BillingAddressRegion = billingAddressRegion;
            organization.BillingAddressLocality = billingAddressLocality;
            organization.BillingAddressStreet = billingAddressStreet;
            organization.BillingAddressUnit = billingAddressUnit;

            var reuslt = await _organizationService.UpdateOrganizationAsync(organization);

            // Update organization
            return RedirectToAction("Details", new { @culture = culture, @id = id, @rcode = 201 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/leave")]
        public async Task<IActionResult> LeaveOrganization(string culture, string organizationId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Get membership
            var membership = (_organizationService.GetMembershipInformationByOrganizationId(organizationId, currentUser.Id, "", 1, 1)).Items?.FirstOrDefault();

            if (membership == null)
            {
                return NotFound();
            }

            // Leave organization
            var removeResult = await _organizationService.RemoveMemberFromOrganizationAsync(membership.Id);

            if (removeResult == null)
            {
                // Something went wrong here.
                return BadRequest();
            }

            if (currentUser.OrganizationId == organizationId)
            {
                currentUser.OrganizationId = "";
                await _userManager.UpdateAsync(currentUser);
            }

            return View("Index", new { @culture = culture, rcode = 250 });
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizationId}/active")]
        public async Task<IActionResult> SetAsActiveOrganization(string culture, string organizationId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Get membership
            var membership = (_organizationService.GetMembershipInformationByOrganizationId(organizationId, currentUser.Id, "", 1, 1)).Items?.FirstOrDefault();

            if (membership == null)
            {
                return NotFound();
            }

            currentUser.OrganizationId = organizationId;

            await _userManager.UpdateAsync(currentUser);

            return RedirectToAction("Details", new { @culture = culture, @id=organizationId, @rcode = 260 });

        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{id}/member/invite")]
        public async Task<IActionResult> Invite(string culture, string id, string name, string email, string membershipType, string memo)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Org?
            var organization = _organizationService.GetOrganization(id);

            if (organization == null)
            {
                return BadRequest();
            }

            // Is he/she already registered?
            var tUser = await _userManager.FindByEmailAsync(email);

            if (tUser == null)
            {
                // Then invite to create an account.
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, email, CancellationToken.None);

                user.Name = name;
                user.OrganizationId = currentUser.OrganizationId;
                user.PreferedLanguage = culture;
                var password = RandomPassword();

                var result = await _userManager.CreateAsync(user, password);

                // back to the list
                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = _config.Value.RootDomain + "/" + culture + "/account/confirmEmail?userId=" + user.Id + "&code=" + code;

                    var content = Helpers.MailHelpers.Invited(culture, organization.Name, name, password, callbackUrl, memo);

                    string title;

                    if (culture == "ja")
                    {
                        title = "[OSU] OSUSHI.APP（" + organization.Name + "）への参加ご招待";
                    }
                    else
                    {
                        title = "[OSU] You have been invited to OSUSHI.APP (" + organization.Name + ")";
                    }

                    await _notificationHandlers.SendEmailAsync(email, title, content);

                    var view = new AccountPreRegisteredViewModel()
                    {
                        Email = email
                    };

                    view.Culture = culture;

                    // Add user to the org.
                    var addMemberResult = await _organizationService.AddMemberToOrganizationAsync(user.Id, organization.Id, membershipType);

                    if (addMemberResult == null)
                    {
                        // Something went wrong in adding member.
                        return BadRequest();
                    }

                    return RedirectToAction("Details", new { @culture = culture, @id = id, @rcode = 210 });
                }

                return RedirectToAction("Details", new { @culture = culture, @id = id, @rcode = 511 });
            }

            if (string.IsNullOrEmpty(tUser.OrganizationId))
            {

                // Then add user to the organization

                tUser.OrganizationId = currentUser.OrganizationId;

                // Update user
                await _userManager.UpdateAsync(currentUser);

                return RedirectToAction("Details", new { @culture = culture, @rcode = 220 });
            }

            // Can't add user to the organization, the user need to leave the organization first.
            return RedirectToAction("Details", new { @culture = culture, @rcode = 510 });

        }

        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        private string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):   
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        private string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }
        private UserModel CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        /// <summary>
        /// Remove user.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/{organizatioId}/member/remove")]
        public async Task<IActionResult> RemoveMember(string culture, string organizationId, string id)
        {
            // Remove member from org.

            // You?
            var currentUser = await _userManager.GetUserAsync(User);

            // Your membership.
            var membership = (_organizationService.GetMembershipInformationByOrganizationId(organizationId, currentUser.Id, "", 1, 1).Items?.FirstOrDefault());

            // Make sure you have membership
            if (membership == null)
            {
                return BadRequest();
            }

            // You have to be admin.
            if (membership.MembershipType != "admin")
            {
                return BadRequest();
            }

            var targetUser = await _userManager.FindByIdAsync(id);

            if (targetUser == null)
            {
                return NotFound();
            }

            // target user membership
            var tmembership = (_organizationService.GetMembershipInformationByOrganizationId(organizationId, targetUser.Id, "", 1, 1).Items?.FirstOrDefault());

            if (tmembership == null)
            {
                return NotFound();
            }

            // Remove
            var removeResult = await _organizationService.RemoveMemberFromOrganizationAsync(tmembership.Id);

            if (removeResult == null)
            {
                // Something went wrong here.
                return BadRequest();
            }

            // We shoul let user know?  

            return RedirectToAction("Details", new { @culture = culture, @rcode = 240 });
        }

        public IActionResult Subscription(string culture, string id)
        {
            // Show subscription
            return View();
        }
        public IActionResult SubscriptionHistory(string culture, string id)
        {
            // Show subscription history
            return View();
        }

    }
}
