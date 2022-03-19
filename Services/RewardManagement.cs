using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NAIKI.DB;
using NAIKI.Backbone;
using NAIKI.Modals;
using System.Configuration;
using NAIKI.Utilis;

namespace NAIKI.Services
{
    public class RewardManagement
    {
        public void AddReward(int userId, int badgeId)
        {
            try
            {
                using (DataClasses1DataContext eDataBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    var record = eDataBase.UserRewardBadges.Where(eRecord => eRecord.UserId == userId && eRecord.RewardBadgeId == badgeId
                        && eRecord.IsActive == true && eRecord.IsDeleted == false).FirstOrDefault();
                    if (record == null)
                    {
                        UserRewardBadge eReward = new UserRewardBadge();
                        eReward.UserId = userId;
                        eReward.RewardBadgeId = badgeId;
                        eReward.EarnedOn = DateTime.Now;
                        eReward.IsActive = true;
                        eReward.IsDeleted = false;
                        eDataBase.UserRewardBadges.InsertOnSubmit(eReward);
                        eDataBase.SubmitChanges();
                    }
                }
               
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public List<BadgeInfo> GetAllBadges()
        {
            try
            {
                List<BadgeInfo> oBadges = new List<BadgeInfo>();
                using (DataClasses1DataContext eDatBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    var eBadges = eDatBase.RewardBadges.Where(eBData => eBData.IsActive == true).ToList();
                    foreach (var eBadge in eBadges)
                    {
                        oBadges.Add(new BadgeInfo()
                        {
                            Id = eBadge.Id,
                            BadgeName = eBadge.BadgeName,
                            IconURL = ConfigurationManager.AppSettings["BaseURL"] + "Images/BadgeIcons/" + eBadge.IconURL,
                            CustomHTML = eBadge.CustomHTML != null & eBadge.CustomHTML != "" ? eBadge.CustomHTML : "N/A",
                            MileStoneCount = eBadge.MileStoneCount
                        });
                    }
                }
                return oBadges;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public List<BadgeInfo> GetBadgesByUserId(int userId)
        {
            try
            {
                List<BadgeInfo> oBadges = new List<BadgeInfo>();
                using (DataClasses1DataContext eDatBase = new DataClasses1DataContext(ConnectionString.NAIKIConnectionString))
                {
                    var eRewards = eDatBase.UserRewardBadges.Where(eBData => eBData.UserId == userId & eBData.IsActive == true & eBData.IsDeleted == false).ToList();
                    foreach (var eReward in eRewards)
                    {
                        var eBadge = eReward.RewardBadge;
                        oBadges.Add(new BadgeInfo()
                        {
                            Id = eReward.Id,
                            BadgeName = eBadge.BadgeName,
                            IconURL = ConfigurationManager.AppSettings["BaseURL"] + "Images/BadgeIcons/" + eBadge.IconURL,
                            CustomHTML = eBadge.CustomHTML != null & eBadge.CustomHTML != "" ? eBadge.CustomHTML : "N/A",
                            MileStoneCount = eBadge.MileStoneCount,
                            EarnedOn = eReward.EarnedOn,
                            EarnedOnString = new CommonMethods().ToShortDateTime(eReward.EarnedOn)

                        });
                    }
                }
                return oBadges;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}