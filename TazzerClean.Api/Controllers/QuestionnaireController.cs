using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataContracts.Entities;
using DataContracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ServiceContracts.Questionnaire;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TazzerClean.Api.Controllers
{
    [Route("api/questionnaire"),Authorize,ApiController]
    public class QuestionnaireController : ControllerBase
    {
        private readonly IQuestionnaireManager _questionnaireManager;
        private readonly ILogger<QuestionnaireController> _logger;

        public QuestionnaireController(IQuestionnaireManager questionnaireManager, 
            ILogger<QuestionnaireController> logger)
        {
            _questionnaireManager = questionnaireManager;
            _logger = logger;
        }
        [HttpGet("getAllQuestionnaires")]
        [ProducesResponseType(typeof(List<QuestionnaireVM>), 200)]
        public async Task<IActionResult> GetAllQuestionnaires()
        {
            try
            {
                var result = await _questionnaireManager.GetAllQuestionnaires();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in QuestionnaireController:GetAllQuestionnaires", ex);
                return BadRequest();
            }
        }

        [HttpGet("getQuestionnaireById")]
        [ProducesResponseType(typeof(List<QuestionnaireVM>), 200)]
        public async Task<IActionResult> GetQuestionnaireById(Guid id)
        {
            try
            {
                if (id == default)
                {
                    return BadRequest();
                }

                var result = await _questionnaireManager.GetQuestionnaireById(id);
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in QuestionnaireController:GetQuestionnaireById",ex);
                return null;
            }
        }

        [HttpGet("getQuestionsByQuestionnaireId")]
        [ProducesResponseType(typeof(List<QuestionVM>), 200)]
        public async Task<IActionResult> GetQuestionsByQuestionnaireId(Guid id)
        {
            try
            {
                if (id == default)
                {
                    return BadRequest();
                }

                var result = await _questionnaireManager.GetQuestionsByQuestionnaireId(id);
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in QuestionnaireController:GetQuestionsByQuestionnaireId",ex);
                return null;
            }
        }

        [HttpGet("getQuestionById")]
        [ProducesResponseType(typeof(List<QuestionVM>), 200)]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            try
            {
                if (id == default)
                {
                    return BadRequest();
                }

                var result = await _questionnaireManager.GetQuestionById(id);
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in QuestionnaireController:GetQuestionById", ex);
                return null;
            }
        }

        [HttpPost("addQuestionnaire")]
        public async Task<IActionResult> AddQuestionnaire(QuestionnaireCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var questionnaire = MapToQuestionnaireEntityForAdd(request);
                if (await _questionnaireManager.IsQuestionnaireAlreadyExists(questionnaire, false))
                {
                    return BadRequest("Questionnaire with this name already exists.");
                }

                if (await _questionnaireManager.IsQuestionnaireWithCategoryAlreadyExists(questionnaire, false))
                {
                    return BadRequest("Questionnaire with this service category already exists.");
                }
                var result=await _questionnaireManager.AddQuestionnaire(questionnaire);
                if (!result)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in QuestionnaireController:AddQuestionnaire",ex);
                return BadRequest();
            }
        }

        [HttpPost("updateQuestionnaire")]
        public async Task<IActionResult> UpdateQuestionnaire(QuestionnaireUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var questionnaire = await MapToQuestionnaireEntityForUpdate(request);
                if (await _questionnaireManager.IsQuestionnaireAlreadyExists(questionnaire, true))
                {
                    return BadRequest("Questionnaire already exists.");
                }

                if (await _questionnaireManager.IsQuestionnaireWithCategoryAlreadyExists(questionnaire, true))
                {
                    return BadRequest("Questionnaire with this service category already exists.");
                }

                var result = await _questionnaireManager.UpdateQuestionnaire(questionnaire);
                if (!result)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in QuestionnaireController:UpdateQuestionnaire",ex);
                return BadRequest();
            }
        }

        [HttpDelete("deleteQuestionnaire")]
        public async Task<bool> DeleteQuestionnaire(Guid id)
        {
            try
            {
                return await _questionnaireManager.DeleteQuestionnaire(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in QuestionnaireController:DeleteQuestionnaire",ex);
                return false;
            }
        }

        [HttpGet("getQuestionsByCategoryId")]
        [ProducesResponseType(typeof(List<QuestionVM>), 200)]
        [Authorize]
        public async Task<IActionResult> GetQuestionByCategoryId(Guid categoryId)
        {
            try
            {
                if (categoryId == default)
                {
                    return BadRequest();
                }

                var result = await _questionnaireManager.GetQuestionByCategoryId(categoryId);
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in QuestionnaireController:GetQuestionsByCategoryId", ex);
                return null;
            }
        }

        #region Private Region
        private Questionnaire MapToQuestionnaireEntityForAdd(QuestionnaireCreateRequest request)
        {
            var questionnaire = new Questionnaire
            {
                ServiceCategoryId = request.ServiceCategoryId,
                QuestionnaireName = request.QuestionnaireName.Trim(),
                Status = request.Status
            };
            if (request.Questions != null)
            {
                questionnaire.Questions = new List<Question>();
                foreach (var question in request.Questions)
                {
                    questionnaire.Questions.Add(new Question()
                    {
                        QuestionType = question.QuestionType,
                        QuestionTxt = question.Question,
                        IsRequired = question.IsRequired,
                        Options =JsonConvert.SerializeObject(question.Options),
                    });
                }
            }

            return questionnaire;
        }
        private async Task<Questionnaire> MapToQuestionnaireEntityForUpdate(QuestionnaireUpdateRequest request)
        {
            var questionnaireFromDb =await _questionnaireManager.GetQuestionnaireById(request.Id);
            var questionnaire = new Questionnaire
            {
                Id = request.Id,
                ServiceCategoryId = request.ServiceCategoryId==default?questionnaireFromDb.ServiceCategoryId:request.ServiceCategoryId,
                QuestionnaireName = string.IsNullOrWhiteSpace(request.QuestionnaireName)? questionnaireFromDb.QuestionnaireName:request.QuestionnaireName.Trim(),
                Status = request.Status
            };
            if (request.Questions != null)
            {
                var questionsFromDb = await _questionnaireManager.GetQuestionsByQuestionnaireId(request.Id);
                questionnaire.Questions = new List<Question>();
                foreach (var question in request.Questions)
                {
                    if (questionsFromDb.Count == 0 || 
                        questionsFromDb.FirstOrDefault(x => x.Question.Trim()==question.Question.Trim())==null)
                    {
                        questionnaire.Questions.Add(new Question()
                        {
                            QuestionnaireId = request.Id,
                            QuestionType = question.QuestionType,
                            QuestionTxt = question.Question,
                            IsRequired = question.IsRequired,
                            Options = JsonConvert.SerializeObject(question.Options)
                        });
                    }
                }
            }

            return questionnaire;
        }
        #endregion
    }
}
