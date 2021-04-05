using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataContracts.Entities;
using DataContracts.ViewModels;

namespace DataAccess.Database.Interfaces
{
    public interface IQuestionnaireRepository
    {
        Task<List<QuestionnaireVM>> GetAllQuestionnaires();
        Task<QuestionnaireVM> GetQuestionnaireById(Guid id);
        Task<bool> DeleteQuestionnaire(Guid id);
        Task<bool> AddQuestionnaire(Questionnaire questionnaire);
        Task<bool> UpdateQuestionnaire(Questionnaire questionnaire);
        Task<bool> IsQuestionnaireAlreadyExists(Questionnaire questionnaire, bool forUpdate = false);
        Task<bool> IsQuestionnaireWithCategoryAlreadyExists(Questionnaire questionnaire, bool forUpdate=false);
        Task<bool> AddQuestion(Questionnaire questionnaire);
        Task<QuestionVM> GetQuestionById(Guid id);
        Task<List<QuestionVM>> GetQuestionsByQuestionnaireId(Guid questionnaireId);
        Task<List<QuestionVM>> GetAllQuestionnairesByCategoryId(Guid categoryId);
    }
}
