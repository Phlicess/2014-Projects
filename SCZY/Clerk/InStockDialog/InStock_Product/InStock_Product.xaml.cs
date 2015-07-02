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

namespace SCZY.Clerk.InStockDialog.InStock_Product
{
    /// <summary>
    /// InStock_Product.xaml 的交互逻辑
    /// </summary>
    public partial class InStock_Product : Window
    {
        private decimal inStockNum;
        private InStock inStock;
        public string type = "Add";
        public InStock_Product(object inStock1)
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
            InStock selectedInStock = inStock1 as InStock;
            inStockNum = selectedInStock.ID;
            inStock = selectedInStock;
        }

        /// <summary>
        /// 初始化产品列表数据 绑定订单订购的产品信息
        /// </summary>
        public void InitList()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<InStockDetail> InStockDetails = sczy.InStockDetails.Where(od => od.InStock_ID == inStockNum).ToList();
            //if (InStockDetails.Count == 0)
            //{
            //    return;
            //}
            List<ViewProduct> viewProducts = new List<ViewProduct>();
            foreach (InStockDetail InStockDetail in InStockDetails)
            {
                ViewProduct viewProduct = new ViewProduct();
                Product product = sczy.Products.FirstOrDefault(p => p.ID == InStockDetail.Product_ID);
                viewProduct.ID = product.ID;
                viewProduct.Brand = product.Brand;
                viewProduct.Level = product.Level;
                viewProduct.Length = product.Length;
                viewProduct.Texture = product.Texture;
                viewProduct.GramWeight = product.GramWeight;
                viewProduct.Width = product.Width;
                viewProduct.Count = InStockDetail.Weight;
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

        /// <summary>
        /// 绑定产品列表数据
        /// </summary>
        public void BingdingProductListView()
        {
            //获取订单产品数据
            SCZYEntities sczy = new SCZYEntities();
            List<InStockDetail> InStockDetails = sczy.InStockDetails.Where(id => id.InStock_ID == inStock.ID).ToList();

            List<ViewProduct> OrderProducts = new List<ViewProduct>();
            foreach (InStockDetail orderProduct in InStockDetails)
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
                viewProduct.Count = orderProduct.Weight;
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
            EditInStock_Product ww = new EditInStock_Product(product, inStockNum);
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
            AddInStock_Product ww = new AddInStock_Product(inStockNum);
            ww.Owner = this;
            ww.SaveProduct += BingdingProductListView;
            ww.ShowDialog();
        }

        /// <summary>
        /// 为入库删除产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            ViewProduct viewProduct = ProductListView.SelectedItem as ViewProduct;
            InStockDetail inStockDetail = sczy.InStockDetails.FirstOrDefault(od => od.InStock_ID == inStockNum && od.Product_ID == viewProduct.ID);

            //将产品数量换算为吨 返回到库存中
            //获取数量的单位 并将数量换算成吨
            Product product = sczy.Products.FirstOrDefault(p => p.ID == viewProduct.ID);
            product.Reservation -= inStockDetail.Weight;

            sczy.InStockDetails.Remove(inStockDetail);
            sczy.SaveChanges();
            //刷新产品列表
            InitList();
        }
    }
}
