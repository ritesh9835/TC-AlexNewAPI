using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class UserProfile
    {

        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Facebook { get; set; }
        public string Website { get; set; }
        public string Linkedin { get; set; }
        public Guid PrimaryService { get; set; }

    }
}
