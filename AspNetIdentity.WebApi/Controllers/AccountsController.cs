using AspNetIdentity.WebApi.Business;
using AspNetIdentity.WebApi.Business.DTO;
using AspNetIdentity.WebApi.Infrastructure;
using AspNetIdentity.WebApi.Models;
using AspNetIdentity.WebApi.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace AspNetIdentity.WebApi.Controllers
{

    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        private QueryReturner returner = null;

        [Authorize]
        [HttpGet]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            returner = new QueryReturner(AppUserManager, dbContext);
            List<UserDTO> result = returner.GetAllUsers(User.Identity.GetUserId());
            if (result == null) return NotFound();
            else return Ok(result);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        [Route("programmers")]
        public IHttpActionResult GetProgrammers()
        {
            returner = new QueryReturner(AppUserManager, AppRoleManager, dbContext, this.Request);
            List<UserDTO> result = returner.GetProgrammers();
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public IHttpActionResult GetUserById(string Id)
        {
            returner = new QueryReturner(AppUserManager, dbContext);
            Object result = returner.GetUserById(User.Identity.GetUserId());
            if (result == null) return NotFound();
            else return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("user/{username}", Name = "GetUserByName")]
        public IHttpActionResult GetUserByName(string username)
        {
            returner = new QueryReturner(AppUserManager, dbContext, this.Request);
            Object result = returner.GetUserByName(username, User.Identity.GetUserId());
            if (result == null) return NotFound();
            else return Ok(result);

        }

        [Authorize]
        [HttpGet]
        [Route("sessionData")]

        public IHttpActionResult getSessionData()
        {
            returner = new QueryReturner(AppUserManager, dbContext);
            UserDTO result = returner.GetSessionInfo(User.Identity.GetUserId());
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("deleteUsers")]
        public async Task<IHttpActionResult> DeleteUsers(string[] ids)
        {
            foreach (var id in ids)
            {
                var user = await this.AppUserManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await this.AppUserManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exist in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("edit/{userName}")]

        public async Task<IHttpActionResult> EditUser([FromBody] EditUserBindingModel userModel, [FromUri] string userName)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = this.AppUserManager.FindByName(userName);

            if (this.AppUserManager.IsInRole(userId, "Admin"))
            {
                user.UserName = userModel.UserName;
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.BirthDate = userModel.BirthDate;
                user.Gend = userModel.Gender;
                user.PhoneNumber = userModel.PhoneNumber;
                user.Email = userModel.Email;
                user.Job = userModel.Job;
                user.Bio = userModel.Bio;
                user.EmailConfirmed = userModel.EmailConfirmed;
                user.AccessFailedCount = userModel.AccessFailedCount;
                var roles = this.AppRoleManager.Roles.Select(r => r.Name).ToArray();
                foreach (var role in roles)
                {
                    this.AppUserManager.RemoveFromRole(user.Id, role);
                }
                this.AppUserManager.AddToRole(user.Id, userModel.Role);
            }
            else
            {
                user.UserName = userModel.UserName;
                user.PhoneNumber = userModel.PhoneNumber;
                user.Email = userModel.Email;
                user.Job = userModel.Job;
                user.Bio = userModel.Bio;
            }

            IdentityResult editUserResult = await this.AppUserManager.UpdateAsync(user);

            if (!editUserResult.Succeeded)
            {
                return GetErrorResult(editUserResult);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = new User()
            {
                UserName = createUserModel.Username,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                Email = createUserModel.Email,
                BirthDate = createUserModel.BirthDate,
                PhoneNumber = createUserModel.PhoneNumber,
                Gend = createUserModel.Gender,
                Job = createUserModel.Job,
                Bio = createUserModel.Bio,
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(user.Id, createUserModel.Role);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user role");
                return BadRequest(ModelState);
            }

            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new
            {
                userId = user.Id,
                code = code
            }));

            EmailService sender = new EmailService();
            IdentityMessage msg = new IdentityMessage();
            msg.Subject = "Projestor Email Confirmation";
            msg.Body = "Click this link to confirm your email:   " + callbackUrl.ToString();
            msg.Destination = user.Email;
            await sender.SendAsync(msg);

            Uri locationHeader = new Uri(Url.Link("GetUserById", new
            {
                id = user.Id
            }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "", string password = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok("Account confirmed! Thank you!");
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("changePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
    }
}