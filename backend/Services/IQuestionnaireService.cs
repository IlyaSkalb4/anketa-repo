using backend.Data.Models;

namespace backend.Services
{
    public interface IQuestionnaireService
    {
        Task<Questionnaire> GetQuestionnaireByIdAsync(int id);
        Task<Questionnaire> CreateQuestionnaireAsync(string title, string description, string userId);
        Task<Questionnaire> UpdateQuestionnaireAsync(int id, string title, string description);
        Task<bool> DeleteQuestionnaireAsync(int id);
        Task<Question> AddQuestionAsync(int questionnaireId, string questionText, InputTypeEnum inputType);
        Task<Question> UpdateQuestionAsync(int questionId, string questionText);
        Task<bool> DeleteQuestionAsync(int questionId);
        Task<Answer> AddAnswerAsync(int questionId, string textValue);
        Task<Answer> UpdateAnswerAsync(int answerId, string textValue);
        Task<bool> DeleteAnswerAsync(int answerId);
    }
} 