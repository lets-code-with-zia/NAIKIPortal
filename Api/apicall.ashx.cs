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

            var methodName = Convert.ToString(context.Request.QueryString["m"]);

            try
            {
                #region Login
                if (methodName.ToLower() == "Login".ToLower())
                {

                }
                #endregion

                #region SignUp
                if (methodName.ToLower() == "SignUp".ToLower())
                {

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