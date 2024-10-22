using EmployeeRewardManagement;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EmployeeRewardManagement
{
    public partial class AdminDashboard : Window
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
                var employee = new Employee
                {
                    EmployeeID = GenerateEmployeeID(),
                    Name = employeeNameTextBox.Text,
                    Address = employeeAddressTextBox.Text,
                    BusinessUnit = employeeBusinessUnitTextBox.Text,
                    JobTitle = employeeJobTitleTextBox.Text,
                    ManagerID = context.Employee
                                        .Where(e => e.Name == employeeManagerComboBox.SelectedValue.ToString())
                                        .Select(e => e.ManagerID)
                                        .FirstOrDefault()
                };

                context.Employee.Add(employee);
                context.SaveChanges();
                MessageBox.Show("Employee added successfully!");
            }

            LoadEmployees();
        }

        private void AddManagerButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FalconDbContext())
            {
                var manager = new Employee
                {
                    EmployeeID = GenerateManagerID(),
                    Name = managerNameTextBox.Text,
                    Address = managerAddressTextBox.Text,
                    BusinessUnit = managerBusinessUnitTextBox.Text,
                    JobTitle = managerJobTitleTextBox.Text,
                    ManagerID = context.Employee
                                        .Where(e => e.Name == managerSuperiorComboBox.SelectedValue.ToString())
                                        .Select(e => e.ManagerID)
                                        .FirstOrDefault()
                };

                context.Employee.Add(manager);
                context.SaveChanges();
                MessageBox.Show("Manager added successfully!");
            }

            LoadManagers();
        }

        private int GenerateEmployeeID()
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

        private int GenerateManagerID()
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

        private void LoadEmployees()
        {
            using (var context = new FalconDbContext())
            {
                employeeDataGrid.ItemsSource = context.Employee
                    .Where(e => e.EmployeeID >= 2000 && e.EmployeeID < 3000)
                    .ToList();
            }
        }

        private void LoadManagers()
        {
            using (var context = new FalconDbContext())
            {

                // Filter EmployeeID starting with 1 (assuming EmployeeID is 4 digits)
                var filteredManagers = context.Employee
                    .Where(e => e.EmployeeID >= 1000 && e.EmployeeID < 2000)
                    .ToList();

                // Set the data source for managerDataGrid
                managerDataGrid.ItemsSource = filteredManagers;
                employeeManagerComboBox.ItemsSource = filteredManagers;
                employeeManagerComboBox.DisplayMemberPath = "Name";
                managerSuperiorComboBox.ItemsSource = filteredManagers;
                managerSuperiorComboBox.DisplayMemberPath = "Name";
            }
        }

        //private void employeeManagerComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    Color selectedColor = (employeeManagerComboBox.SelectedItem as PropertyInfo).GetValue(null, null);
        //}

        //private void managerSuperiorComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    Color selectedColor = (Color)(cmbColors.SelectedItem as PropertyInfo).GetValue(null, null);
        //    this.Background = new SolidColorBrush(selectedColor);
        //}

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

    }
}
