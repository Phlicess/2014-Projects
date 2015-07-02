using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SCZY.Admin.ConsumerDialog
{
    //定义委托 刷新父窗口的ProductListView的数据
    public delegate void SaveConsumerHander(); 
    /// <summary>
    /// EditConsumer.xaml 的交互逻辑
    /// </summary>
    public partial class EditConsumer : Window
    {
        private decimal consumerID;
        private Consumer consumer;
        public string type = "Add";
        public EditConsumer(object selectConsumer)
        {
            InitializeComponent();
            consumer = selectConsumer as Consumer;
            
            Loaded += (sender, args) =>
            {
                SetTitle_Size();
                if (type == "Edit")
                {
                    consumerID = consumer.ID;
                    //如果是编辑订单窗口，就为窗口赋值
                    SetData(selectConsumer as Consumer);
                }
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
        /// 给弹出框赋值
        /// </summary>
        /// <param name="selectedConsumer"></param>
        public void SetData(Consumer selectedConsumer)
        {
            consumer = selectedConsumer;
            ConsumerName.Text = selectedConsumer.Name;
            Phone.Text = selectedConsumer.Phone;
            Tel.Text = selectedConsumer.Tel;
            CompanyName.Text = selectedConsumer.Company;
            Address.Text = selectedConsumer.Address;
            Remark.Text = selectedConsumer.Remark;
        }

        //定义委托事件
        public event SaveConsumerHander SaveConsumer;

        /// <summary>
        /// 保存客户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            if (type == "Edit")
            {
                Consumer oldConsumer = sczy.Consumers.FirstOrDefault(o => o.ID == consumerID);
                oldConsumer.Phone = Phone.Text;
                oldConsumer.Name = ConsumerName.Text;
                oldConsumer.Address = Address.Text;
                oldConsumer.Company = CompanyName.Text;
                oldConsumer.Tel = Tel.Text;
                oldConsumer.Remark = Remark.Text;

                sczy.SaveChanges();
                Close();
            }
            else if (type == "Add")
            {
                Consumer newConsumer = new Consumer();
                newConsumer.Phone = Phone.Text;
                newConsumer.Name = ConsumerName.Text;
                newConsumer.Address = Address.Text;
                newConsumer.Company = CompanyName.Text;
                newConsumer.Tel = Tel.Text;
                newConsumer.Remark = Remark.Text;

                sczy.Consumers.Add(newConsumer);
                sczy.SaveChanges();
                Close();

            }
            if (SaveConsumer != null)
            {
                SaveConsumer();
            }
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
