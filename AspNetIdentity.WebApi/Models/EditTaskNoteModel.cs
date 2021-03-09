using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.WebApi.Models
{
    public class EditTaskNoteModel
    {

        [Required]
        public int TaskId { get; set; }

        [MaxLength(1000)]

        public string Notes { get; set; }

    }

}