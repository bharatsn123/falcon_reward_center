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
                                     new MySqlServerVersion(new Version(8, 0, 39)),
                                     mysqlOptions =>
                                     {
                                         mysqlOptions.EnableRetryOnFailure(
                                             maxRetryCount: 5, // Number of retry attempts
                                             maxRetryDelay: TimeSpan.FromSeconds(3), // Delay between retries
                                             errorNumbersToAdd: null); 
                                     });
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
            using (var context = new FalconDbContext())
            {
                // Step 1: Calculate total points for each employee based on awards
                var employeePoints = context.AwardsGranted
                    .Join(context.Reward,
                          award => award.RewardID,
                          reward => reward.RewardID,
                          (award, reward) => new { award.EmployeeID, Points = reward.Points })
                    .GroupBy(x => x.EmployeeID)
                    .Select(group => new
                    {
                        EmployeeID = group.Key,
                        TotalPoints = group.Sum(x => x.Points)
                    })
                    .ToList();  // Execute and materialize in memory to avoid complex LINQ translation issues

                // Step 2: Retrieve employee data and join in memory
                var leaderboard = context.Employee
                    .ToList()  // Materialize Employee list in memory
                    .Join(employeePoints,
                          emp => emp.EmployeeID,
                          points => points.EmployeeID,
                          (emp, points) => new LeaderboardEntryDTO
                          {
                              Name = emp.Name,
                              BusinessUnit = emp.BusinessUnit,
                              JobTitle = emp.JobTitle,
                              Points = points.TotalPoints
                          })
                    .OrderByDescending(e => e.Points)
                    .Select((e, index) => new LeaderboardEntryDTO
                    {
                        Rank = index + 1,
                        Name = e.Name,
                        BusinessUnit = e.BusinessUnit,
                        JobTitle = e.JobTitle,
                        Points = e.Points
                    })
                    .ToList();

                return leaderboard;
            }
        }


    }
}
