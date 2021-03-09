using System;
using System.ComponentModel.DataAnnotations;
using static AspNetIdentity.WebApi.Infrastructure.User;

namespace AspNetIdentity.WebApi.Models
{
    public class EditUserBindingModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phonenumber is required")]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Job is required")]
        public string Job { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public string Role { get; set; }

        public bool EmailConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

    }
}