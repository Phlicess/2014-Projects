using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SCZY.Clerk.OrderDialog;
using SCZY.Clerk.OrderDialog.Order_Product;
using SCZY.Common;

namespace SCZY
{
    /// <summary>
    /// Page2.xaml 的交互逻辑
    /// </summary>
    public partial class OrderManagePage : Page
    {
        public OrderManagePage()
        {
            InitializeComponent();
            GetScheduled();
        }

        //为数据表格赋值
        public void GetScheduled()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<Order> orderList = sczy.Orders.Where(o => o.OrderState == "已划价").ToList();
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
            sczy.SaveChanges();
        }


        /// <summary>
        /// 对订单产品操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductMagItem_OnClick(object sender, RoutedEventArgs e)
        {
            Order order = (Order)PendingOrder.SelectedItem;
            Order_Product orderProduct = new Order_Product(order);
            Window window = Window.GetWindow(this);
            orderProduct.Owner = window;
            orderProduct.ShowDialog();
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

            //double dExtent = PendingOrder.ScrollViewer.VerticalOffset;
            if (e.Key == Key.Enter)
            {
                SeacherBtn_OnMouseLeftButtonUp(null, null);
            }
        }



        /// <summary>
        /// Get a bool value indicate whether is the VerticalScrollBar at buttom
        /// </summary>
        /// <returns>A bool value indicate whether is the VerticalScrollBar at buttom</returns>
        //public bool IsVerticalScrollBarAtButtom
        //{
        //    get
        //    {
        //        bool isAtButtom = false;

        //        // get the vertical scroll position
        //        double dVer = ScrollViewer.VerticalOffset;

        //        //get the vertical size of the scrollable content area
        //        double dViewport = ScrollViewer.ViewportHeight;

        //        //get the vertical size of the visible content area
        //        string dExtent = ScrollViewer.VerticalOffsetProperty.ToString();

        //        if (dVer != 0)
        //        {
        //            if (dVer + dViewport == dExtent)
        //            {
        //                isAtButtom = true;
        //            }
        //            else
        //            {
        //                isAtButtom = false;
        //            }
        //        }
        //        else
        //        {
        //            isAtButtom = false;
        //        }

        //        if (PendingOrder.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled
        //            || PendingOrder.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
        //        {
        //            isAtButtom = true;
        //        }

        //        return isAtButtom;
        //    }
        //} 
    }
}
