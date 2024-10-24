using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRewardManagement
{
    public class Reward
    {
        public int RewardID { get; set; }
        public string RewardName { get; set; }
        public int Points { get; set; }
    }

    public class RewardStore
    {
        public int StoreItemID { get; set; }
        public string ItemName { get; set; }
        public int RequiredPoints { get; set; }
    }

    public class Transaction
    {
        public int TransactionID { get; set; }
        public int ManagerID { get; set; }
        public int EmployeeID { get; set; }
        public int RewardID { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public Employee Manager { get; set; }
        public Employee Employee { get; set; }
        public Reward Reward { get; set; }
    }

    public class EmployeeAchievementDTO
    {
        public int TransactionID { get; set; }
        public string RewardName { get; set; }
        public int Points { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class LeaderboardEntryDTO
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public string BusinessUnit { get; set; }
        public string JobTitle { get; set; }
        public int Points { get; set; }
    }
}

