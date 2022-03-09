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
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.SerBeeConnectionString))
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
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.SerBeeConnectionString))
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
    }
}