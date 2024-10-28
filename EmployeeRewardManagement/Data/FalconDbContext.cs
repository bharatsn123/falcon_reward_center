using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeRewardManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRewardManagement.Data
{
    public class FalconDbContext : DbContext
    {
        // Requirement fulfilled: At least one example of Generics/Generic based Collection
        // Bonus Requirement fulfilled: Use of Entity framework
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Reward> Reward { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<RewardStore> RewardStore { get; set; }
        public DbSet<AwardsGranted> AwardsGranted { get; set; }

        public FalconDbContext(DbContextOptions<FalconDbContext> options) : base(options) { }

        public FalconDbContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Check if the options have already been configured by tests (in-memory database)
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "Server=170.64.170.2;Database=falcon_reward_center;User=root;Password=C#1Project;" +
                    "AllowPublicKeyRetrieval=True;SslMode=none;",
                    new MySqlServerVersion(new Version(8, 0, 39)),
                    mysqlOptions =>
                    {
                        mysqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(3),
                            errorNumbersToAdd: null
                        );
                    });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RewardStore>()
                .HasKey(r => r.StoreItemID);

            modelBuilder.Entity<AwardsGranted>()
                .HasKey(a => a.AwardID);

            modelBuilder.Entity<AwardsGranted>()
                .HasOne<Reward>()
                .WithMany()
                .HasForeignKey(a => a.RewardID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AwardsGranted>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(a => a.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.RewardStoreItem)
                .WithMany()
                .HasForeignKey(t => t.ItemID)  // Link Transaction.ItemID to RewardStore.StoreItemID
                .OnDelete(DeleteBehavior.Restrict); // Define delete behavior
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
                            TransactionID = a.AwardID,
                            RewardName = r.RewardName,
                            Points = r.Points,
                            TransactionDate = a.GrantedDate
                        };

            return query.ToList();
        }


        // Method to get the employee leaderboard
        public List<LeaderboardEntryDTO> GetEmployeesLeaderboard()
        {
            using (var context = new FalconDbContext())
            {
                // Calculate total points for each employee based on awards
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
                    .ToList();  // Execute and materialise in memory to avoid complex LINQ translation issues

                // Retrieve employee data and join in memory
                var leaderboard = context.Employee
                    .ToList()  // Materialise Employee list in memory
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
