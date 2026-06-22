using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Model
{
    public class SystemAccount
    {
        [Key]
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters")]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(70, ErrorMessage = "Email cannot exceed 70 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string AccountEmail { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "Role")]
        public int AccountRole { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(70, ErrorMessage = "Password cannot exceed 70 characters")]
        [Display(Name = "Password")]
        public string AccountPassword { get; set; } = null!;

        // Navigation properties
        public ICollection<NewsArticle>? CreatedArticles { get; set; }
        public ICollection<NewsArticle>? UpdatedArticles { get; set; }
    }
}
