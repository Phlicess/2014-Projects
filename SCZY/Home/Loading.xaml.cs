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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SCZY.Home
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : Window
    {
        public Loading()
        {
            InitializeComponent();
            
            //Loaded += (LoadingAnimate);
        }
        
        //加载ing动画
        private void LoadingAnimate(object sender, RoutedEventArgs e)
        {
            RotateTransform rtf = new RotateTransform();
            LoadingImage.RenderTransform = rtf;
            DoubleAnimation dbAscending = new DoubleAnimation(359, 0, new Duration(TimeSpan.FromSeconds(1)));
            Storyboard storyboard = new Storyboard();
            dbAscending.RepeatBehavior = RepeatBehavior.Forever;
            storyboard.Children.Add(dbAscending);
            Storyboard.SetTarget(dbAscending, LoadingImage);
            Storyboard.SetTargetProperty(dbAscending, new PropertyPath("RenderTransform.Angle"));
            storyboard.Begin();
        }

    }
}
