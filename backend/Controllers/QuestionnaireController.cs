using backend.Data.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
	[Authorize]
	[Route("[controller]")]
	public class QuestionnaireController : Controller
	{
		private readonly IQuestionnaireService _questionnaireService;
		private readonly UserManager<ApplicationUser> _userManager;

		public QuestionnaireController(IQuestionnaireService questionnaireService, UserManager<ApplicationUser> userManager)
		{
			_questionnaireService = questionnaireService;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int? id)
		{
			if (id.HasValue)
			{
				var questionnaire = await _questionnaireService.GetQuestionnaireByIdAsync(id.Value);
				if (questionnaire == null)
					return NotFound();

				return View(questionnaire);
			}

			return View(null);
		}

		[HttpPost]
		[Route("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromBody] QuestionnaireCreateModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var questionnaire = await _questionnaireService.CreateQuestionnaireAsync(model.Title, model.Description, userId);

			return Ok(new { id = questionnaire.Id });
		}

		[HttpPut]
		[Route("Update")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update([FromBody] QuestionnaireUpdateModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var questionnaire = await _questionnaireService.UpdateQuestionnaireAsync(model.Id, model.Title, model.Description);
			if (questionnaire == null)
				return NotFound();

			return Ok(new { id = questionnaire.Id });
		}

		[HttpDelete]
		[Route("Delete/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _questionnaireService.DeleteQuestionnaireAsync(id);
			if (!result)
				return NotFound();

			return Ok();
		}

		[HttpPost]
		[Route("AddQuestion")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddQuestion([FromBody] AddQuestionModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var question = await _questionnaireService.AddQuestionAsync(model.QuestionnaireId, model.QuestionText, model.InputType);
			if (question == null)
				return NotFound();

			return Ok(new 
			{ 
				id = question.Id,
				questionText = question.QuestionText,
				inputType = question.InputType.ToString(),
				html = GenerateQuestionHtml(question) 
			});
		}

		[HttpPut]
		[Route("UpdateQuestion/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var question = await _questionnaireService.UpdateQuestionAsync(id, model.QuestionText);
			if (question == null)
				return NotFound();

			return Ok();
		}

		[HttpDelete]
		[Route("DeleteQuestion/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteQuestion(int id)
		{
			var result = await _questionnaireService.DeleteQuestionAsync(id);
			if (!result)
				return NotFound();

			return Ok();
		}

		[HttpPost]
		[Route("AddAnswer")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddAnswer([FromBody] AddAnswerModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var answer = await _questionnaireService.AddAnswerAsync(model.QuestionId, model.TextValue);
			if (answer == null)
				return NotFound();

			return Ok(new { id = answer.Id, textValue = answer.TextValue });
		}

		[HttpPut]
		[Route("UpdateAnswer/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateAnswer(int id, [FromBody] UpdateAnswerModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var answer = await _questionnaireService.UpdateAnswerAsync(id, model.TextValue);
			if (answer == null)
				return NotFound();

			return Ok();
		}

		[HttpDelete]
		[Route("DeleteAnswer/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteAnswer(int id)
		{
			var result = await _questionnaireService.DeleteAnswerAsync(id);
			if (!result)
				return NotFound();

			return Ok();
		}

		private string GenerateQuestionHtml(Question question)
		{
			// This is a simple HTML generation for questions based on their type
			// In a real application, you might want to use a dedicated view engine or templates
			var html = $@"
			<div class=""mb-3 p-3 border border-primary rounded bg-light shadow-q"" data-question-id=""{question.Id}"">
				<div class=""mb-3"">
					<input class=""form-control fs-5"" type=""text"" placeholder=""Питання""
						id=""question{question.Id}"" name=""question{question.Id}""
						value=""{question.QuestionText}"" onblur=""updateQuestion({question.Id}, this.value)"">
				</div>";

			switch (question.InputType)
			{
				case InputTypeEnum.Checkbox:
					html += $@"
					<div class=""answer-container"" data-question-id=""{question.Id}"" data-input-type=""{(int)question.InputType}"">
						<div class=""mb-2 align-items-center d-flex"">
							<input type=""checkbox"" class=""form-check-input me-2 fs-5"" disabled>
							<input type=""text"" class=""form-control w-50"" placeholder=""Відповідь"" onblur=""addAnswer({question.Id}, this.value)"">
							<span class=""rounded-circle close-button ms-2"" onclick=""removeAnswerInput(this)"">
								<svg xmlns=""http://www.w3.org/2000/svg"" width=""16"" height=""16""
									fill=""currentColor"" class=""bi bi-x-lg"" viewBox=""0 0 16 16"">
									<path d=""M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8z"">
									</path>
								</svg>
							</span>
						</div>
					</div>";
					break;
				case InputTypeEnum.Radio:
					html += $@"
					<div class=""answer-container"" data-question-id=""{question.Id}"" data-input-type=""{(int)question.InputType}"">
						<div class=""mb-2 align-items-center d-flex"">
							<input type=""radio"" class=""form-check-input me-2 fs-5"" disabled>
							<input type=""text"" class=""form-control w-50"" placeholder=""Відповідь"" onblur=""addAnswer({question.Id}, this.value)"">
							<span class=""rounded-circle close-button ms-2"" onclick=""removeAnswerInput(this)"">
								<svg xmlns=""http://www.w3.org/2000/svg"" width=""16"" height=""16""
									fill=""currentColor"" class=""bi bi-x-lg"" viewBox=""0 0 16 16"">
									<path d=""M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8z"">
									</path>
								</svg>
							</span>
						</div>
					</div>";
					break;
				case InputTypeEnum.Text:
					html += $@"
					<div class=""answer-container"" data-question-id=""{question.Id}"" data-input-type=""{(int)question.InputType}"">
						<div class=""mb-2 align-items-center d-flex"">
							<input type=""text"" class=""form-control"" placeholder=""Відповідь користувача"" disabled>
						</div>
					</div>";
					break;
				case InputTypeEnum.Number:
					html += $@"
					<div class=""answer-container"" data-question-id=""{question.Id}"" data-input-type=""{(int)question.InputType}"">
						<div class=""mb-2 align-items-center d-flex"">
							<input type=""number"" class=""form-control w-25"" placeholder=""123"" disabled>
						</div>
					</div>";
					break;
				case InputTypeEnum.Date:
					html += $@"
					<div class=""answer-container"" data-question-id=""{question.Id}"" data-input-type=""{(int)question.InputType}"">
						<div class=""mb-2 align-items-center d-flex"">
							<input type=""date"" class=""form-control w-25"" disabled>
						</div>
					</div>";
					break;
				case InputTypeEnum.Time:
					html += $@"
					<div class=""answer-container"" data-question-id=""{question.Id}"" data-input-type=""{(int)question.InputType}"">
						<div class=""mb-2 align-items-center d-flex"">
							<input type=""time"" class=""form-control w-25"" disabled>
						</div>
					</div>";
					break;
			}

			html += $@"
				<div class=""mt-4 d-flex align-items-center"">
					<button type=""button"" class=""btn btn-secondary"" onclick=""addAnswerInput({question.Id})"">Додати відповідь</button>
					<div class=""ms-auto d-flex align-items-center"">
						<span class=""rounded-circle close-button ms-2 svg-18"" onclick=""duplicateQuestion({question.Id})"">
							<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20""
								 fill=""currentColor"" class=""bi bi-copy"" viewBox=""0 0 16 16"">
								<path fill-rule=""evenodd""
									d=""M4 2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2zm2-1a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1zM2 5a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1v-1h1v1a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h1v1z"" />
							</svg>
						</span>
						<span class=""rounded-circle close-button svg-18 ms-2"" onclick=""deleteQuestion({question.Id})"">
							<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20""
								 fill=""currentColor"" class=""bi bi-trash"" viewBox=""0 0 16 16"">
								<path d=""M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"" />
								<path d=""M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"" />
							</svg>
						</span>
					</div>
				</div>
			</div>";

			return html;
		}
	}

	#region Request Models
	public class QuestionnaireCreateModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
	}

	public class QuestionnaireUpdateModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}

	public class AddQuestionModel
	{
		public int QuestionnaireId { get; set; }
		public string QuestionText { get; set; }
		public InputTypeEnum InputType { get; set; }
	}

	public class UpdateQuestionModel
	{
		public string QuestionText { get; set; }
	}

	public class AddAnswerModel
	{
		public int QuestionId { get; set; }
		public string TextValue { get; set; }
	}

	public class UpdateAnswerModel
	{
		public string TextValue { get; set; }
	}
	#endregion
}
