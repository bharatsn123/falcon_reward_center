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

namespace EmployeeRewardManagement
{
    public partial class AwardCatalogWindow : Window
    {
        public AwardCatalogWindow()
        {
            InitializeComponent();
            LoadRewards();
        }

        private void LoadRewards()
        {
            using (var context = new FalconDbContext())
            {
                // Fetch all rewards and set them as the DataGrid's data source
                var rewards = context.Reward.ToList();
                RewardsDataGrid.ItemsSource = rewards;
            }
        }
    }
}

