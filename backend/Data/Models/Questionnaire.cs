namespace backend.Data.Models
{
	public class Questionnaire
	{
		public int Id { get; set; }

		public int NumberOfPasses { get; set; }

		public string Title { get; set; } = default!;

		public string Description { get; set; } = default!;

		public string UserId { get; set; } = default!;

		public DateTime DateOfCreation { get; set; }

		public ApplicationUser User { get; set; } = default!;

		public ICollection<Question> Questions { get; set; } = default!;
	}
}
