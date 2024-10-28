using EmployeeRewardManagement.Data;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeRewardManagement
{
    public partial class AdminLoginForm : Window
    {
        private string selectedRole = "";

        public AdminLoginForm()
        {
            InitializeComponent();
            roleComboBox.SelectedIndex = 2; // Set "Employee" as default
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

            UserLogin userLogin;

            switch (selectedRole)
            {
                case "Admin":
                    userLogin = new AdminLogin(username, password);
                    break;
                case "Manager":
                    userLogin = new ManagerLogin(username, password);
                    break;
                case "Employee":
                    userLogin = new EmployeeLogin(username, password);
                    break;
                default:
                    MessageBox.Show("Please select a valid role.");
                    return;
            }

            userLogin.Login(); // Polymorphic call
            this.Close(); // Close the login form after login attempt
        }
    }
}
