using AspNetIdentity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using static AspNetIdentity.WebApi.Infrastructure.User;

namespace AspNetIdentity.WebApi.Models
{
     public class ModelFactory
    {

        private UrlHelper _UrlHelper;
        private AppUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, AppUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserReturnModel Create(User user)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = user.Id }),
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Job = user.Job,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gend,
                BirthDate = user.BirthDate,
                Bio = user.Bio,
                Role = _AppUserManager.GetRolesAsync(user.Id).Result.First(),
                EmailConfirmed = user.EmailConfirmed,
                AccessFailedCount = user.AccessFailedCount
            };
        }

        public RoleReturnModel Create(IdentityRole appRole)
        {

            return new RoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }

    }

    public class UserReturnModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Job { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Bio { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

    }

    public class RoleReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}