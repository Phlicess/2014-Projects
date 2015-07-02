using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using SCZY.Common;

namespace SCZY.Accountant
{
    /// <summary>
    /// OrderPrecedPage.xaml 的交互逻辑
    /// </summary>
    public partial class OrderPrecedPage : Page
    {
        public OrderPrecedPage()
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
            List<Order> orderList = sczy.Orders.Where(h => h.OrderState == "已划价").OrderBy(p => p.TakeDate).ToList();

            // PendingOrder.Items.Refresh();
            if (orderList.Count != 0)
            {
                PendingOrder.Items.Clear();
                PendingOrder.ItemsSource = orderList;  //读取用户并刷新datagrid
                gridPager.ShowPages(PendingOrder, orderList, 5);
                
            }

           
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            Order orderInfo = PendingOrder.SelectedItem as Order;
            if (orderInfo == null)
            {
                return;
            }
            Order selectedOrder = PendingOrder.SelectedCells[0].Item as Order;

            Window editOrderWindow = new FixPriceDialog(selectedOrder);
            Window window = Window.GetWindow(this);
            editOrderWindow.Owner = window;
            editOrderWindow.ShowDialog();

            //SCZYEntities sczy = new SCZYEntities();
            //Order newOrder = new Order();//修改后，查看是否已经划价
            //newOrder = sczy.Orders.FirstOrDefault(p => p.ID == orderInfo.ID);
            //if (newOrder.OrderState == "已划价")
            //{
            //    List<Order> orderList = sczy.Orders.Where(h => h.OrderState == "未划价").OrderBy(p => p.TakeDate).ToList();
            //    PendingOrder.ItemsSource = orderList;  //读取用户并刷新datagrid
            //}
            ////  GetScheduled(); //数据可能改变,重新获取datagril 的数据
        }

        private void PrintOrder(object sender, RoutedEventArgs e)
        {
            Order orderInfo = PendingOrder.SelectedItem as Order;
            if (orderInfo.OrderState == "已划价")
            {
                Thread thread = new Thread(new ParameterizedThreadStart(ThreadPrinter));
                thread.Start(orderInfo);

            }
            else if (orderInfo.OrderState == "未划价")
            {
                Dialog.MessageBox messageBox = new Dialog.MessageBox();
                messageBox.Show();
            }
        }


        /// <summary>
        /// 以线程的方式开启打印
        /// </summary>
        private void ThreadPrinter(object order)
        {
            Order orderInfo = (Order)order;
            Printer printer = new Printer("Order");
            printer.GetOrderData(orderInfo);

            printer.PrintOrders();
        }

        /// <summary>
        /// 查找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<Order> orderList = new List<Order>();

            //获取查找的条件
            string para = SeacherBox.Text;
            if (String.IsNullOrEmpty(para))
            {
                //重新绑定所有数据
                SCZYEntities sczy = new SCZYEntities();
                orderList = sczy.Orders.Where(h => h.OrderState == "已划价").OrderBy(p => p.TakeDate).ToList();

                PendingOrder.ItemsSource = orderList;
                return;
            }
            Order pro = new Order();
            orderList = LikeSeacher.SeacherLike(pro, para,"已划价", "Order");

            PendingOrder.ItemsSource = orderList;
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
            List<Order> orderList = sczy.Orders.Where(h => h.OrderState == "已划价").OrderBy(p => p.TakeDate).ToList();

            PendingOrder.ItemsSource = orderList;
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
    }
}
