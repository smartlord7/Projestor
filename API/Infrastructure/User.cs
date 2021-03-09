using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure
{
    partial class User : IdentityUser
    {

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        public FileWrapper ProfilePicture { get; set; }

        [Required]
        public string Job { get; set; }
        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<Project> Tasks { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public Gender Gend { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string Bio { get; set; }

    }
}
