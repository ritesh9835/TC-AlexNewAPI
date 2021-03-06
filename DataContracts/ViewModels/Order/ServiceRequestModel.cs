using System;

namespace DataContracts.ViewModels.Order
{
    public class ServiceRequestModel
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public int TotalTime { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Postcode { get; set; }

    }
}
