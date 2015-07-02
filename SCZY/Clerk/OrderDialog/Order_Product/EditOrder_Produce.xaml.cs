using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SCZY.Common;
using SCZY.OtherWindows;

namespace SCZY.Clerk.OrderDialog
{
    //定义委托 刷新父窗口的ProductListView的数据
    public delegate void SaveProductHander();     
    /// <summary>
    /// EditOrder_Produce.xaml 的交互逻辑
    /// </summary>
    public partial class EditOrder_Produce : Window
    {
        private decimal orderNum, productNum;
        private ViewProduct viewProduct;
        public EditOrder_Produce(ViewProduct product, decimal orderNum)
        {
            InitializeComponent();
            this.orderNum = orderNum;
            productNum = product.ID;
            viewProduct = product;
            Loaded += (sender, args) =>
            {
                SetTitle_Size();   //设置弹出框标题
                SetValueForWindow();   //赋值初始化
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
            //if (orderDetails.Count == 0)
            //{
            //    return;
            //}
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
            Product selectProduct = sczy.Products.FirstOrDefault(p => p.ID == productNum);
            allProducts.Add(selectProduct);
            ProductDataGrid.ItemsSource = allProducts.OrderBy(ob => ob.Texture);
            
            //让将要修改的产品处于选中状态
            ProductDataGrid.SelectedItem = allProducts.FirstOrDefault(p => p.ID == productNum);

            //订购数量box赋值
            CountBox.Text = viewProduct.Count.ToString();

            //单位变量绑定
            Dictionary<string, string> unitDic = DictionaryClass.GetUnitDic();
            OrderDetail odDetail = sczy.OrderDetails.FirstOrDefault(od => od.Order_ID == orderNum && od.Product_ID == productNum);
            UnitComboBox.ItemsSource = unitDic;
            UnitComboBox.SelectedValuePath = "Key";
            UnitComboBox.DisplayMemberPath = "Value";
            UnitComboBox.SelectedValue = odDetail.Unit;
        }

        /// <summary>
        /// 保存按钮事件 用来保存订单的订购产品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            Product product = ProductDataGrid.SelectedItem as Product;

            //处理用户没有选中产品的异常
            if (product == null)
            {
                WanningBlock.Text = "请选择一种产品!";
                return;
            }

            //张数 克重 宽幅 长度 换算为Double类型用于计算吨数
            double newOutCount, unCount;
            double.TryParse(CountBox.Text, out newOutCount);
            unCount = newOutCount;
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
                wanningWindow.Owner = EditOrder_ProduceWindow;
                wanningWindow.ShowDialog();
                if (!wanningWindow.GetValueForButton())
                {
                    return;
                }
            }

            OrderDetail orderDetail =
                sczy.OrderDetails.FirstOrDefault(od => od.Order_ID == orderNum && od.Product_ID == productNum);
            if (orderDetail == null)
            {
                WanningWindow wanningWindow = new WanningWindow();
                wanningWindow.Title = "警告";
                wanningWindow.TipBlock.Text = "数据库数据不存在,请联系管理员!";
                wanningWindow.Owner = EditOrder_ProduceWindow;
                wanningWindow.ShowDialog();
                return;
            }

            //添加产品成功 1.增加未修改前库存重量 2.减去修改后库存重量
            //1.增加未修改前库存重量 
            double oldOutCount;
            double.TryParse(orderDetail.Count.ToString(), out oldOutCount);
            string oldUnit = orderDetail.Unit;
            if (oldUnit == "令")
            {
                oldOutCount = double.Parse(((product.Length * product.Width * product.GramWeight * oldOutCount * 500) / 10000000000).ToString());
            }
            else if (oldUnit == "张")
            {
                oldOutCount = double.Parse(((product.Length * product.Width * product.GramWeight * oldOutCount) / 10000000000).ToString());
            }

            //2.减去修改后库存重量
            decimal ProductID = product.ID;
            double? sub = newOutCount;
            Product product1 = sczy.Products.FirstOrDefault(p => p.ID == ProductID);
            product1.Reservation += oldOutCount;
            product1.Reservation -= sub;

            //保存数据库 关闭弹出框 更新父窗口数据
            orderDetail.Product_ID = product.ID;
            orderDetail.Count = unCount;
            orderDetail.Unit = UnitComboBox.Text;
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
