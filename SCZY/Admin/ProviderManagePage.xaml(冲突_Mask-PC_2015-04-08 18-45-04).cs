using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SCZY.Admin.ProviderDialog;
using SCZY.Common;
using SCZY.OtherWindows;

namespace SCZY.Admin
{
    /// <summary>
    /// ProviderManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class ProviderManagePage : Page
    {
        public ProviderManagePage()
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
            List<Provider> providerList = sczy.Providers.ToList();

            PendingProvider.Items.Clear();
            PendingProvider.ItemsSource = providerList;

            gridPager.ShowPages(PendingProvider, providerList, 5);
        }

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            EditProvider editProviderWindow = new EditProvider(null);
            editProviderWindow.type = "Add";
            editProviderWindow.Title = "添加供应商";
            editProviderWindow.SaveProvider += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<Provider> providerList = sczy.Providers.ToList();
                PendingProvider.ItemsSource = providerList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editProviderWindow.Owner = window;
            editProviderWindow.ShowDialog();
        }

        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdiItem_OnClick(object sender, RoutedEventArgs e)
        {
            Provider selectedProvider = PendingProvider.SelectedItem as Provider;
            if (selectedProvider == null)
            {
                return;
            }

            EditProvider editProviderWindow = new EditProvider(selectedProvider);
            editProviderWindow.type = "Edit";
            editProviderWindow.SaveProvider += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<Provider> providerList = sczy.Providers.ToList();
                PendingProvider.ItemsSource = providerList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editProviderWindow.Owner = window;
            editProviderWindow.ShowDialog();
        }

        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            Provider providerInfo = PendingProvider.SelectedItem as Provider;
            if (providerInfo == null)
            {
                return;
            }

            decimal ID = providerInfo.ID;

            SCZYEntities sczy = new SCZYEntities();
            Provider selectedProvider = sczy.Providers.FirstOrDefault(c => c.ID == ID);

            //弹出警告框
            WanningWindow wanningWindow = new WanningWindow();
            Window window = Window.GetWindow(this);
            wanningWindow.Owner = window;
            wanningWindow.TipBlock.Text = "删除这个供应商, You Sure?";
            wanningWindow.ShowDialog();
            if (!wanningWindow.GetValueForButton())
            {
                return;
            }

            sczy.Providers.Remove(selectedProvider);
            sczy.SaveChanges();

            List<Provider> providerList = sczy.Providers.ToList();
            PendingProvider.ItemsSource = providerList;
        }


        /// <summary>
        /// 查找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<Provider> providerList = new List<Provider>();

            //获取查找的条件
            string para = SeacherBox.Text;
            if (String.IsNullOrEmpty(para))
            {
                //重新绑定所有数据
                SCZYEntities sczy = new SCZYEntities();
                providerList = sczy.Providers.ToList();

                PendingProvider.ItemsSource = providerList;
                return;
            }
            Provider pro = new Provider();
            providerList = LikeSeacher.SeacherLike(pro, para, "Provider");

            PendingProvider.ItemsSource = providerList;
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
            List<Provider> providerList = sczy.Providers.ToList();

            PendingProvider.ItemsSource = providerList;
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
