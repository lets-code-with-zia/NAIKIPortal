using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NAIKI.Backbone;
using NAIKI.DB;
using NAIKI.Modals;
using NAIKI.Utilis;

namespace NAIKI.Services
{
    public class JobFactory
    {
        public void AcceptJob(int userId , int jobId)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    Job eJob = new Job();
                    eJob = eDataBase.Jobs.Where(eJData => eJData.Id == jobId && eJData.IsDeleted == false && eJData.IsActive == true).FirstOrDefault();
                    if (eJob == null)
                    {
                        throw new Exception("Invalid or no job found");
                    }
                    eJob.StatusId = 2;

                    UserJobLibrary eJLib = new UserJobLibrary();
                    eJLib.UserId = userId;
                    eJLib.JobId = jobId;
                    eJLib.StatusId = eJob.StatusId;
                    eJLib.ActivatedOn = DateTime.Now;
                    eJLib.IsActive = true;
                    eJLib.IsDeleted = false;

                    eDataBase.UserJobLibraries.InsertOnSubmit(eJLib);
                    eDataBase.SubmitChanges();
                }
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void MarkAsDoneJob(int userId , int jobId)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    Job eJob = new Job();
                    eJob = eDataBase.Jobs.Where(eJData => eJData.Id == jobId && eJData.IsDeleted == false && eJData.IsActive == true).FirstOrDefault();
                    if (eJob == null)
                    {
                        throw new Exception("Invalid or no job found");
                    }
                    eJob.StatusId = 3;
                    var eLib = eJob.UserJobLibraries.Where(eLibData => eLibData.IsActive == true && eLibData.IsDeleted == false && eLibData.UserId == userId).FirstOrDefault();
                    if (eLib == null)
                    {
                        throw new Exception("No or invalid entry Found");

                    }
                    eLib.StatusId = 3;
                    eLib.CompletedOn = DateTime.Now;

                    eDataBase.SubmitChanges();

                    var jobCount = eDataBase.UserJobLibraries.Where(eJCount => eJCount.UserId == userId && eJCount.IsActive == true && eJCount.IsDeleted == false && eJCount.StatusId == 3).Count();
                    var eBadge = eDataBase.RewardBadges.Where(eBData => jobCount ==  eBData.MileStoneCount && eBData.IsActive == true).FirstOrDefault();

                    if (eBadge != null)
                    {
                        new RewardManagement().AddReward(userId, eBadge.Id);
                       
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<JobsInfo> GetJobsByUserLocation(int uID)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {

                    var eSetting = eDataBase.UserSettings.Where(eSData => eSData.UserId == uID & eSData.IsActive == true & eSData.IsDeleted == false).FirstOrDefault();
                    if (eSetting == null)
                    {
                        throw new Exception("Location is not set. Kindly set your current location");
                    }
                    var eUser = eSetting.User;
                    List<Job> eJobs = new List<Job>();
                    List<JobsInfo> oJobs = new List<JobsInfo>();
                    eJobs = eDataBase.Jobs.Where(eUData => eUData.IsDeleted == false && eUData.IsActive == true).ToList();
                    foreach (var item in eJobs)
                    {
                        if (new CommonMethods().CalculateMileRadius(item.Location, eSetting.CurrentLoction) <= eSetting.RadiusInMile)
                        {
                            JobsInfo oJob = new JobsInfo();
                            oJob.Id = item.Id;
                            oJob.UserId = item.UserId;
                            oJob.JobTypeId = item.JobTypeId;
                            oJob.JobTypeName = item.JobType.TypeName;
                            oJob.JobDetails = item.Detail;
                            oJob.FileURL = ConfigurationManager.AppSettings["BaseURL"] + "JobFiles/" + item.FileURL;
                            oJob.Location = item.Location;
                            oJob.UserName = item.User.UserName;
                            oJob.StatusId = item.StatusId;
                            oJob.StatusName = item.Status.StatusName;
                            oJob.PostedOn = item.PostedOn;
                            oJob.PostedOnString = new CommonMethods().ToShortDateTime(item.PostedOn);
                            oJobs.Add(oJob);
                        }


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
                        oJob.JobTypeId = item.JobTypeId;
                        oJob.JobTypeName = item.JobType.TypeName;
                        oJob.JobDetails = item.Detail;
                        oJob.FileURL = ConfigurationManager.AppSettings["BaseURL"] + "JobFiles/" + item.FileURL;
                        oJob.Location = item.Location;
                        oJob.UserName = item.User.UserName;
                        oJob.StatusId = item.StatusId;
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
                    eJob.FileURL = eJob.Id + new CommonMethods().GetRandomString(8) + ".jpeg";
                    eDataBase.SubmitChanges();
                    var eUsers = eDataBase.Users.Where(eUData => eUData.Id != oJob.UserId & eUData.IsActive == true & eUData.IsDeleted == false)
                                .Select(eUSelect => new { Id = eUSelect.Id, oSetting = eUSelect.UserSettings.Where(eSData => eSData.IsActive == true & eSData.IsDeleted == false).FirstOrDefault() }).ToList();
                    List<string> uIDs = new List<string>();
                    foreach (var eUser in eUsers)
                    {
                        if (eUser.oSetting != null)
                        {
                            if (new CommonMethods().CalculateMileRadius(oJob.Location, eUser.oSetting.CurrentLoction) <= eUser.oSetting.RadiusInMile)
                            {
                                uIDs.Add(eUser.Id.ToString());
                            }
                        }
                    }
                    if (uIDs.Count > 0)
                    {
                        // notification work
                        new NotificationsManagement().NotifyExternal(uIDs);
                    }
                    oJob.Id = eJob.Id;
                    oJob.FileURL = eJob.FileURL;
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
                    eJobs = eDataBase.Jobs.Where(eJData => eJData.UserId == uID & eJData.IsDeleted == false & eJData.IsActive == true).ToList();
                    foreach (var item in eJobs)
                    {
                        JobsInfo oJob = new JobsInfo();
                        oJob.Id = item.Id;
                        oJob.UserId = item.UserId;
                        oJob.JobTypeId = item.JobTypeId;
                        oJob.JobTypeName = item.JobType.TypeName;
                        oJob.JobDetails = item.Detail;
                        oJob.FileURL = ConfigurationManager.AppSettings["BaseURL"] + "JobFiles/" + item.FileURL;
                        oJob.Location = item.Location;
                        oJob.UserName = item.User.UserName;
                        oJob.StatusId = item.StatusId;
                        oJob.StatusName = item.Status.StatusName;
                        oJob.PostedOn = item.PostedOn;
                        oJob.PostedOnString = new CommonMethods().ToShortDateTime(item.PostedOn);
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

        public List<UserLibraryInfo> GetMyLibrary(int userId)
        {
            try
            {
                List<UserLibraryInfo> oLibs = new List<UserLibraryInfo>();
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    var eLibs = eDataBase.UserJobLibraries.Where(eJData => eJData.UserId == userId & eJData.IsActive == true & eJData.IsDeleted == false).ToList();
                    foreach (var eLib in eLibs)
                    {
                        UserLibraryInfo oLib = new UserLibraryInfo();
                        oLib.Id = eLib.Id;
                        var eJob = eLib.Job;
                        JobsInfo oJob = new JobsInfo();
                        oJob.Id = eJob.Id;
                        oJob.UserId = eJob.UserId;
                        oJob.JobTypeId = eJob.JobTypeId;
                        oJob.JobTypeName = eJob.JobType.TypeName;
                        oJob.JobDetails = eJob.Detail;
                        oJob.FileURL = ConfigurationManager.AppSettings["BaseURL"] + "JobFiles/" + eJob.FileURL;
                        oJob.Location = eJob.Location;
                        oJob.UserName = eJob.User.UserName;
                        oJob.StatusId = eJob.StatusId;
                        oJob.StatusName = eJob.Status.StatusName;
                        oJob.PostedOn = eJob.PostedOn;
                        oJob.PostedOnString = new CommonMethods().ToShortDateTime(eJob.PostedOn);
                        oLib.oJob = oJob;
                        oLib.ActivatedOn = eLib.ActivatedOn;
                        oLib.ActivatedOnString = new CommonMethods().ToShortDateTime(eLib.ActivatedOn);
                        if (eLib.CompletedOn.HasValue)
                        {
                            oLib.CompletedOn = eLib.CompletedOn.Value;
                            oLib.CompletedOnString = new CommonMethods().ToShortDateTime(eLib.CompletedOn.Value);
                        }
                        oLibs.Add(oLib);
                    }
                }
                return oLibs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}