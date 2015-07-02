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
using SCZY.Common;

namespace SCZY.Accountant.Dialog
{
    /// <summary>
    /// DebtMemoryDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DebtMemoryDialog : Window
    {

        public Consumer consumer;

        public DebtMemoryDialog(object consumerObject)
        {

            consumer = (Consumer) consumerObject;
            InitializeComponent();           
            Loaded += (sender, args) =>
            {
                SetTitle_Size();
            };
            GetScheduled();//获取数据

            TotalDebt();
        }



        /// <summary>
        /// 设置弹出框的标题
        /// 设置弹出框的大小
        /// </summary>
        public void SetTitle_Size()
        {
            TextBlock windowTitle = this.Template.FindName("WindowTitle", this) as TextBlock;
            windowTitle.Text = Title;

            Grid temGrid = this.Template.FindName("TemGrid", this) as Grid;
            temGrid.Width = Width;
            temGrid.Height = Height;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        public void GetScheduled()
        {

            double total = 0;
            SCZYEntities sczy = new SCZYEntities();
            List<ViewOrder> viewOrdersList = new List<ViewOrder>(); 
            List<Order> orderList = sczy.Orders.Where(h => h.Consumer_ID == consumer.ID).OrderBy(p => p.TakeDate).ToList();

            foreach (Order order in orderList)
            {
                
                ViewOrder viewOrder = new ViewOrder();
                GetOrderAmount gerGetOrderAmount = new GetOrderAmount(order);
                total += (double)gerGetOrderAmount.GetAmount();
                //防止Paid为空
                if (order.Paid != null)
                {
                    viewOrder.Leaving = DataChange.CutDouble((double) (total - order.Paid));
                    viewOrder.Paid = (float) order.Paid;
                }

                if (order.Paid == null)
                {
                    float a = 0;
                    viewOrder.Leaving = DataChange.CutDouble(total);
                    viewOrder.Paid = a;
                }              
                viewOrder.ID = order.ID;
                viewOrder.ConsumerName = order.ConsumerName;
                viewOrder.OrderDate = order.OrderDate;
                viewOrder.DeliveryDate = order.DeliveryDate;
                
                viewOrder.Amount = total;

                viewOrdersList.Add(viewOrder);

            }

            
            // PendingOrder.Items.Refresh();
            OrderListView.Items.Clear();
            OrderListView.ItemsSource = viewOrdersList.OrderBy(p => p.ID);  //读取用户并刷新datagrid

        }

        /// <summary>
        /// 计算总的欠款
        /// </summary>
        private void TotalDebt()
        {
            double debt = 0;
            for (int i = 0; i < OrderListView.Items.Count; i++)
            {                
                ViewOrder producntView = OrderListView.Items[i] as ViewOrder;
                if (producntView.Leaving != null)
                {
                    debt += producntView.Leaving;
                }
            }
            TotalMoneyBox.Text = debt.ToString();
        }

        private void SaveWindowClick(object sender, RoutedEventArgs e)
        {

        }

        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Debt_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void Memory_OnClick(object sender, RoutedEventArgs e)
        {
            Order order = OrderListView.SelectedItem as Order;
            // Consumer consumer = (Consumer)sender;
            DebtMemoryRecordDialog debtMemoryRecordDialog = new DebtMemoryRecordDialog(order);
            Window window = Window.GetWindow(this);
            debtMemoryRecordDialog.Owner = window;
            debtMemoryRecordDialog.ShowDialog();
        }

        private void ProductInfo_OnClick(object sender, RoutedEventArgs e)
        {
            Order order = OrderListView.SelectedItem as Order;
            // Consumer consumer = (Consumer)sender;
            DebtMemoryProductDialog debtMemoryDialog = new DebtMemoryProductDialog(order);
            Window window = Window.GetWindow(this);
            debtMemoryDialog.Owner = window;
            debtMemoryDialog.ShowDialog();
        }
    }
}
