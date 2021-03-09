using System;
using static AspNetIdentity.WebApi.Infrastructure.Issue;

namespace AspNetIdentity.WebApi.Business.DTO
{
    public class IssueDTO
    {
        public IssueDTO()
        { 
        
        
        }

        public string Name { get; set; }

        public string ProgrammerOrManagerName { get; set; }

        public string UserId { get; set; }

        public int ProjectId { get; set; }

        public DateTime LimitDate { get; set; }

        public DateTime LastALteredDateTime { get; set; }

        public DateTime CreatedDateTIme { get; set; }

        public IssueState State { get; set; }

        public Priority Priority { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }

    }
}