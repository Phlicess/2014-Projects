using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SCZY.Admin.ProductDialog;
using SCZY.Admin.ProductDialog.EditProduct_Provider;
using SCZY.Common;
using SCZY.OtherWindows;

namespace SCZY.Admin
{
    /// <summary>
    /// ProductManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductManagePage : Page
    {
        public ProductManagePage()
        {
            InitializeComponent();

            Infinite();
            
        }
        
        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Infinite()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<Product> productList = sczy.Products.Where(s => s.State == true).ToList();

            PendingProduct.Items.Clear();
            PendingProduct.ItemsSource = productList;

            gridPager.ShowPages(PendingProduct, productList, 5);
        }

        

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            EditProduct editProductWindow = new EditProduct(null);
            editProductWindow.type = "Add";
            editProductWindow.Title = "添加产品";
            editProductWindow.SaveProvider += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<Product> productList = sczy.Products.ToList();
                PendingProduct.ItemsSource = productList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editProductWindow.Owner = window;
            editProductWindow.ShowDialog();
        }

        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdiItem_OnClick(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = PendingProduct.SelectedItem as Product;
            if (selectedProduct == null)
            {
                return;
            }

            EditProduct editProductWindow = new EditProduct(selectedProduct);
            editProductWindow.type = "Edit";
            editProductWindow.SaveProvider += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<Product> productList = sczy.Products.ToList();
                PendingProduct.ItemsSource = productList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editProductWindow.Owner = window;
            editProductWindow.ShowDialog();
        }

        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            Product productInfo = PendingProduct.SelectedItem as Product;
            if (productInfo == null)
            {
                return;
            }

            decimal ID = productInfo.ID;

            SCZYEntities sczy = new SCZYEntities();
            Product selectedProduct = sczy.Products.FirstOrDefault(c => c.ID == ID);

            //弹出警告框
            WanningWindow wanningWindow = new WanningWindow();
            Window window = Window.GetWindow(this);
            wanningWindow.Owner = window;
            wanningWindow.TipBlock.Text = "删除这个产品, You Sure?";
            wanningWindow.ShowDialog();
            if (!wanningWindow.GetValueForButton())
            {
                return;
            }

            selectedProduct.State = false;
            //sczy.Products.Remove(selectedProduct);
            sczy.SaveChanges();

            List<Product> productList = sczy.Products.Where(s => s.State == true).ToList();
            PendingProduct.ItemsSource = productList;
        }

        /// <summary>
        /// 查找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<Product> productList = new List<Product>();

            //获取查找的条件
            string para = SeacherBox.Text;
            if (String.IsNullOrEmpty(para))
            {
                //重新绑定所有数据
                SCZYEntities sczy = new SCZYEntities();
                productList = sczy.Products.ToList();

                PendingProduct.ItemsSource = productList;
                return;
            }
            Product pro = new Product();
            productList = LikeSeacher.SeacherLike(pro, para, "Product");

            PendingProduct.ItemsSource = productList;
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
            List<Product> productList = sczy.Products.ToList();

            PendingProduct.ItemsSource = productList;
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

        /// <summary>
        /// 供应商报价信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProviderItem_OnClick(object sender, RoutedEventArgs e)
        {
            Product product = (Product)PendingProduct.SelectedItem;
            Product_Provider productProvider = new Product_Provider(product);
            Window window = Window.GetWindow(this);
            productProvider.Owner = window;
            productProvider.ShowDialog();
        }
    }
}
