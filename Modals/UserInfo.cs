using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAIKI.Modals
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public string ProfileImageURL { get; set; }
        public string PhoneNumber { get; set; }
        public string RegisteredLocation { get; set; }
        public string CurrentLocation { get; set; }
        public double MileRadius { get; set; }
        public DateTime RegisteredOn { get; set; }
        public string RegisteredOnString { get; set; }
        public string DeviceId { get; set; }
        public bool IsExternal { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}