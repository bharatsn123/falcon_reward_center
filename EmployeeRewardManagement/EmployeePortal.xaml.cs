using System;
using System.Windows;
using System.Windows.Threading;

namespace EmployeeRewardManagement
{
    public partial class EmployeePortal : Window
    {
        public int RewardPoints { get; set; }
        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }

        private DispatcherTimer timer;

        public EmployeePortal(int rewardPoints)
        {
            InitializeComponent();

            // Set the reward points
            RewardPoints = rewardPoints;

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
            MessageBox.Show("View Achievements button clicked!");
            // Logic to navigate to achievements page
        }

        private void VisitRewardStore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Visit Reward Store button clicked!");
            // Logic to navigate to reward store page
        }

        private void ViewLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Leaderboard button clicked!");
            // Logic to navigate to leaderboard page
        }

        private void ViewAwardCatalog_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Award Catalog button clicked!");
            // Logic to navigate to award catalog page
        }
    }
}
