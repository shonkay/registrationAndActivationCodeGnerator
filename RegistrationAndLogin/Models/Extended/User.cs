using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RegistrationAndLogin.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }
    public class UserMetaData
    {
        [Display(Name =" First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " First name required")]
        public string FirstName { get; set; }

        [Display(Name =" Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " Last name required")]
        public string LastName { get; set; }

        [Display(Name =" EmailID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = " Email ID required")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name =" Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage =" Minimum of 8 characters required")]
        [Required(AllowEmptyStrings = false, ErrorMessage =" Password is required")]
        public string Password { get; set; }

        [Display(Name =" Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage =" Confirm Password and Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}