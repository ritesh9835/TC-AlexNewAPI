using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid QuestionnaireId { get; set; }
        public int QuestionType { get; set; }
        public string QuestionTxt { get; set; }
        public bool IsRequired { get; set; }
        public string Options { get; set; }
    }
}
