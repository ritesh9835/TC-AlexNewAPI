using System;
using System.Collections.Generic;

namespace DataContracts.ViewModels
{
    public class QuestionnaireVM
    {
        public Guid Id { get; set; }
        public string QuestionnaireName { get; set; }
        public string ServiceCategory { get; set; }
        public int TotalQuestions { get; set; }
        public Guid ServiceCategoryId { get; set; }
        public bool Status { get; set; }
        public List<QuestionVM> Questions { get; set; }
    }
}
