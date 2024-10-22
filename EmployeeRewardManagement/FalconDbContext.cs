using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRewardManagement
{
    public class FalconDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Reward> Reward { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<RewardStore> RewardStore { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql("Server=170.64.170.2;Database=falcon_reward_center;User=root;Password=C#1Project;" +
              //                 "SslMode=Required;SslCa=C:\\Users\\bhara\\Downloads\\ca_cert.pem;",
                //            new MySqlServerVersion(new Version(8, 0, 39))); // Replace with your MySQL version


            optionsBuilder.UseMySql("Server=170.64.170.2;Database=falcon_reward_center;User=root;Password=C#1Project;" +
                               "AllowPublicKeyRetrieval=True;SslMode =none;",
                            new MySqlServerVersion(new Version(8, 0, 39))); // Replace with your MySQL version



        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<RewardStore>()
                .HasKey(r => r.StoreItemID);  // Define StoreItemID as the primary key

        //modelBuilder.Entity<Manager>()
        //        .HasMany(m => m.Employees)
        //        .WithOne(e => e.Manager)
        //        .HasForeignKey(e => e.ManagerID);

        //    modelBuilder.Entity<Manager>()
        //        .HasOne(m => m.SuperiorManager)
        //        .WithMany()
        //        .HasForeignKey(m => m.SuperiorManagerID);
        }
    }

}
