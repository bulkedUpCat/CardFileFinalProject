using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contexts
{
    /// <summary>
    /// Application database context
    /// </summary>
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.TextMaterials)
                .WithOne(tm => tm.Author)
                .HasForeignKey(u => u.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SavedTextMaterials)
                .WithMany(tm => tm.UsersWhoSaved)
                .UsingEntity(t => t.ToTable("SavedTextMaterials"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.LikedTextMaterials)
                .WithMany(tm => tm.UsersWhoLiked)
                .UsingEntity(t => t.ToTable("LikedTextMaterials"));

            modelBuilder.Entity<User>()
                .HasOne(u => u.Ban)
                .WithOne(b => b.User)
                .HasForeignKey<Ban>(b => b.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TextMaterial> TextMaterials { get; set; }
        public DbSet<TextMaterialCategory> TextMaterialCategory { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Ban> Bans { get; set; }
    }
}
