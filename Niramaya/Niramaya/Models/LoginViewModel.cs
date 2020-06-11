using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Niramaya.Business;

namespace Niramaya.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(15, ErrorMessage = "Username cannot be longer than 15 characters.")]

        [RegularExpression(@"^[^<>.,?;:'()!~%\-@#/*""\s]+$",
         ErrorMessage = "Username should not contain space or special characters except '_'.")]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$",
         ErrorMessage = "Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter and one number:.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required."), DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string confPassword { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public static string varEmail { get; set; }//reference variable for storing email static if Email parameter becomes null on register click
        public static DateTime OtpCrtDate { get; set; }

        [Required]
        public string OTP { get; set; }
        public static string varOTP { get; set; }//reference variable for check equality of otp on confirm button

        public static int timeout = 2;
        public static int ResendCounter = 1;

        public static bool disableRegisterFlow { get; set; }

    }
}
