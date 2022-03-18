﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NAIKI.Backbone;
using NAIKI.DB;

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
                        var record = eDataBase.UserRewardBadges.Where(eRecord => eRecord.UserId == userId && eRecord.RewardBadgeId == eBadge.Id 
                        && eRecord.IsActive == true && eRecord.IsDeleted == false).FirstOrDefault();
                        if (record == null)
                        {
                            UserRewardBadge eReward = new UserRewardBadge();
                            eReward.UserId = userId;
                            eReward.RewardBadgeId = eBadge.Id;
                            eReward.EarnedOn = DateTime.Now;
                            eReward.IsActive = true;
                            eReward.IsDeleted = false;
                            eDataBase.UserRewardBadges.InsertOnSubmit(eReward);
                            eDataBase.SubmitChanges();
                        }
                       
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}