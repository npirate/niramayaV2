using Microsoft.AspNetCore.Mvc.Rendering;
using Niramaya.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Models
{
    public class ProfileViewModel
    {
        public static string doc_username { get; set; }

        public static bool isProfileExist = false;
        public string Queryuname { get; set; }
        public static int doc_userid { get; set; }

        public static int Totaldoccount = 0;

        public static PaginatedList pager = new PaginatedList(Totaldoccount, 1, 10);// Initially pass pageindex default 1 with pagesize 10

        public string doc_SearchText { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string doc_Fname { get; set; }

        [Display(Name = "Middle Name")]
        public string doc_Mname { get; set; }

        [Display(Name = "Last Name")]
        public string doc_Lname { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string doc_Gender { get; set; }

        public List<SelectListItem> Genderlist { get; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "Male", Text = "Male" },
        new SelectListItem { Value = "Female", Text = "Female" },
        new SelectListItem { Value = "Other", Text = "Other"  },
    };

        [Required(ErrorMessage = "Date of Birth is required.")]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? doc_DOB { get; set; }

        [Display(Name = "Graduation Degree")]
        public string doc_Degree { get; set; }

        [Display(Name = "Postgraduation Degree")]
        public string doc_PostDegree { get; set; }

        [Required, Display(Name = "Phone")]
        [RegularExpression(@"^[0-9]{10}$",
         ErrorMessage = "Mobile number should be of 10 digit.")]
        public string doc_Phone { get; set; }

        [Display(Name = "Email")]
        public string doc_Email { get; set; }

        [Display(Name = "Clinic Name")]
        public string doc_Clinicname { get; set; }

        [Display(Name = "Adress")]
        [StringLength(100, ErrorMessage = "Address1 cannot be longer than 100 characters.")]
        public string doc_Address1 { get; set; }

        [StringLength(100, ErrorMessage = "Address2 cannot be longer than 100 characters.")]
        public string doc_Address2 { get; set; }

        [Display(Name = "Pincode")]
        [StringLength(10, ErrorMessage = "Pincode cannot be longer than 10 characters.")]
        public string doc_Pincode { get; set; }

        [Display(Name = "City")]
        [StringLength(25, ErrorMessage = "City cannot be longer than 25 characters.")]
        public string doc_City { get; set; }

        [Display(Name = "State")]
        [StringLength(25, ErrorMessage = "State cannot be longer than 25 characters.")]
        public string doc_State { get; set; }

        [Display(Name = "Clinics Phone")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")]
        public string doc_ClinicsPhone { get; set; }

        public string[] Days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public List<string> DayChecked { get; set; }

        public bool isChecked { get; set; }

        [Required, Display(Name = "Appointment Duration")]
        public int doc_Duration { get; set; }

        [Display(Name = "Services")]
        [MaxLength(200)]
        public string doc_Services { get; set; }

        public TimeSpan? from1 { get; set; }
        public TimeSpan? to1 { get; set; }
        public TimeSpan? from2 { get; set; }
        public TimeSpan? to2 { get; set; }
        public TimeSpan? from3 { get; set; }
        public TimeSpan? to3 { get; set; }
    }
}
