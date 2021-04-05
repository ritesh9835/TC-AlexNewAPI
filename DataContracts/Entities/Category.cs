using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class Category
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public int Sequence { get; set; }

        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
