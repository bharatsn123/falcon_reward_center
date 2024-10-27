using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace EmployeeRewardManagement
{
    public partial class EmployeeUnifiedPortal : Window, INotifyPropertyChanged
    {
        private int rewardPoints;
        public int RewardPoints
        {
            get => rewardPoints;
            set
            {
                rewardPoints = value;
                OnPropertyChanged();
            }
        }

        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public bool IsManager { get; set; } // New property to determine if user is a manager
        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }

        private DispatcherTimer timer;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EmployeeUnifiedPortal(int employeeId, string name, int points, bool isManager)
        {
            InitializeComponent();

            EmployeeId = employeeId;
            EmployeeName = name;
            RewardPoints = points;
            IsManager = isManager; // Set manager status

            // Set the window title based on role
            Title = IsManager ? "Manager Portal" : "Employee Portal";

            // Initialize date and time
            CurrentDate = DateTime.Now.ToString("d MMMM yyyy");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Bind data context for live updates
            DataContext = this;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("HH:mm");
            OnPropertyChanged(nameof(CurrentTime)); // Update binding for current time
        }

        private void ViewAchievements_Click(object sender, RoutedEventArgs e)
        {
            var achievementsWindow = new EmployeeAchievements(EmployeeId);
            achievementsWindow.Show();
        }

        private void VisitRewardStore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rewardStoreControl = new RewardStoreWindow(EmployeeId, RewardPoints);
                rewardStoreControl.PointsUpdated += OnPointsUpdated;
                ContentFrame.Content = rewardStoreControl;
                BackButton.Visibility = Visibility.Visible;
                MainPortalGrid.Visibility = Visibility.Collapsed;
                RewardStoreGrid.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void OnPointsUpdated(int updatedPoints)
        {
            RewardPoints = updatedPoints;
            OnPropertyChanged(nameof(RewardPoints));
        }

        private void ViewLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            var leaderboardWindow = new EmployeeLeaderboard();
            leaderboardWindow.Show();
        }

        private void ViewAwardCatalog_Click(object sender, RoutedEventArgs e)
        {
            var awardCatalogWindow = new AwardCatalogWindow();
            awardCatalogWindow.ShowDialog();
        }

        private void ViewAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            if (IsManager)
            {
                MessageBox.Show("View All Employees button clicked!");
            }
        }

        private void GrantAward_Click(object sender, RoutedEventArgs e)
        {
            if (IsManager)
            {
                var grantAwardWindow = new GrantAwardWindow(EmployeeId);  // Pass current manager ID
                grantAwardWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Only managers can grant awards.");
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = null;
            MainPortalGrid.Visibility = Visibility.Visible;
            RewardStoreGrid.Visibility = Visibility.Collapsed;
        }
    }

}
