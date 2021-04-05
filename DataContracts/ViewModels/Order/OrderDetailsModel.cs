using System;
using System.Collections.Generic;
using System.Text;
using TazzerClean.Util;

namespace DataContracts.ViewModels.Order
{
    public class OrderDetailsModel
    {
        public OrderDetailsModel()
        {
            this.ExtraServiceList = new List<Guid>(); 
        }
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public int TotalTime { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Postcode { get; set; }

        public Guid ServiceRequestId { get; set; }
        public Guid SubscriptionId { get; set; }
        public ServiceFrequency ServiceFrequency { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Flat { get; set; }
        public string Street { get; set; }
        public decimal Amount { get; set; }
        public decimal GrandTotal { get; set; }
        public string Promocode { get; set; }
        public string TransactionDetails { get; set; }
        public string ExtraServices { get; set; }
        public List<Guid> ExtraServiceList { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProfessionalId { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
    }
}
