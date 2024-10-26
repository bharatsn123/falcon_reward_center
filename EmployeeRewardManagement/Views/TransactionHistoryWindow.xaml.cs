using Microsoft.EntityFrameworkCore;
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
    public partial class TransactionHistoryWindow : Window
    {
        private int employeeID;

        public TransactionHistoryWindow(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
            LoadTransactionHistory();
        }

        private void LoadTransactionHistory()
        {
            using (var context = new FalconDbContext())
            {
                var transactions = context.Transaction
                    .Where(t => t.EmployeeID == employeeID)
                    .Include(t => t.RewardStoreItem)
                    .ToList();

                TransactionListView.ItemsSource = transactions;
            }
        }
    }
}
