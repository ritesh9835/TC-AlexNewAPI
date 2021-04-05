using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class Unit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Display { get; set; }
        public int Amount { get; set; }
    }
}
