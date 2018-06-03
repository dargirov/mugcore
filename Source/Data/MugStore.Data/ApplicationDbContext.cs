using Microsoft.EntityFrameworkCore;
using MugStore.Data.Models;
using System;
using System.Linq;

namespace MugStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Bulletin> Bulletins { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<DeliveryInfo> DeliveryInfoes { get; set; }

        public DbSet<Courier> Couriers { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<ProductTagProduct> ProductTagProducts { get; set; }

        public DbSet<PostTagPost> PostTagPosts { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // query filters
            modelBuilder.Entity<Order>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<City>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Image>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Bulletin>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ProductImage>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ProductTag>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<DeliveryInfo>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Courier>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Feedback>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Log>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Post>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PostTag>().HasQueryFilter(x => !x.IsDeleted);

            // constraints
            modelBuilder.Entity<Bulletin>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<Image>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<ProductImage>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // many to many relations
            modelBuilder.Entity<ProductTagProduct>()
                .HasKey(x => new { x.ProductId, x.ProductTagId });

            modelBuilder.Entity<ProductTagProduct>()
                .HasOne(x => x.Product)
                .WithMany(x => x.ProductTags)
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<ProductTagProduct>()
                .HasOne(x => x.ProductTag)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductTagId);

            modelBuilder.Entity<PostTagPost>()
                .HasKey(x => new { x.PostId, x.PostTagId });

            modelBuilder.Entity<PostTagPost>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<PostTagPost>()
                .HasOne(x => x.PostTag)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.PostTagId);
        }
    }
}
