using NAIKI.Modals;
using NAIKI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NAIKI.Api
{
    /// <summary>
    /// Summary description for apicall
    /// </summary>
    public class apicall : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            jsonSerializer.MaxJsonLength = Int32.MaxValue;

            var jsonString = String.Empty;

            var methodName = Convert.ToString(context.Request.QueryString["methodName"]);

            try
            {
                #region Login
                if (methodName.ToLower() == "Login".ToLower())
                {
                    if (string.IsNullOrEmpty(context.Request.QueryString["loginId"]))
                    {
                        throw new Exception("Kindly provide login ID");
                    }

                    if (string.IsNullOrEmpty(context.Request.QueryString["isExternal"]))
                    {
                        throw new Exception("Kindly mention account type");
                    }

                    var isExternal = false;
                    try
                    {
                        isExternal = Convert.ToBoolean(context.Request.QueryString["isExternal"]);
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Kindly mention account type");
                    }

                    if(!isExternal & string.IsNullOrEmpty(context.Request.QueryString["password"]))
                    {
                        throw new Exception("Kindly provide password");
                    }

                    var loginId = context.Request.QueryString["loginId"];
                    var password = context.Request.QueryString["password"];
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(new UserManagement().Login(loginId, password, isExternal)));
                    return;

                }
                #endregion

                #region SignUp
                if (methodName.ToLower() == "SignUp".ToLower())
                {
                    context.Request.InputStream.Position = 0;
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }

                    var oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);
                    new UserManagement().RegisterUser(oUser);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(oUser));
                    return;
                }
                #endregion

                #region UpdateMyLocation
                if (methodName.ToLower() == "UpdateMyLocation".ToLower())
                {
                    int userId = 0;
                    int.TryParse(context.Request.QueryString["uID"], out userId);
                    if (userId == 0)
                    {
                        throw new Exception("Invalid or no user id found");
                    }
                    if (string.IsNullOrEmpty(context.Request.QueryString["coordinates"]))
                    {
                        throw new Exception("Kindly provide current location");
                    }
                    var location = context.Request.QueryString["coordinates"];
                    UserManagement oUser = new UserManagement();
                    oUser.UpdateMyLocation(userId , location);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    context.Response.Write(jsonSerializer.Serialize(new Dictionary<string, dynamic>() { { "IsError", false }, { "ErrorMessage", "" } }));

                }
                #endregion

                #region GetLocationsByUserId
                if (methodName.ToLower() == "GetLocationsByUserId".ToLower())
                {
                    int userId = 0;
                    int.TryParse(context.Request.QueryString["uID"], out userId);
                    if (userId == 0)
                    {
                        throw new Exception("Invalid or no user id found");
                    }
                    UserManagement oUser = new UserManagement();
                    string location = oUser.GetLocationsByUserId(userId);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    context.Response.Write(jsonSerializer.Serialize(new Dictionary<string, dynamic>() { { "IsError", false }, { "UserLocation", location } }));
                }
                #endregion

                #region GetJobsByUserId
                if (methodName.ToLower() == "GetJobsByUserId".ToLower())
                {
                    int userId = 0;
                    int.TryParse(context.Request.QueryString["uID"], out userId);
                    if (userId == 0)
                    {
                        throw new Exception("Invalid or no user id found");
                    }

                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    context.Response.Write(jsonSerializer.Serialize(new Dictionary<string, dynamic>() { { "IsError", false }, { "oData", new UserManagement().GetJobsByUserId(userId) } }));
                }
                #endregion

                #region GetJobsByLocation
                if (methodName.ToLower() == "GetJobsByLocation".ToLower())
                {
                    if (string.IsNullOrEmpty(context.Request.QueryString["coordinates"]))
                    {
                        throw new Exception("Kindly provide current location");
                    }
                    var location = context.Request.QueryString["coordinates"];

                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    context.Response.Write(jsonSerializer.Serialize(new Dictionary<string, dynamic>() { { "IsError", false }, { "oData", new UserManagement().GetJobsByLocation(location) } }));
                }
                #endregion

                #region PostJob
                if (methodName.ToLower() == "PostJob".ToLower())
                {
                    int userId = 0;
                    int.TryParse(context.Request.QueryString["uID"], out userId);
                    if (userId == 0)
                    {
                        throw new Exception("Invalid or no user id found");
                    }

                    int jobTypeId = 0;
                    int.TryParse(context.Request.QueryString["jTID"] , out jobTypeId);
                    if (jobTypeId == 0)
                    {
                        throw new Exception("Invalid or no job type form");
                    }

                    if (string.IsNullOrEmpty(context.Request.QueryString["coordinates"]))
                    {
                        throw new Exception("Kindly provide current location");
                    }

                    if (string.IsNullOrEmpty(context.Request.QueryString["details"]))
                    {
                        throw new Exception("Kindly provide Job details");
                    }
                    
                    if(context.Request.Files.Count <= 0)
                    {
                        throw new Exception("Kindly provide location Image");
                    }

                    var location = context.Request.QueryString["coordinates"];
                    var jobDetails = context.Request.QueryString["details"];
                    var file = context.Request.Files[0];
                    JobsInfo oJob = new JobsInfo();
                    oJob.UserId = userId;
                    oJob.Location = location;
                    oJob.JobTypeId = jobTypeId;
                    oJob.StatusId = 1;
                    oJob.JobDetails = jobDetails;
                    oJob.FileURL = file.FileName;
                    new UserManagement().PostJob(oJob);
                    
                    file.SaveAs(context.Server.MapPath("~/JobFiles/" + oJob.Id + file.FileName));
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    context.Response.Write(jsonSerializer.Serialize(oJob));

                }
                #endregion

                #region AcceptJob
                if (methodName.ToLower() == "AcceptJob".ToLower())
                {
                    int userId = 0;
                    int.TryParse(context.Request.QueryString["uID"], out userId);
                    if (userId == 0)
                    {
                        throw new Exception("Invalid or no user id found");
                    }
                    int jobTypeId = 0;
                    int.TryParse(context.Request.QueryString["jID"], out jobTypeId);
                    if (jobTypeId == 0)
                    {
                        throw new Exception("Invalid or no job type form");
                    }
                    JobFactory oFactory = new JobFactory();

                    oFactory.AcceptJob(userId , jobTypeId);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    context.Response.Write(jsonSerializer.Serialize(new Dictionary<string, dynamic>() { { "IsError", false }, { "ErrorMessage", "" } }));
                }
                #endregion

                #region MarkAsDoneJob
                if (methodName.ToLower() == "MarkAsDoneJob".ToLower())
                {

                    int userId = 0;
                    int.TryParse(context.Request.QueryString["uID"], out userId);
                    if (userId == 0)
                    {
                        throw new Exception("Invalid or no user id found");
                    }
                    int jobTypeId = 0;
                    int.TryParse(context.Request.QueryString["jID"], out jobTypeId);
                    if (jobTypeId == 0)
                    {
                        throw new Exception("Invalid or no job type form");
                    }
                    JobFactory oFactory = new JobFactory();

                    oFactory.MarkAsDoneJob(userId, jobTypeId);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    context.Response.Write(jsonSerializer.Serialize(new Dictionary<string, dynamic>() { { "IsError", false }, { "ErrorMessage", "" } }));
                }
                #endregion

                #region GetMyJobs
                if (methodName.ToLower() == "GetMyJobs".ToLower())
                {

                }
                #endregion

                #region GetMyLibrary
                if (methodName.ToLower() == "GetMyLibrary".ToLower())
                {

                }
                #endregion

                #region GetAllBadges
                if (methodName.ToLower() == "GetAllBadges".ToLower())
                {

                }
                #endregion

                #region GetBadgesByUserId
                if (methodName.ToLower() == "GetBadgesByUserId".ToLower())
                {

                }
                #endregion

            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;

                context.Response.Write(jsonSerializer.Serialize(new Dictionary<string, dynamic>() { { "IsError", true}, { "ErrorMessage", ex.Message } }));
                
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}