using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Database.Repositories
{
    public class QuestionnaireRepository: IQuestionnaireRepository
    {
        private readonly IConfiguration _config;

        public QuestionnaireRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<QuestionnaireVM>> GetAllQuestionnaires()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestionnaire";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var query = await _connection.QueryMultipleAsync(
                procedure,
                new {action= "SELECTALL" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var questionnaires = (await query.ReadAsync<QuestionnaireVM>()).ToList();
            return questionnaires;
        }
        public async Task<QuestionnaireVM> GetQuestionnaireById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestionnaire";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var query = await _connection.QueryMultipleAsync(
                procedure,
                new { id,action = "BYID" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var questionnaire = await query.ReadSingleAsync<QuestionnaireVM>();
            
            return questionnaire;
        }
        public async Task<bool> DeleteQuestionnaire(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestionnaire";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var rowsAffected=await _connection.ExecuteAsync(
                procedure,
                new { id,action="DELETE" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );
            
            return rowsAffected > 0;
        }
        public async Task<bool> AddQuestionnaire(Questionnaire questionnaire)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestionnaire";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var rowsAffected=await _connection.ExecuteAsync(
                procedure,
                new
                {
                    questionnaire.Id, questionnaire.ServiceCategoryId, 
                    questionnaire.QuestionnaireName, questionnaire.TotalQuestions, questionnaire.Status,
                    action="ADD"
                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );
            
            if (questionnaire.Questions == null)
            {
                return rowsAffected > 0;
            }

            return await AddQuestion(questionnaire);
        }
        public async Task<bool> AddQuestion(Questionnaire questionnaire)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var rowsAffected = 0;
            if (questionnaire.Questions != null && questionnaire.Questions.Count>0)
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                foreach (var question in questionnaire.Questions)
                {
                    question.QuestionnaireId = questionnaire.Id;
                    question.Id = Guid.NewGuid();

                    var procedure = "spQuestions";
                    rowsAffected = await _connection.ExecuteAsync(
                        procedure,
                        new
                        {
                            question.Id,
                            QuestionnaireId = questionnaire.Id,
                            question.QuestionType,
                            question.QuestionTxt,
                            question.IsRequired,
                            question.Options,
                            Action = "ADD"
                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 10
                    );
                }
                
            }
            else
            {
                return true;
            }

            return rowsAffected > 0;
        }
        public async Task<bool> UpdateQuestionnaire(Questionnaire questionnaire)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestionnaire";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var rowsAffected=await _connection.ExecuteAsync(
                procedure,
                new
                {
                    questionnaire.Id, questionnaire.ServiceCategoryId, 
                    questionnaire.QuestionnaireName, questionnaire.TotalQuestions,
                    questionnaire.Status, action = "UPDATE"
                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );
            
            if (questionnaire.Questions == null)
            {
                return rowsAffected > 0;
            }

            return await AddQuestion(questionnaire);
        }
        public async Task<bool> IsQuestionnaireAlreadyExists(Questionnaire questionnaire, bool forUpdate = false)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestionnaire_AlreadyExists";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var result = await _connection.ExecuteScalarAsync<bool>(
                procedure,
                new { questionnaire.Id,questionnaire.QuestionnaireName,action = forUpdate?"UPDATE":"ADD" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            
            return result;
        }
        public async Task<bool> IsQuestionnaireWithCategoryAlreadyExists(Questionnaire questionnaire,bool forUpdate=false)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestionnaireWithCategory_AlreadyExists";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var result = await _connection.ExecuteScalarAsync<bool>(
                procedure,
                new { questionnaire.Id, questionnaire.ServiceCategoryId, action = forUpdate ? "UPDATE" : "ADD" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            
            return result;
        }
        public async Task<List<QuestionVM>> GetQuestionsByQuestionnaireId(Guid questionnaireId)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestions";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var query = await _connection.QueryMultipleAsync(
                procedure,
                new { questionnaireId, Action = "BYQUESTIONNAIREID" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var questions = (await query.ReadAsync<QuestionVM>()).ToList();
            
            return questions;
        }
        public async Task<QuestionVM> GetQuestionById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestions";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var query = await _connection.QueryMultipleAsync(
                procedure,
                new { id,Action="BYID"},
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var questions = await query.ReadSingleAsync<QuestionVM>();
            
            return questions;
        }
        public async Task<List<QuestionVM>> GetAllQuestionnairesByCategoryId(Guid categoryId)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            var procedure = "spQuestions";
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            var query = await _connection.QueryMultipleAsync(
                procedure,
                new { categoryId, Action = "BYCATEGORYID" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var questions = (await query.ReadAsync<QuestionVM>()).ToList();
            
            return questions;
        }
    }
}
