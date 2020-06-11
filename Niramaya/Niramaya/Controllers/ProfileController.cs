using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Niramaya.Business;
using Niramaya.Models;

namespace Niramaya.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;

        ProfileBusiness profileBusiness = new ProfileBusiness();
        CommonBusiness commonBusiness = new CommonBusiness();

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }
        // GET: view of search Patient

        public ActionResult ProfileIndexView(ProfileViewModel profileViewModel)
        {
            ModelState.Clear();
            showprofile(profileViewModel);
            return View("~/Views/Profile/ProfileView.cshtml", profileViewModel);
        }

        private void showprofile(ProfileViewModel profileViewModel)
        {
            var usernameSession = HttpContext.Session.GetString(Startup.Username_sess);

            if (!string.IsNullOrEmpty(usernameSession))
            {
                var userData = commonBusiness.GetUserDataService(usernameSession);
                DataRow dr = userData.Tables[0].Rows[0];
                profileViewModel.doc_Email = dr["email"].ToString();
                ProfileViewModel.doc_username = dr["username"].ToString();
                ProfileViewModel.doc_userid = (int)dr["userid"];

                ViewBag.username = usernameSession;// used to show in global profile url

                ViewBag.ispublished = profileBusiness.GetPublishedProfileDataCountService(profileViewModel, usernameSession) > 0;// if true show publish button in cshtml

                if (ViewBag.ispublished)
                {
                    ViewBag.profileurl = HttpContext.Request.Host.Value;
                }

                var profileData = profileBusiness.GetProfileService(profileViewModel, usernameSession);
                dr = profileData.Tables[0].Rows[0];

                if (profileData.Tables[0].Rows.Count > 0)
                {
                    profileViewModel.doc_Fname = dr["doc_Fname"].ToString();
                    profileViewModel.doc_Mname = dr["doc_Mname"].ToString();
                    profileViewModel.doc_Lname = dr["doc_Lname"].ToString();
                    profileViewModel.doc_Gender = dr["doc_Gender"].ToString();
                    profileViewModel.doc_DOB = DateTime.Parse(dr["doc_DOB"].ToString());
                    profileViewModel.doc_Degree = dr["doc_GradDegree"].ToString();
                    profileViewModel.doc_PostDegree = dr["doc_PostGrad"].ToString();
                    profileViewModel.doc_Phone = dr["doc_Phone"].ToString();
                    profileViewModel.doc_Email = dr["doc_Email"].ToString();
                    profileViewModel.doc_Clinicname = dr["doc_Clinicname"].ToString();
                    profileViewModel.doc_Address1 = dr["clinic_address1"].ToString();
                    profileViewModel.doc_Address2 = dr["clinic_address2"].ToString();
                    profileViewModel.doc_City = dr["clinic_city"].ToString();
                    profileViewModel.doc_State = dr["clinic_state"].ToString();
                    profileViewModel.doc_Pincode = dr["clinic_pincode"].ToString();
                    profileViewModel.doc_ClinicsPhone = dr["clinic_phone"].ToString();
                    profileViewModel.doc_Services = dr["doc_Services"].ToString();

                    profileViewModel.DayChecked = dr["daychecked"].ToString().Split(",").Select(s => s.ToString()).ToList();
                    profileViewModel.from1 = dr["F1"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["F1"].ToString());
                    profileViewModel.to1 = dr["T1"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["T1"].ToString());
                    profileViewModel.from2 = dr["F2"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["F2"].ToString());
                    profileViewModel.to2 = dr["T2"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["T2"].ToString());
                    profileViewModel.from3 = dr["F3"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["F3"].ToString());
                    profileViewModel.to3 = dr["T3"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["T3"].ToString());

                }

            }
            else
            {
                ViewBag.ErrorRegister = "Session has expired. Please login again!";
            }
        }

        [HttpPost]
        public object saveprofile(ProfileViewModel profileViewModel)
        {
            if (!ModelState.IsValid)
            {
                //return RedirectToAction(nameof(Index));
                return View("~/Views/Profile/ProfileView.cshtml", profileViewModel);
            }
            //calling main login check method.
            if (profileBusiness.saveProfileService(profileViewModel))
            {
                ModelState.Clear();
                //showprofile(profileViewModel);//fetch profile data after save
                ViewBag.SuccessRegister = "Profile has been saved successfully.";
            }
            else
            {
                ViewBag.ErrorRegister = "Some problem occurred while saving profile data.";
            }

            return View("~/Views/Profile/ProfileView.cshtml", profileViewModel);

        }
        public object doPublish(ProfileViewModel profileViewModel)
        {
            var usernameSession = HttpContext.Session.GetString(Startup.Username_sess);
            if (!string.IsNullOrEmpty(usernameSession))
            {
                if (profileBusiness.PublishProfileService(profileViewModel, usernameSession))
                {
                    ModelState.Clear();
                    showprofile(profileViewModel);//fetch profile data after save
                    ViewBag.SuccessRegister = "Profile has been published successfully.";
                }
                else
                {
                    ViewBag.ErrorRegister = "Some problem occurred while publishing profile!";
                }
            }
            else
            {
                ViewBag.ErrorRegister = "Your session has expired!";
            }
            return View("~/Views/Profile/ProfileView.cshtml", profileViewModel);
        }

        public object doUnpublish(ProfileViewModel profileViewModel)
        {           
            var usernameSession = HttpContext.Session.GetString(Startup.Username_sess);
            if (!string.IsNullOrEmpty(usernameSession))
            {
                if (profileBusiness.UnPublishProfileService(profileViewModel, usernameSession))
                {
                    ModelState.Clear();
                    showprofile(profileViewModel);//fetch profile data after save
                    ViewBag.SuccessRegister = "Profile has been unpublished successfully.";
                }
                else
                {
                    ViewBag.ErrorRegister = "Some problem occurred while unpublishing profile!";
                }
            }
            else
            {
                ViewBag.ErrorRegister = "Your session has expired!";
            }
            return View("~/Views/Profile/ProfileView.cshtml", profileViewModel);
        }
    }
}