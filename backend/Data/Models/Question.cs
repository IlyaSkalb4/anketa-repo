namespace backend.Data.Models
{
	public class Question
	{
		public int Id { get; set; }

		public string QuestionText { get; set; } = default!;

		public ICollection<Answer> Answers { get; set; } = default!;

		public ICollection<Questionnaire> Questionnaires { get; set; } = default!;
	}
}
