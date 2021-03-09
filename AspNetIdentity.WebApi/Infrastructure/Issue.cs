using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetIdentity.WebApi.Infrastructure
{
    public partial class Issue
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string UserId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LimitDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastAlteredDateTime { get; set; }

        [EnumDataType(typeof(IssueState))]
        public IssueState State { get; set; }

        [Required]
        [EnumDataType(typeof(Priority))]
        public Priority Prio { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
    }
}