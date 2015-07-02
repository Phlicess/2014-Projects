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

namespace SCZY.Clerk.OrderDialog.Order_Product
{
    /// <summary>
    /// Order_Product.xaml 的交互逻辑
    /// </summary>
    public partial class Order_Product : Window
    {
        private decimal orderNum;
        private Order order;
        public string type = "Add";
        public Order_Product(object selectedOrder)
        {
            InitializeComponent();
            
            Loaded += (sender, args) =>
            {
                //SetComboBoxValue();
                InitList();  //初始化订单绑定的产品列表
                SetTitle_Size();
                if (type == "Edit")
                {
                    //如果是编辑订单窗口，就为窗口赋值
                    //SetData(selectedOrder as Order);
                }
            };
            Order selectedOrder1 = selectedOrder as Order;
            orderNum = selectedOrder1.ID;
            order = selectedOrder1;
        }

        /// <summary>
        /// 初始化产品列表数据 绑定订单订购的产品信息
        /// </summary>
        public void InitList()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<OrderDetail> orderDetails = sczy.OrderDetails.Where(od => od.Order_ID == orderNum).ToList();
            //if (orderDetails.Count == 0)
            //{
            //    return;
            //}
            List<ViewProduct> viewProducts = new List<ViewProduct>();
            foreach (OrderDetail orderDetail in orderDetails)
            {
                ViewProduct viewProduct = new ViewProduct();
                Product product = sczy.Products.FirstOrDefault(p => p.ID == orderDetail.Product_ID);
                viewProduct.ID = product.ID;
                viewProduct.Brand = product.Brand;
                viewProduct.Level = product.Level;
                viewProduct.Length = product.Length;
                viewProduct.Texture = product.Texture;
                viewProduct.GramWeight = product.GramWeight;
                viewProduct.Width = product.Width;
                viewProduct.Count = orderDetail.Count;
                viewProduct.Unit = orderDetail.Unit;
                viewProducts.Add(viewProduct);
            }

            ProductListView.ItemsSource = viewProducts;

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

        ///// <summary>
        ///// 为下拉列表绑定数据
        ///// </summary>
        //public void SetComboBoxValue()
        //{
        //    //绑定是否加急的Combobox
        //    Dictionary<bool, string> urgdic = new Dictionary<bool, string>()
        //    {
        //        {true, "加急"},
        //        {false, "正常"}
        //    };
        //    Urgent.ItemsSource = urgdic;
        //    Urgent.DisplayMemberPath = "Value";
        //    Urgent.SelectedValuePath = "Key";

        //    //送货方式下拉列表的数据字典
        //    Dictionary<string, string> disdic = DictionaryClass.GetDisDic();
        //    //绑定Combobox数据字典
        //    Dispatch.ItemsSource = disdic;
        //    Dispatch.DisplayMemberPath = "Value";
        //    Dispatch.SelectedValuePath = "Key";

        //    if (type == "Add")
        //    {
        //        Urgent.SelectedValue = false;
        //        Dispatch.SelectedValue = "上门自提";
        //        return;
        //    }

        //    Urgent.SelectedValue = order.Urgent;
        //    Dispatch.SelectedValue = order.Dispatch;
            
        //}

        ///// <summary>
        ///// 给弹出框赋值
        ///// </summary>
        ///// <param name="selectedOrder"></param>
        //public void SetData(Order selectedOrder)
        //{
        //    order = selectedOrder;
        //    OrderNum.Text = selectedOrder.ID.ToString();
        //    ConsumerName.Text = selectedOrder.ConsumerName;
        //    OrderDate.Text = selectedOrder.OrderDate.ToString();
        //    TakeDate.Text = selectedOrder.TakeDate.ToString();
        //    DeliveryDate.Text = selectedOrder.DeliveryDate.ToString();
        //    Paid.Text = selectedOrder.Paid.ToString();
        //    Phone.Text = selectedOrder.Phone;
        //    Remark.Text = selectedOrder.Remark;

        //    //获取订单产品数据
        //    BingdingProductListView();
        //}

        /// <summary>
        /// 绑定产品列表数据
        /// </summary>
        public void BingdingProductListView()
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
                viewProduct.Level = product1.Level;
                //单独赋值中间表字段 吨数
                viewProduct.Count = orderProduct.Count;
                viewProduct.Unit = orderProduct.Unit;
                OrderProducts.Add(viewProduct);
            }
            //ProductListView.Items.Clear();
            ProductListView.ItemsSource = OrderProducts;
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

        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewProduct product = ProductListView.SelectedItem as ViewProduct;
            if (product == null)
            {
                return;
            }
            EditOrder_Produce ww = new EditOrder_Produce(product, orderNum);
            ww.Owner = this;
            ww.SaveProduct += BingdingProductListView;
            ww.ShowDialog();
        }

       
        /// <summary>
        /// 为订单添加产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            AddOrder_Product ww = new AddOrder_Product(orderNum);
            ww.Owner = this;
            ww.SaveProduct += BingdingProductListView;
            ww.ShowDialog();
        }

        /// <summary>
        /// 为订单删除产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            ViewProduct viewProduct = ProductListView.SelectedItem as ViewProduct;
            OrderDetail orderDetail = sczy.OrderDetails.FirstOrDefault(od => od.Order_ID == orderNum && od.Product_ID == viewProduct.ID);
            
            //将产品数量换算为吨 返回到库存中
            //获取数量的单位 并将数量换算成吨
            string unit = orderDetail.Unit;
            double inCount;
            if (unit == "令")
            {
                inCount = double.Parse(((viewProduct.Length * viewProduct.Width * viewProduct.GramWeight * orderDetail.Count * 500) / 10000000000).ToString());
            }
            else if (unit == "张")
            {
                inCount = double.Parse(((viewProduct.Length * viewProduct.Width * viewProduct.GramWeight * orderDetail.Count) / 10000000000).ToString());
            }
            else
            {
                double.TryParse(orderDetail.Count.ToString(), out inCount);
            }
            Product product = sczy.Products.FirstOrDefault(p => p.ID == viewProduct.ID);
            product.Reservation += inCount;

            sczy.OrderDetails.Remove(orderDetail);
            sczy.SaveChanges();
            InitList();  //初始化订单绑定的产品列表
        }
    }
}
