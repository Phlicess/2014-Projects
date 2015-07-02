using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using SCZY.Common;
using SCZY.Home;

namespace SCZY
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool i;
        Rect rcnormal;//定义一个全局rect记录还原状态下窗口的位置和大小。
        public MainWindow()
        {
            InitializeComponent();

            SelectMenu();  //选择用户角色的菜单
            GetNickName();
            this.StateChanged += WinStateChanged;

            TitleDockPanel.PreviewMouseLeftButtonDown += (o, args) =>
            {
                if (args.ClickCount == 2)
                {
                    if (!i)
                    {
                        Left = -5;//设置位置
                        Top = -5;
                        Rect rc = SystemParameters.WorkArea;
                        Width = rc.Width + 10;
                        Height = rc.Height + 10;
                        i = true;
                        TitleDockPanel.Width = Width - 10;
                        MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Resize.png", UriKind.Relative));
                        MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Resize_Mo.png", UriKind.Relative));
                    }
                    else if (i)
                    {
                        Width = 1050;
                        Height = 700;
                        TitleDockPanel.Width = Width - 10;
                        i = false;
                        MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Max.png", UriKind.Relative));
                        MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Max_Mo.png", UriKind.Relative));
                    }
                }
            };
        }
        

        /// <summary>
        /// 获取用户昵称用于Hello ***
        /// </summary>
        private void GetNickName()
        {
            HelloTextBlock.Text = "你好:  " + UserInfo.name;
        }

        /// <summary>
        /// Mini菜单点击用户名转到个人资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            if (UserInfo.role == "文员")
            {
                ClerkInfo_Click(null, e);
            }
            else if (UserInfo.role == "财务")
            {
                AccountantInfo_Click(null, e);
            }
            else if (UserInfo.role == "管理员")
            {
                AdminInfo_Click(null, e);
            }
            else
            {
                MessageBox.Show("你没有权限进行此操作,请重新登录或者联系管理员..");
            }
        }


        //选择菜单栏按钮
        private void SelectMenu()
        {
            if (UserInfo.role == "管理员")
            {
                MenuPanel_Admin.Visibility = Visibility.Visible;
                MenuPanel_Accountant.Visibility = Visibility.Collapsed;
                MenuPanel_Clerk.Visibility = Visibility.Collapsed;
            }
            else if (UserInfo.role == "财务")
            {
                MenuPanel_Accountant.Visibility = Visibility.Visible;
                MenuPanel_Admin.Visibility = Visibility.Collapsed;
                MenuPanel_Clerk.Visibility = Visibility.Collapsed;
            }
            else if (UserInfo.role == "文员")
            {
                MenuPanel_Clerk.Visibility = Visibility.Visible;
                MenuPanel_Accountant.Visibility = Visibility.Collapsed;
                MenuPanel_Admin.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("你没有相关权限,请重新登录或者联系管理员..");
            }
        }


        //欢迎登陆
        private void Welcome_Click(object sender, RoutedEventArgs e)
        {
            //WelcomeImg.Source = new BitmapImage(new Uri("../Images/Menu_Mo.png", UriKind.Relative));
            MainFrame.Navigate(new Uri("../Home/Welcome.xaml", UriKind.Relative));
            ClerkSetBackground(Welcome_Clerk);
            AccountantSetBackground(Welcome_Accountant);
            AdminSetBackground(Welcome_Admin);
            TipTitleDP.Visibility = Visibility.Collapsed;
        }

        
        //Clerk数据录入员的菜单
        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Clerk/NewOrderPage.xaml", UriKind.Relative));  //当点击按钮的时候，Frame中的内容会变成相应的页面
            ClerkSetBackground(NewOrder);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "新的订单";
            TipTitleBlock.ToolTip = "未划价的订单";
            ClickMenuMainFrameAnimate();
        }

        private void OrderManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Clerk/OrderManagePage.xaml", UriKind.Relative));
            ClerkSetBackground(OrderManage);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "订单管理";
            TipTitleBlock.ToolTip = "已经划价的订单";
            ClickMenuMainFrameAnimate();
        }
        
        private void NewStock_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Clerk/NewInStockPage.xaml", UriKind.Relative));
            ClerkSetBackground(NewStock);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "新的库存";
            TipTitleBlock.ToolTip = "未划价的入库单";
            ClickMenuMainFrameAnimate();
        }
        private void StockManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Clerk/StockManagePage.xaml", UriKind.Relative));
            ClerkSetBackground(StockManage);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "库存管理";
            TipTitleBlock.ToolTip = "已经划价的入库单";
            ClickMenuMainFrameAnimate();
        }

        private void ClerkInfo_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Clerk/ClerkInfoPage.xaml", UriKind.Relative));
            ClerkSetBackground(ClerkInfo);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "个人资料";
            TipTitleBlock.ToolTip = "查看或修改个人资料";
            ClickMenuMainFrameAnimate();
        }

        /// <summary>
        /// 退出登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Login LoginWin = new Login();
            LoginWin.Show();
            Close();
        }


        
        /// <summary>
        /// 设置菜单背景颜色为灰色的函数
        /// </summary>
        /// <param name="white">菜单StackPanel的Name</param>
        private void ClerkSetBackground(DockPanel white)
        {
            //白色笔刷
            BrushConverter brush = new BrushConverter();
            Brush whiteBrush = brush.ConvertFromInvariantString("#fff") as Brush;
            //灰色笔刷
            Brush blackBrush = brush.ConvertFromInvariantString("#f0f0f1") as Brush;
            Welcome_Clerk.Background = blackBrush;
            NewOrder.Background = blackBrush;
            OrderManage.Background = blackBrush;
            NewStock.Background = blackBrush;
            StockManage.Background = blackBrush;
            ClerkInfo.Background = blackBrush;
            ClerkExit.Background = blackBrush;

            white.Background = whiteBrush;
        }

        //Accountant财务人员的菜单  Print_Click

        //private void Print_Click(object sender, RoutedEventArgs e)
        //{
        //    MainFrame.Navigate(new Uri("../Accountant/PrintPage.xaml", UriKind.Relative));  //当点击按钮的时候，Frame中的内容会变成相应的页面
        //    AccountantSetBackground(Print);
        //    TipTitleDP.Visibility = Visibility.Visible;
        //    TipTitleBlock.Text = "打印订单";
        //    ClickMenuMainFrameAnimate();
        //}

        private void OrderPricing_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Accountant/OrderPricingPage.xaml", UriKind.Relative));  //当点击按钮的时候，Frame中的内容会变成相应的页面
            AccountantSetBackground(OrderPricing);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "订单划价";
            ClickMenuMainFrameAnimate();
        }

        private void BenifitManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Accountant/BenifitManagePage.xaml", UriKind.Relative));
            AccountantSetBackground(BenifitManage);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "收益管理";
            ClickMenuMainFrameAnimate();
        }
        private void InStockPriced_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Accountant/InStockPricedPage.xaml", UriKind.Relative));
            AccountantSetBackground(InStockPriced);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "划价入库";
            ClickMenuMainFrameAnimate();
        }

        private void InStockPricing_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Accountant/InStockPricingPage.xaml", UriKind.Relative));
            AccountantSetBackground(InStockPricing);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "入库划价";
            ClickMenuMainFrameAnimate();
        }
        private void FinanceManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Accountant/FinanceManagePage.xaml", UriKind.Relative));
            AccountantSetBackground(FinanceManage);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "财务管理";
            ClickMenuMainFrameAnimate();
        }

        private void AccountantInfo_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Accountant/AccountantInfoPage.xaml", UriKind.Relative));
            AccountantSetBackground(AccountantInfo);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "个人资料";
            TipTitleBlock.ToolTip = "查看或修改个人资料";
            ClickMenuMainFrameAnimate();
        }


        private void OrderPriced_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Accountant/OrderPricedPage.xaml", UriKind.Relative));  //当点击按钮的时候，Frame中的内容会变成相应的页面
            AccountantSetBackground(OrderPriced);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "划价订单";
            ClickMenuMainFrameAnimate();
        }

        /// <summary>
        /// 设置菜单背景颜色为灰色的函数
        /// </summary>
        /// <param name="white">菜单StackPanel的Name</param>
        private void AccountantSetBackground(DockPanel white)
        {
            BrushConverter brush = new BrushConverter();
            Brush whiteBrush = brush.ConvertFromInvariantString("#fff") as Brush;
            //灰色笔刷
            Brush blackBrush = brush.ConvertFromInvariantString("#f0f0f1") as Brush;
            Welcome_Accountant.Background = blackBrush;
            OrderPricing.Background = blackBrush;
            InStockPricing.Background = blackBrush;
            FinanceManage.Background = blackBrush;
            StockManage.Background = blackBrush;
            OrderPriced.Background = blackBrush;
            InStockPriced.Background = blackBrush;
            FinanceManage.Background = blackBrush;
            BenifitManage.Background = blackBrush;
            AccountantInfo.Background = blackBrush;

            white.Background = whiteBrush;
        }

        //Admin管理员的菜单
        private void ProductManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Admin/ProductManagePage.xaml", UriKind.Relative));  //当点击按钮的时候，Frame中的内容会变成相应的页面
            AdminSetBackground(ProductManage);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "产品管理";
            ClickMenuMainFrameAnimate();
        }

        private void ConsumerManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Admin/ConsumerManagePage.xaml", UriKind.Relative));
            AdminSetBackground(ConsumerManage);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "客户管理";
            ClickMenuMainFrameAnimate();
        }

        private void ProviderManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Admin/ProviderManagePage.xaml", UriKind.Relative));
            AdminSetBackground(ProviderManage);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "供应商管理";
            ClickMenuMainFrameAnimate();
        }

        private void UserManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Admin/UserManagePage.xaml", UriKind.Relative));
            AdminSetBackground(UserManage);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "用户管理";
            ClickMenuMainFrameAnimate();
        }


        private void AdminInfo_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("../Admin/AdminInfoPage.xaml", UriKind.Relative));
            AdminSetBackground(AdminInfo);
            TipTitleDP.Visibility = Visibility.Visible;
            TipTitleBlock.Text = "个人资料";
            ClickMenuMainFrameAnimate();
        }


        /// <summary>
        /// 设置菜单背景颜色为灰色的函数
        /// </summary>
        /// <param name="white">菜单StackPanel的Name</param>
        private void AdminSetBackground(DockPanel white)
        {
            //白色笔刷
            BrushConverter brush = new BrushConverter();
            Brush whiteBrush = brush.ConvertFromInvariantString("#fff") as Brush;
            //灰色笔刷
            Brush blackBrush = brush.ConvertFromInvariantString("#f0f0f1") as Brush;
            Welcome_Admin.Background = blackBrush;
            ProductManage.Background = blackBrush;
            ConsumerManage.Background = blackBrush;
            ProviderManage.Background = blackBrush;
            UserManage.Background = blackBrush;
            AdminInfo.Background = blackBrush;

            white.Background = whiteBrush;
        }


        /// <summary>
        /// 点击左侧菜单 右侧Frame动画
        /// </summary>
        private void ClickMenuMainFrameAnimate()
        {
            //文字平移，Margin属性是Thickness类型，选择ThicknessAnimation
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = new Thickness(3, 3, 0, 0);             //起始值
            ta.To = new Thickness(0, 0, 0, 0);        //结束值
            ta.Duration = TimeSpan.FromSeconds(0.4);         //动画持续时间

            Storyboard sb = new Storyboard();
            DoubleAnimation dop = new DoubleAnimation();
            dop.Duration = sb.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 400));
            dop.From = 0;
            dop.To = 1;
            Storyboard.SetTarget(dop, MainDockPanel);
            //Storyboard.SetTarget(dop, TipTitleDP);
            //Storyboard.SetTarget(dop, BorderBlock);
            Storyboard.SetTargetProperty(dop, new PropertyPath("Opacity", new object[] { }));
            sb.Children.Add(dop);
            sb.Begin();

            this.MainFrame.BeginAnimation(Frame.MarginProperty, ta);//开始动画
            this.TipTitleDP.BeginAnimation(DockPanel.MarginProperty, ta);//开始动画
            this.BorderBlock.BeginAnimation(TextBlock.MarginProperty, ta);//开始动画

        }


        /// <summary>
        /// 窗口拖动函数 如果是最大化状态就还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWinDrag(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Width > 1050)
                {
                    Width = 1050;
                    Height = 700;
                    TitleDockPanel.Width = Width - 10;
                    //Top = (SystemParameters.PrimaryScreenHeight - 700) / 2;
                    Left = (SystemParameters.PrimaryScreenWidth - 1050) / 2;
                    MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Max.png", UriKind.Relative));
                    MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Max_Mo.png", UriKind.Relative));
                    DragMove();
                }
                else
                {
                    Width = 1050;
                    Height = 700;
                    TitleDockPanel.Width = Width - 10;
                    //Top = (SystemParameters.PrimaryScreenHeight - 700) / 2;
                    //Left = (SystemParameters.PrimaryScreenWidth - 1050) / 2;
                    MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Max.png", UriKind.Relative));
                    MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Max_Mo.png", UriKind.Relative));
                    DragMove();
                }
            }
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    Point p1 = Mouse.GetPosition(this);
            //    Timer ttTimer = new Timer();
            //    ttTimer.Interval = 10;
            //    ttTimer.Tick += (o, args) =>
            //    {
            //        Point p2 = Mouse.GetPosition(this);
            //        if (p1 != p2)
            //        {
            //        }
            //    };
            //    ttTimer.Start();
            //}
            //TitleDockPanel.PreviewMouseLeftButtonDown += (o, arg) =>
            //{
                
            //};
        }

        
        //最小化事件
        private void MinWin_Click(object sender, MouseEventArgs e)
        {
            WinMinAnimate();
        }

        //关闭事件
        private void CloseWin_Click(object sender, MouseEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        //鼠标双击事件模拟
        private void MainWin_DoubleMouse(object sender, MouseButtonEventArgs e)
        {
            MouseDoubleClick += (o, args) =>
            {
                if (i == false)
                {
                    Left = -5;//设置位置
                    Top = -5;
                    Rect rc = SystemParameters.WorkArea;
                    Width = rc.Width + 10;
                    Height = rc.Height + 10;
                    i = true;
                    TitleDockPanel.Width = Width - 10;
                    MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Resize.png", UriKind.Relative));
                    MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Resize_Mo.png", UriKind.Relative));
                }
                else if (i)
                {
                    Width = 1050;
                    Height = 700;
                    TitleDockPanel.Width = Width - 10;
                    i = false;
                    MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Max.png", UriKind.Relative));
                    MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Max_Mo.png", UriKind.Relative));
                }
            };
        }


        //拖动最大化or还原 
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualHeight > SystemParameters.WorkArea.Height + 10 || ActualWidth > SystemParameters.WorkArea.Width + 10)
            {
                WindowState = WindowState.Normal;
                Visibility = Visibility.Collapsed;
                Visibility = Visibility.Visible;
                rcnormal = new Rect(Left, Top, Width, Height);//保存下当前位置与大小
                Left = -5;//设置位置
                Top = -5;
                Rect rc = SystemParameters.WorkArea;
                Width = rc.Width + 10;
                Height = rc.Height + 10;
                TitleDockPanel.Width = Width - 10;
                MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Resize.png", UriKind.Relative));
                MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Resize_Mo.png", UriKind.Relative));
            }
        }


        //最大化or还原按钮
        private void MaxWin_Click(object sender, MouseEventArgs e)
        {
            if (ActualHeight == 700 || ActualWidth == 1050)
            {
                WindowState = WindowState.Normal;
                Visibility = Visibility.Collapsed;
                Visibility = Visibility.Visible;
                rcnormal = new Rect(Left, Top, Width, Height);//保存下当前位置与大小
                Left = -5;//设置位置
                Top = -5;
                Rect rc = SystemParameters.WorkArea;
                Width = rc.Width + 10;
                Height = rc.Height + 10;
                TitleDockPanel.Width = Width - 10;
                MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Resize.png", UriKind.Relative));
                MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Resize_Mo.png", UriKind.Relative));
            }
            else
            {
                Width = 1050;
                Height = 700;
                Top = (SystemParameters.PrimaryScreenHeight - 700) / 2;
                Left = (SystemParameters.PrimaryScreenWidth - 1050) / 2;
                TitleDockPanel.Width = Width - 10;
                MaxWin.Source = new BitmapImage(new Uri("../Images/Main_Max.png", UriKind.Relative));
                MaxWin_Mo.Source = new BitmapImage(new Uri("../Images/Main_Max_Mo.png", UriKind.Relative));
            }
            
        }
        
        
        /// <summary>
        /// 版权超链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BindingCopyRight(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.valueteam.sinaapp.com/");
        }


        /// <summary>
        /// 最小化窗口动画效果
        /// 只是改变透明度
        /// </summary>
        private void WinMinAnimate()
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation dh = new DoubleAnimation();
            DoubleAnimation dw = new DoubleAnimation();
            DoubleAnimation dop = new DoubleAnimation();
            dop.Duration = dh.Duration = dw.Duration = sb.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
            dop.To = dh.To = dw.To = 0;
            Storyboard.SetTarget(dh, this);
            Storyboard.SetTarget(dw, this);
            Storyboard.SetTarget(dop, this);
            Storyboard.SetTargetProperty(dop, new PropertyPath("Opacity", new object[] { }));
            Storyboard.SetTargetProperty(dh, new PropertyPath("Height", new object[] { }));
            Storyboard.SetTargetProperty(dw, new PropertyPath("Width", new object[] { }));
            //sb.Children.Add(dh);
            //sb.Children.Add(dw);
            sb.Children.Add(dop);
            sb.Completed += new EventHandler(sb_Completed);   //(a, b) => { this.Close(); };
            sb.Begin();
        }
        void sb_Completed(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化窗口动画
        /// 只是改变透明度
        /// </summary>
        private void WinNormalAnimate()
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation dh = new DoubleAnimation();
            DoubleAnimation dw = new DoubleAnimation();
            DoubleAnimation dop = new DoubleAnimation();
            dop.Duration = dh.Duration = dw.Duration = sb.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
            dop.From = 0;
            dh.From = 0;
            dw.From = 0;
            dop.To = 1;
            dh.To = 700;
            dw.To = 1050;
            Storyboard.SetTarget(dop, this);
            Storyboard.SetTarget(dh, this);
            Storyboard.SetTarget(dw, this);
            Storyboard.SetTargetProperty(dop, new PropertyPath("Opacity", new object[] { }));
            //Storyboard.SetTargetProperty(dw, new PropertyPath("Width", new object[] { }));
            //Storyboard.SetTargetProperty(dh, new PropertyPath("Height", new object[] { }));
            //sb.Children.Add(dw);
            //sb.Children.Add(dh);
            sb.Children.Add(dop);
            sb.Completed += new EventHandler(Max_sb_Completed);   //(a, b) => { this.Close(); };
            sb.Begin();
        }
        void Max_sb_Completed(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
        }



        /// <summary>
        /// 判断是否是最小化用来激活相应动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinStateChanged(object sender, EventArgs e)
        {
            Window w = (sender as Window);
            if (w.Opacity == 0 && w.WindowState == WindowState.Normal)
            {
                WinNormalAnimate();
                return;
            }
            if (w.Opacity != 0 && w.WindowState == WindowState.Minimized)
            {
                WinMinAnimate();
            }
            
        }


        private void TitleDockPanel_OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }


        
        //重置查询条件按钮
        private void Reset_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void SeacherImage_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

    }


}
