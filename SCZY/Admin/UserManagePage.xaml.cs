using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SCZY.Admin.UserDialog;
using SCZY.Common;
using SCZY.OtherWindows;

namespace SCZY.Admin
{
    /// <summary>
    /// UserManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagePage : Page
    {
        public UserManagePage()
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
            List<User> userList = sczy.Users.ToList();

            PendingUser.Items.Clear();
            PendingUser.ItemsSource = userList;

            gridPager.ShowPages(PendingUser, userList, 5);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            EditUser editUserWindow = new EditUser(null);
            editUserWindow.type = "Add";
            editUserWindow.Title = "添加用户";
            editUserWindow.SaveUser += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<User> userList = sczy.Users.ToList();
                PendingUser.ItemsSource = userList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editUserWindow.Owner = window;
            editUserWindow.ShowDialog();
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdiItem_OnClick(object sender, RoutedEventArgs e)
        {
            User selectedUser = PendingUser.SelectedItem as User;
            if (selectedUser == null)
            {
                return;
            }

            EditUser editUserWindow = new EditUser(selectedUser);
            editUserWindow.type = "Edit";
            editUserWindow.SaveUser += delegate
            {
                SCZYEntities sczy = new SCZYEntities();
                List<User> userList = sczy.Users.ToList();
                PendingUser.ItemsSource = userList;  //读取用户并刷新datagrid
            };
            Window window = Window.GetWindow(this);
            editUserWindow.Owner = window;
            editUserWindow.ShowDialog();
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelItem_OnClick(object sender, RoutedEventArgs e)
        {
            User userInfo = PendingUser.SelectedItem as User;
            if (userInfo == null)
            {
                return;
            }

            decimal ID = userInfo.ID;

            SCZYEntities sczy = new SCZYEntities();
            User selectedUser = sczy.Users.FirstOrDefault(c => c.ID == ID);

            //弹出警告框
            WanningWindow wanningWindow = new WanningWindow();
            Window window = Window.GetWindow(this);
            wanningWindow.Owner = window;
            wanningWindow.TipBlock.Text = "删除这个用户, You Sure?";
            wanningWindow.ShowDialog();
            if (!wanningWindow.GetValueForButton())
            {
                return;
            }

            sczy.Users.Remove(selectedUser);
            sczy.SaveChanges();

            List<User> userList = sczy.Users.ToList();
            PendingUser.ItemsSource = userList;
        }


        /// <summary>
        /// 查找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeacherBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<User> userList = new List<User>();

            //获取查找的条件
            string para = SeacherBox.Text;
            if (String.IsNullOrEmpty(para))
            {
                //重新绑定所有数据
                SCZYEntities sczy = new SCZYEntities();
                userList = sczy.Users.ToList();

                PendingUser.ItemsSource = userList;
                return;
            }
            User pro = new User();
            userList = LikeSeacher.SeacherLike(pro, para, "[User]");

            PendingUser.ItemsSource = userList;
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
            List<User> userList = sczy.Users.ToList();

            PendingUser.ItemsSource = userList;
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
