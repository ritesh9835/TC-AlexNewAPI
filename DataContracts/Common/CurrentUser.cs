using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Common
{
    public class CurrentUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
