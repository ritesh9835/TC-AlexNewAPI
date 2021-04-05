using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataContracts.Entities
{
    public class Questionnaire
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string QuestionnaireName { get; set; }
        [Required]
        public Guid ServiceCategoryId { get; set; }
        public int TotalQuestions { get; set; }
        public bool Status { get; set; }
        public List<Question> Questions { get; set; }
    }
}
