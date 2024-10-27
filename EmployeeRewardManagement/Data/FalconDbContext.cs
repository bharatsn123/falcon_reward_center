using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeRewardManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRewardManagement.Data
{
    public class FalconDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Reward> Reward { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<RewardStore> RewardStore { get; set; }
        public DbSet<AwardsGranted> AwardsGranted { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=170.64.170.2;Database=falcon_reward_center;User=root;Password=C#1Project;" +
                                     "AllowPublicKeyRetrieval=True;SslMode=none;",
                                     new MySqlServerVersion(new Version(8, 0, 39))); // Replace with your MySQL version
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RewardStore>()
        .HasKey(r => r.StoreItemID);

            modelBuilder.Entity<AwardsGranted>()
    .HasKey(a => a.AwardID);  // Set AwardID as the primary key

            modelBuilder.Entity<AwardsGranted>()
                .HasOne<Reward>()  // Assuming a Reward entity is defined
                .WithMany()        // No navigation property in Reward
                .HasForeignKey(a => a.RewardID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AwardsGranted>()
                .HasOne<Employee>()  // Assuming an Employee entity is defined
                .WithMany()
                .HasForeignKey(a => a.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Update Transaction table mapping:
            // Define ItemID as foreign key linking to RewardStore's StoreItemID
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.RewardStoreItem)  // Navigation property in Transaction class
                .WithMany()  // No inverse navigation needed
                .HasForeignKey(t => t.ItemID)  // Link Transaction.ItemID to RewardStore.StoreItemID
                .OnDelete(DeleteBehavior.Restrict); // Define delete behavior (optional)
        }

        // Method to get points of an employee directly from DbContext
        public int GetEmployeePoints(int employeeID)
        {
            var employee = Employee.FirstOrDefault(e => e.EmployeeID == employeeID);
            return employee != null ? employee.Points : 0;
        }

        public List<EmployeeAchievementDTO> GetEmployeeAchievements(int employeeID)
        {
            var query = from a in AwardsGranted
                        join r in Reward on a.RewardID equals r.RewardID  // Join on RewardID to get reward details
                        where a.EmployeeID == employeeID
                        select new EmployeeAchievementDTO
                        {
                            TransactionID = a.AwardID,  // Using AwardsGrantedID instead of TransactionID
                            RewardName = r.RewardName,
                            Points = r.Points,
                            TransactionDate = a.GrantedDate  // Assuming GrantedDate is the date of the award
                        };

            return query.ToList();
        }


        // Method to get the employee leaderboard
        public List<LeaderboardEntryDTO> GetEmployeesLeaderboard()
        {
            // Fetch the employees from the database and bring the data into memory
            var employees = Employee
                .OrderByDescending(e => e.Points) // Order by Points in descending order
                .AsEnumerable()                   // Move evaluation to in-memory
                .Select((e, index) => new LeaderboardEntryDTO
                {
                    Rank = index + 1,              // Add ranking (starts from 1)
                    Name = e.Name,
                    BusinessUnit = e.BusinessUnit,
                    JobTitle = e.JobTitle,
                    Points = e.Points
                })
                .ToList();

            return employees;
        }
    }
}
