namespace backend.Data.Models
{
	public enum InputTypeEnum
	{
		Text = 1,
		Number = 2,
		Radio = 3,
		Checkbox = 4,
		Date = 5,
		Time = 6,
		Image = 7,
	}

	public class Question
	{
		public int Id { get; set; }

		public string QuestionText { get; set; } = default!;

		public InputTypeEnum InputType { get; set; }

		public ICollection<Answer> Answers { get; set; } = default!;

		public ICollection<Questionnaire> Questionnaires { get; set; } = default!;
	}
}
