using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
        public string ImgUrl { get; set; }

        // Property to hold the processed BitmapImage for display
        [NotMapped] // Ignore this property in the database schema
        public ImageSource ImageSource { get; set; }

    }

    public class Transaction
    {
        public int TransactionID { get; set; }
        public int EmployeeID { get; set; }
        public int ItemID { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public Employee Employee { get; set; }
        public RewardStore RewardStoreItem { get; set; }
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

