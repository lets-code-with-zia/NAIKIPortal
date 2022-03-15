using System;
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
    }
}