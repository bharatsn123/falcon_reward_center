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
using EmployeeRewardManagement.Models;
using EmployeeRewardManagement.Data;
using System.ComponentModel;

namespace EmployeeRewardManagement
{
    public partial class RewardStoreWindow : Window, INotifyPropertyChanged
    {
        private int employeeID;
        private int _employeePoints;

        // Public property for the window bindings to work
        public int EmployeePoints
        {
            get { return _employeePoints; }
            set
            {
                if (_employeePoints != value)
                {
                    _employeePoints = value;
                    OnPropertyChanged(nameof(EmployeePoints));
                }
            }
        }

        // Event handling for any change in employee points
        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<int> PointsUpdated;

        public RewardStoreWindow(int employeeID, int employeePoints)
        {
            InitializeComponent();
            DataContext = this; // Set the DataContext so bindings work
            this.employeeID = employeeID;
            this._employeePoints = employeePoints;
            LoadRewards();
        }

        private void LoadRewards()
        {
            using (var context = new FalconDbContext())
            {
                // Fetch rewards from the RewardStore table
                var rewards = context.RewardStore.ToList();

                // Process each reward to include a BitmapImage based on ImgUrl
                foreach (var reward in rewards)
                {
                    if (!string.IsNullOrEmpty(reward.ImgUrl))
                    {
                        try
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(reward.ImgUrl, UriKind.Absolute);
                            bitmap.EndInit();

                            // Assign the BitmapImage to the ImageSource property
                            reward.ImageSource = bitmap;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to load image for {reward.ItemName}: {ex.Message}");
                        }
                    }
                }

                // Set the processed rewards list as the ItemsSource for the ListView
                RewardStoreListView.ItemsSource = rewards;
            }
        }

        private void RedeemButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected reward
            var selectedReward = RewardStoreListView.SelectedItem as RewardStore;
            if (selectedReward == null)
            {
                MessageBox.Show("Please select a reward to redeem.");
                return;
            }

            if (_employeePoints >= selectedReward.RequiredPoints)
            {
                // Deduct points and save the transaction
                RedeemReward(selectedReward);
            }
            else
            {
                MessageBox.Show("Insufficient points to redeem this reward.");
            }
        }

        private void RedeemReward(RewardStore selectedReward)
        {
            using (var context = new FalconDbContext())
            {
                // Fetch the employee from the database
                var employee = context.Employee.FirstOrDefault(e => e.EmployeeID == employeeID);

                if (employee != null)
                {
                    // Check if the employee has enough points
                    if (employee.Points >= selectedReward.RequiredPoints)
                    {
                        // Deduct points from the employee's total
                        employee.Points -= selectedReward.RequiredPoints;

                        // Create a transaction record with RewardStore.StoreItemID as ItemID
                        var transaction = new Transaction
                        {
                            EmployeeID = employeeID,
                            ItemID = selectedReward.StoreItemID,  // Use ItemID for transactions
                            TransactionDate = DateTime.Now
                        };

                        // Add the transaction and update employee points in the database
                        context.Transaction.Add(transaction);
                        context.SaveChanges();

                        // Update EmployeePoints directly to reflect the new value
                        EmployeePoints = employee.Points;

                        PointsUpdated?.Invoke(employee.Points); // Trigger the event with updated points

                        MessageBox.Show($"You have successfully redeemed: {selectedReward.ItemName}");
                    }
                    else
                    {
                        MessageBox.Show("Insufficient points to redeem this reward.");
                    }
                }
                else
                {
                    MessageBox.Show("Employee not found.");
                }
            }
        }


        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var historyWindow = new TransactionHistoryWindow(employeeID);
            historyWindow.ShowDialog();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
