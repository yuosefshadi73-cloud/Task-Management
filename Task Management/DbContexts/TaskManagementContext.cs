using Microsoft.EntityFrameworkCore;
//using System.Reflection.Emit;
using Task_Management.Models;

namespace Task_Management.DbContexts
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext( DbContextOptions<TaskManagementContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<TaskItem> Tasks { get; set; }

        public DbSet<Lookup> Lookups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lookup>().HasData(

                new Lookup { Id = 1, MajorCode = 1, MinorCode = 0, Name = "Task Status" },

                new Lookup { Id = 2, MajorCode = 1, MinorCode = 1, Name = "Initiated" },

                new Lookup { Id = 3, MajorCode = 1, MinorCode = 2, Name = "In Progress" },

                new Lookup { Id = 4, MajorCode = 1, MinorCode = 3, Name = "Completed" },

                new Lookup { Id = 5, MajorCode = 1, MinorCode = 4, Name = "Cancelled" }
            );
        }
    }
}