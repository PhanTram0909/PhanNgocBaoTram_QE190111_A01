using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagementSystem.Model
{
    public class NewsArticle
    {
        [Key]
        [StringLength(20)]
        [Display(Name = "Article ID")]
        public string NewsArticleId { get; set; } = null!;

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Title")]
        public string NewsTitle { get; set; } = null!;

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(250, ErrorMessage = "Headline cannot exceed 250 characters")]
        [Display(Name = "Headline")]
        public string Headline { get; set; } = null!;

        [StringLength(4000, ErrorMessage = "Content cannot exceed 4000 characters")]
        [Display(Name = "Content")]
        public string? NewsContent { get; set; }

        [StringLength(250, ErrorMessage = "Source cannot exceed 250 characters")]
        [Display(Name = "News Source")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public short CategoryId { get; set; }

        [Display(Name = "Status")]
        public bool NewsStatus { get; set; } = true;

        [Display(Name = "Created By")]
        public short? CreatedById { get; set; }

        [Display(Name = "Updated By")]
        public short? UpdatedById { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public NewsCategory? Category { get; set; }

        [ForeignKey("CreatedById")]
        public SystemAccount? CreatedBy { get; set; }

        [ForeignKey("UpdatedById")]
        public SystemAccount? UpdatedBy { get; set; }

        public ICollection<NewsTag>? NewsTags { get; set; }
    }
}
