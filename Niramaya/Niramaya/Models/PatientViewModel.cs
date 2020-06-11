using Niramaya.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Models
{
    public class PatientViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string Fname { get; set; }
        [Required(ErrorMessage = "Middle Name is required.")]
        public string Mname { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string Lname { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Date of Birth is required.")]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string PatientEmail { get; set; }

        public string Address1 { get; set; }

        [Required(ErrorMessage = "Pin code is required.")]
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Occupation { get; set; }

        [Display(Name = "First Name")]
        public string PtFname { get; set; }
        public string PtMname { get; set; }

        [Display(Name = "Last Name")]
        public string PtLname { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? PtDOB { get; set; }

        [Display(Name = "Mobile")]
        public string PtMobile { get; set; }

        [Display(Name = "Pin Code")]
        public string PtPincode { get; set; }

        public static int hasData = 0;

        //Pagination

        public static int Totalpatientcount = 0;

        public static PaginatedList pager = new PaginatedList(Totalpatientcount,1,10);// Initially pass pageindex default 1 with pagesize 10
    }
}
