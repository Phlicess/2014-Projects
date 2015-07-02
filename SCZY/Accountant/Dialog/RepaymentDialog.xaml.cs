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
    /// RepaymentDialog.xaml 的交互逻辑
    /// </summary>
    public partial class RepaymentDialog : Window
    {
        private Consumer consumer = new Consumer();
        private string type = null;
        public RepaymentDialog(string typeDialog,object textConsumer)
        {
            type = typeDialog;
            consumer = (Consumer)textConsumer;
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

            double? total = 0;
            double? paided = 0;
            double? leaving = 0;
            SCZYEntities sczy = new SCZYEntities();
            List<Order> orders = new List<Order>();
            orders = sczy.Orders.Where(p => p.Consumer_ID == consumer.ID && p.OrderState == "已划价").ToList();
            if (orders.Count != 0)
            {
                foreach (Order order in orders)
                {
                    double? cash;
                 //   ViewOrder viewOrder = new ViewOrder();
                    if (order.Paid != null)
                    {
                        paided += order.Paid;
                    }                   
                    GetOrderAmount gerGetOrderAmount = new GetOrderAmount(order);
                    cash = gerGetOrderAmount.GetAmount();
                    if (cash != null)
                    {
                        total += cash;
                    }          
                }               
            }
            leaving += (total - paided);
            ConsumerName.Text = consumer.Name;
            Company.Text = consumer.Company;
            Address.Text = consumer.Address;
            Tel.Text = consumer.Tel;
            Phone.Text = consumer.Phone;
            //Paid.Text = paided.ToString();
            Leaving.Text = DataChange.CutDouble(leaving).ToString();
            Total.Text = DataChange.CutDouble(total).ToString();
            Remark.Text = consumer.Remark;
        }


        private void SaveWindowClick(object sender, RoutedEventArgs e)
        {

            if (Paid.Text == null || Paid.Text == "")
            {
                MessageBox messageBox = new MessageBox("输入值不能为负数");
                messageBox.ShowDialog();
                return;
            }
            if (Paid.Text != null && Paid.Text != "")
            { 
                if (type == "repay")
                {
                    RepayOnClick();

                }
                if (type == "editRepay")
                {
                    EditRepay();
                }                
            }          
        }


        public void RepayOnClick()
        {
            if (double.Parse(Paid.Text) < 0)
            {
                MessageBox messageBox = new MessageBox("输入值不能为负数");
                messageBox.ShowDialog();
                return;
            }
            SCZYEntities sczy = new SCZYEntities();
            List<Order> orders = new List<Order>();
            double? cash = double.Parse(Paid.Text);
           // orders = sczy.Orders.Where(p => p.Consumer_ID == consumer.ID).OrderBy(p =>p.ID).ToList();
            orders = sczy.Orders.Where(p => p.Consumer_ID == consumer.ID && p.OrderState == "已划价").OrderBy(p => p.ID).ToList();
            double total = 0;
            foreach (Order order in orders)
            {
                GetOrderAmount gerGetOrderAmount = new GetOrderAmount(order);
                total = (double)gerGetOrderAmount.GetAmount();
                if (total > order.Paid)
                {
                    double? difference = total - order.Paid;
                    if (difference > cash)
                    {
                        order.Paid += cash;
                        cash = 0;
                        ///添加还款的细节记录
                        List<OrderMoneyDetail> orderMoneyDetails = new List<OrderMoneyDetail>();
                        orderMoneyDetails = sczy.OrderMoneyDetails.Where(p => p.ID == order.ID).ToList();
                        int i = 0;
                        for (; i < orderMoneyDetails.Count; i++)
                        {
                            System.DateTime currentTime = new System.DateTime();
                            System.DateTime orderDataTime = new System.DateTime();
                            currentTime = System.DateTime.Now;
                            orderDataTime = (DateTime)orderMoneyDetails[i].Date;
                            string strYMD = currentTime.ToString("d");
                            string strOrder = orderDataTime.ToString("d");
                            if (strYMD == strOrder)
                            {
                                orderMoneyDetails[i].Paid = orderMoneyDetails[i].Paid + difference;
                                break;
                            }
                        }
                        if (i >= orderMoneyDetails.Count)
                        {
                            System.DateTime currentTime = new System.DateTime();
                            currentTime = System.DateTime.Now;
                            OrderMoneyDetail orderMoneyDetail = new OrderMoneyDetail();
                            orderMoneyDetail.Remark = Remark.Text;
                            orderMoneyDetail.Date = currentTime;
                            orderMoneyDetail.Paid = difference;
                            orderMoneyDetail.Order_ID = order.ID;
                            sczy.OrderMoneyDetails.Add(orderMoneyDetail);
                        }
                        break;
                    }
                    if (difference <= cash)
                    {
                        order.Paid = total;
                        cash = cash - difference;

                        ///添加还款的细节记录
                        List<OrderMoneyDetail> orderMoneyDetails = new List<OrderMoneyDetail>();
                        orderMoneyDetails = sczy.OrderMoneyDetails.Where(p => p.ID == order.ID).ToList();
                        int i = 0;
                        for (; i < orderMoneyDetails.Count; i++)
                        {
                            System.DateTime currentTime = new System.DateTime();
                            System.DateTime orderDataTime = new System.DateTime();
                            currentTime = System.DateTime.Now;
                            orderDataTime = (DateTime)orderMoneyDetails[i].Date;
                            string strYMD = currentTime.ToString("d");
                            string strOrder = orderDataTime.ToString("d");
                            if (strYMD == strOrder)
                            {
                                orderMoneyDetails[i].Paid = difference;
                                break;
                            }
                        }
                        if (i >= orderMoneyDetails.Count)
                        {
                            System.DateTime currentTime = new System.DateTime();
                            currentTime = System.DateTime.Now;
                            OrderMoneyDetail orderMoneyDetail = new OrderMoneyDetail();
                            orderMoneyDetail.Remark = Remark.Text;
                            orderMoneyDetail.Date = currentTime;
                            orderMoneyDetail.Paid = difference;
                            orderMoneyDetail.Order_ID = order.ID;
                            sczy.OrderMoneyDetails.Add(orderMoneyDetail);
                        }
                    }
                }
            }
            if (cash > 0)
            {
                int count = orders.Count;
                orders[count -1].Paid = orders[count -1].Paid - cash;
            }
            sczy.SaveChanges();

            this.Close();
        }

        /// <summary>
        /// 减少还款
        /// </summary>
        public void EditRepay()
        {
            if (double.Parse(Paid.Text) < 0)
            {
                MessageBox messageBox = new MessageBox("输入值不能为负数");
                messageBox.ShowDialog();
                return;
            }
            SCZYEntities sczy = new SCZYEntities();
            List<Order> orders = new List<Order>();
            double? cash = double.Parse(Paid.Text);

           // orders = sczy.Orders.Where(p => p.Consumer_ID == consumer.ID).OrderByDescending(p=>p.ID).ToList();
            orders = sczy.Orders.Where(p => p.Consumer_ID == consumer.ID && p.OrderState == "已划价").OrderBy(p => p.ID).OrderByDescending(p => p.ID).ToList();
            foreach (var order in orders)
            {
                double total = 0;
                GetOrderAmount gerGetOrderAmount = new GetOrderAmount(order);
                total = (double)gerGetOrderAmount.GetAmount();
                if (order.Paid >= total)
                {
                    if (order.Paid >= cash)
                    {
                        order.Paid = order.Paid - cash;
                        cash = 0;

                        ///添加还款的细节记录
                        List<OrderMoneyDetail> orderMoneyDetails = new List<OrderMoneyDetail>();
                        orderMoneyDetails = sczy.OrderMoneyDetails.Where(p => p.ID == order.ID).ToList();
                        int i = 0;
                        for (; i < orderMoneyDetails.Count; i++)
                        {
                            System.DateTime currentTime = new System.DateTime();
                            System.DateTime orderDataTime = new System.DateTime();
                            currentTime = System.DateTime.Now;
                            orderDataTime = (DateTime)orderMoneyDetails[i].Date;
                            string strYMD = currentTime.ToString("d");
                            string strOrder = orderDataTime.ToString("d");
                            if (strYMD == strOrder)
                            {
                                orderMoneyDetails[i].Paid = orderMoneyDetails[i].Paid - cash;
                                break;
                            }
                        }
                        if (i >= orderMoneyDetails.Count)
                        {
                            System.DateTime currentTime = new System.DateTime();
                            currentTime = System.DateTime.Now;
                            OrderMoneyDetail orderMoneyDetail = new OrderMoneyDetail();
                            orderMoneyDetail.Remark = Remark.Text;
                            orderMoneyDetail.Date = currentTime;
                            orderMoneyDetail.Paid = (-cash);
                            orderMoneyDetail.Order_ID = order.ID;
                            sczy.OrderMoneyDetails.Add(orderMoneyDetail);
                        }

                        break;
                    }
                    if (order.Paid < cash)
                    {
                        cash = cash - order.Paid;
                        order.Paid = 0;

                        ///添加还款的细节记录
                        List<OrderMoneyDetail> orderMoneyDetails = new List<OrderMoneyDetail>();
                        orderMoneyDetails = sczy.OrderMoneyDetails.Where(p => p.ID == order.ID).ToList();
                        int i = 0;
                        for (; i < orderMoneyDetails.Count; i++)
                        {
                            System.DateTime currentTime = new System.DateTime();
                            System.DateTime orderDataTime = new System.DateTime();
                            currentTime = System.DateTime.Now;
                            orderDataTime = (DateTime)orderMoneyDetails[i].Date;
                            string strYMD = currentTime.ToString("d");
                            string strOrder = orderDataTime.ToString("d");
                            if (strYMD == strOrder)
                            {
                                orderMoneyDetails[i].Paid = orderMoneyDetails[i].Paid - cash;
                                break;
                            }
                        }
                        if (i >= orderMoneyDetails.Count)
                        {
                            System.DateTime currentTime = new System.DateTime();
                            currentTime = System.DateTime.Now;
                            OrderMoneyDetail orderMoneyDetail = new OrderMoneyDetail();
                            orderMoneyDetail.Remark = Remark.Text;
                            orderMoneyDetail.Date = currentTime;
                            orderMoneyDetail.Paid = orderMoneyDetail.Paid - cash;
                            orderMoneyDetail.Order_ID = order.ID;
                            sczy.OrderMoneyDetails.Add(orderMoneyDetail);
                        }
                    }
                }
            }
            if (cash > 0)
            {
              //  int count = orders.Count;
                orders[0].Paid = orders[0].Paid - cash;
            }
            sczy.SaveChanges();
            this.Close();
        }
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
