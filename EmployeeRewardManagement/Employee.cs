using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRewardManagement
{
    public class Employee
    {
        public int EmployeeID { get; set; } // Employee ID starts with 2
        public string Name { get; set; }
        public string Address { get; set; }
        public string BusinessUnit { get; set; }
        public string JobTitle { get; set; }
        public int Points { get; set; }
        public int? ManagerID { get; set; } // Foreign key to Manager
        public string? Subordinates { get; set; } // Foreign key to Manager
        //public Manager Manager { get; set; } // Navigation property
    }

}
