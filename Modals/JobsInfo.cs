using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAIKI.Modals
{
    public class JobsInfo
    {
        public int Id { get; set; }

        public string JobTypeName { get; set; }

        public int UserId { get; set; }

        public string Location { get; set; }

        public string FileURL { get; set; }

        public string JobDetails { get; set; }

        public string UserName{ get; set; }

        public string StatusName { get; set; }

        public string PostedOnString { get; set; }

        public DateTime PostedOn { get; set; }
        public int JobTypeId { get; set; }
        public int StatusId { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}