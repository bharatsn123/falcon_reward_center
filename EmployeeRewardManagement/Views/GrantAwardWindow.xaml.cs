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
    public partial class GrantAwardWindow : Window
    {
        private int managerID;

        public GrantAwardWindow(int managerID)
        {
            InitializeComponent();
            this.managerID = managerID;
            LoadEmployees();
            LoadRewards();
        }

        private void LoadEmployees()
        {
            using (var context = new FalconDbContext())
            {
                // Load employees reporting to the current manager
                EmployeeComboBox.ItemsSource = context.Employee
                    .Where(e => e.ManagerID == managerID)
                    .ToList();
            }
        }

        private void LoadRewards()
        {
            using (var context = new FalconDbContext())
            {
                RewardComboBox.ItemsSource = context.Reward.ToList();
            }
        }

        private void GrantAwardButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected employee and reward
            var selectedEmployee = EmployeeComboBox.SelectedItem as Employee;
            var selectedReward = RewardComboBox.SelectedItem as Reward;

            if (selectedEmployee != null && selectedReward != null)
            {
                using (var context = new FalconDbContext())
                {
                    // Update the employee's points
                    var employee = context.Employee.FirstOrDefault(e => e.EmployeeID == selectedEmployee.EmployeeID);
                    if (employee != null)
                    {
                        employee.Points += selectedReward.Points;

                        // Insert a record in the AwardsGranted table
                        var awardRecord = new AwardsGranted
                        {
                            EmployeeID = selectedEmployee.EmployeeID,
                            RewardID = selectedReward.RewardID,
                            GrantedDate = DateTime.Now
                        };

                        context.AwardsGranted.Add(awardRecord);

                        context.SaveChanges();
                        MessageBox.Show($"{selectedReward.RewardName} granted to {employee.Name}!");
                    }
                }
            }
        }

    }
}
