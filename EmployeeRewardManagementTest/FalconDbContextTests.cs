using NUnit.Framework;
using EmployeeRewardManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using EmployeeRewardManagement.Models;
using EmployeeRewardManagement;

namespace EmployeeRewardManagementTest
{
    [TestFixture]
    public class FalconDbContextTests
    {
        [Test]
        public void GetEmployeePoints_ReturnsCorrectPoints_ForValidEmployeeID()
        {
            var options = new DbContextOptionsBuilder<FalconDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            using (var context = new FalconDbContext(options))
            {
                context.Employee.Add(new Employee
                {
                    EmployeeID = 1,
                    Points = 150,
                    Address = "123 Main St",
                    BusinessUnit = "IT",
                    JobTitle = "Software Engineer",
                    Name = "John Doe",
                    Password = "password123"
                });
                context.SaveChanges();
            }

            using (var context = new FalconDbContext(options))
            {
                int points = context.GetEmployeePoints(1);
                Assert.AreEqual(150, points);
            }
        }

        [Test]
        public void GetEmployeeAchievements_ReturnsAchievements_ForValidEmployeeID()
        {
            var options = new DbContextOptionsBuilder<FalconDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            using (var context = new FalconDbContext(options))
            {
                context.Employee.Add(new Employee
                {
                    EmployeeID = 1,
                    Points = 150,
                    Address = "123 Main St",
                    BusinessUnit = "IT",
                    JobTitle = "Software Engineer",
                    Name = "John Doe",
                    Password = "password123"
                });
                context.Reward.Add(new Reward { RewardID = 1, RewardName = "Outstanding Performance", Points = 50 });
                context.AwardsGranted.Add(new EmployeeRewardManagement.AwardsGranted { EmployeeID = 1, RewardID = 1, GrantedDate = DateTime.Now });
                context.SaveChanges();
            }

            using (var context = new FalconDbContext(options))
            {
                var achievements = context.GetEmployeeAchievements(1);
                Assert.AreEqual(1, achievements.Count);
                Assert.AreEqual("Outstanding Performance", achievements[0].RewardName);
                Assert.AreEqual(50, achievements[0].Points);
            }
        }
    }
}
