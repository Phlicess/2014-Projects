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
    //定义委托 刷新父窗口的ProductListView的数据
    public delegate void SaveProviderHander();     
    /// <summary>
    /// EditProduct_Provider.xaml 的交互逻辑
    /// </summary>
    public partial class EditProduct_Provider : Window
    {
        private decimal productID, providerID;
        private ViewProvider viewProvider;
        public EditProduct_Provider(object viewProvider, decimal productNum)
        {
            InitializeComponent();

            productID = productNum;
            providerID = (viewProvider as ViewProvider).ID;

            this.viewProvider = viewProvider as ViewProvider;

            Init();  //初始化弹出框
            Loaded += delegate
            {
                SetTitle_Size();
            };
        }

        /// <summary>
        /// 初始化弹出框 （赋值）
        /// </summary>
        public void Init()
        {
            SCZYEntities sczy = new SCZYEntities();

            List<Provider> allProviders = sczy.Providers.ToList();
            //List<Provider> providers = new List<Provider>();
            //Product product = sczy.Products.FirstOrDefault(p => p.ID == productID);
            List<ChanPin_GongYingShang> chanPinGongYings =
                sczy.ChanPin_GongYingShang.Where(c => c.Product_ID == productID).ToList();
            //去除已经报价的供应商
            foreach (var chanPinGongYing in chanPinGongYings)
            {
                decimal prID = chanPinGongYing.Provider_ID;
                allProviders.Remove(sczy.Providers.FirstOrDefault(p => p.ID == prID));
            }
            //添加上需要修改的供应商
            Provider provider = sczy.Providers.FirstOrDefault(p => p.ID == providerID);
            allProviders.Add(provider);
            ProviderDataGrid.ItemsSource = allProviders;
            ProviderDataGrid.SelectedItem = provider;

            //赋值报价PriceBox
            PriceBox.Text = viewProvider.Price.ToString();
        }


        /// <summary>
        /// 保存供应商的报价信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            Provider selectProvider = ProviderDataGrid.SelectedItem as Provider;

            //处理用户没有选中产品的异常
            if (selectProvider == null)
            {
                WanningBlock.Text = "请选择个供应商!";
                return;
            }
            ChanPin_GongYingShang chanPinGongYing =
                sczy.ChanPin_GongYingShang.FirstOrDefault(p => p.Provider_ID == providerID && p.Product_ID == productID);
            chanPinGongYing.Provider_ID = selectProvider.ID;
            double price;
            double.TryParse(PriceBox.Text, out price);
            chanPinGongYing.Price = price;
            sczy.SaveChanges();
            Close();
            if (SaveProvider != null)
            {
                SaveProvider();
            }
        }


        //定义委托事件
        public event SaveProviderHander SaveProvider;

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
