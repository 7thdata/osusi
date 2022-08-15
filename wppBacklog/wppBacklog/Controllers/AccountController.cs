using clsBacklog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;
using wppBacklog.Handlers.Interfaces;
using wppBacklog.Models;

namespace wppBacklog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IUserStore<UserModel> _userStore;
        private readonly IUserEmailStore<UserModel> _emailStore;
        private readonly INotificationHandlers _notificationHandlers;
        private readonly IOptions<AppConfigModel> _config;
        private readonly SignInManager<UserModel> _signInManager;

        public AccountController(
         UserManager<UserModel> userManager,
           IUserStore<UserModel> userStore,
           INotificationHandlers notificationHandlers,
           IOptions<AppConfigModel> config,
           SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _notificationHandlers = notificationHandlers;
            _config = config;
            _signInManager = signInManager;
        }

        private IUserEmailStore<UserModel> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<UserModel>)_userStore;
        }

        [Route("/{culture}/account/login")]
        [Route("/account/login")]
        public IActionResult Login(string culture)
        {
            var languages = Request.GetTypedHeaders()
                  .AcceptLanguage
                  ?.OrderByDescending(x => x.Quality ?? 1)
                  .Select(x => x.Value.ToString())
                  .ToArray() ?? Array.Empty<string>();

            if (string.IsNullOrEmpty(culture))
            {
                culture = languages[0];
            }

            var view = new AccountLoginViewModel();

            if (culture == "ja")
            {
                view.Title = "ログイン";
                view.Email = "メールアドレス";
                view.Password = "パスワード";
                view.Login = "新規登録";
            }
            else
            {
                view.Title = "Login";
                view.Email = "Email Address";
                view.Password = "Password";
                view.Login = "Login";
            }

            view.Culture = culture;

            return View(view);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/account/login")]

        public async Task<IActionResult> Login(string culture, string email, string password)
        {

            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { @area = "Usr", @culture = culture });
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("/{culture}/account/register")]
        public IActionResult Register(string culture, int errorCode = 0)
        {
            var view = new AccountRegisterViewModel()
            {
                ErrorCode = errorCode
            };

            if (culture == "ja")
            {
                view.Title = "新規登録";
                view.Name = "お名前";
                view.ConfirmPassword = "確認用パスワード";
                view.Email = "メールアドレス";
                view.Password = "パスワード";
                view.Register = "新規登録";
                view.AgreeToTheTerm = "利用規約に同意する";
            }
            else
            {
                view.Title = "Register";
                view.Name = "Your Name";
                view.ConfirmPassword = "Confirm Password";
                view.Email = "Email Address";
                view.Password = "Password";
                view.Register = "Register";
                view.AgreeToTheTerm = "I have agreed to the service term.";
            }

            view.Culture = culture;

            return View(view);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/account/register")]
        public async Task<IActionResult> PreRegistered(string culture, string email, string name, string password, string passwordConfirm, string returnUrl)
        {
            // Validate Input
            if (password != passwordConfirm)
            {
                // Password need to match
                return BadRequest();
            }

            // Check to see if this user exist.
            var checkUser = await _userManager.FindByEmailAsync(email);

            if (checkUser != null)
            {
                // Do something here.
                return BadRequest();
            }

            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, email, CancellationToken.None);

            user.Name = name;
            user.PreferedLanguage = culture;

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = _config.Value.RootDomain + "/" + culture + "/account/confirmEmail?userId=" + user.Id + "&code=" + code;

                var content = Helpers.MailHelpers.PreRegistered(culture, name, callbackUrl);

                string title;

                if (culture == "ja")
                {
                    title = "[BB7] Backlog by SEVENTHの仮登録ありがとうございます";
                }
                else
                {
                    title = "[BB7] Thank you for registering at Backlog by SEVENTH";
                }

                await _notificationHandlers.SendEmailAsync(email, title, content);

                var view = new AccountPreRegisteredViewModel()
                {
                    Email = email
                };

                view.Culture = culture;
                return View(view);
            }

            return BadRequest();

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

        [Route("/{culture}/account/confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string culture, string code, string userId, string returnUrl)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                var view = new AccountConfirmEmailViewModel();

                if (culture == "ja")
                {
                    view.Title = "メールアドレスの確認ができました。";
                }
                else
                {
                    view.Title = "Email address has been confirmed";
                }

                view.Culture = culture;

                return View(view);
            }

            return BadRequest();
        }

        [Route("/{culture}/account/forgotPassword")]
        public IActionResult ForgotPassword(string culture)
        {
            var view = new AccountForgotPasswordViewModel();
            return View(view);
        }

        [Route("/{culture}/account/resendConfirmationCode")]
        public IActionResult ResendConfirmationCode(string culture)
        {
            var view = new AccountResendConfirmationCodeViewModel()
            {
                Culture = culture
            };

            if (culture == "ja")
            {
                view.Title = "メール所有の確認メールの再送";
                view.Label = "メールアドレス入力してください。";
                view.Email = "メールアドレス";
                view.ActionName = "送信";
            }
            else
            {
                view.Title = "Resend Confirmation Email";
                view.Label = "Please type in your email address.";
                view.Email = "Email";
                view.ActionName = "Send";
            }

            return View(view);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        [Route("/{culture}/account/resendConfirmationCode")]
        public async Task<IActionResult> ResendConfirmationCodeSent(string culture, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = _config.Value.RootDomain + "/" + culture + "/account/confirmEmail?userId=" + user.Id + "&code=" + code;

                var content = Helpers.MailHelpers.PreRegistered(culture, user.Name, callbackUrl);

                string title;

                if (culture == "ja")
                {
                    title = "[BB7] Backlog by SEVENTHの仮登録ありがとうございます";
                }
                else
                {
                    title = "[BB7] Thank you for registering at Backlog by SEVENTH";
                }

                await _notificationHandlers.SendEmailAsync(email, title, content);

            }

            var view = new AccountResendConfirmationCodeSentViewModel()
            {
                Culture = culture
            };

            if (culture == "ja")
            {
                view.Title = "メール所有の確認メールを再送しました";
                view.Label = "メール所有の確認メールを再送いたしました。メール内にある確認リンクをクリックし本登録を行ってください。";

            }
            else
            {
                view.Title = "Resent Confirmation Email";
                view.Label = "We have sent confirmation email. Please check your email and click on the confirmation link to complete your registration.";
            }

            return View(view);
        }
    }
}
