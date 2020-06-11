using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Niramaya.Models;
using System.Data.SqlClient;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data;
using Konscious.Security.Cryptography;
using System.Text;
using Niramaya.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;

namespace Niramaya.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        HomeBusiness homeBusiness = new HomeBusiness();
        CommonBusiness commonBusiness = new CommonBusiness();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Profile(BookViewModel bookViewModel)
        {
            var profileData = homeBusiness.GetBookProfileService(bookViewModel);
            DataRow dr = profileData.Tables[0].Rows[0];

            if (profileData.Tables[0].Rows.Count > 0)
            {
                BookViewModel.doc_userid = (int)dr["userid"];
                bookViewModel.doc_username = dr["username"].ToString();
                BookViewModel.username = bookViewModel.doc_username; //giving value to static variable because doc_username becomes null as it does not have any field in cshtml
                bookViewModel.doc_Fname = dr["doc_Fname"].ToString();
                bookViewModel.doc_Mname = dr["doc_Mname"].ToString();
                bookViewModel.doc_Lname = dr["doc_Lname"].ToString();
                bookViewModel.doc_Gender = dr["doc_Gender"].ToString();
                bookViewModel.doc_DOB = DateTime.Parse(dr["doc_DOB"].ToString());
                bookViewModel.doc_Degree = dr["doc_GradDegree"].ToString();
                bookViewModel.doc_PostDegree = dr["doc_PostGrad"].ToString();
                bookViewModel.doc_Phone = dr["doc_Phone"].ToString();
                bookViewModel.doc_Email = dr["doc_Email"].ToString();
                bookViewModel.doc_Clinicname = dr["doc_Clinicname"].ToString();
                bookViewModel.doc_Address1 = dr["clinic_address1"].ToString();
                bookViewModel.doc_Address2 = dr["clinic_address2"].ToString();
                bookViewModel.doc_City = dr["clinic_city"].ToString();
                bookViewModel.doc_State = dr["clinic_state"].ToString();
                bookViewModel.doc_Pincode = dr["clinic_pincode"].ToString();
                bookViewModel.doc_ClinicsPhone = dr["clinic_phone"].ToString();
                bookViewModel.doc_Services = dr["doc_Services"].ToString();

                bookViewModel.from1 = dr["F1"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["F1"].ToString());
                bookViewModel.to1 = dr["T1"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["T1"].ToString());
                bookViewModel.from2 = dr["F2"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["F2"].ToString());
                bookViewModel.to2 = dr["T2"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["T2"].ToString());
                bookViewModel.from3 = dr["F3"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["F3"].ToString());
                bookViewModel.to3 = dr["T3"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["T3"].ToString());

                bookViewModel.allowedDays = commonBusiness.AssignValidTimeslot(bookViewModel.from1, bookViewModel.to1, bookViewModel.from2, bookViewModel.to2, bookViewModel.from3, bookViewModel.to3);

                ModelState.Clear();

            }
            else
            {
                ViewBag.ErrorRegister = "No data found for Doctor's profile!";
            }
            return View("Book", bookViewModel);
        }
        public IActionResult PreRegister()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public object LoginUser(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                // ... 
                //return RedirectToAction(nameof(Index));
                View("Login", loginViewModel);
            }
            string username = homeBusiness.doUserLogin(loginViewModel);//calling main login check method.
            if (!string.IsNullOrEmpty(username))
            {
                HttpContext.Session.SetString(Startup.Username_sess, username);
                return RedirectToAction("ProfileIndexView", "Profile");
            }
            else
            {
                ViewBag.ErrorRegister = "Username or password is incorrect.";
                return View("Login", loginViewModel);
            }

        }

        [HttpPost]
        public object ValidateRegister(LoginViewModel loginViewModel)
        {
            if (!string.IsNullOrEmpty(loginViewModel.Email))
            {
                float emailCount = homeBusiness.isEmailRegisteredService(loginViewModel);

                if (emailCount > 0 && !LoginViewModel.disableRegisterFlow)
                {
                    ViewBag.ErrorRegister = "This Email ID is already registered with us. ";

                }
                else if (emailCount == 0 && LoginViewModel.disableRegisterFlow)
                {
                    ViewBag.ErrorRegister = "This Email ID is not registered with us. ";//Forget password flow
                }
                else if (emailCount == -1)
                {
                    ViewBag.ErrorRegister = "Encountered issues with Email. Please contact system administrator";
                }
                else
                {
                    SendOTP(loginViewModel);
                }

                return View("PreRegister", loginViewModel);
            }

            if (!ModelState.IsValid)
            {
                //return RedirectToAction(nameof(Index));
                View("PreRegister", loginViewModel);
            }
            return null;
        }

        public object SendOTP(LoginViewModel loginViewModel)
        {
            LoginViewModel.ResendCounter++;
            LoginViewModel.OtpCrtDate = System.DateTime.Now;
            ViewBag.Email = loginViewModel.Email;
            LoginViewModel.varEmail = loginViewModel.Email;// because loginviewmodel.email becomes null in registerUser event, so storing it in static thing.
            LoginViewModel.varOTP = homeBusiness.GenerateOTP().ToString();
            if (LoginViewModel.ResendCounter > 3)
            {
                ViewBag.ErrorRegister = "OTP resend Limit reached!";
            }
            else
            {
                if (commonBusiness.SendEmail("UserRegistration", ViewBag.Email, true, LoginViewModel.varOTP))
                {
                    ViewBag.ErrorRegister = "OTP has been sent to Email ID.";
                }
                else
                    ViewBag.ErrorRegister = "There is some problem sending Email. Kindly check if Email provided is correct.";
            }
            ModelState.Clear();
            return View("PreRegister", loginViewModel);
        }

        [HttpPost]
        public object RegisterUser(LoginViewModel loginViewModel)
        {
            if (!string.IsNullOrEmpty(loginViewModel.Password) && !string.IsNullOrEmpty(loginViewModel.confPassword))
            {
                if (loginViewModel.Password == loginViewModel.confPassword)
                {
                    float usernameCount = homeBusiness.isUsernameUniqueService(loginViewModel);

                    if (usernameCount > 0)
                    {
                        ViewBag.ErrorRegister = "This Username is already registered with us. Please choose different Username!";
                    }
                    else
                    {
                        if (homeBusiness.doUserRegister(loginViewModel))
                        {
                            ViewBag.SuccessRegister = "User Registration complete.";
                            ModelState.Clear();
                            return View("Login");
                        }
                        else
                        {
                            ViewBag.ErrorRegister = "Problem encountered while registering user";
                        }
                    }
                }
                else
                    ViewBag.ErrorRegister = "Password does not match with confirm Password! Please contact system administrator";
            }
            else
            {
                ViewBag.ErrorRegister = "Encountered issues Password. Please contact system administrator";
            }

            ModelState.Clear();
            return View("Register", loginViewModel); ;
        }

        [HttpPost]
        public object ValidateOTP(LoginViewModel loginViewModel)
        {
            if (LoginViewModel.varOTP.Equals(loginViewModel.OTP.ToString()))
            {
                ModelState.Clear();
                System.TimeSpan timeSub = System.DateTime.Now - LoginViewModel.OtpCrtDate;
                if (timeSub.TotalMinutes > LoginViewModel.timeout)
                {
                    ViewBag.ErrorRegister = "OTP has expired!";
                    return View("PreRegister", loginViewModel);
                }
                else
                {
                    return View("Register", loginViewModel);
                }
            }
            else
            {
                LoginViewModel.ResendCounter++;
                ViewBag.ErrorRegister = "OTP is incorrect.";
                if (LoginViewModel.ResendCounter > 5)
                    ViewBag.ErrorRegister = "Too many failure attempts!";

                ViewBag.Email = "xx";//just givng value to render Preregister view with partial otp view
                return View("PreRegister", loginViewModel);
            }
        }

        public object hideRegister(LoginViewModel loginViewModel)
        {
            LoginViewModel.disableRegisterFlow = true;
            ModelState.Clear();
            return View("PreRegister", loginViewModel);

        }

        [HttpPost]
        public object UpdateUserPassword(LoginViewModel loginViewModel)
        {
            if (!string.IsNullOrEmpty(loginViewModel.Password) && !string.IsNullOrEmpty(loginViewModel.confPassword))
            {
                if (loginViewModel.Password == loginViewModel.confPassword)
                {
                    loginViewModel.Email = LoginViewModel.varEmail;
                    if (!string.IsNullOrEmpty(loginViewModel.Email))
                    {
                        if (homeBusiness.doUpdatePassword(loginViewModel))
                            return View("Index");
                        else
                        {
                            ViewBag.ErrorRegister = "Problem encountered while updating user password";
                            ModelState.Clear();
                            return View("Register", loginViewModel);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorRegister = "You might had refreshed page. Please repeat process of 'Forget Password'";
                        ModelState.Clear();
                        return View("Register", loginViewModel);
                    }

                }
                else
                    ViewBag.ErrorRegister = "Password does not match with confirm Password! Please contact system administrator";
            }
            else
            {
                ViewBag.ErrorRegister = "Encountered issue updating Password. Please contact system administrator";
            }

            return null;
        }

        [AllowAnonymous]
        public object SearchDoc(ProfileViewModel profileViewModel)
        {
            if (!ModelState.IsValid)
            {
                // ... 
                //return RedirectToAction(nameof(Index));
                View("Index");
            }

            ProfileViewModel.Totaldoccount = homeBusiness.GetSearchDocCountService(profileViewModel);

            if (ProfileViewModel.Totaldoccount > 0)
            {
                ProfileViewModel.pager.UpdatePager(ProfileViewModel.Totaldoccount, ProfileViewModel.pager.PageIndex, 10);

                List<ProfileViewModel> doclist = new List<ProfileViewModel>();

                var docData = homeBusiness.SearchDocService(profileViewModel, 1);

                foreach (DataRow dr in docData.Tables[0].Rows)                // loop for adding add from dataset to list<modeldata>  
                {
                    doclist.Add(new ProfileViewModel
                    {
                        // adding data from dataset row in to list<modeldata>  
                        Queryuname = dr["username"].ToString(),
                        doc_Fname = dr["doc_Fname"].ToString(),
                        doc_Lname = dr["doc_Lname"].ToString(),
                        doc_Degree = dr["doc_GradDegree"].ToString(),
                        doc_PostDegree = dr["doc_PostGrad"].ToString(),
                        doc_Phone = dr["doc_Phone"].ToString(),
                        doc_Clinicname = dr["doc_Clinicname"].ToString(),

                    }); ; ;
                }

                TempData["docdata"] = doclist;
            }

            return View("Index");

        }

        public object NextPage(ProfileViewModel profileViewModel)
        {

            ProfileViewModel.pager.UpdatePager(ProfileViewModel.Totaldoccount, ProfileViewModel.pager.PageIndex, 10);
            if (ProfileViewModel.pager.PageIndex <= ProfileViewModel.pager.TotalPages)
                ProfileViewModel.pager.PageIndex++;
            return SearchDoc(profileViewModel);
        }

        public object PreviousPage(ProfileViewModel profileViewModel)
        {
            ProfileViewModel.pager.UpdatePager(ProfileViewModel.Totaldoccount, ProfileViewModel.pager.PageIndex, 10);
            if (ProfileViewModel.pager.PageIndex <= PatientViewModel.pager.TotalPages)
                ProfileViewModel.pager.PageIndex--;
            return SearchDoc(profileViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public object BookAppointment(BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
            {
                View("Book", bookViewModel);
            }

            if (homeBusiness.insertBookingService(bookViewModel))
            {
                if (commonBusiness.SendBookingEmail("BookingRegistration", bookViewModel.doc_Email, true, bookViewModel))
                {
                    ProfileViewModel.Totaldoccount = 0; // so that middle part does not render in index.html
                    ViewBag.SuccessRegister = "Booking process is complete.";
                    return View("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "Problem sending email to doctor. Please contact administrator.";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.ErrorRegister = "Problem occurred booking an appointment. Please contact doctor on Phone.";
                return View("Index");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
