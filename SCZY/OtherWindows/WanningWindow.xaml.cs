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

namespace SCZY.OtherWindows
{
    /// <summary>
    /// WanningWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WanningWindow : Window
    {
        private bool nextOrGiveUp { get; set; }
        public bool GetValueForButton()
        {
            return nextOrGiveUp;
        }
        public WanningWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                SetTitle_Size();
            };
            nextOrGiveUp = false;
        }

        /// <summary>
        /// 记录用户点击了继续按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            nextOrGiveUp = true;
            Close();
        }

        /// <summary>
        /// 记录用户点击了放弃按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GiveUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            nextOrGiveUp = false;
            Close();
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
            temGrid.Height = Height;
            temGrid.Width = Width;
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
