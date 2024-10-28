using EmployeeRewardManagement.Data;
using System.Windows;

namespace EmployeeRewardManagement.Data
{
    // Requirement Fulfilled: At least one example of polymorphism which achieves a useful purpose
    // Abstract base class
    public abstract class UserLogin
    {
        protected string Username { get; set; }
        protected string Password { get; set; }

        public UserLogin(string username, string password)
        {
            Username = username;
            Password = password;
        }

        // Abstract method for role-specific login logic
        public abstract void Login();
    }

    // AdminLogin class
    public class AdminLogin : UserLogin
    {
        public AdminLogin(string username, string password) : base(username, password) { }

        public override void Login()
        {
            using (var context = new FalconDbContext())
            {
                var admin = context.Admin.FirstOrDefault(a => a.Username == Username && a.PasswordHash == Password);
                if (admin != null)
                {
                    var adminDashboard = new AdminDashboard();
                    adminDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Admin credentials.");
                }
            }
        }
    }

    // ManagerLogin class
    public class ManagerLogin : UserLogin
    {
        public ManagerLogin(string username, string password) : base(username, password) { }

        public override void Login()
        {
            using (var context = new FalconDbContext())
            {
                int userID = int.TryParse(Username, out userID) ? userID : -1;
                var manager = context.Employee.FirstOrDefault(m => m.EmployeeID == userID && m.Password == Password && m.EmployeeID.ToString().StartsWith("1"));

                if (manager != null)
                {
                    var managerDashboard = new EmployeeUnifiedPortal(userID, manager.Name, manager.Points, true);
                    managerDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Manager credentials.");
                }
            }
        }
    }

    // EmployeeLogin class
    public class EmployeeLogin : UserLogin
    {
        public EmployeeLogin(string username, string password) : base(username, password) { }

        public override void Login()
        {
            using (var context = new FalconDbContext())
            {
                int userID = int.TryParse(Username, out userID) ? userID : -1;
                var employee = context.Employee.FirstOrDefault(e => e.EmployeeID == userID && e.Password == Password && e.EmployeeID.ToString().StartsWith("2"));

                if (employee != null)
                {
                    var employeeDashboard = new EmployeeUnifiedPortal(userID, employee.Name, employee.Points, false);
                    employeeDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Employee credentials.");
                }
            }
        }
    }
}
