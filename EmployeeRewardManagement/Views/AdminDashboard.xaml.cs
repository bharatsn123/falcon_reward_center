using EmployeeRewardManagement;
using EmployeeRewardManagement.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EmployeeRewardManagement
{
    // Requirement fulfilled: At least two examples of Interface
    // Interface for data loading operations
    public interface IDataLoader
    {
        void LoadEmployees();
        void LoadManagers();
    }

    // Interface for user actions like adding employees and managers
    public interface IUserIdGenerator
    {
        int GenerateEmployeeID();
        int GenerateManagerID();
    }

    public partial class AdminDashboard : Window, IDataLoader, IUserIdGenerator
    {
        public AdminDashboard()
        {
            InitializeComponent();
            LoadEmployees();
            LoadManagers();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FalconDbContext())
            {
                // Retrieve the selected manager's ID, if a manager is selected
                int? managerID = null;
                if (employeeManagerComboBox.SelectedItem is Employee selectedManager && selectedManager.EmployeeID != 0)
                {
                    managerID = selectedManager.EmployeeID;
                }

                // Create a new employee
                var employee = new Employee
                {
                    EmployeeID = GenerateEmployeeID(),
                    Name = employeeNameTextBox.Text,
                    Address = employeeAddressTextBox.Text,
                    BusinessUnit = employeeBusinessUnitTextBox.Text,
                    JobTitle = employeeJobTitleTextBox.Text,
                    ManagerID = managerID,
                    Password = "pass" // default password
                };

                // Add the new employee
                context.Employee.Add(employee);
                context.SaveChanges();

                // Update the manager's Subordinates field
                if (managerID.HasValue)
                {
                    var manager = context.Employee.FirstOrDefault(e => e.EmployeeID == managerID);
                    if (manager != null)
                    {
                        if (string.IsNullOrEmpty(manager.Subordinates))
                        {
                            manager.Subordinates = employee.EmployeeID.ToString();
                        }
                        else
                        {
                            manager.Subordinates += $", {employee.EmployeeID}";
                        }
                        context.SaveChanges();
                    }
                }

                MessageBox.Show("Employee added successfully!");
            }

            LoadEmployees();
            LoadManagers();
        }


        private void AddManagerButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FalconDbContext())
            {
                // Generate a new ID for the manager
                var newManagerID = GenerateManagerID();

                // Find the selected superior manager's ID, if a superior manager is selected
                int? superiorManagerID = null;
                if (managerSuperiorComboBox.SelectedItem is Employee selectedSuperiorManager && selectedSuperiorManager.EmployeeID != 0)
                {
                    superiorManagerID = selectedSuperiorManager.EmployeeID;
                }

                // Add the new manager as an employee
                var manager = new Employee
                {
                    EmployeeID = newManagerID,
                    Name = managerNameTextBox.Text,
                    Address = managerAddressTextBox.Text,
                    BusinessUnit = managerBusinessUnitTextBox.Text,
                    JobTitle = managerJobTitleTextBox.Text,
                    ManagerID = superiorManagerID,
                    Password = "pass" // Default password, replace as needed
                };
                context.Employee.Add(manager);
                context.SaveChanges();

                // If the superior manager exists, update their Subordinates field
                if (superiorManagerID.HasValue)
                {
                    var superiorManager = context.Employee.FirstOrDefault(e => e.EmployeeID == superiorManagerID);
                    if (superiorManager != null)
                    {
                        // Append the new manager's ID to the subordinates field
                        superiorManager.Subordinates = string.IsNullOrEmpty(superiorManager.Subordinates)
                            ? newManagerID.ToString()
                            : $"{superiorManager.Subordinates},{newManagerID}";

                        // Save the updated superior manager record
                        context.SaveChanges();
                    }
                }

                MessageBox.Show("Manager added successfully!");
            }

            // Refresh employee and manager lists
            LoadEmployees();
            LoadManagers();
        }



        public int GenerateEmployeeID()
        {
            using (var context = new FalconDbContext())
            {
                var lastEmployee = context.Employee
                    .Where(e => e.EmployeeID >= 2000 && e.EmployeeID < 3000) // Filters for Employee IDs starting with 2
                    .OrderByDescending(e => e.EmployeeID)
                    .FirstOrDefault();
                return lastEmployee != null ? lastEmployee.EmployeeID + 1 : 2001;
            }
        }

        public int GenerateManagerID()
        {
            using (var context = new FalconDbContext())
            {
                var lastManager = context.Employee
                    .Where(e => e.EmployeeID >= 1000 && e.EmployeeID < 2000) // Filters for Manager IDs starting with 1
                    .OrderByDescending(e => e.EmployeeID)
                    .FirstOrDefault();
                return lastManager != null ? lastManager.EmployeeID + 1 : 1001;
            }
        }

        public void LoadEmployees()
        {
            using (var context = new FalconDbContext())
            {
                employeeDataGrid.ItemsSource = context.Employee
                    .Where(e => e.EmployeeID >= 2000 && e.EmployeeID < 3000)
                    .ToList();
            }
        }

        public void LoadManagers()
        {
            using (var context = new FalconDbContext())
            {

                // Filter EmployeeID starting with 1 (assuming EmployeeID is 4 digits)
                var filteredManagers = context.Employee
                    .Where(e => e.EmployeeID >= 1000 && e.EmployeeID < 2000)
                    .ToList();

                // Add the default "No Superior Manager" option at the beginning
                var managerListWithDefault = new List<Employee> { new Employee { Name = "No Superior Manager", EmployeeID = 0 } };
                managerListWithDefault.AddRange(filteredManagers);

                // Set the data source for managerDataGrid and ComboBox
                managerDataGrid.ItemsSource = filteredManagers;
                employeeManagerComboBox.ItemsSource = filteredManagers;
                employeeManagerComboBox.DisplayMemberPath = "Name";

                managerSuperiorComboBox.ItemsSource = managerListWithDefault;
                managerSuperiorComboBox.DisplayMemberPath = "Name";

            }
        }


        private void employeeNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            employeeNamePlaceholder.Visibility = string.IsNullOrEmpty(employeeNameTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void employeeAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            employeeAddressPlaceholder.Visibility = string.IsNullOrEmpty(employeeAddressTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void employeeBusinessUnitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            employeeBusinessUnitPlaceholder.Visibility = string.IsNullOrEmpty(employeeBusinessUnitTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void employeeJobTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            employeeJobTitlePlaceholder.Visibility = string.IsNullOrEmpty(employeeJobTitleTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void managerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            managerNamePlaceholder.Visibility = string.IsNullOrEmpty(managerNameTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void managerAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            managerAddressPlaceholder.Visibility = string.IsNullOrEmpty(managerAddressTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void managerBusinessUnitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            managerBusinessUnitPlaceholder.Visibility = string.IsNullOrEmpty(managerBusinessUnitTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void managerJobTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            managerJobTitlePlaceholder.Visibility = string.IsNullOrEmpty(managerJobTitleTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out.");
            this.Close(); // Log out and close the admin dashboard
                          
        }
    }
}
