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
        public string Password { get; set; } // Password

    }

    public class Manager
    {
        public int ManagerID { get; set; } // Manager ID starts with 1
        public string Name { get; set; }
        public string Address { get; set; }
        public string BusinessUnit { get; set; }
        public string JobTitle { get; set; }
        public int Points { get; set; }
        public int? SuperiorManagerID { get; set; } // Self-referencing foreign key
        public Manager SuperiorManager { get; set; } // Navigation property
        public ICollection<Employee> Employees { get; set; } // Navigation property to Employees
    }

}
