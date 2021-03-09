using AspNetIdentity.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using static AspNetIdentity.WebApi.Infrastructure.User;

namespace AspNetIdentity.WebApi.Business.DTO
{
    public class UserDTO
    {
        public UserDTO() 
        { 
        }
       
        public UserDTO(User user, string role, string url = null)
        {
            Id = user.Id;
            Url = url;
            UserName = user.UserName;
            Role = role;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Job = user.Job;
            BirthDate = user.BirthDate;
            Gender = user.Gend;
            Bio = user.Bio;
            EmailConfirmed = user.EmailConfirmed;
            AccessFailedCount = user.AccessFailedCount;
        }

        public string Id { get; set; }

        public string Url { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public bool EmailConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Job { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string Bio { get; set; }

    }
}
    