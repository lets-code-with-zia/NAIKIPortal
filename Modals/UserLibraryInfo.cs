using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAIKI.Modals
{
    public class UserLibraryInfo
    {
        public int Id { get; set; }
        public JobsInfo oJob { get; set; }
        public DateTime ActivatedOn { get; set; }
        public string ActivatedOnString { get; set; }
        public DateTime CompletedOn { get; set; }
        public string CompletedOnString { get; set; }
    }
}