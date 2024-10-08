﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutorAppBackend.Models;

namespace TutorAppBackend.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<User> User { get; set; }
        public DbSet<Subject> Subject { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<Comment> Comment { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>().ToTable("Messages");

            // Message -> Sender
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message -> Receiver
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship for Comment -> User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict cascading deletes

            // Configure the relationship for Comment -> Post
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict cascading deletes
        }

    }

}
