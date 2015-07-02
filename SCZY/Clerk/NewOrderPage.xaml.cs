using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SCZY.Clerk.OrderDialog;
using SCZY.Clerk.OrderDialog.Order_Product;
using SCZY.Common;
using SCZY.OtherWindows;

namespace SCZY
{
    /// <summary>
    /// NewOrderPage.xaml 的交互逻辑
    /// </summary>
    public partial class NewOrderPage : Page
    {
        public NewOrderPage()
        {
            InitializeComponent();
            GetScheduled();
        }

        public void GetScheduled()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<Order> orderList = sczy.Orders.Where(o => o.OrderState == "未划价").ToList();
            PendingOrder.Items.Clear();
            PendingOrder.ItemsSource = orderList;  //读取用户并刷新datagrid

            gridPager.ShowPages(PendingOrder, orderList, 5);
        }
        
        /// <summary>
        /// DataGrid右键菜单事件--修改订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditItem_OnClick(object sender, RoutedEventArgs e)
        {
            Order orderInfo = PendingOrder.SelectedItem as Order;
            if (orderInfo == null)
            {
                return;
            }
            Order selectedOrder = PendingOrder.SelectedCells[0].Item as Order;

            EditOrder editOrderWindow = new EditOrder(selectedOrder);
            editOrderWindow.type = "Edit";
            editOrderWindow.SaveOrder += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<Order> orderList = sczy.Orders.Where(o => o.OrderState == "未划价").ToList();
                PendingOrder.ItemsSource = orderList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editOrderWindow.Owner = window;
            editOrderWindow.ShowDialog();
            
        }

        /// <summary>
        /// DataGrid右键菜单事件--添加订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            //Order selectedOrder = PendingOrder.SelectedCells[0].Item as Order;
            Order selectedOrder = new Order();
            EditOrder editOrderWindow = new EditOrder(selectedOrder);
            editOrderWindow.type = "Add";
            editOrderWindow.Title = "添加产品信息";
            editOrderWindow.SaveOrder += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<Order> orderList = sczy.Orders.Where(o => o.OrderState == "未划价").ToList();
                PendingOrder.ItemsSource = orderList;  //读取用户并刷新datagrid
            }; ;
            Window window = Window.GetWindow(this);
            editOrderWindow.Owner = window;
            editOrderWindow.ShowDialog();
        }

        /// <summary>
        /// DataGrid右键菜单事件--删除订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            Order order = PendingOrder.SelectedItem as Order;
            if (order == null)
            {
                return;
            }

            //弹出警告框
            WanningWindow wanningWindow = new WanningWindow();
            Window window = Window.GetWindow(this);
            wanningWindow.Owner = window;
            wanningWindow.TipBlock.Text = "删除订单？";
            wanningWindow.ShowDialog();
            if (!wanningWindow.GetValueForButton())
            {
                return;
            }

            //找出订单的关联产品，删除后将产品返回到库存
            decimal orderID = order.ID;
            List<OrderDetail> orderDetails = sczy.OrderDetails.Where(od => od.Order_ID == orderID).ToList();
            foreach (OrderDetail orderDetail in orderDetails)
            {
                Product product = sczy.Products.FirstOrDefault(p => p.ID == orderDetail.Order_ID);
                string oldUnit = orderDetail.Unit;
                double? oldOutCount = orderDetail.Count;
                if (oldUnit == "令")
                {
                    oldOutCount = double.Parse(((product.Length * product.Width * product.GramWeight * oldOutCount * 500) / 10000000000).ToString());
                }
                else if (oldUnit == "张")
                {
                    oldOutCount = double.Parse(((product.Length * product.Width * product.GramWeight * oldOutCount) / 10000000000).ToString());
                }
                product.Reservation -= oldOutCount;
            }

            //删除订单信息
            sczy.Orders.Remove(sczy.Orders.FirstOrDefault(o => o.ID == order.ID));
            sczy.SaveChanges();
            List<Order> orderList = sczy.Orders.Where(o => o.OrderState == "未划价").ToList();
            PendingOrder.ItemsSource = orderList;  //读取用户并刷新datagrid
        }


        /// <summary>
        /// 打印生产单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintItem_OnClick(object sender, RoutedEventArgs e)
        {
            Printer printer = new Printer("Product");
            Order order = PendingOrder.SelectedItem as Order;
            if (order == null)
            {
                return;
            }
            printer.GetOrderData(order);

            Thread thread = new Thread(printer.PrintOrders);
            thread.Start();
        }


        /// <summary>
        /// 对订单产品操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductMagItem_OnClick(object sender, RoutedEventArgs e)
        {
            Order order = (Order) PendingOrder.SelectedItem;
            Order_Product orderProduct = new Order_Product(order);
            Window window = Window.GetWindow(this);
            orderProduct.Owner = window;
            orderProduct.ShowDialog();
        }

       
        /// <summary>
        /// 鼠标选中字体颜色渐变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFontColorAnimate(object sender, RoutedEventArgs e)
        {
            #region 背景颜色动画
            
            TextBlock textBlock = sender as TextBlock;
            SolidColorBrush myBrush = new SolidColorBrush();

            ColorAnimation myColorAnimation = new ColorAnimation();
            myColorAnimation.From = Color.FromRgb(131, 131, 131);
            myColorAnimation.To = Color.FromRgb(75, 152, 220);
            myColorAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            myBrush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation, HandoffBehavior.Compose);
            textBlock.Foreground = myBrush;
            #endregion

        }

        /// <summary>
        /// 鼠标选中字体颜色渐变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutFontColorAnimate(object sender, RoutedEventArgs e)
        {
            #region 背景颜色动画
            TextBlock textBlock = sender as TextBlock;
            SolidColorBrush myBrush = new SolidColorBrush();

            ColorAnimation myColorAnimation = new ColorAnimation();
            myColorAnimation.From = Color.FromRgb(75, 152, 220);
            myColorAnimation.To = Color.FromRgb(131, 131, 131);
            myColorAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            myBrush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation, HandoffBehavior.Compose);
            textBlock.Foreground = myBrush;
            #endregion

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
                orderList = sczy.Orders.ToList();

                PendingOrder.ItemsSource = orderList;
                return;
            }
            Order pro = new Order();
            orderList = LikeSeacher.SeacherLike(pro, para, "[Order]");

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
            List<Order> orderList = sczy.Orders.ToList();

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
