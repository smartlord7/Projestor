using AspNetIdentity.WebApi.Business.DTO;
using AspNetIdentity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace AspNetIdentity.WebApi.Business
{
    public class QueryReturner
    {

        private AppUserManager AppUserManager = null;
        private AppDbContext DbContext = null;
        private AppRoleManager AppRoleManager = null;
        private UrlHelper UrlHelper;

        public QueryReturner(AppUserManager manager, AppDbContext dbContext)
        {
            AppUserManager = manager;
            DbContext = dbContext;
        }

        public QueryReturner(AppUserManager userManager, AppRoleManager roleManager, AppDbContext dbContext, HttpRequestMessage request)
        {
            AppUserManager = userManager;
            AppRoleManager = roleManager;
            DbContext = dbContext;
            UrlHelper = new UrlHelper(request);
        }

        public QueryReturner(AppUserManager userManager, AppDbContext dbContext, HttpRequestMessage request)
        {
            AppUserManager = userManager;
            DbContext = dbContext;
            UrlHelper = new UrlHelper(request);
        }

        public List<UserDTO> GetAllUsers(string userId)
        {
            if (userId == null) return null;
            List<UserDTO> result = new List<UserDTO>();
            if (AppUserManager.IsInRoleAsync(userId, "Admin").Result)
            {
                foreach (var u in DbContext.Users)
                {
                    result.Add(new UserDTO
                    {
                        UserName = u.UserName,
                        Id = u.Id,
                        Role = AppUserManager.GetRoles(u.Id).First(),
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        AccessFailedCount = u.AccessFailedCount,
                        EmailConfirmed = u.EmailConfirmed
                    });
                }
            }
            else
            {
                foreach (var u in DbContext.Users)
                {
                    result.Add(new UserDTO
                    {
                        UserName = u.UserName,
                        Role = AppUserManager.GetRoles(u.Id).First(),
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                    });
                }
            }
            return result;
        }

        public UserDTO GetUserById(string id)
        {
            if (id == null) return null;
            var user = AppUserManager.FindById(id);
            if (user == null) return null;
            var role = AppUserManager.GetRoles(user.Id).First();
            var url = UrlHelper.Link("GetUserById", new { id = user.Id });
            return new UserDTO(user, role, url);
        }

        public UserDTO GetUserByName(string userName, string currentUserId)
        {
            if (currentUserId == null || userName == null) return null;
            var user = AppUserManager.FindByNameAsync(userName).Result;
            if (user == null) return null;
            var role = AppUserManager.GetRoles(user.Id).First();

            if (this.AppUserManager.IsInRole(currentUserId, "Admin"))
            {
                var url = UrlHelper.Link("GetUserById", new { id = user.Id });
                return new UserDTO(user, role, url);
            }
            else
            {
                return new UserDTO
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = role,
                    Job = user.Job,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gend,
                    BirthDate = user.BirthDate,
                    Bio = user.Bio
                };
            }
        }

        public List<UserDTO> GetProgrammers()
        {
            var programmerRoleId = AppRoleManager.Roles.FirstOrDefault(r => r.Name == "Programmer").Id;
            return DbContext.Users.Where(u => u.Roles.FirstOrDefault(r => r.RoleId == programmerRoleId) != null).Select(user => new UserDTO
            {
                UserName = user.UserName,
                Id = user.Id
            }).ToList();
        }

        public UserDTO GetSessionInfo(string userId)
        {
            if (userId == null) return null;
            var user = AppUserManager.FindById(userId);
            if (user == null) return null;
            return new UserDTO
            {
                Role = AppUserManager.GetRoles(userId).First(),
                UserName = user.UserName
            };
        }
    }
}