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

        /// <summary>
        /// Details of the organzation.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        [Route("/{culture}/organization")]
        public async Task<IActionResult> Details(string culture, string keyword, string sort, int currentPage = 1, int itemsPerPage = 50, int rcode = 0)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (string.IsNullOrEmpty(currentUser.OrganizationId))
            {
                // If you don't have any organization, then create one.
                var nView = new UsrOrganizationDetailsViewModel()
                {
                    Organization = null,
                    Title = "Create New Organization",
                    Culture = culture,
                    RCode = rcode
                };

                // Show your organization here.
                return View(nView);
            }

            // Get my organization.
            var organization = _organizationService.GetOrganization(currentUser.OrganizationId);
            var members = _organizationService.GetMembershipInformationByOrganizationIdView(currentUser.OrganizationId, keyword, sort, currentPage, itemsPerPage);

            var view = new UsrOrganizationDetailsViewModel()
            {
                Organization = organization,
                Title = "Organization",
                Culture = culture,
                Members = members
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

            // Update user 
            currentUser.OrganizationId = organizationId;

            await _userManager.UpdateAsync(currentUser);

            return RedirectToAction("Details", new { @culture = culture });
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
        [Route("/{culture}/organization/update")]
        public async Task<IActionResult> Update(string culture, string billingName,
            string billingAddressCountry, string billingAddressPostalCode, string billingAddressRegion,
            string billingAddressLocality, string billingAddressStreet, string billingAddressUnit)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Later: You want to make sure you have permision to do this.
            if (currentUser.OrganizationId != null)
            {
                // Get my organization.
                var organization = _organizationService.GetOrganization(id: currentUser.OrganizationId);

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
            }

            // Update organization
            return RedirectToAction("Details", new { @culture = culture });
        }

        /// <summary>
        /// Leave organization, just set organization id to blank.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public async Task<IActionResult> LeaveOrganization(string culture)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Leave organization
            currentUser.OrganizationId = "";

            await _userManager.UpdateAsync(currentUser);

            return View("Details", new { @culture = culture });
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/invite")]
        public async Task<IActionResult> Invite(string culture, string name, string email, string membershipType, string memo)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Add member

            if (string.IsNullOrEmpty(currentUser.OrganizationId))
            {
                return BadRequest();
            }

            // Org?
            var organization = _organizationService.GetOrganization(currentUser.OrganizationId);

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

                    var content = Helpers.MailHelpers.Invited(culture, organization.Name, name, password, callbackUrl);

                    string title;

                    if (culture == "ja")
                    {
                        title = "[BB7] BACKLOG BY SEVENTH（" + organization.Name + "）への参加ご招待";
                    }
                    else
                    {
                        title = "[BB7] You have been invited to BACKLOG BY SEVENTH (" + organization.Name + ")";
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

                    return RedirectToAction("Details", new { @culture = culture, @rcode = 210 });
                }

                return RedirectToAction("Details", new { @culture = culture, @rcode = 511 });
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
        public IActionResult RemoveMember(string culture, string id)
        {
            // Remove member
            return View();
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
        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/organization/leave")]
        public async Task<IActionResult> Leave(string culture)
        {
            // You?
            var currentUser = await _userManager.GetUserAsync(User);

            if (!string.IsNullOrEmpty(currentUser.OrganizationId))
            {
                var membership = await _organizationService.LeaveOrganizationAsync(currentUser.OrganizationId, currentUser.Id);

                if (membership == null)
                {
                    return RedirectToAction("Details", new { @culture = culture, @rcode = 540 });
                }

                return RedirectToAction("Details", new { @culture = culture, @rcode = 230 });
            }

            return RedirectToAction("Details", new { @culture = culture, @rcode = 530 });
        }
    }
}
