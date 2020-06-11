using Niramaya.Database;
using Niramaya.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Business
{
    public class ProfileBusiness : ProfileViewModel
    {
        ProfileDAL profileDAL = new ProfileDAL();

        public bool saveProfileService(ProfileViewModel profileViewModel)
        {
            return (profileDAL.saveProfileData(profileViewModel) && UpdateAppointmentService(profileViewModel));
        }
        public bool PublishProfileService(ProfileViewModel profileViewModel, string username)
        {
            return (profileDAL.PublishProfileData(profileViewModel, username));
        }
        public bool UnPublishProfileService(ProfileViewModel profileViewModel, string username)
        {
            return (profileDAL.UnPublishProfileData(profileViewModel, username));
        }
        public DataSet GetProfileService(ProfileViewModel profileViewModel, string username)
        {
            return profileDAL.GetProfileData(profileViewModel, username);
        }

        public int GetPublishedProfileDataCountService(ProfileViewModel profileViewModel, string username)
        {
            return profileDAL.GetPublishedProfileDataCount(profileViewModel, username);
        }

        public bool UpdateAppointmentService(ProfileViewModel profileViewModel)
        {
            return profileDAL.UpdateAppointmentData(profileViewModel);
        }

    }
}
