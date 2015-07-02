using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SCZY.Accountant.Dialog;
using SCZY.Common;

namespace SCZY.Accountant
{
    /// <summary>
    /// FinanceManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class FinanceManagePage : Page
    {
        public FinanceManagePage()
        {
            InitializeComponent();
            GetScheduled();//获取数据
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        public void GetScheduled()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<Consumer> orderList = sczy.Consumers.OrderBy(p => p.ID).ToList();
            string para = SeacherBox.Text;
            // PendingOrder.Items.Refresh();
            if (orderList.Count != 0)
            {
                ConsumersGrid.Items.Clear();
                ConsumersGrid.ItemsSource = orderList;  //读取用户并刷新datagrid

                gridPager.ShowPages(ConsumersGrid, orderList, 5);
            }
            Consumer pro = new Consumer();
            orderList = LikeSeacher.SeacherLike(pro, para, "Consumer");
        }

        private void DebtMemory_Click(object sender, RoutedEventArgs e)
          {
            Consumer consumer = ConsumersGrid.SelectedItem as Consumer;
            if (consumer != null)
            {
                // Consumer consumer = (Consumer)sender;
                DebtMemoryDialog debtMemoryDialog = new DebtMemoryDialog(consumer);
                Window window = Window.GetWindow(this);
                debtMemoryDialog.Owner = window;
                debtMemoryDialog.ShowDialog();
            }

        }

        /// <summary>
        /// 还款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Repayment_Click(object sender, RoutedEventArgs e)
        {
            Consumer consumer = ConsumersGrid.SelectedItem as Consumer;
            RepaymentDialog repaymentDialog = new RepaymentDialog("repay",consumer);
            Window window = Window.GetWindow(this);
            repaymentDialog.Owner = window;
            repaymentDialog.ShowDialog();
        }

        private void EditRepayment_Click(object sender, RoutedEventArgs e)
        {
            Consumer consumer = ConsumersGrid.SelectedItem as Consumer;
            RepaymentDialog repaymentDialog = new RepaymentDialog("editRepay", consumer);
            Window window = Window.GetWindow(this);
            repaymentDialog.Owner = window;
            repaymentDialog.ShowDialog();
        }

        /// <summary>
        /// 查找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<Consumer> orderList = new List<Consumer>();

            //获取查找的条件
            string para = SeacherBox.Text;
            if (String.IsNullOrEmpty(para))
            {
                //重新绑定所有数据
                SCZYEntities sczy = new SCZYEntities();
                orderList = sczy.Consumers.OrderBy(p => p.ID).ToList();

                ConsumersGrid.ItemsSource = orderList;
                return;
            }
            Consumer pro = new Consumer();
            orderList = LikeSeacher.SeacherLike(pro, para, "Consumer");

            ConsumersGrid.ItemsSource = orderList;
        }

        /// <summary>
        /// 重置所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //清空条件输入框的字符串
            SeacherBox.Text = null;
            SeacherBox.Focus();

            //重新绑定所有数据
            SCZYEntities sczy = new SCZYEntities();
            List<Consumer> orderList = sczy.Consumers.OrderBy(p => p.ID).ToList();

            ConsumersGrid.ItemsSource = orderList;
        }

        /// <summary>
        /// 绑定回车键查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SeacherBtn_OnMouseLeftButtonUp(null, null);
            }
        }

        private void DebtWarnLoadRow(object sender, DataGridRowEventArgs e)
        {

            //Dialog.MessageBox messageBox = new Dialog.MessageBox("拼命加载中...", "");
            //messageBox.Show();

            double totalDebt = 0;
            SCZYEntities sczy = new SCZYEntities();

            //for (int i = 0; i < ConsumersGrid.Items.Count;i++)
            //{              
                Consumer consumer = e.Row.Item as Consumer;
                
                //var row = ConsumersGrid.Items[i] as DataGridRow;
                List<Order> orders = new List<Order>();
                orders = sczy.Orders.Where(p => p.Consumer_ID == consumer.ID && p.OrderState == "已划价").ToList();
                foreach (var order in orders)
                {
                    if (order.AggregateAmount != null && order.Paid != null)
                    {
                        totalDebt += (double)(order.AggregateAmount - order.Paid);

                    }

                }
                for (int j = 0; j < orders.Count; j++)
                {
                        Order order = orders[j] as Order;
                        DateTime nowTime = DateTime.Now;
                        //DateTime orderDateTime = null;
                        string sql =
                            "SELECT * FROM OrderMoneyDetails WHERE [Date] = (SELECT MAX([Date]) FROM OrderMoneyDetails WHERE  Order_ID = " +
                            order.ID + " GROUP BY Order_ID )";
                        // OrderMoneyDetail orderMoneyDetail = sczy.OrderMoneyDetails.SqlQuery(sql) as OrderMoneyDetail;
                        OrderMoneyDetail orderMoneyDetail = sczy.OrderMoneyDetails.SqlQuery(sql).FirstOrDefault();
                        // DateTime? orderDateTime = sczy.OrderMoneyDetails.Where(p => p.Order_ID == order.ID).Max(p => p.Date);
                        // DateTime datys = new DateTime(0,0,30);

                        //换过款，超过一个月没有还款的用户
                        if (orderMoneyDetail != null)
                        {
                            if (orderMoneyDetail.Date != null)
                            {
                                DateTime tiemDateTime = (DateTime)orderMoneyDetail.Date;
                                //orderDateTime = (DateTime) orderDateTime;
                                TimeSpan ts1 = new TimeSpan(nowTime.Ticks);
                                TimeSpan ts2 = new TimeSpan(tiemDateTime.Ticks);
                                TimeSpan ts3 = ts1.Subtract(ts2).Duration();

                                if (ts3.Days > 30 && totalDebt > 100)
                                {
                                    //  e.Row.BorderThickness = new Thickness(1,1,1,0);
                                    e.Row.Foreground = new SolidColorBrush(Colors.Red);
                                    return;
                                } 
                            }

                        }      
                }

                
                //应对钉子客户，恩。不还款那种
                foreach (var order in orders)
                {
                    if (orders.Count != 0)
                    {
                        int j = 0;
                        for (; j < orders.Count; j++)
                        {
                            List<OrderMoneyDetail> orderMoneyDetails = new List<OrderMoneyDetail>();
                            orderMoneyDetails = sczy.OrderMoneyDetails.Where(p => p.Order_ID == order.ID).ToList();
                            if (orderMoneyDetails.Count != 0)
                            {
                                break;
                            }
                        }
                        if (j >= orders.Count)
                        {
                            e.Row.Foreground = new SolidColorBrush(Colors.Yellow);
                            return;
                        }
                    }
                   
                }
                //messageBox.Close();
          
        }
    }
}
