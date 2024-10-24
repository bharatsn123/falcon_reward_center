using System;
using System.Collections.Generic;
using System.Windows;

namespace EmployeeRewardManagement
{
    public partial class EmployeeLeaderboard : Window
    {
        public EmployeeLeaderboard()
        {
            InitializeComponent();
            LoadLeaderboard();
        }

        private void LoadLeaderboard()
        {
            using (var context = new FalconDbContext())
            {
                // Get the leaderboard from the database
                List<LeaderboardEntryDTO> leaderboard = context.GetEmployeesLeaderboard();

                // Bind the leaderboard list to the DataGrid
                LeaderboardDataGrid.ItemsSource = leaderboard;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the leaderboard window
        }
    }
}
