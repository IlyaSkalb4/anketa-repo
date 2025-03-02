using backend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<ApplicationUser> userManager;
		private SignInManager<ApplicationUser> signInManager;
		private readonly IEmailSender emailSender;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.emailSender = emailSender;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(string email, bool rememberMe)
		{
			if (string.IsNullOrEmpty(email))
			{
				ModelState.AddModelError(string.Empty, "Email is required.");
				return View();
			}

			var user = new ApplicationUser { UserName = email, Email = email };
			var result = await userManager.CreateAsync(user);

			if (result.Succeeded)
			{
				var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
				var confirmationLink = Url.Action("ConfirmEmail", "Account",
					new { userId = user.Id, token = token, rememberMe = rememberMe }, Request.Scheme);

				await emailSender.SendEmailAsync(email, "Confirm your email",
					$"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>");

				return RedirectToAction("LoginLinkSent", "Account");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View();
		}

		[HttpGet]
		public IActionResult EmailConfirmed()
		{ 
			return View(); 
		}

			[HttpGet]
		public async Task<IActionResult> ConfirmEmail(string userId, string token, bool rememberMe)
		{
			if (userId == null || token == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			var result = await userManager.ConfirmEmailAsync(user, token);
			if (result.Succeeded)
			{
				await signInManager.SignInAsync(user, isPersistent: rememberMe);
				return RedirectToAction("Index", "Home");
			}

			return View("Error");
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(string email, bool rememberMe)
		{
			var user = await userManager.FindByEmailAsync(email);
			if (user != null && await userManager.IsEmailConfirmedAsync(user))
			{
				var token = await userManager.GenerateUserTokenAsync(user, "Default", "passwordless-login");
				var loginLink = Url.Action("PasswordlessLogin", "Account", 
					new { userId = user.Id, token = token, rememberMe = rememberMe }, Request.Scheme);

				await emailSender.SendEmailAsync(email,
					"Login to your account", $"Click this link to log in: <a href='{loginLink}'>link</a>");

				return RedirectToAction("LoginLinkSent");
			}

			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return View();
		}

		[HttpGet]
		public IActionResult LoginLinkSent()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> PasswordlessLogin(string userId, string token, bool rememberMe)
		{
			if (userId == null || token == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			var isValid = await userManager.VerifyUserTokenAsync(user, "Default", "passwordless-login", token);
			if (isValid)
			{
				await signInManager.SignInAsync(user, isPersistent: rememberMe);
				return RedirectToAction("EmailConfirmed", "Account");
			}

			return View("Error");
		}

		[HttpPost]
		public async Task<IActionResult> Logout(string? returnUrl)
		{
			await signInManager.SignOutAsync();

			if (string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}

		[HttpPost]
		public IActionResult ExternalLogin(string provider, string returnUrl = null)
		{
			var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
			var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return new ChallengeResult(provider, properties);
		}

		[HttpGet]
		public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null)
		{
			var info = await signInManager.GetExternalLoginInfoAsync();
			if (info == null)
			{
				return RedirectToAction("Login");
			}

			string[] userInfo =
			[
				info.Principal.FindFirstValue(ClaimTypes.Surname)!,
				info.Principal.FindFirstValue(ClaimTypes.Email)!
			];

			var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
				info.ProviderKey, isPersistent: false);
			if (result.Succeeded)
			{
				return RedirectToLocal(returnUrl);
			}
			else
			{
				ApplicationUser? user = await userManager.FindByEmailAsync(userInfo[1]);

				IdentityResult identityResult;

				if (user == null)
				{
					user = new ApplicationUser { UserName = userInfo[1], Email = userInfo[1] };
					identityResult = await userManager.CreateAsync(user);
				}
				else
				{
					identityResult = await userManager.AddLoginAsync(user, info);
				}

				if (identityResult.Succeeded)
				{
					await signInManager.SignInAsync(user, false);

					return RedirectToLocal(returnUrl);
				}

				foreach (var error in identityResult.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}

				return View("Login");
			}
		}
		
		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}
	}
}
