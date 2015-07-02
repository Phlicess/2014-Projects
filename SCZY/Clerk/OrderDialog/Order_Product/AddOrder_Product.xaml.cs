using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SCZY.Common;
using SCZY.OtherWindows;

namespace SCZY.Clerk.OrderDialog
{
    /// <summary>
    /// AddOrder_Product.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrder_Product : Window
    {
        private decimal orderNum;
        public AddOrder_Product(decimal orderNum)
        {
            InitializeComponent();
            this.orderNum = orderNum;
            Loaded += (sender, args) =>
            {
                SetTitle_Size();   //设置弹出框标题
                SetValueForWindow();   //查出所有没订购的产品
            };
        }

        /// <summary>
        /// 给弹出框赋值 构造ViewProduct对象
        /// </summary>
        public void SetValueForWindow()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<Product> allProducts = sczy.Products.ToList();  //所有产品类列表
            List<Product> productList = new List<Product>();  //获取此订单绑定的所有产品
            List<OrderDetail> orderDetails = sczy.OrderDetails.Where(od => od.Order_ID == orderNum).ToList();
            //如果此订单没有产品则跳过下面绑定步骤
            if (orderDetails.Count == 0)
            {
                ProductDataGrid.ItemsSource = allProducts.OrderBy(ob => ob.Texture);
            }

            //获取所有此订单的产品
            foreach (OrderDetail orderDetail in orderDetails)
            {
                productList.Add(sczy.Products.FirstOrDefault(p => p.ID == orderDetail.Product_ID));
            }

            //除去已经绑定的产品
            foreach (Product product in productList)
            {
                if (allProducts.Contains(product))
                {
                    allProducts.Remove(product);
                }
            }
            ProductDataGrid.ItemsSource = allProducts.OrderBy(ob => ob.Texture);
            ProductDataGrid.SelectedIndex = 0;

            //绑定单位下拉列表的数据
            Dictionary<string, string> unitDic = DictionaryClass.GetUnitDic();
            UnitComboBox.ItemsSource = unitDic;
            UnitComboBox.SelectedValuePath = "Key";
            UnitComboBox.DisplayMemberPath = "Value";
            UnitComboBox.SelectedValue = "张";
        }
        
        /// <summary>
        /// 添加产品后保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            double outCount;
            double.TryParse(CountBox.Text, out outCount);
            Product selectProduct = ProductDataGrid.SelectedItem as Product;

            if (selectProduct == null)
            {
                WanningWindow wanningWindow = new WanningWindow();
                wanningWindow.Title = "警告";
                wanningWindow.TipBlock.Text = "请选择一种产品!";
                wanningWindow.Owner = AddOrder_ProduceWindow;
                wanningWindow.ShowDialog();
                return;
            }

            OrderDetail newOrderDetail = new OrderDetail();
            newOrderDetail.Order_ID = orderNum;
            newOrderDetail.Product_ID = selectProduct.ID;
            newOrderDetail.Count = outCount;
            newOrderDetail.Unit = UnitComboBox.Text;

            //库存产品减去下单产品的重量
            decimal selectProductID = selectProduct.ID;
            //double sub = DataChange.GetWeight(outGramWeight.ToString(), selectProduct.Width.ToString(), selectProduct.Length.ToString(), outCount.ToString());
            Product product = sczy.Products.FirstOrDefault(p => p.ID == selectProductID);

            //张数 克重 宽幅 长度 换算为Double类型用于计算吨数
            double newOutCount;
            double.TryParse(CountBox.Text, out newOutCount);
            //获取数量的单位 并将数量换算成吨
            string unit = UnitComboBox.Text;
            if (unit == "令")
            {
                newOutCount = double.Parse(((product.Length * product.Width * product.GramWeight * newOutCount * 500) / 10000000000).ToString());
            }
            else if (unit == "张")
            {
                newOutCount = double.Parse(((product.Length * product.Width * product.GramWeight * newOutCount) / 10000000000).ToString());
            }

            //判断库存是否紧张
            if (newOutCount >= product.Reservation)
            {
                WanningWindow wanningWindow = new WanningWindow();
                wanningWindow.Title = "确定继续?";
                wanningWindow.TipBlock.Text = "此产品库存可能不足,是否继续下单?";
                wanningWindow.Owner = AddOrder_ProduceWindow;
                wanningWindow.ShowDialog();
                if (!wanningWindow.GetValueForButton())
                {
                    return;
                }
            }

            product.Reservation -= newOutCount;

            sczy.OrderDetails.Add(newOrderDetail);
            sczy.SaveChanges();
            Close();
            if (SaveProduct != null)
            {
                SaveProduct();
            }
        }


        //定义委托事件
        public event SaveProductHander SaveProduct;


        /// <summary>
        /// 设置弹出框的标题
        /// 设置弹出框的大小
        /// </summary>
        public void SetTitle_Size()
        {
            TextBlock windowTitle = this.Template.FindName("WindowTitle", this) as TextBlock;
            windowTitle.Text = Title;

            Grid temGrid = this.Template.FindName("TemGrid", this) as Grid;
            temGrid.Height = Height;
            temGrid.Width = Width;
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
    }
}
