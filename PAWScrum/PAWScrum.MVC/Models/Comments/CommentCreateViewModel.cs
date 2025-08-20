using System.ComponentModel.DataAnnotations;

namespace PAWScrum.MVC.Models.Comments
{
    public class CommentCreateViewModel
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;
    }
}
