using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeRewardManagement.Models
{
    public class AwardsGranted
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AwardID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public int RewardID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime GrantedDate { get; set; }
    }
}
