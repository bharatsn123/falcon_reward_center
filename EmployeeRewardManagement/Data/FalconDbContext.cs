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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=170.64.170.2;Database=falcon_reward_center;User=root;Password=C#1Project;" +
                                     "AllowPublicKeyRetrieval=True;SslMode=none;",
                                     new MySqlServerVersion(new Version(8, 0, 39))); // Replace with your MySQL version
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define StoreItemID as the primary key in RewardStore
            modelBuilder.Entity<RewardStore>()
                .HasKey(r => r.StoreItemID);

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
            /**
             * Usage:
             *  using (var context = new FalconDbContext())
             *  {
             *      int employeeID = 1001; // Example Employee ID
             *      List<EmployeeAchievementDTO> employeeAchievements = context.GetEmployeeAchievements(employeeID);
             *      // Display or process the achievements
             *      foreach (var achievement in employeeAchievements)
             *      {
             *          Console.WriteLine($"Transaction ID: {achievement.TransactionID}, Reward: {achievement.RewardName}, Points: {achievement.Points}, Date: {achievement.TransactionDate}");
             *      }
             *  }
             */
            var query = from t in Transaction
                        join r in Reward on t.ItemID equals r.RewardID  // Updated to ItemID
                        where t.EmployeeID == employeeID
                        select new EmployeeAchievementDTO
                        {
                            TransactionID = t.TransactionID,
                            RewardName = r.RewardName,
                            Points = r.Points,
                            TransactionDate = t.TransactionDate
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
