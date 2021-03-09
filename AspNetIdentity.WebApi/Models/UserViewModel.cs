using AspNetIdentity.WebApi.Infrastructure;
using System;
using static AspNetIdentity.WebApi.Infrastructure.User;

namespace AspNetIdentity.WebApi.Models
{
    public class UserViewModel
    {

        public UserViewModel(User user) {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Job = user.Job;
            BirthDate = user.BirthDate;
            Gend = user.Gend;
            Bio = user.Bio;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Job { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gend { get; set; }

        public string Bio { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

    }
}