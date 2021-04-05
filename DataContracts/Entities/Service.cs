using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public Guid? PriceUnit { get; set; }
        public decimal Quantity { get; set; }
        public Guid? QuantityUnit { get; set; }
        public decimal Duration { get; set; }
        public Guid? DurationUnit { get; set; }
        public string Extra { get; set; }
    }
}
