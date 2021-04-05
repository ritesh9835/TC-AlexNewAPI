using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataContracts.ViewModels;

namespace ServiceContracts.Questionnaire
{
    public interface IQuestionnaireManager
    {
        Task<List<QuestionnaireVM>> GetAllQuestionnaires();
        Task<QuestionnaireVM> GetQuestionnaireById(Guid id);
        Task<bool> DeleteQuestionnaire(Guid id);
        Task<bool> AddQuestionnaire(DataContracts.Entities.Questionnaire questionnaire);
        Task<bool> UpdateQuestionnaire(DataContracts.Entities.Questionnaire questionnaire);
        Task<bool> IsQuestionnaireAlreadyExists(DataContracts.Entities.Questionnaire questionnaire, bool forUpdate);
        Task<bool> IsQuestionnaireWithCategoryAlreadyExists(DataContracts.Entities.Questionnaire questionnaire, bool forUpdate);
        Task<List<QuestionVM>> GetQuestionsByQuestionnaireId(Guid id);
        Task<QuestionVM> GetQuestionById(Guid id);
        Task<List<QuestionVM>> GetQuestionByCategoryId(Guid id);
    }
}
