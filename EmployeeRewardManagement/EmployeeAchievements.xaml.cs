using System;
using System.Collections.Generic;
using System.Windows;

namespace EmployeeRewardManagement
{
    public partial class EmployeeAchievements : Window
    {
        private int employeeID;

        // Constructor takes the employeeID to fetch the achievements
        public EmployeeAchievements(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;

            // Call method to load achievements
            LoadEmployeeAchievements();
        }

        private void LoadEmployeeAchievements()
        {
            using (var context = new FalconDbContext())
            {
                // Get the achievements from the database
                List<EmployeeAchievementDTO> achievements = context.GetEmployeeAchievements(employeeID);

                // Bind the achievements list to the DataGrid
                AchievementsDataGrid.ItemsSource = achievements;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the achievements window
        }
    }
}
