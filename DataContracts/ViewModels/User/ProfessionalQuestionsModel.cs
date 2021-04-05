using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.User
{
    public class ProfessionalQuestionsModel
    {
        public ProfessionalQuestionsModel()
        {
            this.Questions = new List<Question>();
        }

        public List<UserServices> UserServices { get; set; }
        public List<Question> Questions { get; set; }
    }
    public class Question
    {
        public Guid QuestionId { get; set; }
        public string Choices { get; set; }
    }
}
