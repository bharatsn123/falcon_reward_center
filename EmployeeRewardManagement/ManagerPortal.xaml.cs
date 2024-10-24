using System;
using System.Windows;
using System.Windows.Threading;

namespace EmployeeRewardManagement
{
    public partial class ManagerPortal : Window
    {
        public int RewardPoints { get; set; }
        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }

        private DispatcherTimer timer;

        public ManagerPortal(int rewardPoints)
        {
            InitializeComponent();

            // Set reward points
            RewardPoints = rewardPoints;

            // Set the current date
            CurrentDate = DateTime.Now.ToString("d MMMM yyyy");

            // Start a timer to update the time
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Bind the data context for live updates
            DataContext = this;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the current time every second
            CurrentTime = DateTime.Now.ToString("HH:mm");
            DataContext = null;
            DataContext = this; // Refresh data binding
        }

        // Button click events
        private void ViewAchievements_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Your Achievements button clicked!");
            // Logic to navigate to Achievements page
        }

        private void VisitRewardStore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Visit Reward Store button clicked!");
            // Logic to navigate to Reward Store page
        }

        private void ViewLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Leaderboard button clicked!");
            // Logic to navigate to Leaderboard page
        }

        private void ViewAwardCatalog_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Award Catalog button clicked!");
            // Logic to navigate to Award Catalog page
        }

        private void ViewAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View All Employees button clicked!");
            // Logic to navigate to View All Employees page
        }

        private void GrantAward_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Grant Award button clicked!");
            // Logic to navigate to Grant Award page
        }
    }
}
