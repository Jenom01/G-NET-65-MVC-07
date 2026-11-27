using GymManagementSystem_BLL.Errors;
using GymManagementSystem_BLL.ViewModels.Accounts;
using GymManagementSystem_DAL.Data.Identity;
using GymManagementSystem_PL.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem_PL.Controllers
{
    public class AccountController(SignInManager<GymUser> signInManager,
        UserManager<GymUser> userManager, ILogger<AccountController> logger) : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null) return FailedLogin(model);

            var result = await signInManager.CheckPasswordSignInAsync(
                user,
                model.Password,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var lifetime = model.RememberMe ? TimeSpan.FromHours(24) : TimeSpan.FromHours(8);

                await signInManager.SignInWithClaimsAsync(user, new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(lifetime)
                }, []);

                logger.LogInformation("User {Email} signed in for {Hours}h", model.Email, lifetime.TotalHours);

                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                logger.LogWarning("Sign-in blocked, account is locked out: {Email}", model.Email);
                ModelState.AddError(AccountErrors.LockedOut);
                return View(model);
            }

            return FailedLogin(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var email = User.Identity?.Name;

            await signInManager.SignOutAsync();
            logger.LogInformation("User {Email} signed out", email);

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            return View();
        }

        private IActionResult FailedLogin(LoginViewModel model)
        {
            logger.LogWarning("Failed sign-in attempt for {Email}", model.Email);
            ModelState.AddError(AccountErrors.InvalidCredentials);
            return View(model);
        }
    }
}
