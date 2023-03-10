using NAIKI.Modals;
using NAIKI.Services;
using NAIKI.Utilis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
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
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
            try
            {
                #region Login
                if (methodName.ToLower() == "Login".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    UserInfo oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);
                    if (string.IsNullOrEmpty(oUser.LoginId))
                    {
                        throw new Exception("Kindly provide login ID");
                    }
                    if(!oUser.IsExternal & string.IsNullOrEmpty(oUser.Password))
                    {
                        throw new Exception("Kindly provide password");
                    }
                    oUser = new UserManagement().Login(oUser.LoginId, oUser.Password, oUser.IsExternal);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", oUser);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
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
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", oUser);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region UpdateMyLocation
                if (methodName.ToLower() == "UpdateMyLocation".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    UserInfo oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);

                    if (oUser.Id == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    if (oUser.MileRadius == 0)
                    {
                        throw new Exception("Kindly provide radius in miles");
                    }
                    if (string.IsNullOrEmpty(oUser.CurrentLocation))
                    {
                        throw new Exception("Kindly provide current location");
                    }
                    new UserManagement().UpdateMyLocation(oUser.Id , oUser.CurrentLocation, oUser.MileRadius);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;

                }
                #endregion

                #region GetLocationsByUserId
                if (methodName.ToLower() == "GetLocationsByUserId".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    UserInfo oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);
                    if (oUser.Id == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    var oLocations = new UserManagement().GetLocationsByUserId(oUser.Id);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", oLocations);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region GetJobsByUserLocation
                if (methodName.ToLower() == "GetJobsByUserLocation".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    UserInfo oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);
                    if (oUser.Id == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    var oJobs = new JobFactory().GetJobsByUserLocation(oUser.Id);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", oJobs);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region GetJobsByLocation
                if (methodName.ToLower() == "GetJobsByLocation".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    UserInfo oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);
                    if (string.IsNullOrEmpty(oUser.CurrentLocation))
                    {
                        throw new Exception("Kindly provide current location");
                    }
                    var oJobs = new JobFactory().GetJobsByLocation(oUser.CurrentLocation);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", oJobs);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region PostJob
                if (methodName.ToLower() == "PostJob".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    JobsInfo oJob = jsonSerializer.Deserialize<JobsInfo>(jsonString);
                    if (oJob.UserId == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    if (oJob.JobTypeId == 0)
                    {
                        throw new Exception("Invalid or no job type found");
                    }

                    if (string.IsNullOrEmpty(oJob.Location))
                    {
                        throw new Exception("Kindly provide current location");
                    }

                    if (string.IsNullOrEmpty(oJob.JobDetails))
                    {
                        throw new Exception("Kindly provide Job details");
                    }
                    
                    if(string.IsNullOrEmpty(oJob.FileURL))
                    {
                        throw new Exception("Kindly provide location image");
                    }

                    oJob.StatusId = 1;
                    new JobFactory().PostJob(oJob);
                    byte[] bytes = Convert.FromBase64String(oJob.FileURL);

                    Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }
                    //string fileName = oJob.Id + new CommonMethods().GetRandomString(8) + ".jpeg";
                    image.Save(context.Server.MapPath("~/JobFiles/" + oJob.FileURL));
                    oJob.FileURL = ConfigurationManager.AppSettings["BaseURL"] + "JobFiles/" + oJob.FileURL;
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", oJob);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;

                }
                #endregion

                #region AcceptJob
                if (methodName.ToLower() == "AcceptJob".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    JobsInfo oJob = jsonSerializer.Deserialize<JobsInfo>(jsonString);
                    if (oJob.UserId == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    if (oJob.Id == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    new JobFactory().AcceptJob(oJob.UserId , oJob.Id);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region MarkAsDoneJob
                if (methodName.ToLower() == "MarkAsDoneJob".ToLower())
                {


                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    JobsInfo oJob = jsonSerializer.Deserialize<JobsInfo>(jsonString);
                    if (oJob.UserId == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    if (oJob.Id == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    new JobFactory().MarkAsDoneJob(oJob.UserId, oJob.Id);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region GetMyJobs
                if (methodName.ToLower() == "GetMyJobs".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    UserInfo oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);
                    if (oUser.Id == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    var oJobs = new JobFactory().GetJobsByUserId(oUser.Id);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", oJobs);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region GetMyLibrary
                if (methodName.ToLower() == "GetMyLibrary".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    UserInfo oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);
                    if (oUser.Id == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    var oJobs = new JobFactory().GetMyLibrary(oUser.Id);
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", oJobs);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region GetAllBadges
                if (methodName.ToLower() == "GetAllBadges".ToLower())
                {
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", new RewardManagement().GetAllBadges());
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

                #region GetBadgesByUserId
                if (methodName.ToLower() == "GetBadgesByUserId".ToLower())
                {
                    using (var inputStream = new System.IO.StreamReader(context.Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }
                    UserInfo oUser = jsonSerializer.Deserialize<UserInfo>(jsonString);
                    if (oUser.Id == 0)
                    {
                        throw new Exception("Invalid params");
                    }
                    dict.Add("IsError", false);
                    dict.Add("ErrorMessage", "");
                    dict.Add("oData", new RewardManagement().GetBadgesByUserId(oUser.Id));
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(jsonSerializer.Serialize(dict));
                    return;
                }
                #endregion

            }
            catch (Exception ex)
            {
                dict.Add("IsError", true);
                dict.Add("ErrorMessage", ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.Write(jsonSerializer.Serialize(dict));
                return;
                
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