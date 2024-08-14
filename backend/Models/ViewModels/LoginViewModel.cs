using System.ComponentModel.DataAnnotations;

namespace backend.Models.ViewModels
{
	public class LoginViewModel
	{
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

		[Display(Name = "Email")]
		[Required]
		public string Email { get; set; } = default!;

		public string? ReturnUrl { get; set; }
	}
}
