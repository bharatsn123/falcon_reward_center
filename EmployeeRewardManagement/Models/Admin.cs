using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRewardManagement.Models
{
    public class Admin
    {
        public int AdminID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
