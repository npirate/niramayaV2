using System;
using System.Collections;
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
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;

        CommonBusiness commonBusiness = new CommonBusiness();
        PatientBusiness patientBusiness = new PatientBusiness();

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }
        // GET: view of add Patient
        public ActionResult PatientIndexAdd(PatientViewModel patientViewModel)
        {
            ModelState.Clear();
            return View("~/Views/Patient/AddPatient.cshtml", patientViewModel);
        }

        // GET: view of search Patient

        public ActionResult PatientIndexSearch(PatientViewModel patientViewModel)
        {
            ModelState.Clear();
            return View("~/Views/Patient/SearchPatient.cshtml", patientViewModel);
        }

        [HttpPost]
        public object AddPatient(PatientViewModel patientViewModel)
        {
            if (!ModelState.IsValid)
            {
                // ... 
                //return RedirectToAction(nameof(Index));
                View("Login", patientViewModel);
            }

            string[] inputArray = new string[14] { patientViewModel.Fname, patientViewModel.Mname, patientViewModel.Lname, patientViewModel.Gender, patientViewModel.DOB.ToString(), patientViewModel.Mobile, patientViewModel.PatientEmail, patientViewModel.Address1, patientViewModel.Pincode, patientViewModel.City, patientViewModel.State, patientViewModel.Occupation, DateTime.Now.ToString(), DateTime.Now.ToString() };

            ArrayList inputArrayList = commonBusiness.addStringArraytoArraylist(inputArray);


            if (patientBusiness.AddPatientService(patientViewModel, inputArrayList))
            {
                ViewBag.SuccessRegister = "Patient data added successfully.";
                return View("~/Views/Patient/AddPatient.cshtml", patientViewModel);
            }
            else
            {
                ViewBag.ErrorRegister = "Incorrect data supplied while adding patient data.";
                return View("~/Views/Patient/AddPatient.cshtml", patientViewModel);
            }

        }

        public object SearchPatient(PatientViewModel patientViewModel)
        {
            if (!ModelState.IsValid)
            {
                // ... 
                //return RedirectToAction(nameof(Index));
                View("Login", patientViewModel);
            }

            string[] inputArray = new string[6] { patientViewModel.PtFname, patientViewModel.PtMname, patientViewModel.PtLname, patientViewModel.PtDOB.ToString(), patientViewModel.PtMobile, patientViewModel.PtPincode };

            ArrayList inputArrayList = commonBusiness.addStringArraytoArraylist(inputArray);

            PatientViewModel.Totalpatientcount = patientBusiness.GetPatientDataCountService(patientViewModel, inputArrayList);

            if (PatientViewModel.Totalpatientcount > 0)
            {
                PatientViewModel.pager.UpdatePager(PatientViewModel.Totalpatientcount, PatientViewModel.pager.PageIndex, 10);

                TempData["modeldata"] = providePtList(patientViewModel, inputArrayList, PatientViewModel.pager.PageIndex);//provide list to TempData to show in View

                ViewBag.SuccessRegister = "Patient data searched successfully.";
                return View("~/Views/Patient/SearchPatient.cshtml", patientViewModel);
                //return View(pvmlist);
            }
            else if (PatientViewModel.Totalpatientcount == 0)
            {
                ViewBag.ErrorRegister = "No search result found.";
                return View("~/Views/Patient/SearchPatient.cshtml", patientViewModel);
            }
            else
            {
                ViewBag.ErrorRegister = "Incorrect data supplied while searching patient data.";
                return View("~/Views/Patient/SearchPatient.cshtml", patientViewModel);
            }

        }

        private List<PatientViewModel> providePtList(PatientViewModel patientViewModel, ArrayList inputArrayList, int pageIndex = 1)
        {
            List<PatientViewModel> pvmlist = new List<PatientViewModel>();

            var ptData = patientBusiness.SearchPatientService(patientViewModel, inputArrayList, pageIndex);

            PatientViewModel.hasData = 1;                                 // default is 0 but to show data in grid in cshtml view making it 1
            foreach (DataRow dr in ptData.Tables[0].Rows)                // loop for adding add from dataset to list<modeldata>  
            {
                pvmlist.Add(new PatientViewModel
                {
                    // adding data from dataset row in to list<modeldata>  

                    Fname = dr["Fname"].ToString(),
                    Lname = dr["Lname"].ToString(),
                    Gender = dr["Gender"].ToString(),
                    DOB = DateTime.Parse(dr["DOB"].ToString()),
                    Mobile = dr["Mob"].ToString(),


                });
            }
            return pvmlist;
        }

        public object NextPage(PatientViewModel patientViewModel)
        {

            PatientViewModel.pager.UpdatePager(PatientViewModel.Totalpatientcount, PatientViewModel.pager.PageIndex, 10);
            if (PatientViewModel.pager.PageIndex <= PatientViewModel.pager.TotalPages)
                PatientViewModel.pager.PageIndex++;
            return SearchPatient(patientViewModel);
        }

        public object PreviousPage(PatientViewModel patientViewModel)
        {
            PatientViewModel.pager.UpdatePager(PatientViewModel.Totalpatientcount, PatientViewModel.pager.PageIndex, 10);
            if (PatientViewModel.pager.PageIndex <= PatientViewModel.pager.TotalPages)
                PatientViewModel.pager.PageIndex--;
            return SearchPatient(patientViewModel);
        }
    }
}