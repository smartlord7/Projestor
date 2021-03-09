using System.ComponentModel.DataAnnotations;
using static AspNetIdentity.WebApi.Infrastructure.Issue;

namespace AspNetIdentity.WebApi.Models
{
    public class AlterTaskStateModel
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [EnumDataType(typeof(IssueState))]
        public IssueState State { get; set; }

    }
}