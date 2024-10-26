using System;
using System.Windows;
using System.Windows.Threading;
using EmployeeRewardManagement.Data;

namespace EmployeeRewardManagement
{
    public partial class ManagerPortal : Window
    {
        public string EmployeeName { get; set; } // To store the employee name
        public int RewardPoints { get; set; }
        public int EmployeeId { get; set; }
        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }

        private DispatcherTimer timer;

        public ManagerPortal(int employeeId, string name, int points)
        {
            InitializeComponent();

            this.EmployeeName = name;

            this.EmployeeId = employeeId;
            // Set reward points
            RewardPoints = points;

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
            // Logic to navigate to Reward Store page
        }

        private void ViewLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            var leaderboardWindow = new EmployeeLeaderboard();
            leaderboardWindow.Show();
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

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Close(); // Close the manager portal and log out
            // You can navigate to the login page or close the app
        }

       
    }
}
