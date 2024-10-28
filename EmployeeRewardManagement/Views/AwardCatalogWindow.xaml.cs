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
using LiveCharts;
using LiveCharts.Wpf;

namespace EmployeeRewardManagement
{
    public partial class AwardCatalogWindow : Window
    {
        // Chart data properties
        public SeriesCollection PointsChartValues { get; set; }
        public List<string> AwardsLabels { get; set; }

        public AwardCatalogWindow()
        {
            InitializeComponent();
            LoadRewards();

            // Set DataContext for binding
            DataContext = this;
        }

        private void LoadRewards()
        {
            using (var context = new FalconDbContext())
            {
                var rewards = context.Reward.ToList();
                RewardsDataGrid.ItemsSource = rewards;

                // Populate chart data
                PointsChartValues = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Points",
                        Values = new ChartValues<int>(rewards.Select(r => r.Points))
                    }
                };

                AwardsLabels = rewards.Select(r => r.RewardName).ToList();
            }
        }
    }
}
