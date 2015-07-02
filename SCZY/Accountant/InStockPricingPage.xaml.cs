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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SCZY.Accountant.Dialog;
using SCZY.Common;

namespace SCZY.Accountant
{
    /// <summary>
    /// StockPricingPage.xaml 的交互逻辑
    /// </summary>
    public partial class InStockPricingPage : Page
    {
        public InStockPricingPage()
        {
            InitializeComponent();
            GetScheduled();//获取数据
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public void GetScheduled()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<InStock> inStocks = sczy.InStocks.Where(p => p.InStockState == "未划价").OrderBy(p => p.ID).ToList();
            // PendingOrder.Items.Refresh();
            if (inStocks != null)
            {
                PendingInStock.Items.Clear();
                PendingInStock.ItemsSource = inStocks;  //读取用户并刷新datagrid
                gridPager.ShowPages(PendingInStock, inStocks, 5);
            }

            
        }

        private void FixPrices_OnClick(object sender, RoutedEventArgs e)
        {
            InStock inStock = PendingInStock.SelectedItem as InStock;
            if (inStock == null)
            {
                return;
            }
            // Order selectedOrder = PendingInStocks.SelectedCells[0].Item as Order;

            Window inStockFixPriceDialog = new InStockFixPriceDialog(inStock);
            Window window = Window.GetWindow(this);
            inStockFixPriceDialog.Owner = window;
            inStockFixPriceDialog.ShowDialog();


            SCZYEntities sczy = new SCZYEntities();
            InStock newInStock = new InStock();//修改后，查看是否已经划价
            newInStock = sczy.InStocks.FirstOrDefault(p => p.ID == inStock.ID);
            if (newInStock.InStockState == "已划价")
            {
                List<InStock> inStocks = sczy.InStocks.Where(h => h.InStockState == "未划价").OrderBy(p => p.ID).ToList();
                PendingInStock.ItemsSource = inStocks;  //读取用户并刷新datagrid
            }
            //  GetScheduled(); //数据可能改变,重新获取datagril 的数据
        }

        /// <summary>
        /// 查找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<InStock> inStocks = new List<InStock>();

            //获取查找的条件
            string para = SeacherBox.Text;
            if (String.IsNullOrEmpty(para))
            {
                //重新绑定所有数据
                SCZYEntities sczy = new SCZYEntities();
                inStocks = sczy.InStocks.Where(p => p.InStockState == "未划价").OrderBy(p => p.ID).ToList();

                PendingInStock.ItemsSource = inStocks;
                return;
            }
            InStock pro = new InStock();
            inStocks = LikeSeacher.SeacherLike(pro, para, "InStock");

            PendingInStock.ItemsSource = inStocks;
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
            List<InStock> inStocks = sczy.InStocks.Where(p => p.InStockState == "未划价").OrderBy(p => p.ID).ToList();

            PendingInStock.ItemsSource = inStocks;
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
    }
}
