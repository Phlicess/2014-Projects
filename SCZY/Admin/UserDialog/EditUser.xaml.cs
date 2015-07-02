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

namespace SCZY.Admin.UserDialog
{
    //定义委托 刷新父窗口的ProductListView的数据
    public delegate void SaveUserHander(); 
    /// <summary>
    /// EditUser.xaml 的交互逻辑
    /// </summary>
    public partial class EditUser : Window
    {
        private decimal userID;
        private User user;
        public string type = "Add";
        public EditUser(object selectUser)
        {
            InitializeComponent();
            user = selectUser as User;

            Loaded += (sender, args) =>
            {
                SetTitle_Size();
                Dictionary<string, string> roleDictionary = new Dictionary<string, string>()
                {
                    {"财务", "财务"},
                    {"文员", "文员"}
                };
                Role.ItemsSource = roleDictionary;
                Role.DisplayMemberPath = "Value";
                Role.SelectedValuePath = "Key";
                

                if (type == "Edit")
                {
                    userID = user.ID;
                    //如果是编辑供应商窗口，就为窗口赋值
                    SetData(selectUser as User);
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
        /// <param name="selectedUser"></param>
        public void SetData(User selectedUser)
        {
            user = selectedUser;
            UserName.Text = selectedUser.UserName;
            Name.Text = selectedUser.Name;
            Role.Text = selectedUser.Role;
            Phone.Text = selectedUser.Phone;
            Role.Text = selectedUser.Role;
            Remark.Text = selectedUser.Remark;
        }

        //定义委托事件
        public event SaveUserHander SaveUser;

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
                User oldUser = sczy.Users.FirstOrDefault(o => o.ID == userID);
                oldUser.Phone = Phone.Text;
                oldUser.UserName = UserName.Text;
                oldUser.Name = Name.Text;
                oldUser.Role = Role.Text;
                oldUser.PassWord = MD5Hash.GetMd5Hash(PassWord.Password);
                oldUser.Remark = Remark.Text;

                sczy.SaveChanges();
                Close();
            }
            else if (type == "Add")
            {
                User newUser = new User();
                newUser.Phone = Phone.Text;
                newUser.UserName = UserName.Text;
                newUser.Name = Name.Text;
                newUser.Role = Role.Text;
                newUser.PassWord = MD5Hash.GetMd5Hash(PassWord.Password);
                newUser.Remark = Remark.Text;

                sczy.Users.Add(newUser);
                sczy.SaveChanges();
                Close();

            }
            if (SaveUser != null)
            {
                SaveUser();
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
