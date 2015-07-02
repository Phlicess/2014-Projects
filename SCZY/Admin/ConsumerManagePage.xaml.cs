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
using SCZY.Admin.ConsumerDialog;
using SCZY.Common;
using SCZY.OtherWindows;

namespace SCZY.Admin
{
    /// <summary>
    /// ConsumerManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class ConsumerManagePage : Page
    {
        public ConsumerManagePage()
        {
            InitializeComponent();

            Infinite();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Infinite()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<Consumer> consumerList = sczy.Consumers.Where(c => c.State == true).ToList();

            PendingConsumer.Items.Clear();
            PendingConsumer.ItemsSource = consumerList;

            gridPager.ShowPages(PendingConsumer, consumerList, 5);
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            EditConsumer editConsumerWindow = new EditConsumer(null);
            editConsumerWindow.type = "Add";
            editConsumerWindow.Title = "添加客户";
            editConsumerWindow.SaveConsumer += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<Consumer> consumerList = sczy.Consumers.ToList();
                PendingConsumer.ItemsSource = consumerList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editConsumerWindow.Owner = window;
            editConsumerWindow.ShowDialog();
        }

        /// <summary>
        /// 修改客户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdiItem_OnClick(object sender, RoutedEventArgs e)
        {
            Consumer selectedConsumer = PendingConsumer.SelectedItem as Consumer;
            if (selectedConsumer == null)
            {
                return;
            }

            EditConsumer editConsumerWindow = new EditConsumer(selectedConsumer);
            editConsumerWindow.type = "Edit";
            editConsumerWindow.SaveConsumer += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<Consumer> consumerList = sczy.Consumers.ToList();
                PendingConsumer.ItemsSource = consumerList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editConsumerWindow.Owner = window;
            editConsumerWindow.ShowDialog();
        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            Consumer consumerInfo = PendingConsumer.SelectedItem as Consumer;
            if (consumerInfo == null)
            {
                return;
            }

            decimal ID = consumerInfo.ID;

            SCZYEntities sczy = new SCZYEntities();
            Consumer selectedConsumer = sczy.Consumers.FirstOrDefault(c => c.ID == ID);

            //弹出警告框
            WanningWindow wanningWindow = new WanningWindow();
            Window window = Window.GetWindow(this);
            wanningWindow.Owner = window;
            wanningWindow.TipBlock.Text = "删除这个客户, You Sure?";
            wanningWindow.ShowDialog();
            if (!wanningWindow.GetValueForButton())
            {
                return;
            }

            selectedConsumer.State = false;
            //sczy.Consumers.Remove(selectedConsumer);
            sczy.SaveChanges();

            List<Consumer> consumerList = sczy.Consumers.Where(c => c.State == true).ToList();
            PendingConsumer.ItemsSource = consumerList;
        }


        /// <summary>
        /// 查找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<Consumer> consumerList = new List<Consumer>();

            //获取查找的条件
            string para = SeacherBox.Text;
            if (String.IsNullOrEmpty(para))
            {
                //重新绑定所有数据
                SCZYEntities sczy = new SCZYEntities();
                consumerList = sczy.Consumers.ToList();

                PendingConsumer.ItemsSource = consumerList;
                return;
            }
            Consumer pro = new Consumer();
            consumerList = LikeSeacher.SeacherLike(pro, para, "Consumer");

            PendingConsumer.ItemsSource = consumerList;
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
            List<Consumer> consumerList = sczy.Consumers.ToList();

            PendingConsumer.ItemsSource = consumerList;
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
