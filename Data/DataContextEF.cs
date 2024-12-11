using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
        // constructor
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config; // private to inject the config value
        }

        // set db sets
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSalary> UserSalaries { get; set; }
        public virtual DbSet<UserJobInfo> UserJobInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"), // EntityFrameworkCore.SqlServer for UseSqlServer
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema"); // for this command <dotnet add package Microsoft.EntityFrameworkCore.Relational --version 0.0.0>\

            modelBuilder.Entity<User>()
                .ToTable("Users", "TutorialAppSchema") // table name should be plural and match the db table name with sql
                .HasKey(u => u.UserId); // set primary key auto

            modelBuilder.Entity<UserSalary>()
                .ToTable("UserSalary", "TutorialAppSchema") // table name should be plural and match the db table name with sql
                .HasKey(u => u.UserId);


            modelBuilder.Entity<UserJobInfo>()
                .ToTable("UserJobInfo", "TutorialAppSchema") // table name should be plural and match the db table name with sql
                .HasKey(u => u.UserId);
        }

    }
}