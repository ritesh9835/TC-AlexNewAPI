using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TazzerClean.Util;

namespace DataContracts.Entities
{
    public class Pages
    {
        public Pages()
        {
            this.SectionsList = new List<Sections>();
        }
        public Guid Id { get; set; }
        public string PageTitle { get; set; } 
        [JsonProperty]
        public List<Sections> SectionsList { get; set; }
        public string PageImage { get; set; }
        public PagesTemplate Template { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string Sections { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }

    }
    public class Sections
    {
        public string SectionTitle { get; set; }
        public string SectionContent { get; set; }
    }
}
