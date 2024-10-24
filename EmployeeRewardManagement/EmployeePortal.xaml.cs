using System;
using System.Windows;
using System.Windows.Threading;

namespace EmployeeRewardManagement
{
    public partial class EmployeePortal : Window
    {
        public int RewardPoints { get; set; }

        public string EmployeeName { get; set; }

        public int EmployeeId { get; set; }
        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }

        private DispatcherTimer timer;

        public EmployeePortal(int employeeId, string name, int points)
        {
            InitializeComponent();

            // Set the reward points
            RewardPoints = points;

            EmployeeName = name;

            EmployeeId = employeeId;

            // Start a timer to update the time
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Set the current date
            CurrentDate = DateTime.Now.ToString("d MMMM yyyy");

            // Bind data context for live updates
            DataContext = this;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the current time every second
            CurrentTime = DateTime.Now.ToString("HH:mm");
            DataContext = null;
            DataContext = this;  // Refresh data binding
        }

        // Button click events
        private void ViewAchievements_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("View Achievements button clicked!");
            // Logic to navigate to achievements page

            var employeeAchievementsWindow = new EmployeeAchievements(EmployeeId);
            employeeAchievementsWindow.Show();
        }

        private void VisitRewardStore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Visit Reward Store button clicked!");
            // Logic to navigate to reward store page
        }

        private void ViewLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            var leaderboardWindow = new EmployeeLeaderboard();
            leaderboardWindow.Show();
        }

        private void ViewAwardCatalog_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Award Catalog button clicked!");
            // Logic to navigate to award catalog page
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Close(); // Close the manager portal and log out
            // You can navigate to the login page or close the app
        }
    }
}
