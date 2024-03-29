﻿namespace TeisterMask.Data
{
    using Microsoft.EntityFrameworkCore;
    using TeisterMask.Data.Models;

    public class TeisterMaskContext : DbContext
    {
        public TeisterMaskContext() { }

        public TeisterMaskContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTask> EmployeesTasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(x=>x.Tasks)
                .WithOne(x=>x.Project);

            modelBuilder.Entity<EmployeeTask>()
               .HasOne(x=>x.Employee)
               .WithMany(x=>x.EmployeesTasks);

            modelBuilder.Entity<EmployeeTask>()
                .HasOne(x=>x.Task)
                .WithMany(x=>x.EmployeesTasks);

            modelBuilder.Entity<EmployeeTask>()
                .HasKey(x => new { x.TaskId, x.EmployeeId });
        }
    }
}