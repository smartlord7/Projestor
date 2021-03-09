using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.WebApi.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50)]

        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

    }
}