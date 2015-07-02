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
using SCZY.Clerk.InStockDialog;
using SCZY.Clerk.InStockDialog.InStock_Product;
using SCZY.Common;

namespace SCZY
{
    /// <summary>
    /// StockManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class StockManagePage : Page
    {
        public StockManagePage()
        {
            InitializeComponent();
            GetScheduled();
        }


        //为数据表格赋值
        public void GetScheduled()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<InStock> inStockList = sczy.InStocks.Where(o => o.InStockState == "已划价").ToList();
            PendingInStock.Items.Clear();
            PendingInStock.ItemsSource = inStockList;  //读取用户并刷新datagrid

            gridPager.ShowPages(PendingInStock, inStockList, 5);
        }


        /// <summary>
        /// DataGrid右键菜单事件--修改库存单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditItem_OnClick(object sender, RoutedEventArgs e)
        {
            InStock inStockInfo = PendingInStock.SelectedItem as InStock;
            if (inStockInfo == null)
            {
                return;
            }
            InStock selectedInStock = PendingInStock.SelectedCells[0].Item as InStock;

            EditInStock editInStockWindow = new EditInStock(selectedInStock);
            editInStockWindow.type = "Edit";
            Window window = Window.GetWindow(this);
            editInStockWindow.Owner = window;
            editInStockWindow.ShowDialog();
        }

        /// <summary>
        /// DataGrid右键菜单事件--删除库存单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            InStock inStock = PendingInStock.SelectedItem as InStock;
            if (inStock == null)
            {
                return;
            }
            //找出库存单的关联产品，删除后将产品返回到库存
            decimal inStockID = inStock.ID;
            List<InStockDetail> inStockDetails = sczy.InStockDetails.Where(od => od.InStock_ID == inStockID).ToList();
            foreach (InStockDetail inStockDetail in inStockDetails)
            {
                Product product = sczy.Products.FirstOrDefault(p => p.ID == inStockDetail.InStock_ID);
                double? oldOutCount = inStockDetail.Weight;
                product.Reservation -= oldOutCount;
            }
            sczy.SaveChanges();
        }


        /// <summary>
        /// 对库存单产品操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductMagItem_OnClick(object sender, RoutedEventArgs e)
        {
            InStock inStock = (InStock)PendingInStock.SelectedItem;
            InStock_Product inStockProduct = new InStock_Product(inStock);
            Window window = Window.GetWindow(this);
            inStockProduct.Owner = window;
            inStockProduct.ShowDialog();
        }


        /// <summary>
        /// 查找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<InStock> inStockList = new List<InStock>();

            //获取查找的条件
            string para = SeacherBox.Text;
            if (String.IsNullOrEmpty(para))
            {
                //重新绑定所有数据
                SCZYEntities sczy = new SCZYEntities();
                inStockList = sczy.InStocks.ToList();

                PendingInStock.ItemsSource = inStockList;
                return;
            }
            InStock pro = new InStock();
            inStockList = LikeSeacher.SeacherLike(pro, para, "InStock");

            PendingInStock.ItemsSource = inStockList;
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
            List<InStock> inStockList = sczy.InStocks.ToList();

            PendingInStock.ItemsSource = inStockList;
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
