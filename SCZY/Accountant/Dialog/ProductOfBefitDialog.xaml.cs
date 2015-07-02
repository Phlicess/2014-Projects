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
    /// ProductOfBefitDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ProductOfBefitDialog : Window
    {
        private DateTime dateTime; //需要的月份
        public ProductOfBefitDialog(object selecectOrder)
        {
            dateTime = (DateTime) selecectOrder;
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
            double moneyOfStock = 0;
            double moneyOfSold = 0;
            double retainedProfits = 0;

            
            List<BenefitDialogViewProduct> benefitDialogViewProducts = new List<BenefitDialogViewProduct>();

            List<Product> products = new List<Product>();
            products = GetTheMonthProduct(dateTime); //该月份交易的所有产品
            foreach (var product in products)
            {
                BenefitDialogViewProduct benefitDialogViewProduct = new BenefitDialogViewProduct();
                if (product != null)
                {
                     moneyOfStock = GetTheProductsOfMoneyOfStock(product);
                     moneyOfSold = GetTheProductsOfMoneyOfSold(product);
                     retainedProfits = GetTheProductsOfRetainedProfits(product);

                     
                     benefitDialogViewProduct.Month = dateTime;
                     benefitDialogViewProduct.MoneyOfSold = moneyOfSold;
                     benefitDialogViewProduct.MoneyOfStock = moneyOfStock;
                     benefitDialogViewProduct.RetainedProfits = retainedProfits;

                     benefitDialogViewProduct.Brand = product.Brand;
                     benefitDialogViewProduct.Texture = product.Texture;
                     benefitDialogViewProduct.GramWeight = product.GramWeight;
                     benefitDialogViewProduct.Width = product.Width;
                     benefitDialogViewProduct.Length = product.Length;
                     benefitDialogViewProduct.Level = product.Level;
                     //benefitDialogViewProduct. = product.Texture;

                     benefitDialogViewProducts.Add(benefitDialogViewProduct);
                }
               
            }


            ProductListView.Items.Clear();
            ProductListView.ItemsSource = benefitDialogViewProducts;

        }

        /// <summary>
        /// 获取指定月份交易的所有产品
        /// </summary>
        /// <returns></returns>
        public List<Product> GetTheMonthProduct(DateTime dateTime)
        {
            List<Product> products = new List<Product>();

            SCZYEntities sczy = new SCZYEntities();          
            List<InStock> inStocks = sczy.InStocks.Where(od => od.InStockState == "已划价").ToList();
            foreach (var inStock in inStocks)
            {
                if (inStock.InStockDate != null)
                { 
                    DateTime inStockTime = (DateTime)(inStock.InStockDate);
                    DateTime inStockTime1 = new DateTime(inStockTime.Year,inStockTime.Month,1);
                    if (inStockTime1 ==  dateTime)
                    {
                         List<InStockDetail> inStockDetails = sczy.InStockDetails.Where(p => p.InStock_ID == inStock.ID).ToList();
                         foreach (var inStockDetail in inStockDetails)
                         {
                             Product product = new Product();
                             product = sczy.Products.FirstOrDefault(p => p.ID == inStockDetail.Product_ID);
                             products.Add(product);
                         }                       
                    }
                     
                }
               
            }

            return products;
        }

        /// <summary>
        /// 获得产品进货钱数
        /// </summary>
        /// <returns>进货钱数</returns>
        public double GetTheProductsOfMoneyOfStock(Product product)
        {
            double moneyOfStock = 0;

            SCZYEntities sczy = new SCZYEntities();


            if (product != null)
            {
                List<InStockDetail> inStockDetails = new List<InStockDetail>();
                inStockDetails = sczy.InStockDetails.Where(p => p.Product_ID == product.ID).ToList();
                foreach (var inStockDetail in inStockDetails)
                {
                    InStock inStock =new InStock();
                    inStock = sczy.InStocks.FirstOrDefault(p => p.ID == inStockDetail.InStock_ID);
                    if (inStock.InStockState == "已划价")
                    {
                        moneyOfStock += (double)(inStockDetail.Price*inStockDetail.Weight);
                    }
                }
            }
            moneyOfStock = DataChange.CutDouble(moneyOfStock);
            return moneyOfStock;

        }

        /// <summary>
        /// 获得产品卖货钱数
        /// </summary>
        /// <param name="products"></param>
        /// <returns>卖货钱数</returns>
        public double GetTheProductsOfMoneyOfSold(Product product)
        {
            double moneyOfSold = 0;
            SCZYEntities sczy = new SCZYEntities();

            if (product != null)
            {
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                orderDetails = sczy.OrderDetails.Where(p => p.Product_ID == product.ID).ToList();
                foreach (var orderDetail in orderDetails)
                {
                    //Order order = new Order();
                    //order = sczy.Orders.FirstOrDefault(p => p.ID == orderDetail.InStock_ID);
                  
                   // moneyOfStock += (double)(inStockDetail.Price * inStockDetail.Weight);

                    if (orderDetail != null && orderDetail.Price != null)
                    {
                        Double? cash = 0;
                        switch (orderDetail.Unit)//根据出货的计数方式不同（"吨,张,令"），选择不同的计算方式
                        {
                            case "张":
                                cash = product.Length * product.Width * product.GramWeight * orderDetail.Count / 100 / 100 / 1000 / 1000 * orderDetail.Price;
                                break;
                            case "吨":
                                cash = orderDetail.Price * orderDetail.Count;
                                break;
                            case "令":
                                cash = product.Length * product.Width * product.GramWeight * orderDetail.Count * 500 / 100 / 100 / 1000 / 1000 * orderDetail.Price;
                                break;
                        }
                        //dr["货款"] = DataChange.CutDouble(cash);
                        moneyOfSold += (double)cash;
                    }
                    
                }
            }
            moneyOfSold = DataChange.CutDouble(moneyOfSold);
            return moneyOfSold;
        }

        /// <summary>
        /// 获得产品净利润
        /// </summary>
        /// <param name="products"></param>
        /// <returns>净利润</returns>
        public double GetTheProductsOfRetainedProfits(Product product)
        {
            double retainedProfits = 0;

            retainedProfits = GetTheProductsOfMoneyOfSold(product) - GetTheProductsOfMoneyOfStock(product);

            return retainedProfits;
        }

        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
