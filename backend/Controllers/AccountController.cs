using backend.Data.Models;
using backend.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<ApplicationUser> userManager;
		private SignInManager<ApplicationUser> signInManager;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{ 
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult EmailConfirmed()
		{
			return View();
		}

		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(model.Email);

				if (user != null && await userManager.IsEmailConfirmedAsync(user))
				{
					await signInManager.SignInAsync(user, isPersistent: false);

					return RedirectToAction("Index", "Home");
				}
				else if(user == null) 
				{
					user = new ApplicationUser { UserName = model.Email, Email = model.Email };
					var result = await userManager.CreateAsync(user);

					if (result.Succeeded)
					{
						var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

						var callbackUrl = Url.Action("ConfirmEmail", "Account",
							new { userId = user.Id, code }, protocol: Request.Scheme);

						return RedirectToAction("RegistrationSuccess");
					}
					foreach (var item in result.Errors)
					{
						ModelState.AddModelError(string.Empty, item.Description);
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid email attempt.");
				}
			}
			
			return View(model);
		}

		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var user = await userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound("User not found.");
			}

			var result = await userManager.ConfirmEmailAsync(user, code);

			if (result.Succeeded)
			{
				await signInManager.SignInAsync(user, isPersistent: false);

				return RedirectToAction("EmailConfirmed", "Account");
			}
			else
			{
				return View("Error");
			}
		}
	}
}
