using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Database.Interfaces;
using DataContracts.ViewModels;
using Newtonsoft.Json;

namespace ServiceContracts.Questionnaire
{
    public class QuestionnaireManager:IQuestionnaireManager
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;

        public QuestionnaireManager(IQuestionnaireRepository questionnaireRepository)
        {
            _questionnaireRepository = questionnaireRepository;
        }

        public async Task<List<QuestionnaireVM>> GetAllQuestionnaires()
        {
           return await _questionnaireRepository.GetAllQuestionnaires();
        }

        public async Task<QuestionnaireVM> GetQuestionnaireById(Guid id)
        {
            return await _questionnaireRepository.GetQuestionnaireById(id);
        }

        public async Task<bool> DeleteQuestionnaire(Guid id)
        {
            return await _questionnaireRepository.DeleteQuestionnaire(id);
        }

        public async Task<bool> AddQuestionnaire(DataContracts.Entities.Questionnaire questionnaire)
        {
            questionnaire.Id = Guid.NewGuid();
            if (questionnaire.Questions!=null)
            {
                questionnaire.TotalQuestions = questionnaire.Questions.Count;
            }
            return await _questionnaireRepository.AddQuestionnaire(questionnaire);
        }

        public async Task<bool> UpdateQuestionnaire(DataContracts.Entities.Questionnaire questionnaire)
        {
            var questionFromDb = await _questionnaireRepository.GetQuestionsByQuestionnaireId(questionnaire.Id);
            questionnaire.TotalQuestions = questionFromDb.Count;
            if (questionnaire.Questions!=null)
            {
                questionnaire.TotalQuestions += questionnaire.Questions.Count;
            }
            return await _questionnaireRepository.UpdateQuestionnaire(questionnaire);
        }

        public async Task<bool> IsQuestionnaireAlreadyExists(DataContracts.Entities.Questionnaire questionnaire, bool forUpdate)
        {
            return await _questionnaireRepository.IsQuestionnaireAlreadyExists(questionnaire,forUpdate);
        }

        public async Task<bool> IsQuestionnaireWithCategoryAlreadyExists(DataContracts.Entities.Questionnaire questionnaire, bool forUpdate)
        {
            return await _questionnaireRepository.IsQuestionnaireWithCategoryAlreadyExists(questionnaire, forUpdate);
        }

        public async Task<List<QuestionVM>> GetQuestionsByQuestionnaireId(Guid id)
        {
            var questions =await _questionnaireRepository.GetQuestionsByQuestionnaireId(id);
            foreach (var question in questions)
            {
                question.Choices = JsonConvert.DeserializeObject<QuestionOptionsVM>(question.Options);
                question.Options = null;
            }
            return questions;
        }

        public async Task<QuestionVM> GetQuestionById(Guid id)
        { 
            var question=await _questionnaireRepository.GetQuestionById(id);
            question.Choices = JsonConvert.DeserializeObject<QuestionOptionsVM>(question.Options);
            question.Options = null;
            return question;
        }

        public async Task<List<QuestionVM>> GetQuestionByCategoryId(Guid id)
        {
            var questions = await _questionnaireRepository.GetAllQuestionnairesByCategoryId(id);
            foreach(var question in questions)
            {
                question.Choices = JsonConvert.DeserializeObject<QuestionOptionsVM>(question.Options);
                question.Options = null;
            }
            return questions;
        }
    }
}
