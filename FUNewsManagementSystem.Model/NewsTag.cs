using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagementSystem.Model
{
    public class NewsTag
    {
        [Required]
        [StringLength(20)]
        public string NewsArticleId { get; set; } = null!;

        [Required]
        public int TagId { get; set; }

        // Navigation properties
        [ForeignKey("NewsArticleId")]
        public NewsArticle? NewsArticle { get; set; }

        [ForeignKey("TagId")]
        public Tag? Tag { get; set; }
    }
}
