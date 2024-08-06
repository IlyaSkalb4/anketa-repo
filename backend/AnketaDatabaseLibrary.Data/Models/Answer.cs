namespace AnketaDatabaseLibrary.Data.Models
{
	public class Answer
	{
		public int Id { get; set; }

		public string? TextValue { get; set; }

		public float? FloatValue { get; set; }

		public bool? BooleanValue { get; set; }

		public ICollection<Question> Questions { get; set; } = default!;

		public ICollection<User> Users { get; set; } = default!;
	}
}
