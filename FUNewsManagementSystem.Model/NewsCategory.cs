using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagementSystem.Model
{
    public class NewsCategory
    {
        [Key]
        public short CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        [Display(Name = "Description")]
        public string? CategoryDescription { get; set; }

        [Display(Name = "Parent Category")]
        public short? ParentCategoryId { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("ParentCategoryId")]
        public NewsCategory? ParentCategory { get; set; }
        public ICollection<NewsCategory>? SubCategories { get; set; }
        public ICollection<NewsArticle>? NewsArticles { get; set; }
    }
}
