using NAIKI.Backbone;
using NAIKI.DB;
using NAIKI.Modals;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NAIKI.Services
{
    public class UserManagement
    {
        public void RegisterUser(UserInfo oUser)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    var record = eDataBase.Users.Where(eUData => eUData.LoginID == oUser.LoginId & eUData.IsDeleted == false).FirstOrDefault();
                    if(record != null)
                    {
                        oUser.IsError = true;
                        oUser.ErrorMessage = "User with the same Email already exists.";
                        return;
                    }

                    User eUser = new User();
                    eUser.UserName = oUser.UserName;
                    eUser.Email = oUser.Email;
                    eUser.LoginID = oUser.LoginId;
                    eUser.Password = oUser.Password;
                    eUser.IsExternal = oUser.IsExternal;
                    eUser.PhoneNumber = oUser.PhoneNumber;
                    eUser.UserTypeId = 2;
                    eUser.RegisteredLocation = oUser.RegisteredLocation;
                    eUser.DeviceId = oUser.DeviceId;
                    eUser.RegisteredOn = DateTime.Now;
                    eUser.ImageURL = oUser.ProfileImageURL;
                    eUser.IsActive = true;
                    eUser.IsDeleted = false;
                    eUser.IsVerified = true;
                    eDataBase.Users.InsertOnSubmit(eUser);
                    eDataBase.SubmitChanges();
                    oUser.Id = eUser.Id;
                }
            }
            catch (Exception ex)
            {

                oUser.IsError = true;
                oUser.ErrorMessage = ex.Message;
            }
        }
        public UserInfo Login(string loginId, string password, bool isExternal)
        {
            UserInfo oUser = new UserInfo();
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    User eUser = new User();
                    eUser = eDataBase.Users.Where(eUData => eUData.LoginID == loginId & eUData.IsDeleted == false &
                               eUData.IsActive == true).FirstOrDefault();

                    if (eUser == null)
                    {
                        oUser.IsError = true;
                        oUser.ErrorMessage = "No User exists with the provided credentials";
                        return oUser;
                    }

                    if (!eUser.IsExternal)
                    {
                        if (eUser.Password != password)
                        {
                            oUser.IsError = true;
                            oUser.ErrorMessage = "No User exists with the provided credentials";
                            return oUser;
                        }
                    }

                    

                    if (!eUser.IsVerified)
                    {
                        oUser.IsError = true;
                        oUser.ErrorMessage = "Account is not verified";
                        return oUser;
                    }
                    oUser.Id = eUser.Id;
                    oUser.UserName = eUser.UserName;
                    oUser.Email = eUser.Email;
                    oUser.LoginId = eUser.LoginID;
                    oUser.Password = eUser.Password;
                    oUser.IsExternal = eUser.IsExternal;
                    oUser.PhoneNumber = eUser.PhoneNumber;
                    oUser.UserTypeId = eUser.UserTypeId;
                    oUser.UserTypeName = eUser.UserType.TypeName;
                    oUser.RegisteredLocation = eUser.RegisteredLocation;
                    oUser.DeviceId = eUser.DeviceId;
                    oUser.RegisteredOn = eUser.RegisteredOn;
                    oUser.RegisteredOnString = eUser.RegisteredOn.ToString(ConfigurationManager.AppSettings["DateFormat"]);
                    oUser.ProfileImageURL = eUser.ImageURL;

                }
            }
            catch (Exception ex)
            {

                oUser.IsError = true;
                oUser.ErrorMessage = ex.Message;
            }

            return oUser;
        }

        public void UpdateMyLocation(int uID , string location)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    User eUser = new User();
                    eUser = eDataBase.Users.Where(eUData => eUData.Id == uID && eUData.IsDeleted == false && eUData.IsActive == true).FirstOrDefault();
                    if (eUser == null)
                    {
                        throw new Exception("Invalid or no user id found");
                    }
                    eUser.RegisteredLocation = location;
                    eDataBase.SubmitChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public string GetLocationsByUserId(int uID)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    User eUser = new User();
                    eUser = eDataBase.Users.Where(eUData => eUData.Id == uID && eUData.IsDeleted == false && eUData.IsActive == true).FirstOrDefault();
                    if (eUser == null)
                    {
                        throw new Exception("Invalid or no user id found");
                    }
                    string location = eUser.RegisteredLocation;
                    return location;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<JobsInfo> GetJobsByUserId(int uID)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    List<Job> eJobs = new List<Job>();
                    List<JobsInfo> oJobs = new List<JobsInfo>();
                    eJobs = eDataBase.Jobs.Where(eUData => eUData.UserId == uID && eUData.IsDeleted == false && eUData.IsActive == true).ToList();
                    foreach (var item in eJobs)
                    {
                        JobsInfo oJob = new JobsInfo();
                        oJob.Id = item.Id;
                        oJob.UserId = item.UserId;
                        oJob.JobTypeName = item.JobType.TypeName;
                        oJob.JobDetails = item.Detail;
                        oJob.FileURL = ConfigurationManager.AppSettings["BaseURL"] + "JobFiles/" + item.FileURL;
                        oJob.Location = item.Location;
                        oJob.UserName = item.User.UserName;
                        oJob.StatusName = item.Status.StatusName;
                        oJob.PostedOn = item.PostedOn;
                        oJob.PostedOnString = item.PostedOn.ToString(ConfigurationManager.AppSettings["ShortDateFormat"]);
                        oJobs.Add(oJob);

                    }

                    return oJobs;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<JobsInfo> GetJobsByLocation(string location)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    List<Job> eJobs = new List<Job>();
                    List<JobsInfo> oJobs = new List<JobsInfo>();
                    eJobs = eDataBase.Jobs.Where(eJData => eJData.Location == location && eJData.IsDeleted == false && eJData.IsActive == true).ToList();
                    foreach (var item in eJobs)
                    {
                        JobsInfo oJob = new JobsInfo();
                        oJob.Id = item.Id;
                        oJob.UserId = item.UserId;
                        oJob.JobTypeName = item.JobType.TypeName;
                        oJob.JobDetails = item.Detail;
                        oJob.FileURL = ConfigurationManager.AppSettings["BaseURL"] + "JobFiles/" + item.FileURL;
                        oJob.Location = item.Location;
                        oJob.UserName = item.User.UserName;
                        oJob.StatusName = item.Status.StatusName;
                        oJob.PostedOn = item.PostedOn;
                        oJob.PostedOnString = item.PostedOn.ToString(ConfigurationManager.AppSettings["ShortDateFormat"]);
                        oJobs.Add(oJob);

                    }

                    return oJobs;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void PostJob(JobsInfo oJob)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    Job eJob = new Job();
                    eJob.UserId = oJob.UserId;
                    eJob.JobTypeId = oJob.JobTypeId;
                    eJob.StatusId = oJob.StatusId;
                    eJob.Location = oJob.Location;
                    eJob.Detail = oJob.JobDetails;
                    eJob.IsActive = true;
                    eJob.IsDeleted = false;
                    eJob.PostedOn = DateTime.Now;
                    eDataBase.Jobs.InsertOnSubmit(eJob);
                    eDataBase.SubmitChanges();
                    eJob.FileURL = eJob.Id + oJob.FileURL;
                    eDataBase.SubmitChanges();
                    oJob.Id = eJob.Id;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}