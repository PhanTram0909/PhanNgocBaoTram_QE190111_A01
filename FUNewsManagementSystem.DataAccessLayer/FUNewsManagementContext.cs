using FUNewsManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.DataAccessLayer
{
    public class FUNewsManagementContext : DbContext
    {
        public FUNewsManagementContext(DbContextOptions<FUNewsManagementContext> options)
            : base(options)
        {
        }

        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== MAP TABLE NAMES (theo DB giảng viên) =====
            modelBuilder.Entity<SystemAccount>().ToTable("SystemAccount");
            modelBuilder.Entity<NewsCategory>().ToTable("Category");
            modelBuilder.Entity<NewsArticle>().ToTable("NewsArticle");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<NewsTag>().ToTable("NewsTag");

            // Fix typo trong DB giảng viên: "CategoryDesciption" (thiếu 'r')
            modelBuilder.Entity<NewsCategory>()
                .Property(c => c.CategoryDescription)
                .HasColumnName("CategoryDesciption");

            // ===== COMPOSITE KEY =====
            modelBuilder.Entity<NewsTag>()
                .HasKey(nt => new { nt.NewsArticleId, nt.TagId });

            // ===== RELATIONSHIPS =====

            // NewsCategory tự tham chiếu (parent-child)
            modelBuilder.Entity<NewsCategory>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // NewsArticle → CreatedBy
            modelBuilder.Entity<NewsArticle>()
                .HasOne(a => a.CreatedBy)
                .WithMany(u => u.CreatedArticles)
                .HasForeignKey(a => a.CreatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // NewsArticle → UpdatedBy
            modelBuilder.Entity<NewsArticle>()
                .HasOne(a => a.UpdatedBy)
                .WithMany(u => u.UpdatedArticles)
                .HasForeignKey(a => a.UpdatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // NewsArticle → Category
            modelBuilder.Entity<NewsArticle>()
                .HasOne(a => a.Category)
                .WithMany(c => c.NewsArticles)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // NewsTag → NewsArticle
            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.NewsArticle)
                .WithMany(a => a.NewsTags)
                .HasForeignKey(nt => nt.NewsArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            // NewsTag → Tag
            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NewsTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
