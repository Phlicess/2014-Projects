using System;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SCZY.Common;

namespace SCZY.Home
{
    /// <summary>
    /// 登录验证类
    /// 用于线程调用 和处理登陆错误信息
    /// </summary>
    partial class Login
    {
        private string userName;
        private string password;
        static string result = "";
        private User user;
        private void Login_Commit(object sender, DoWorkEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            try
            {
                User _user = sczy.Users.FirstOrDefault(u => u.UserName == userName);
                user = _user;
            }
            catch (Exception)
            {
                MessageBox.Show("登陆超时!请检查网络!");
                return;
            }
            
            if (user == null)
            {
                result = "UserNameError";
                return;
            }

            if (password == "")
            {
                password = null;
            }

            //比对密码
            if (user.PassWord == MD5Hash.GetMd5Hash(password))
            {
                if (user.Role == "管理员")
                {
                    UserInfo userInfo = new UserInfo();
                    userInfo.ID = user.ID;
                    userInfo.Name = user.Name;
                    userInfo.UserName = user.UserName;
                    userInfo.Role = user.Role;
                    result = "";
                }
                if (user.Role == "文员")
                {
                    UserInfo userInfo = new UserInfo();
                    userInfo.ID = user.ID;
                    userInfo.Name = user.Name;
                    userInfo.UserName = user.UserName;
                    userInfo.Role = user.Role;
                    result = "";
                }
                if (user.Role == "财务")
                {
                    UserInfo userInfo = new UserInfo();
                    userInfo.ID = user.ID;
                    userInfo.Name = user.Name;
                    userInfo.UserName = user.UserName;
                    userInfo.Role = user.Role;
                    result = "";
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Storyboard sb = new Storyboard();
                    DoubleAnimation dh = new DoubleAnimation();
                    DoubleAnimation dw = new DoubleAnimation();
                    DoubleAnimation dop = new DoubleAnimation();
                    dop.Duration = dh.Duration = dw.Duration = sb.Duration = new Duration(new TimeSpan(0, 0, 2));
                    dop.To = dh.To = dw.To = 0;
                    Storyboard.SetTarget(dop, this);
                    Storyboard.SetTarget(dh, this);
                    Storyboard.SetTarget(dw, this);
                    Storyboard.SetTargetProperty(dop, new PropertyPath("Opacity", new object[] { }));
                    Storyboard.SetTargetProperty(dh, new PropertyPath("Height", new object[] { }));
                    Storyboard.SetTargetProperty(dw, new PropertyPath("Width", new object[] { }));
                    sb.Children.Add(dh);
                    sb.Children.Add(dw);
                    sb.Children.Add(dop);
                    sb.Completed += new EventHandler(sb_Completed);   //(a, b) => { this.Close(); };
                    sb.Begin();
                }));
            }
            else
            {
                result = "PasswordError";
                //LoadingImage.Opacity = 0;
            }
        }


        /// <summary>
        /// 处理用户名或者密码错误的函数 *也就是提示用户名 密码错误的消息*
        /// </summary>
        private bool CommitInfoError()
        {
            if (result == "UserNameError")
            {
                UserNameImage.Opacity = 1;
                PasswordImage.Opacity = 0;
                UserName.IsEnabled = Password.IsEnabled = Commit.IsEnabled = true;
                Keyboard.Focus(UserName);
                LoadingImage.Visibility = Visibility.Collapsed;
                return false;
            }
            if(result == "PasswordError")
            {
                UserNameImage.Opacity = 0;
                PasswordImage.Opacity = 1;
                UserName.IsEnabled = Password.IsEnabled = Commit.IsEnabled = true;
                Keyboard.Focus(Password);
                return false;
            }
            return true;
        }
    }




    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            Loaded += (MainWindow_Loaded);
        }

        //输入框获取焦点
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(UserName);
            Thread thread = new Thread(InitDB);
            thread.Start();
        }

        private void InitDB(object obj)
        {
            SCZYEntities sczy = new SCZYEntities();
            try
            {
                sczy.Users.Count();
            }
            catch (Exception)
            {
                return;
            }
        }

        //变换焦点
        private void ChangeFocus(object sender, KeyEventArgs k)
        {
            if (k.Key == Key.Enter)
            {
                Keyboard.Focus(Password);
            }
        }


        //建立静态变量
        //public static string[] UserInfo = {};
        

        //关闭登录对话框 弹出主对话框
        private void Close_NewWin(object sender, RunWorkerCompletedEventArgs eventHandler)
        {
            if (!CommitInfoError() || UserInfo.name == "")
            {
                LoadingImage.Visibility = Visibility.Collapsed;
                UserName.IsEnabled = Password.IsEnabled = Commit.IsEnabled = true;
                return;
            }
            
            //WinCloseAnimate();  //登录窗口关闭动画
            MainWindow wm = new MainWindow();
            wm.Show();
            WinOpenAnimate(wm);  //打开主窗口窗口动画
            Close();
        }


        //登录同时New一个线程去验证登录信息
        private void LoginCommit(object sender, RoutedEventArgs e)
        {
            LoadingAnimate();

            string userName = UserName.Text;
            string password = Password.Password;
            Login login_Commit = new Login();
            login_Commit.userName = userName;
            login_Commit.password = password;
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (login_Commit.Login_Commit);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerCompleted += (Close_NewWin);
            backgroundWorker.RunWorkerAsync();
        }

        

        //绑定回车按钮
        private void BindingEnter(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    LoginCommit(null, null);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        //窗口拖动函数
        private void LoginDrag(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Opacity = 0.7;
                ResizeMode = ResizeMode.CanMinimize;
                DragMove();
            }
            Opacity = 0.97;
        }

        
        //最小化事件
        private void MinWin_Click(object sender, MouseEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //关闭事件
        private void CloseWin_Click(object sender, MouseEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }


        //超链接
        private void BindingCopyRight(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.valueteam.sinaapp.com/");
        }


        /// <summary>
        /// 等待加载动画
        /// </summary>
        private void LoadingAnimate()
        {
            LoadingImage.Visibility = Visibility.Visible;
            UserName.IsEnabled = Password.IsEnabled = Commit.IsEnabled = false;   //验证信息的时候让窗口不可被点击 防止多次登录
            RotateTransform rtf = new RotateTransform();
            LoadingImage.RenderTransform = rtf;
            DoubleAnimation dbAscending = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(0.8)));
            Storyboard storyboard = new Storyboard();
            dbAscending.RepeatBehavior = RepeatBehavior.Forever;
            storyboard.Children.Add(dbAscending);
            Storyboard.SetTarget(dbAscending, LoadingImage);
            Storyboard.SetTargetProperty(dbAscending, new PropertyPath("RenderTransform.Angle"));
            storyboard.Begin();
        }
        
        /// <summary>
        /// 登录窗口关闭动画
        /// </summary>
        private void WinCloseAnimate()
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation dh = new DoubleAnimation();
            DoubleAnimation dw = new DoubleAnimation();
            DoubleAnimation dop = new DoubleAnimation();
            dop.Duration = dh.Duration = dw.Duration = sb.Duration = new Duration(new TimeSpan(0, 0, 2));
            dop.To = dh.To = dw.To = 0;
            Storyboard.SetTarget(dop, this);
            Storyboard.SetTarget(dh, this);
            Storyboard.SetTarget(dw, this);
            Storyboard.SetTargetProperty(dop, new PropertyPath("Opacity", new object[] { }));
            Storyboard.SetTargetProperty(dh, new PropertyPath("Height", new object[] { }));
            Storyboard.SetTargetProperty(dw, new PropertyPath("Width", new object[] { }));
            sb.Children.Add(dh);
            sb.Children.Add(dw);
            sb.Children.Add(dop);
            sb.Completed += new EventHandler(sb_Completed);   //(a, b) => { this.Close(); };
            sb.Begin();
        }
        void sb_Completed(object sender, EventArgs e)
        {
            Close();
        }



        /// <summary>
        /// 打开主窗口动画
        /// </summary>
        private void WinOpenAnimate(Window Window)
        {
            DoubleAnimation winAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(winAnimation);
            Storyboard.SetTarget(winAnimation, Window);
            Storyboard.SetTargetProperty(storyboard, new PropertyPath("Opacity"));
            storyboard.Begin();
        }
    }
}
