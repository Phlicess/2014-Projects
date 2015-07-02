using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SCZY.Common;

namespace SCZY.Clerk.OrderDialog
{
    //定义委托 刷新父窗口的ProductListView的数据
    public delegate void SaveOrderHander(); 
    /// <summary>
    /// EditOrder.xaml 的交互逻辑
    /// </summary>
    public partial class EditOrder : Window
    {
        private decimal orderID;
        private Order order;
        public string type = "Add";
        public EditOrder(object selectedOrder)
        {
            InitializeComponent();
            order = selectedOrder as Order;
            orderID = order.ID;
            
            Loaded += (sender, args) =>
            {
                SetComboBoxValue();
                SetTitle_Size();
                if (type == "Edit")
                {
                    //如果是编辑订单窗口，就为窗口赋值
                    SetData(selectedOrder as Order);
                }
            };

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
        /// 为下拉列表绑定数据
        /// </summary>
        public void SetComboBoxValue()
        {
            //绑定是否加急的Combobox
            Dictionary<bool, string> urgdic = new Dictionary<bool, string>()
            {
                {true, "加急"},
                {false, "正常"}
            };
            Urgent.ItemsSource = urgdic;
            Urgent.DisplayMemberPath = "Value";
            Urgent.SelectedValuePath = "Key";

            //送货方式下拉列表的数据字典
            Dictionary<string, string> disdic = DictionaryClass.GetDisDic();
            //绑定Combobox数据字典
            Dispatch.ItemsSource = disdic;
            Dispatch.DisplayMemberPath = "Value";
            Dispatch.SelectedValuePath = "Key";

            if (type == "Add")
            {
                Urgent.SelectedValue = false;
                Dispatch.SelectedValue = "上门自提";
            }
            else if(type == "Edit")
            {
                Urgent.SelectedValue = order.Urgent;
                Dispatch.SelectedValue = order.Dispatch;
            }


            //客户下拉列表绑定
            SCZYEntities sczy = new SCZYEntities();
            List<Consumer> consumers = sczy.Consumers.ToList();
            ConsumerName.ItemsSource = consumers;
            ConsumerName.DisplayMemberPath = "Name";
            ConsumerName.SelectedValuePath = "ID";
            //int i = int.Parse(order.Consumer_ID.ToString());
            if (type == "Edit")
            {
                ConsumerName.SelectedValue = order.Consumer_ID;
            }
            else if (type == "Add")
            {
                ConsumerName.SelectedIndex = 0;
            }
            

        }

        /// <summary>
        /// 给弹出框赋值
        /// </summary>
        /// <param name="selectedOrder"></param>
        public void SetData(Order selectedOrder)
        {
            order = selectedOrder;
            OrderNum.Text = selectedOrder.ID.ToString();
            ConsumerName.Text = selectedOrder.ConsumerName;
            OrderDate.Text = selectedOrder.OrderDate.ToString();
            TakeDate.Text = selectedOrder.TakeDate.ToString();
            DeliveryDate.Text = selectedOrder.DeliveryDate.ToString();
            Paid.Text = selectedOrder.Paid.ToString();
            Phone.Text = selectedOrder.Phone;
            Remark.Text = selectedOrder.Remark;
        }


        //定义委托事件
        public event SaveOrderHander SaveOrder;
        /// <summary>
        /// 保存订单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            if (type == "Edit")
            {
                Order oldOrder = sczy.Orders.FirstOrDefault(o => o.ID == orderID);
                oldOrder.OrderDate = OrderDate.DisplayDate;
                oldOrder.DeliveryDate = DeliveryDate.DisplayDate;
                oldOrder.TakeDate = TakeDate.DisplayDate;
                oldOrder.Phone = Phone.Text;
                oldOrder.Dispatch = Dispatch.Text;
                oldOrder.Consumer_ID = Decimal.Parse(ConsumerName.SelectedValue.ToString());
                oldOrder.ConsumerName = ConsumerName.Text;
                oldOrder.Remark = Remark.Text;
                if (Urgent.Text == "加急")
                {
                    oldOrder.Urgent = true;
                }
                else
                {
                    oldOrder.Urgent = false;
                }

                sczy.SaveChanges();
                Close();
            }
            else if(type == "Add")
            {
                Order newOrder = new Order();
                newOrder.OrderDate = OrderDate.DisplayDate;
                newOrder.DeliveryDate = DeliveryDate.DisplayDate;
                newOrder.TakeDate = TakeDate.DisplayDate;
                newOrder.ConsumerName = ConsumerName.Text;
                newOrder.Consumer_ID = Decimal.Parse(ConsumerName.SelectedValue.ToString());
                newOrder.OrderState = "未划价";
                newOrder.Phone = Phone.Text;
                newOrder.Dispatch = Dispatch.Text;
                newOrder.ConsumerName = ConsumerName.Text;
                newOrder.Remark = Remark.Text;
                if (Urgent.Text == "加急")
                {
                    newOrder.Urgent = true;
                }
                else
                {
                    newOrder.Urgent = false;
                }

                sczy.Orders.Add(newOrder);
                sczy.SaveChanges();
                Close();

            }
            if (SaveOrder != null)
            {
                SaveOrder();
            }
        }
       


        /// <summary> 
        /// 关闭窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        ///// <summary>
        ///// 绑定产品列表数据
        ///// </summary>
        //public void BingdingProductListView()
        //{
        //    //获取订单产品数据
        //    SCZYEntities sczy = new SCZYEntities();
        //    List<OrderDetail> orderDetails = sczy.OrderDetails.Where(od => od.Order_ID == order.ID).ToList();

        //    List<ViewProduct> OrderProducts = new List<ViewProduct>();
        //    foreach (OrderDetail orderProduct in orderDetails)
        //    {
        //        ViewProduct viewProduct = new ViewProduct();
        //        Product product1 = sczy.Products.FirstOrDefault(p => p.ID == orderProduct.Product_ID);
        //        //对象赋值
        //        viewProduct.ID = product1.ID;
        //        viewProduct.GramWeight = product1.GramWeight;
        //        viewProduct.Length = product1.Length;
        //        viewProduct.Texture = product1.Texture;
        //        viewProduct.Brand = product1.Brand;
        //        viewProduct.Width = product1.Width;
        //        viewProduct.Level = product1.Level;
        //        //单独赋值中间表字段 吨数
        //        viewProduct.Count = orderProduct.Count;
        //        viewProduct.Unit = orderProduct.Unit;
        //        OrderProducts.Add(viewProduct);
        //    }
        //    //ProductListView.Items.Clear();
        //    ProductListView.ItemsSource = OrderProducts;
        //}

        ///// <summary>
        ///// 修改产品信息
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void EditItem_OnClick(object sender, RoutedEventArgs e)
        //{
        //    ViewProduct product = ProductListView.SelectedItem as ViewProduct;
        //    if (product == null)
        //    {
        //        return;
        //    }
        //    EditOrder_Produce ww = new EditOrder_Produce(product, orderNum);
        //    ww.Owner = this;
        //    ww.SaveProduct += BingdingProductListView;
        //    ww.ShowDialog();
        //}

       
        ///// <summary>
        ///// 为订单添加产品
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void AddItem_OnClick(object sender, RoutedEventArgs e)
        //{
        //    AddOrder_Product ww = new AddOrder_Product(orderNum);
        //    ww.Owner = this;
        //    ww.SaveProduct += BingdingProductListView;
        //    ww.ShowDialog();
        //}

        ///// <summary>
        ///// 为订单删除产品
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void DelItem_OnClick(object sender, RoutedEventArgs e)
        //{
        //    SCZYEntities sczy = new SCZYEntities();
        //    ViewProduct viewProduct = ProductListView.SelectedItem as ViewProduct;
        //    OrderDetail orderDetail = sczy.OrderDetails.FirstOrDefault(od => od.Order_ID == orderNum && od.Product_ID == viewProduct.ID);

        //    //将产品数量换算为吨 返回到库存中
        //    //获取数量的单位 并将数量换算成吨
        //    string unit = orderDetail.Unit;
        //    double inCount;
        //    if (unit == "令")
        //    {
        //        inCount = double.Parse(((viewProduct.Length * viewProduct.Width * viewProduct.GramWeight * orderDetail.Count * 500) / 10000000000).ToString());
        //    }
        //    else if (unit == "张")
        //    {
        //        inCount = double.Parse(((viewProduct.Length * viewProduct.Width * viewProduct.GramWeight * orderDetail.Count) / 10000000000).ToString());
        //    }
        //    else
        //    {
        //        double.TryParse(orderDetail.Count.ToString(), out inCount);
        //    }
        //    Product product = sczy.Products.FirstOrDefault(p => p.ID == viewProduct.ID);
        //    product.Reservation += inCount;

        //    sczy.OrderDetails.Remove(orderDetail);
        //    sczy.SaveChanges();
        //}
    }
}
