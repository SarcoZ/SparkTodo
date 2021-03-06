﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SparkTodo.Models
{
    /// <summary>
    /// SparkTodoEntity
    /// </summary>
    public class SparkTodoEntity:IdentityDbContext<IdentityUser>
    {
        public SparkTodoEntity(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserAccount>().Property(u => u.UserId).ValueGeneratedOnAdd();
            builder.Entity<UserAccount>().HasIndex(u => u.UserId);
            builder.Entity<Category>().HasKey(c=>c.CategoryId);
            builder.Entity<TodoItem>().HasKey(t=>t.TodoId);

            base.OnModelCreating(builder);
        }

        public virtual DbSet<UserAccount> UserAccounts {get;set;}

        public virtual DbSet<Category> Categories {get;set;}

        public virtual DbSet<TodoItem> TodoItems {get;set;}
    }
}
