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

namespace SCZY.Accountant.Dialog
{
    /// <summary>
    /// DebtMemoryRecordDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DebtMemoryRecordDialog : Window
    {
        private Order order;
        public DebtMemoryRecordDialog(object orderObject)
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                SetTitle_Size();
            };
            order = (Order)orderObject;//父窗口的订单信息
            GetScheduled();//获取数据
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        public void GetScheduled()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<OrderMoneyDetail> orderMoneyDetails = new List<OrderMoneyDetail>();
            orderMoneyDetails = sczy.OrderMoneyDetails.Where(p => p.Order_ID == order.ID).ToList();

            OrderDetailsListView.Items.Clear();
            OrderDetailsListView.ItemsSource = orderMoneyDetails.OrderBy(p => p.Date);

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
        /// 取消按钮的响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
