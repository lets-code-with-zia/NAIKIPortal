using NAIKI.Backbone;
using NAIKI.DB;
using NAIKI.Modals;
using NAIKI.Utilis;
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
                    UserSetting eSetting = new UserSetting();
                    eSetting.CurrentLoction = oUser.RegisteredLocation;
                    eSetting.RadiusInMile = oUser.MileRadius;
                    eSetting.LastUpdated = DateTime.Now;
                    eSetting.UserId = eUser.Id;
                    eSetting.IsActive = true;
                    eSetting.IsDeleted = false;
                    eDataBase.UserSettings.InsertOnSubmit(eSetting);
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
                    var eSetting = eDataBase.UserSettings.Where(eSData => eSData.UserId == eUser.Id & eSData.IsActive == true & eSData.IsDeleted == false).FirstOrDefault();
                    if(eSetting != null)
                    {
                        oUser.CurrentLocation = eSetting.CurrentLoction;
                        oUser.MileRadius = eSetting.RadiusInMile;
                    }

                }
            }
            catch (Exception ex)
            {

                oUser.IsError = true;
                oUser.ErrorMessage = ex.Message;
            }

            return oUser;
        }

        public void UpdateMyLocation(int uID , string location, double mileRadius)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                   
                    var eSetting = eDataBase.UserSettings.Where(eSData => eSData.UserId == uID && eSData.IsDeleted == false && eSData.IsActive == true).FirstOrDefault();
                    if (eSetting == null)
                    {
                        throw new Exception("Invalid or no data found");
                    }
                    eSetting.CurrentLoction = location;
                    eSetting.RadiusInMile = mileRadius;
                    eSetting.LastUpdated = DateTime.Now;
                    eDataBase.SubmitChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<LocationInfo> GetLocationsByUserId(int uID)
        {
            try
            {
                List<LocationInfo> oLocations = new List<LocationInfo>();
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    var eSettings = eDataBase.UserSettings.Where(eSData => eSData.UserId == uID && eSData.IsDeleted == false && eSData.IsActive == true).ToList();
                    if (eSettings.Count <= 0)
                    {
                        throw new Exception("Invalid or no data found");
                    }
                    foreach (var eSetting in eSettings)
                    {
                        LocationInfo oLocation = new LocationInfo();
                        oLocation.Id = eSetting.Id;
                        oLocation.Cordinates = eSetting.CurrentLoction;
                        oLocation.MileRadius = eSetting.RadiusInMile;
                        oLocation.LastUpdated = eSetting.LastUpdated;
                        oLocation.LastUpdatedString = new CommonMethods().ToShortDateTime(eSetting.LastUpdated);
                        oLocations.Add(oLocation);
                    }
                    return oLocations;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

       
    }
}