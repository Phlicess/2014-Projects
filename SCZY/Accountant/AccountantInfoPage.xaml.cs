using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SCZY.Common;

namespace SCZY.Accountant
{
    /// <summary>
    /// AccountantInfoPage.xaml 的交互逻辑
    /// </summary>
    public partial class AccountantInfoPage : Page
    {
        public AccountantInfoPage()
        {
            InitializeComponent();
            Init();
        }
        /// <summary>
        /// 初始化用户数据
        /// </summary>
        public void Init()
        {
            SCZYEntities sczy = new SCZYEntities();
            decimal userID = UserInfo.id;
            User user = sczy.Users.FirstOrDefault(u => u.ID == userID);
            NameBox.Text = user.Name;
            PhoneBox.Text = user.Phone;
            RemarkBox.Text = user.Remark;
            UserNameBox.Text = user.UserName;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            decimal userID = UserInfo.id;
            User user = sczy.Users.FirstOrDefault(u => u.ID == userID);

            if (String.IsNullOrEmpty(UserNameBox.Text))
            {
                UserNameBox.Focus();
                UserNameWarning.Text = "* 不能为空";
                return;
            }

            if (String.IsNullOrEmpty(NameBox.Text))
            {
                NameBox.Focus();
                NameWarning.Text = "* 不能为空";
                return;
            }
            user.Name = NameBox.Text;
            user.Phone = PhoneBox.Text;
            user.Remark = RemarkBox.Text;
            user.UserName = UserNameBox.Text;

            sczy.SaveChanges();

            Accountant.Dialog.MessageBox mb = new Accountant.Dialog.MessageBox("修改成功..", new Timer(1000));

            mb.ShowDialog();

            NameWarning.Text = "";
            UserNameWarning.Text = "";
        }


        /// <summary>
        /// 保存密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassworkBlock_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            decimal userID = UserInfo.id;
            User user = sczy.Users.FirstOrDefault(u => u.ID == userID);

            //判断密码
            if (MD5Hash.GetMd5Hash(OldPasswordBox.Password) == user.PassWord)
            {
                //新密码不一致
                if (NewPasswordBox.Password != ReNewPasswordBox.Password)
                {
                    NewPasswordBox.Focus();
                    NewPasswordBoxWarning.Text = "* 两次密码不一致";
                    return;
                }
                //一致 加密保存
                if ((NewPasswordBox.Password == ReNewPasswordBox.Password))
                {
                    user.PassWord = MD5Hash.GetMd5Hash(ReNewPasswordBox.Password);
                    sczy.SaveChanges();
                    OldPasswordBox.Password = "";
                    NewPasswordBox.Password = "";
                    ReNewPasswordBox.Password = "";

                    OldPPasswordWarning.Text = "";
                    NewPasswordBoxWarning.Text = "";
                    Accountant.Dialog.MessageBox mb = new Accountant.Dialog.MessageBox("修改成功..", new Timer(1000));
                    mb.ShowDialog();
                    return;
                }
            }
            //原密码比对出错
            OldPasswordBox.Focus();
            OldPPasswordWarning.Text = "* 原密码错误";
        }
    }
}
