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

namespace SCZY.Accountant.Dialog
{
    /// <summary>
    /// DebtMemoryProductDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DebtMemoryProductDialog : Window
    {

        private Order order;
        public DebtMemoryProductDialog(object orderObject)
        {
            InitializeComponent();
            order = (Order) orderObject;
            GetScheduled();//获取数据
             Loaded += (sender, args) =>
            {
                SetTitle_Size();
            };
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
        /// 获取数据
        /// </summary>
        public void GetScheduled()
        {

            double? money = 0;
            SCZYEntities sczy = new SCZYEntities();
            List<ViewProduct> viewProductsList = new List<ViewProduct>();
            List<OrderDetail> orderDetails =
                sczy.OrderDetails.Where(h => h.Order_ID == order.ID).OrderBy(p => p.ID).ToList();

            foreach (OrderDetail orderDetail in orderDetails)
            {
                ViewProduct viewProduct = new ViewProduct();
                Product product =new Product();
                product = sczy.Products.FirstOrDefault(p => p.ID == orderDetail.Product_ID);
               // viewProduct = (ViewProduct) product;
                viewProduct.ID = product.ID;
                viewProduct.Brand = product.Brand;
                viewProduct.Texture = product.Texture;
                viewProduct.Length = product.Length;
                viewProduct.Level = product.Level;
                viewProduct.GramWeight = product.GramWeight;
                viewProduct.Width = product.Width;

                switch (orderDetail.Unit)//根据出货的计数方式不同（"吨,张,令"），选择不同的计算方式
                {
                    case "张":
                        money = product.Length * product.Width * product.GramWeight * orderDetail.Count / 100 / 100 / 1000 / 1000 * orderDetail.Price;
                        break;
                    case "吨":
                        money = orderDetail.Price * orderDetail.Count;
                        break;
                    case "令":
                        money = product.Length * product.Width * product.GramWeight * orderDetail.Count * 500 / 100 / 100 / 1000 / 1000 * orderDetail.Price;
                        break;
                }

                viewProduct.Money = DataChange.CutDouble(money);
                viewProductsList.Add(viewProduct);
            }

            OrderListView.Items.Clear();
            OrderListView.ItemsSource = viewProductsList.OrderBy(p => p.ID);  //读取用户并刷新datagrid
        }

        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
