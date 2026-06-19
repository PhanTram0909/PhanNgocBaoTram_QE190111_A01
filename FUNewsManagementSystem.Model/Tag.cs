using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Model
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters")]
        [Display(Name = "Tag Name")]
        public string TagName { get; set; } = null!;

        [StringLength(250, ErrorMessage = "Note cannot exceed 250 characters")]
        [Display(Name = "Note")]
        public string? Note { get; set; }

        // Navigation
        public ICollection<NewsTag>? NewsTags { get; set; }
    }
}
