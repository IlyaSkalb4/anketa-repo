using Microsoft.AspNetCore.Identity;

namespace backend.Data.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? LastLocation { get; set; }
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Sex { get; set; }
		public string? Weight { get; set; }
		public string? Height { get; set; }
		public int? Age { get; set; }
		public DateTime? DateOfChange { get; set; }

		public virtual ICollection<Answer>? Answers { get; set; }
		public virtual ICollection<Questionnaire>? Questionnaires { get; set; }
	}
}
