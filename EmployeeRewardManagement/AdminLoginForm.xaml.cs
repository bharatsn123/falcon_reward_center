using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeRewardManagement
{
    public partial class AdminLoginForm : Window
    {
        private String selectedRole = "";
        public AdminLoginForm()
        {
            InitializeComponent();
            // Set "Employee" as the default selected role programmatically
            roleComboBox.SelectedIndex = 2;
        }

        private string HashPassword(string password)
        {
            // Simplified for demonstration purposes, apply hashing here
            return password;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Password;
            ComboBoxItem selectedRoleItem = (ComboBoxItem)roleComboBox.SelectedItem;
            selectedRole = selectedRoleItem.Content.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            switch (selectedRole)
            {
                case "Admin":
                    AdminLogin(username, password);
                    break;
                case "Manager":
                    EmployeeManagerLogin(username, password);
                    break;
                case "Employee":
                    EmployeeManagerLogin(username, password);
                    break;
                default:
                    MessageBox.Show("Please select a role.");
                    break;
            }

            
        }



        private void AdminLogin(string username, string password)
        {

            using (var context = new FalconDbContext())
            {
                var admin = context.Admin.FirstOrDefault(a => a.Username == username && a.PasswordHash == password);
                if (admin != null)
                {
                    var adminDashboard = new AdminDashboard();
                    adminDashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid login credentials.");
                }
            }

        }

        private void EmployeeManagerLogin(string username, string password)
        {
            using (var context = new FalconDbContext())
            {
                // Convert the username to a number to handle validation
                long userID;
                if (!long.TryParse(username, out userID))
                {
                    MessageBox.Show("Invalid username format. Are you trying to login as Admin role?");
                    return;
                }

                // Check if the ID is a Manager ID (starts with 1) or Employee ID (starts with 2)
                bool isManagerID = username.StartsWith("1");
                bool isEmployeeID = username.StartsWith("2");

                // Validate the selected role
                if (selectedRole == "Manager" && isEmployeeID)
                {
                    MessageBox.Show("You cannot log in as a Manager with an Employee ID.");
                    return;
                }
                if (selectedRole == "Employee" && isManagerID)
                {
                    MessageBox.Show("You cannot log in as an Employee with a Manager ID.");
                    return;
                }

                // Now proceed with checking the login credentials
                if (selectedRole == "Employee")
                {
                    var employee = context.Employee.FirstOrDefault(e => e.EmployeeID == userID && e.Password == password);
                    if (employee != null)
                    {
                        MessageBox.Show("Employee login successful!");
                        // Open Employee dashboard
                    }
                    else
                    {
                        MessageBox.Show("Invalid Employee credentials.");
                    }
                }
                else if (selectedRole == "Manager")
                {
                    var manager = context.Employee.FirstOrDefault(m => m.ManagerID == userID && m.Password == password);
                    if (manager != null)
                    {
                        MessageBox.Show("Manager login successful!");
                        // Open Manager dashboard
                    }
                    else
                    {
                        MessageBox.Show("Invalid Manager credentials.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid role selected.");
                }
            }
        }


    }
}
