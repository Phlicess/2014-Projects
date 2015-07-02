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
    /// BenifitMange.xaml 的交互逻辑
    /// </summary>
    public partial class BenifitMange : Page
    {
       
        public BenifitMange()
        {
            InitializeComponent();
            
            //Loaded += (sender, args) =>
            //{
            //    messageBox.ShowDialog();
            //};
            GetScheduled();//获取数据
            
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public void GetScheduled()
        {
            double moneyOfStock = 0;//每月进货钱数
            double moneyOfSold = 0;//每月卖货钱数
            double retainedProfits = 0;//每月净利润
            //double grossProfits = 0;//每月毛利润
            Dialog.MessageBox messageBox = new Dialog.MessageBox("拼命加载中...", "");
            messageBox.Show();
            
            SCZYEntities sczy = new SCZYEntities();
            List<ViewBenefit> viewBenefits = new List<ViewBenefit>();

            List<DateTime> monthTimes = new List<DateTime>();
            List<DateTime> months = new List<DateTime>(); //具体到月份的时间
            monthTimes = GetAllMonths();//所有的时间

            foreach (DateTime item in monthTimes)
            {
                DateTime dateTime = new DateTime(item.Year,item.Month,1);
                months.Add(dateTime);
            }
          //  months = months.Distinct();
            foreach (DateTime item in months.Distinct())
            {
                ViewBenefit viewBenefit = new ViewBenefit();//datagril显示需要的数据
                moneyOfStock = GetMoneyOfStock(item);//获得每月的进货钱数
                moneyOfSold = GetMoneyOfSold(item);//获得每月卖货的钱数
                //retainedProfits = GetRetainedProfits(moneyOfStock,moneyOfSold);//获得每月净利润
                retainedProfits = moneyOfSold - moneyOfStock;

                //grossProfits = GetGrossProdits(item);//获得每月毛利润

                viewBenefit.Month = item;
                viewBenefit.MoneyOfSold = moneyOfSold;
                viewBenefit.MoneyOfStock = moneyOfStock;
                viewBenefit.RetainedProfits = retainedProfits;
                //viewBenefit.GrossProdits = grossProfits;

                viewBenefits.Add(viewBenefit);
            }

            messageBox.Close();
            // PendingOrder.Items.Refresh();
            BenefitGrid.Items.Clear();
            BenefitGrid.ItemsSource = viewBenefits;  //读取用户并刷新datagrid

            gridPager.ShowPages(BenefitGrid, viewBenefits, 5);
        }

        /// <summary>
        /// 获取所有时间
        /// </summary>
        /// <returns>所有时间</returns>
        public List<DateTime> GetAllMonths()
        {
            SCZYEntities sczy = new SCZYEntities();
           // List<ViewBenefit> viewBenefits = new List<ViewBenefit>();
            //获取数据库中所有的月份
            DateTime date = new DateTime();           
            List<DateTime> orderDateTimes = new List<DateTime>();

            List<Order> orders = sczy.Orders.Where(p => p.OrderState == "已划价").Distinct().ToList();
            foreach (var item in orders)
            {
                if (item.DeliveryDate !=null && item.DeliveryDate.ToString()!="")
                {
                    orderDateTimes.Add((DateTime)item.DeliveryDate);

                }

            }

           // List<DateTime> inStocksDateTimes = new List<DateTime>();
            List<InStock> inStocks = sczy.InStocks.Where(p => p.InStockState == "已划价").Distinct().ToList();
            foreach (var item in inStocks)
            {
                if (item.InStockDate != null && item.InStockDate.ToString() != "")
                {
                    orderDateTimes.Add((DateTime)item.InStockDate);
                }
            }

           // orderDateTimes.AddRange(inStocksDateTimes);//所有的月份  
            return orderDateTimes;
        }

        /// <summary>
        /// 获取每月的进货钱数
        /// </summary>
        /// <returns></returns>
        public double GetMoneyOfStock(DateTime time)
        {
            double moneyOfStock = 0;
            SCZYEntities sczy = new SCZYEntities();
            List<InStock> inStocks = new List<InStock>();
            inStocks = sczy.InStocks.Where(p => p.InStockState == "已划价").ToList();

            foreach (var inStock in inStocks)
            {
                if (inStock.InStockDate != null && inStock.InStockDate.ToString() != "")
                {
                     DateTime inStockDateTime = (DateTime)inStock.InStockDate;
                     DateTime dateTimeNow = new DateTime(inStockDateTime.Year,inStockDateTime.Month,1);
                     if (dateTimeNow == time)
                    {
                         List<InStockDetail> inStockDetails = new List<InStockDetail>();
                         inStockDetails = sczy.InStockDetails.Where(p => p.InStock_ID == inStock.ID).ToList();
                         foreach (var inStockDetail in inStockDetails)
                        {
                             //Product product = new Product();
                             //product = sczy.Products.FirstOrDefault(p => p.ID == inStockDetail.Product_ID);
                            if (inStockDetail.Price != null && inStockDetail.Weight != null)
                            {
                                moneyOfStock += (double)(inStockDetail.Price*inStockDetail.Weight);
                            }
                            
                        }
                    }
                }
               
            }
            moneyOfStock = DataChange.CutDouble(moneyOfStock);//约分数据
            return moneyOfStock;
        }

        /// <summary>
        /// 获取每月的卖货钱数
        /// </summary>
        /// <returns></returns>
        public double GetMoneyOfSold(DateTime time)
        {
            double montyOfSold = 0;
            SCZYEntities sczy = new SCZYEntities();
            List<Order> orders = new List<Order>();
            orders = sczy.Orders.Where(p => p.OrderState == "已划价").ToList();


            foreach (var order in orders)
            {
                if (order != null)
                {
                     if (order.DeliveryDate != null && order.DeliveryDate.ToString() != "")
                {
                    DateTime orderDeliveryDate = (DateTime)order.DeliveryDate;
                    DateTime dateTimeNow = new DateTime(orderDeliveryDate.Year, orderDeliveryDate.Month, 1);
                    if (dateTimeNow == time)
                    {
                        if (order.AggregateAmount != null)
                        {
                                // GetOrderAmount getOrderAmount = new GetOrderAmount(order);
                                montyOfSold += (double)order.AggregateAmount;
                        }                   
                    }
                }
                }
               
            }
            montyOfSold = DataChange.CutDouble(montyOfSold);//约分数据
            return montyOfSold;
        }

        /// <summary>
        /// 获取每月的净利润
        /// </summary>
        /// <returns></returns>
        public double GetRetainedProfits(double moneyOfStock,double moneyOfSold)
        {
            double retainedProfits = 0;

            retainedProfits = moneyOfSold - moneyOfStock;
            return retainedProfits;
        }
        ///// <summary>
        ///// 获取每月的毛利润
        ///// </summary>
        ///// <returns></returns>
        //public double GetGrossProdits(DateTime dateTime)
        //{
        //    return 0;
        //}

        /// <summary>
        /// 查看具体每种产品的收益
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductBenefit_Click(object sender, RoutedEventArgs e)
        {
            ViewBenefit viewBenefit = BenefitGrid.SelectedItem as ViewBenefit;
            if (viewBenefit.Month != null)
            {
                // Consumer consumer = (Consumer)sender;
                ProductOfBefitDialog productOfBefitDialog = new ProductOfBefitDialog(viewBenefit.Month);
                Window window = Window.GetWindow(this);
                productOfBefitDialog.Owner = window;
                productOfBefitDialog.ShowDialog();
            }
        }

        ///// <summary>
        ///// 查找事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    List<ViewBenefit> viewBenefits = new List<ViewBenefit>();

        //    //获取查找的条件
        //    string para = SeacherBox.Text;
        //    if (String.IsNullOrEmpty(para))
        //    {
        //        //重新绑定所有数据
        //        double moneyOfStock = 0;//每月进货钱数
        //        double moneyOfSold = 0;//每月卖货钱数
        //        double retainedProfits = 0;//每月净利润
        //        //double grossProfits = 0;//每月毛利润



        //        SCZYEntities sczy = new SCZYEntities();
               

        //        List<DateTime> monthTimes = new List<DateTime>();
        //        List<DateTime> months = new List<DateTime>(); //具体到月份的时间
        //        monthTimes = GetAllMonths();//所有的时间
        //        foreach (DateTime item in monthTimes)
        //        {
        //            // DateTime dateTime = new DateTime();
        //            //dateTime.AddYears(item.Year); ;
        //            //dateTime.AddMonths(item.Month);
        //            DateTime dateTime = new DateTime(item.Year, item.Month, 1);
        //            months.Add(dateTime);
        //        }
        //        //  months = months.Distinct();
        //        foreach (DateTime item in months.Distinct())
        //        {
        //            ViewBenefit viewBenefit = new ViewBenefit();//datagril显示需要的数据
        //            moneyOfStock = GetMoneyOfStock(item);//获得每月的进货钱数
        //            moneyOfSold = GetMoneyOfSold(item);//获得每月卖货的钱数
        //            retainedProfits = GetRetainedProfits(item);//获得每月净利润
        //            //grossProfits = GetGrossProdits(item);//获得每月毛利润

        //            viewBenefit.Month = item;
        //            viewBenefit.MoneyOfSold = moneyOfSold;
        //            viewBenefit.MoneyOfStock = moneyOfStock;
        //            viewBenefit.RetainedProfits = retainedProfits;
        //            //viewBenefit.GrossProdits = grossProfits;

        //            viewBenefits.Add(viewBenefit);
        //        }

        //        BenefitGrid.ItemsSource = viewBenefits;
        //        return;
        //    }
        //    ViewBenefit pro = new ViewBenefit();
        //    viewBenefits = LikeSeacher.SeacherLike(pro, para, "ViewBenefit");

        //    BenefitGrid.ItemsSource = viewBenefits;
        //}

        ///// <summary>
        ///// 重置所有
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ResetBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    //清空条件输入框的字符串
        //    SeacherBox.Text = null;
        //    SeacherBox.Focus();

        //    //重新绑定所有数据
        //    SCZYEntities sczy = new SCZYEntities();
        //    List<Consumer> consumerList = sczy.Consumers.ToList();

        //    BenefitGrid.ItemsSource = consumerList;
        //}

        ///// <summary>
        ///// 绑定回车键查找
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void SeacherBox_OnKeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        SeacherBtn_OnMouseLeftButtonUp(null, null);
        //    }
        //}
    }
}
