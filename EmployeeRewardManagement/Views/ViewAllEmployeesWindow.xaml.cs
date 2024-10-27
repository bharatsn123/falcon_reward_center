using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EmployeeRewardManagement.Data;
using EmployeeRewardManagement.Models;

namespace EmployeeRewardManagement
{
    public partial class ViewAllEmployeesWindow : Window
    {
        private int managerID;

        public ViewAllEmployeesWindow(int managerID)
        {
            InitializeComponent();
            this.managerID = managerID;
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (var context = new FalconDbContext())
            {
                // Fetch employees with the current manager's ID
                var employees = context.Employee
                    .Where(e => e.ManagerID == managerID)
                    .Select(e => new
                    {
                        e.EmployeeID,
                        e.Name,
                        e.BusinessUnit,
                        e.JobTitle,
                        e.Points
                    })
                    .ToList();

                // Bind the data to the DataGrid
                EmployeesDataGrid.ItemsSource = employees;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

