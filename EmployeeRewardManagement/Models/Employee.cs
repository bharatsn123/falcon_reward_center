﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EmployeeRewardManagement
{

    public interface Person
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public string Password
        {
            get; set;
        }
    }

    public class Employee : Person
    {
        public int EmployeeID { get; set; } // Employee ID starts with 2
        public string Name { get; set; }
        public string Address { get; set; }
        public string BusinessUnit { get; set; }
        public string JobTitle { get; set; }
        public int Points { get; set; }
        public int? ManagerID { get; set; }
        public string? Subordinates { get; set; }
        public string Password { get; set; } // Password

    }

    public class AwardsGranted
    {
        [Key]
        public int AwardID { get; set; }
        public int EmployeeID { get; set; }
        public int RewardID { get; set; }
        public DateTime GrantedDate { get; set; }
    }

}
