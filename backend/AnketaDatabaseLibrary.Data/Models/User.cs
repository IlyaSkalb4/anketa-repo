namespace AnketaDatabaseLibrary.Data.Models
{
	public class User
	{
		public string Id { get; set; } = default!;

		public string Email { get; set; } = default!;

		public string? LastLocation { get; set; }

		public string? Name { get; set; }

		public string? Surname { get; set; }

		public string? Sex { get; set; }

		public string? Weight { get; set; }

		public string? Height { get; set; }

		public string? HealthStatus { get; set; }

		public int? Age { get; set; }

		public DateTime? DateOfChange { get; set; }

		public ICollection<Answer>? Answers { get; set; }
	}
}
