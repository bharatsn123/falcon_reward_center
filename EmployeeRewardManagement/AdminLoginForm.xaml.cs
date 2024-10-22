using System.Linq;
using System.Windows;

namespace EmployeeRewardManagement
{
    public partial class AdminLoginForm : Window
    {
        public AdminLoginForm()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = adminUsernameTextBox.Text;
            string password = adminPasswordTextBox.Password;

            using (var context = new FalconDbContext())
            {
                var admin = context.Admin.FirstOrDefault(a => a.Username == username && a.PasswordHash == HashPassword(password));
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

        private string HashPassword(string password)
        {
            // Simplified for demonstration purposes, apply hashing here
            return password;
        }
    }
}
