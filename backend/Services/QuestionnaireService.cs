using AnketaDatabaseLibrary.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly AnketaDatabaseContext _context;

        public QuestionnaireService(AnketaDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Questionnaire> GetQuestionnaireByIdAsync(int id)
        {
            return await _context.Questionnaires
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Questionnaire> CreateQuestionnaireAsync(string title, string description, string userId)
        {
            var questionnaire = new Questionnaire
            {
                Title = title,
                Description = description,
                UserId = userId,
                DateOfCreation = DateTime.Now,
                LastEditDate = DateTime.Now,
                NumberOfPasses = 0
            };

            _context.Questionnaires.Add(questionnaire);
            await _context.SaveChangesAsync();
            return questionnaire;
        }

        public async Task<Questionnaire> UpdateQuestionnaireAsync(int id, string title, string description)
        {
            var questionnaire = await _context.Questionnaires.FindAsync(id);
            if (questionnaire == null)
                return null;

            questionnaire.Title = title;
            questionnaire.Description = description;
            questionnaire.LastEditDate = DateTime.Now;
            
            await _context.SaveChangesAsync();
            return questionnaire;
        }

        public async Task<bool> DeleteQuestionnaireAsync(int id)
        {
            var questionnaire = await _context.Questionnaires.FindAsync(id);
            if (questionnaire == null)
                return false;

            _context.Questionnaires.Remove(questionnaire);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Question> AddQuestionAsync(int questionnaireId, string questionText, InputTypeEnum inputType)
        {
            var questionnaire = await _context.Questionnaires
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == questionnaireId);

            if (questionnaire == null)
                return null;

            var question = new Question
            {
                QuestionText = questionText,
                InputType = inputType
            };

            // Initialize if null (shouldn't happen with proper initialization)
            if (questionnaire.Questions == null)
                questionnaire.Questions = new List<Question>();

            questionnaire.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> UpdateQuestionAsync(int questionId, string questionText)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
                return null;

            question.QuestionText = questionText;
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
                return false;

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Answer> AddAnswerAsync(int questionId, string textValue)
        {
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
                return null;

            var answer = new Answer
            {
                TextValue = textValue
            };

            if (question.Answers == null)
                question.Answers = new List<Answer>();

            question.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<Answer> UpdateAnswerAsync(int answerId, string textValue)
        {
            var answer = await _context.Answers.FindAsync(answerId);
            if (answer == null)
                return null;

            answer.TextValue = textValue;
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<bool> DeleteAnswerAsync(int answerId)
        {
            var answer = await _context.Answers.FindAsync(answerId);
            if (answer == null)
                return false;

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 