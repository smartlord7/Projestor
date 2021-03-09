using AspNetIdentity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace AspNetIdentity.WebApi.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : BaseApiController
    {

        [Authorize]
        [HttpGet]
        [Route("list", Name = "ListProjects")]

        public IHttpActionResult GetProjects()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            if (this.AppUserManager.IsInRole(userId, "Admin"))
            {
                return Ok(dbContext.Projects.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.ManagerId,
                    managerName = dbContext.Users.FirstOrDefault(u => u.Id == p.ManagerId).UserName,
                    p.Budget,
                    p.CreatedDateTime,
                    p.LastAlteredDateTime,
                }).ToList());
            }
            else if (this.AppUserManager.IsInRole(userId, "Manager"))
            {
                return Ok(dbContext.Projects
                .Select(proj => new
                {
                    proj.Id,
                    proj.ManagerId,
                    proj.CreatedDateTime,
                    proj.Name,
                    proj.Budget,
                })
                .Where(proj => proj.ManagerId == userId).ToList());
            }
            else {
                return Ok(dbContext.Issues.Include(t => t.Project).Select(t => new
                {
                    t.UserId,
                    id = t.Project.Id,
                    t.Project.Name,
                    t.Project.Budget,
                    t.Project.CreatedDateTime,
                    t.Project.LastAlteredDateTime,
                    managerName = dbContext.Users.FirstOrDefault(u => u.Id == t.Project.ManagerId).UserName
                }).Where(t => t.UserId == userId).Distinct().ToList());
            }
        }


        [Authorize]
        [HttpGet]
        [Route("project/{id}", Name = "GetProjectById")]

        public IHttpActionResult GetProject(int id)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            var proj = dbContext.Projects.FirstOrDefault(t => t.Id == id);
            if (proj == null) return NotFound();
            var tasks = dbContext.Issues.Select(t => new Issue()).Where(t => t.UserId == userId && t.ProjectId == proj.Id);
            if (proj.ManagerId != userId && !this.AppUserManager.IsInRole(userId, "Admin") && tasks == null) return Unauthorized();
            return Ok(proj);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("delete", Name = "DeleteProjects")]
        public IHttpActionResult DeleteProjects(int[] ids)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            foreach (var id in ids) 
            {
                var projResult = dbContext.Projects.FirstOrDefault(p => p.Id == id);
                if (projResult == null) return NotFound();
                if (projResult.ManagerId != userId && !this.AppUserManager.IsInRole(userId, "Admin")) return Unauthorized();
                dbContext.Projects.Remove(projResult);
                dbContext.SaveChanges();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [Route("edit", Name = "EditProject")]

        public IHttpActionResult EditProject(Project proj)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) return NotFound();
            proj.ManagerId = userId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (proj.Id == 0) proj.CreatedDateTime = DateTime.Now;
            proj.LastAlteredDateTime = DateTime.Now;
            dbContext.Projects.AddOrUpdate(proj);
            dbContext.SaveChanges();
            return Ok();
        }

        
    }
}
