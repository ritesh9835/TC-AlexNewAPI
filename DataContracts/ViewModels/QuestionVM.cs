using System;

namespace DataContracts.ViewModels
{
    public class QuestionVM
    {
        public Guid Id { get; set; }
        public Guid QuestionnaireId { get; set; }
        public int QuestionType { get; set; }
        public string Question { get; set; }
        public bool IsRequired { get; set; }
        public string Options { get; set; }
        public QuestionOptionsVM Choices { get; set; }
    }
}
