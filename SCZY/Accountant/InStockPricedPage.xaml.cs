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
    /// InStockPricedPage.xaml 的交互逻辑
    /// </summary>
    public partial class InStockPricedPage : Page
    {
        public InStockPricedPage()
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
            List<InStock> inStocks = sczy.InStocks.Where(p => p.InStockState == "已划价").OrderBy(p => p.ID).ToList();
            // PendingOrder.Items.Refresh();
            if (inStocks != null)
            {
                PendingInStocks.Items.Clear();
                PendingInStocks.ItemsSource = inStocks;  //读取用户并刷新datagrid
                gridPager.ShowPages(PendingInStocks, inStocks, 5);
            }

           
        }

        private void EditPriceInStock(object sender, RoutedEventArgs e)
        {
            InStock inStock = PendingInStocks.SelectedItem as InStock;
            if (inStock == null)
            {
                return;
            }
           // Order selectedOrder = PendingInStocks.SelectedCells[0].Item as Order;

            Window inStockFixPriceDialog = new InStockFixPriceDialog(inStock);
            Window window = Window.GetWindow(this);
            inStockFixPriceDialog.Owner = window;
            inStockFixPriceDialog.ShowDialog();
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
                inStocks = sczy.InStocks.Where(p => p.InStockState == "已划价").OrderBy(p => p.ID).ToList();

                PendingInStocks.ItemsSource = inStocks;
                return;
            }
            InStock pro = new InStock();
            inStocks = LikeSeacher.SeacherLike(pro, para, "InStock");

            PendingInStocks.ItemsSource = inStocks;
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

            SCZYEntities sczy = new SCZYEntities();
            List<InStock> inStocks = sczy.InStocks.Where(p => p.InStockState == "已划价").OrderBy(p => p.ID).ToList();

            PendingInStocks.ItemsSource = inStocks;
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
