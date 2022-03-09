using NAIKI.Modals;
using NAIKI.Services;
using System;
using System.Collections.Generic;
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
                    
                }
                #endregion

                #region GetLocationsByUserId
                if (methodName.ToLower() == "GetLocationsByUserId".ToLower())
                {

                }
                #endregion

                #region GetJobsByUserId
                if (methodName.ToLower() == "GetJobsByUserId".ToLower())
                {

                }
                #endregion

                #region GetJobsByLocation
                if (methodName.ToLower() == "GetJobsByLocation".ToLower())
                {

                }
                #endregion

                #region PostJob
                if (methodName.ToLower() == "PostJob".ToLower())
                {

                }
                #endregion

                #region AcceptJob
                if (methodName.ToLower() == "AcceptJob".ToLower())
                {

                }
                #endregion

                #region MarkAsDoneJob
                if (methodName.ToLower() == "MarkAsDoneJob".ToLower())
                {

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

                context.Response.Write(new Dictionary<string, dynamic>() { { "IsError", true}, { "ErrorMessage", ex.Message } });
                
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