using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContracts.ViewModels
{
    public class QuestionnaireCreateRequest
    {
        [Required]
        [MaxLength(200)]
        public string QuestionnaireName { get; set; }
        [Required]
        public Guid ServiceCategoryId { get; set; }
        public bool Status { get; set; }
        public List<QuestionCreateRequest> Questions { get; set; }

        public class QuestionCreateRequest
        {
            [Required]
            public int QuestionType { get; set; }
            [Required]
            public string Question { get; set; }
            public bool IsRequired { get; set; }
            [Required]
            public QuestionOptionsCreateRequest Options { get; set; }
        }

        public class QuestionOptionsCreateRequest
        {
            public string Option1 { get; set; }
            public string Option2 { get; set; }
            public string Option3 { get; set; }
            public string Option4 { get; set; }
        }
    }
    public class QuestionnaireUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string QuestionnaireName { get; set; }
        [Required]
        public Guid ServiceCategoryId { get; set; }
        public bool Status { get; set; }
        public List<QuestionUpdateRequest> Questions { get; set; }

        public class QuestionUpdateRequest
        {
            [Required]
            public int QuestionType { get; set; }
            [Required]
            public string Question { get; set; }
            public bool IsRequired { get; set; }
            [Required]
            public QuestionOptionsUpdateRequest Options { get; set; }
        }

        public class QuestionOptionsUpdateRequest
        {
            public string Option1 { get; set; }
            public string Option2 { get; set; }
            public string Option3 { get; set; }
            public string Option4 { get; set; }
        }
    }
}
