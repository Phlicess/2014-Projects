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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SCZY.Common;

namespace SCZY.Accountant
{
    /// <summary>
    /// FixPriceDialog.xaml 的交互逻辑
    /// </summary>
    public partial class FixPriceDialog : Window
    {

        Order orderOne = new Order(); //订单信息
  
        public FixPriceDialog(object selecectOrder)
        {
            InitializeComponent();
            orderOne = selecectOrder as Order;
            if (orderOne != null)
            {
                 BingdingProductListView(selecectOrder as Order);
            }
           
            Loaded += (sender, args) =>
            {
                SetTitle_Size();
            };
        }

        /// <summary>
        /// 绑定产品列表数据
        /// </summary>
        public void BingdingProductListView(Order order)
        {
            //获取订单产品数据
            SCZYEntities sczy = new SCZYEntities();
            List<OrderDetail> orderDetails = sczy.OrderDetails.Where(od => od.Order_ID == order.ID).ToList();

            List<ViewProduct> OrderProducts = new List<ViewProduct>();
            foreach (OrderDetail orderProduct in orderDetails)
            {
                ViewProduct viewProduct = new ViewProduct();
                Product product1 = sczy.Products.FirstOrDefault(p => p.ID == orderProduct.Product_ID);
                //对象赋值                
                viewProduct.ID = product1.ID;
                viewProduct.GramWeight = product1.GramWeight;
                viewProduct.Length = product1.Length;
                viewProduct.Texture = product1.Texture;
                viewProduct.Brand = product1.Brand;
                viewProduct.Width = product1.Width;                
                //单独赋值中间表字段 吨数
                viewProduct.Sheet = orderProduct.Count;
                viewProduct.Price = orderProduct.Price;
                viewProduct.Unit = orderProduct.Unit;
                OrderProducts.Add(viewProduct);
            }
            //ProductListView.Items.Clear();
            ProductListView.ItemsSource = OrderProducts;
        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        /// 保存按钮的响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveWindowClick(object sender, RoutedEventArgs e)
        {
            //SCZYEntities sczy = new SCZYEntities();
            //for (int i = 0; i < ProductListView.Items.Count; i++)
            //{
            //    ViewProduct producntView = ProductListView.Items[i] as ViewProduct;
            //    if (producntView.Price != null)
            //    {                      
            //            OrderDetail orderDetail = new OrderDetail();
            //            orderDetail = sczy.OrderDetails.FirstOrDefault(p => p.Order_ID == orderOne.ID && p.Product_ID == producntView.ID);
            //            orderDetail.Price = producntView.Price;
            //    }
               

            //}

            //sczy.SaveChanges();

            //Order order = sczy.Orders.FirstOrDefault(p => p.ID == orderOne.ID);
            //List<OrderDetail> listDetails = sczy.OrderDetails.Where(p => p.Order_ID == orderOne.ID).ToList();
            //foreach (var item in listDetails)
            //{
            //    if (item.Price == null || item.Price == 0)
            //    {
            //        order.OrderState = "未划价";
            //        sczy.SaveChanges();
            //        this.Close();
            //        return;
            //    }
            //}
            //order.OrderState = "已划价";
            
            //sczy.SaveChanges();
            //this.Close();
            Save();
        }

        //private void GetTotalMoney(object sender, RoutedEventArgs e)
        //{
        //    double? totalMoney = 0;
        //    SCZYEntities sczy = new SCZYEntities();

        //    for (int i = 0; i < ProductListView.Items.Count; i++)
        //    {
        //        System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;


        //        double price = 0;
        //        if (textBox.Text != null)
        //        {
        //            price = double.Parse(textBox.Text); //控件的值  
        //        }
               
        //        ViewProduct producntView = ProductListView.Items[i] as ViewProduct;
        //        OrderDetail orderDetail = new OrderDetail();
        //        orderDetail = sczy.OrderDetails.FirstOrDefault(p => p.Order_ID == orderOne.ID && p.Product_ID == producntView.ID);

        //        if (orderDetail.Unit == "吨")
        //        {
        //            //totalMoney = orderDetail.Count * 
        //            totalMoney += price * orderDetail.Count;

        //        }
        //        else if (orderDetail.Unit == "张")
        //        {
        //            totalMoney += producntView.Length * producntView.Width * producntView.GramWeight * orderDetail.Count / 100 / 100 / 1000 / 1000 * price;

        //        }
        //        else if (orderDetail.Unit == "令")
        //        {
        //            totalMoney += producntView.Length * producntView.Width * producntView.GramWeight * orderDetail.Count * 500 / 100 / 100 / 1000 / 1000 * price;

        //        }
                
        //    }

        //    TotalMoneyBox.Text = totalMoney.ToString();

        //}

        /// <summary>
        /// 显示一个订单所有产品的总钱数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriceChange(object sender, TextChangedEventArgs e)
        {
            double? totalMoney = 0;
            SCZYEntities sczy = new SCZYEntities();

            for (int i = 0; i < ProductListView.Items.Count; i++)
            {
                System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;


                double price = 0;
                if (textBox.Text != null && textBox.Text != "")
                {
                    double Out;                    
                    double.TryParse(textBox.Text,out Out); //控件的值  
                    if (Out != null)
                    {
                        price = Out; //控件的值  
                    } 
                }

                ViewProduct producntView = ProductListView.Items[i] as ViewProduct;
                OrderDetail orderDetail = new OrderDetail();
                orderDetail = sczy.OrderDetails.FirstOrDefault(p => p.Order_ID == orderOne.ID && p.Product_ID == producntView.ID);

                if (orderDetail.Unit == "吨")
                {
                    //totalMoney = orderDetail.Count * 
                    totalMoney += price * orderDetail.Count;

                }
                else if (orderDetail.Unit == "张")
                {
                    totalMoney += producntView.Length * producntView.Width * producntView.GramWeight * orderDetail.Count / 100 / 100 / 1000 / 1000 * price;

                }
                else if (orderDetail.Unit == "令")
                {
                    totalMoney += producntView.Length * producntView.Width * producntView.GramWeight * orderDetail.Count * 500 / 100 / 100 / 1000 / 1000 * price;

                }

            }

            TotalMoneyBox.Text = totalMoney.ToString();
        }

        private void SaveAndPrintClick(object sender, RoutedEventArgs e)
        {
            Save();
            PirntOrder();
        }

        /// <summary>
        /// 保存修改单价后的改变
        /// </summary>
        public void Save()
        {
            SCZYEntities sczy = new SCZYEntities();
            for (int i = 0; i < ProductListView.Items.Count; i++)
            {
                ViewProduct producntView = ProductListView.Items[i] as ViewProduct;
                if (producntView.Price != null)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail = sczy.OrderDetails.FirstOrDefault(p => p.Order_ID == orderOne.ID && p.Product_ID == producntView.ID);
                    orderDetail.Price = producntView.Price;
                }


            }

            sczy.SaveChanges();

            //设置订单是否划价的状态
            Order order = sczy.Orders.FirstOrDefault(p => p.ID == orderOne.ID);
            List<OrderDetail> listDetails = sczy.OrderDetails.Where(p => p.Order_ID == orderOne.ID).ToList();
            foreach (var item in listDetails)
            {
                if (item.Price == null || item.Price == 0)
                {
                    order.OrderState = "未划价";
                    sczy.SaveChanges();
                    this.Close();
                    return;
                }
            }
            order.OrderState = "已划价";

            //计算每个订单的总金额
            GetOrderAmount getOrderAmount = new GetOrderAmount(order);
            double? result = getOrderAmount.GetAmount();
            if (result != null )
            {
                order.AggregateAmount = result;
            }
            sczy.SaveChanges();
            this.Close();
        }

        /// <summary>
        /// 打印订单
        /// </summary>
        public void PirntOrder()
        {
            //Order orderInfo = PendingOrder.SelectedItem as Order;
            if (orderOne.OrderState == "已划价")
            {
                Thread thread = new Thread(new ParameterizedThreadStart(ThreadPrinter));
                thread.Start(orderOne);

            }
            else if (orderOne.OrderState == "未划价")
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
    }
}
