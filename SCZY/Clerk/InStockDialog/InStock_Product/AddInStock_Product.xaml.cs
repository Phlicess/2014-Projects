using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SCZY.Clerk.OrderDialog;
using SCZY.OtherWindows;

namespace SCZY.Clerk.InStockDialog.InStock_Product
{
    /// <summary>
    /// AddInStock_Product.xaml 的交互逻辑
    /// </summary>
    public partial class AddInStock_Product : Window
    {
        private decimal inStockNum;
        public AddInStock_Product(decimal inStockNum)
        {
            InitializeComponent();

            this.inStockNum = inStockNum;
            Loaded += (sender, args) =>
            {
                SetTitle_Size();   //设置弹出框标题
                SetValueForWindow();   //查出所有没订购的产品
            };
        }

        /// <summary>
        /// 给弹出框赋值 构造ViewInStock对象
        /// </summary>
        public void SetValueForWindow()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<Product> allProducts = sczy.Products.ToList();  //所有产品类列表
            List<Product> productList = new List<Product>();  //获取此订单绑定的所有产品
            List<InStockDetail> InStockDetails = sczy.InStockDetails.Where(od => od.InStock_ID == inStockNum).ToList();
            //如果此订单没有产品则跳过下面绑定步骤
            if (InStockDetails.Count == 0)
            {
                ProductDataGrid.ItemsSource = allProducts.OrderBy(ob => ob.Brand);
            }

            //获取所有此订单的产品
            foreach (InStockDetail InStockDetail in InStockDetails)
            {
                productList.Add(sczy.Products.FirstOrDefault(p => p.ID == InStockDetail.Product_ID));
            }

            //除去已经绑定的产品
            foreach (Product product in productList)
            {
                if (allProducts.Contains(product))
                {
                    allProducts.Remove(product);
                }
            }
            ProductDataGrid.ItemsSource = allProducts.OrderBy(ob => ob.Brand);
            ProductDataGrid.SelectedIndex = 0;
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
                wanningWindow.Owner = AddInStock_ProductWindow;
                wanningWindow.ShowDialog();
                return;
            }

            InStockDetail InStockDetail = new InStockDetail();
            InStockDetail.InStock_ID = inStockNum;
            InStockDetail.Product_ID = selectProduct.ID;
            InStockDetail.Weight = outCount;

            //库存产品加上入库产品的重量
            decimal selectProductID = selectProduct.ID;
            Product product = sczy.Products.FirstOrDefault(p => p.ID == selectProductID);
            product.Reservation += outCount;

            sczy.InStockDetails.Add(InStockDetail);
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
