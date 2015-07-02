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
    /// InStockFixPriceDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InStockFixPriceDialog : Window
    {
        private InStock inStock = new InStock();
        public InStockFixPriceDialog(object selecectOrder)
        {
            inStock = (InStock) selecectOrder;
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                SetTitle_Size();
            };
            GetScheduled();//获取数据
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
            SCZYEntities sczy = new SCZYEntities();
            List<InStockDetail> inStockDetails = sczy.InStockDetails.Where(od => od.InStock_ID == inStock.ID).ToList();

            List<ViewInStock> viewInStocks = new List<ViewInStock>();
            foreach (var inStockDetail in inStockDetails)
            {
                ViewInStock viewInStock = new ViewInStock();
                Product product1 = sczy.Products.FirstOrDefault(p => p.ID == inStockDetail.Product_ID);
                //对象赋值                
                viewInStock.ID = product1.ID;
                viewInStock.GramWeight = product1.GramWeight;
                viewInStock.Length = product1.Length;
                viewInStock.Texture = product1.Texture;
                viewInStock.Brand = product1.Brand;
                viewInStock.Width = product1.Width;
                //单独赋值中间表字段 吨数
                //viewInStock.Sheet = orderProduct.Count;
                viewInStock.Price = inStockDetail.Price;
                //viewInStock.Unit = orderProduct.Unit;
                viewInStocks.Add(viewInStock);
            }

            ProductListView.Items.Clear();
            ProductListView.ItemsSource = viewInStocks.OrderBy(p => p.ID);

        }

        private void PriceChange(object sender, TextChangedEventArgs e)
        {
            double? totalMoney = 0;
            SCZYEntities sczy = new SCZYEntities();

            for (int i = 0; i < ProductListView.Items.Count; i++)
            {
                System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;


                double price = 0;
                if (textBox.Text != null && textBox.Text != "")
                {
                    double Out;
                    double.TryParse(textBox.Text, out Out); //控件的值  
                    if (Out != null)
                    {
                        price = Out; //控件的值  
                    }
                }

                ViewInStock viewInStock = ProductListView.Items[i] as ViewInStock;
                InStockDetail inStockDetail = new InStockDetail();

                inStockDetail = sczy.InStockDetails.FirstOrDefault(p => p.InStock_ID == inStock.ID && p.Product_ID == viewInStock.ID);
                totalMoney += price * inStockDetail.Weight;
                //if (orderDetail.Unit == "吨")
                //{
                //    //totalMoney = orderDetail.Count * 
                //    totalMoney += price * orderDetail.Count;

                //}
                //else if (orderDetail.Unit == "张")
                //{
                //    totalMoney += producntView.Length * producntView.Width * producntView.GramWeight * orderDetail.Count / 100 / 100 / 1000 / 1000 * price;

                //}
                //else if (orderDetail.Unit == "令")
                //{
                //    totalMoney += producntView.Length * producntView.Width * producntView.GramWeight * orderDetail.Count * 500 / 100 / 100 / 1000 / 1000 * price;

                //}

            }

            TotalMoneyBox.Text = totalMoney.ToString();
        }

        private void SaveWindowClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            for (int i = 0; i < ProductListView.Items.Count; i++)
            {
                ViewInStock viewInStock = ProductListView.Items[i] as ViewInStock;
                if (viewInStock.Price != null)
                {
                    InStockDetail inStockDetail = new InStockDetail();
                    inStockDetail = sczy.InStockDetails.FirstOrDefault(p => p.InStock_ID == inStock.ID && p.Product_ID == viewInStock.ID);
                    inStockDetail.Price = viewInStock.Price;
                }


            }

            sczy.SaveChanges();

            InStock inStock1 = sczy.InStocks.FirstOrDefault(p => p.ID == inStock.ID);
            List<InStockDetail> inStockDetails = sczy.InStockDetails.Where(p => p.InStock_ID == inStock.ID).ToList();
            foreach (var item in inStockDetails)
            {
                if (item.Price == null || item.Price == 0)
                {
                    inStock1.InStockState = "未划价";
                    sczy.SaveChanges();
                    this.Close();
                    return;
                }
            }
            inStock1.InStockState = "已划价";

            sczy.SaveChanges();
            this.Close();
        }

        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
