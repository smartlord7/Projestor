using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetIdentity.WebApi.Infrastructure
{
    public class Project
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string ManagerId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastAlteredDateTime { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [ForeignKey("ManagerId")]
        public virtual User User {get; set;}

    }
}