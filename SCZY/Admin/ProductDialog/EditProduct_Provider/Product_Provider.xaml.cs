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

namespace SCZY.Admin.ProductDialog.EditProduct_Provider
{
    /// <summary>
    /// Product_Provider.xaml 的交互逻辑
    /// </summary>
    public partial class Product_Provider : Window
    {
        private decimal productID;
        public string type = "Add";
        public Product_Provider(object selectedProduct)
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
                    //SetData(selectedProduct as Product);
                }
            };
            Product selectedProduct1 = selectedProduct as Product;
            productID = selectedProduct1.ID;
        }


        /// <summary>
        /// 初始化供应商列表数据 绑定供应商报价信息
        /// </summary>
        public void InitList()
        {
            SCZYEntities sczy = new SCZYEntities();
            string sql =
                String.Format("select Provider.* from Product inner join ChanPin_GongYingShang on Product.ID = ChanPin_GongYingShang.Product_ID inner join Provider on Provider.ID = ChanPin_GongYingShang.Provider_ID where Product.ID = {0}", productID);
            List<Provider> providers = sczy.Providers.SqlQuery(sql).ToList();
            List<ViewProvider> viewProviders = new List<ViewProvider>();

            foreach (Provider provider in providers)
            {
                decimal providerID = provider.ID;
                ChanPin_GongYingShang chanPinGongYing = sczy.ChanPin_GongYingShang.FirstOrDefault(
                    cf => cf.Provider_ID == providerID && cf.Product_ID == productID);
                if (chanPinGongYing == null)
                {
                    continue;
                }
                ViewProvider viewProvider = new ViewProvider();
                viewProvider.ID = provider.ID;
                viewProvider.Name = provider.Name;
                viewProvider.Phone = provider.Phone;
                viewProvider.Tel = provider.Tel;
                viewProvider.Address = provider.Address;
                viewProvider.Company = provider.Company;
                viewProvider.Price = chanPinGongYing.Price;
                viewProviders.Add(viewProvider);
            }

            ProviderListView.ItemsSource = viewProviders;
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
        /// 关闭窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 修改供应商报价信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewProvider viewProvider = ProviderListView.SelectedItem as ViewProvider;
            if (viewProvider == null)
            {
                return;
            }
            EditProduct_Provider ww = new EditProduct_Provider(viewProvider, productID);
            ww.Owner = this;
            ww.SaveProvider += InitList;
            ww.ShowDialog();
        }


        /// <summary>
        /// 为产品添加供应商报价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            AddProduct_Provider ww = new AddProduct_Provider(productID);
            ww.Owner = this;
            ww.SaveProvider += InitList;
            ww.ShowDialog();
        }

        /// <summary>
        /// 为产品删除供应商报价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();

            ViewProvider selectViewPro = ProviderListView.SelectedItem as ViewProvider;
            decimal providerID = selectViewPro.ID;
            sczy.ChanPin_GongYingShang.Remove(
                sczy.ChanPin_GongYingShang.FirstOrDefault(c => c.Product_ID == productID && c.Provider_ID == providerID));
            sczy.SaveChanges();
            InitList();
        }
    }
}
