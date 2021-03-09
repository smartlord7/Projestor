using AspNetIdentity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Routing;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AspNetIdentity.WebApi.Models;
using System.Data.Entity.Migrations;
using static AspNetIdentity.WebApi.Infrastructure.Issue;

namespace AspNetIdentity.WebApi.Controllers
{
    [RoutePrefix("api/issues")]
    public class IssuesController : BaseApiController
    {

        [Authorize]
        [HttpGet]
        [Route("list", Name = "GetAllIssues")]
        public IHttpActionResult GetIssues()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            if (this.AppUserManager.IsInRole(userId, "Admin"))
            {
                return Ok(dbContext.Issues.Include(t => t.Project).Select(t => new
                {
                    t.Id,
                    t.Name,
                    projectName = t.Project.Name,
                    t.ProjectId,
                    t.UserId,
                    programmerName = dbContext.Users.FirstOrDefault(u => u.Id == t.UserId).UserName,
                    t.CreatedDateTime,
                    t.LastAlteredDateTime,
                    t.LimitDate,
                    t.Prio,
                    t.State
                }).ToList());
            }
            else if (this.AppUserManager.IsInRole(userId, "Manager"))
            {
                return Ok(dbContext.Issues.Include(t => t.Project).Select(t => new
                {
                    t.Id,
                    t.Name,
                    projectName = t.Project.Name,
                    t.Project.ManagerId,
                    t.ProjectId,
                    t.UserId,
                    programmerName = dbContext.Users.FirstOrDefault(u => u.Id == t.UserId).UserName,
                    t.CreatedDateTime,
                    t.LastAlteredDateTime,
                    t.LimitDate,
                    t.Prio,
                    t.State
                }).Where(t => t.ManagerId == userId).ToList());
            }
            else {
                return Ok(dbContext.Issues.Include(t => t.Project).Select(t => new
                {
                    t.Id,
                    t.Name,
                    projectName = t.Project.Name,
                    t.Project.ManagerId,
                    managerName = dbContext.Users.FirstOrDefault(u => u.Id == t.Project.ManagerId).UserName,
                    t.ProjectId,
                    t.UserId,
                    t.CreatedDateTime,
                    t.LastAlteredDateTime,
                    t.LimitDate,
                    t.Prio,
                    t.State
                }).Where(t => t.UserId == userId).ToList());
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("issue/{id}", Name = "DeleteIssueById")]
        public IHttpActionResult DeleteIssue(int id)
        {
            var task = dbContext.Issues.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            dbContext.Issues.Remove(task);
            dbContext.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("issue/{id}", Name = "GetIssueDetailed")]

        public IHttpActionResult GetIssue(int id)
        {
            var user = this.AppUserManager.FindById(User.Identity.GetUserId());
            if (user == null) return NotFound();
            var taskResult = dbContext.Issues.Include(t => t.Project).FirstOrDefault(t => t.Id == id);
            if (taskResult == null) return NotFound();
            var projResult = dbContext.Projects.FirstOrDefault(p => p.Id == taskResult.ProjectId);
            if (projResult == null) return NotFound();
            if (taskResult.UserId != user.Id && projResult.ManagerId != user.Id && !this.AppUserManager.IsInRole(user.Id, "Admin")) return Unauthorized();
            return Ok(new
            {
                taskResult.UserId,
                taskResult.Id,
                taskResult.ProjectId,
                taskResult.Name,
                taskResult.LimitDate,
                taskResult.Prio,
                taskResult.State,
                taskResult.LastAlteredDateTime,
                taskResult.CreatedDateTime,
                projectName = taskResult.Project.Name,
                taskResult.Description
            });
        }

        [Authorize(Roles = "Programmer, Admin")]
        [HttpPost]
        [Route("issuesState", Name = "AlterIssuesState")]

        public IHttpActionResult AlterIssuesState(List<AlterTaskStateModel> alterTaskStateModelList)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            foreach (var alterTaskStateModel in alterTaskStateModelList)
            {
                var task = dbContext.Issues.FirstOrDefault(t => t.Id == alterTaskStateModel.Id);
                if (task == null) return NotFound();
                if (task.UserId != userId && !this.AppUserManager.IsInRole(userId, "Admin")) return Unauthorized();
                task.State = alterTaskStateModel.State;
                dbContext.SaveChanges();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [Route("edit", Name = "EditIssue")]
        public IHttpActionResult EditIssue(Issue issue)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (issue.Id == 0)
            {
                issue.State = IssueState.NOT_STARTED;
                issue.CreatedDateTime = DateTime.Now;
            }
            issue.LastAlteredDateTime = DateTime.Now;
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            var projResult = dbContext.Projects.FirstOrDefault(p => p.Id == issue.ProjectId);
            if (projResult == null) return NotFound();
            if (projResult.ManagerId != userId && !this.AppUserManager.IsInRole(userId, "Admin")) return Unauthorized();
            dbContext.Issues.AddOrUpdate(issue);
            dbContext.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("projectIssues/{id}", Name = "GetIssuesByProjectId")]

        public IHttpActionResult GetProjectIssues(int id)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            var projResult = dbContext.Projects.FirstOrDefault(p => p.Id == id);
            if (projResult == null) return NotFound();
            if (projResult.ManagerId != userId && this.AppUserManager.IsInRole(userId, "Manager")) return Unauthorized();
            var user = this.AppUserManager.FindById(userId);
            if (!this.AppUserManager.IsInRole(userId, "Programmer"))
            {
                return Ok(dbContext.Issues
                             .Select(issue => new
                             {
                                 issue.Id,
                                 issue.Name,
                                 issue.CreatedDateTime,
                                 issue.LimitDate,
                                 issue.Project.ManagerId,
                                 issue.Prio,
                                 issue.ProjectId,
                                 issue.State,
                                 projectName = dbContext.Projects.FirstOrDefault(p => p.Id == issue.ProjectId).Name,
                                 programmerName = dbContext.Users.FirstOrDefault(u => u.Id == issue.UserId).UserName,
                                 dbContext.Users.FirstOrDefault(u => u.Id == issue.UserId).UserName,
                                 issue.UserId,
                             })
                             .Where(t => t.ProjectId == id)
                             .ToList());
            }
            else {
                return Ok(dbContext.Issues
                             .Select(issue => new
                             {
                                 issue.Id,
                                 issue.Name,
                                 issue.CreatedDateTime,
                                 issue.LimitDate,
                                 issue.Prio,
                                 issue.ProjectId,
                                 issue.State,
                                 issue.UserId,
                             })
                             .Where(t => t.ProjectId == id && t.UserId == userId)
                             .ToList());
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("delete", Name = "DeleteIssuesByIds")]

        public IHttpActionResult DeleteIssues(int[] ids)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            foreach (var id in ids)
            {
                Issue issueResult = null;
                if (this.AppUserManager.IsInRole(userId, "Manager")) 
                issueResult = dbContext.Issues.Include(t => t.Project).FirstOrDefault(t => t.Project.ManagerId == userId && t.Id == id);
                else if (this.AppUserManager.IsInRole(userId, "Admin"))
                    issueResult = dbContext.Issues.Include(t => t.Project).FirstOrDefault(t => t.Id == id);
                if (issueResult == null) return NotFound();
                dbContext.Issues.Remove(issueResult);
                dbContext.SaveChanges();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("issueNotes", Name = "EditIssueNotes")]
        public IHttpActionResult EditIssueNotes(EditTaskNoteModel editTaskNoteModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            var issue = dbContext.Issues.FirstOrDefault(t => t.Id == editTaskNoteModel.TaskId);
            if (issue == null) return NotFound();
            var proj = dbContext.Projects.FirstOrDefault(p => p.Id == issue.ProjectId);
            if (proj == null) return NotFound();
            if (issue.UserId != userId && proj.ManagerId != userId && !this.AppUserManager.IsInRole(userId, "Admin")) return Unauthorized();
            issue.Notes = editTaskNoteModel.Notes;
            dbContext.SaveChanges();
            return Ok();

        }

        [Authorize]
        [HttpGet]
        [Route("issueNotes/{id}", Name = "GetIssueNotes")]

        public IHttpActionResult GetIssueNotes(int id)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            var issue = dbContext.Issues.FirstOrDefault(t => t.Id == id);
            if (issue == null) return NotFound();
            var proj = dbContext.Projects.FirstOrDefault(p => p.Id == issue.ProjectId);
            if (proj == null) return NotFound();
            if (issue.UserId != userId && proj.ManagerId != userId && !this.AppUserManager.IsInRole(userId, "Admin")) return Unauthorized();
            return Ok(issue.Notes);
        }

    }
}