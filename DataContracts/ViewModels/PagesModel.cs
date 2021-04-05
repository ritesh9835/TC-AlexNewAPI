using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TazzerClean.Util;

namespace DataContracts.ViewModels
{
    public class PagesModel
    {
        public Guid Id { get; set; }
        public string PageTitle { get; set; }
        public List<Sections> Sections { get; set; }
        public string PageImage { get; set; }
        public PagesTemplate Template { get; set; }
        public bool IsActive { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}
