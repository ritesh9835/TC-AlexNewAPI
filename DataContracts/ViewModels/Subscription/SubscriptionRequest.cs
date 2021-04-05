using System;
using System.ComponentModel.DataAnnotations;

namespace DataContracts.ViewModels.Subscription
{
    public class SubscriptionCreateRequest
    {
        public Guid Id { get; set; }
        public string SubscriptionName { get; set; }
        public Weekly Weekly { get; set; }
        public BiWeekly BiWeekly { get; set; }
        public Monthly Monthly { get; set; }
    }
    public class Weekly
    {
        public int Month3 { get; set; }
        public int Month6 { get; set; }
        public int Month12 { get; set; }
    }
    public class Monthly
    {
        public int Month3 { get; set; }
        public int Month6 { get; set; }
        public int Month12 { get; set; }
    }
    public class BiWeekly
    {
        public int Month3 { get; set; }
        public int Month6 { get; set; }
        public int Month12 { get; set; }
    }

    public class SubscriptionUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        public string SubscriptionName { get; set; }
        public WeeklyForUpdate Weekly { get; set; }
        public BiWeeklyForUpdate BiWeekly { get; set; }
        public MonthlyForUpdate Monthly { get; set; }
    }
    public class WeeklyForUpdate
    {
        [Required]
        public Guid Id { get; set; }
        public int Month3 { get; set; }
        public int Month6 { get; set; }
        public int Month12 { get; set; }
    }
    public class MonthlyForUpdate
    {
        [Required]
        public Guid Id { get; set; }
        public int Month3 { get; set; }
        public int Month6 { get; set; }
        public int Month12 { get; set; }
    }
    public class BiWeeklyForUpdate
    {
        [Required]
        public Guid Id { get; set; }
        public int Month3 { get; set; }
        public int Month6 { get; set; }
        public int Month12 { get; set; }
    }
}
