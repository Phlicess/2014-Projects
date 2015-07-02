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
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SCZY.Accountant.Dialog
{
    /// <summary>
    /// MessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox : Window
    {
        private Timer timer = new Timer(3000);  
        public MessageBox()
        {        
            InitializeComponent();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();  
        }


        public MessageBox(string str)
        {
            InitializeComponent();
            Messager.Content = str;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        public MessageBox(string str, Timer timer1)
        {
            InitializeComponent();
            
            Messager.Content = str;
            timer1.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer1.Start();
        }

        public MessageBox(string str, string nu)
        {
            InitializeComponent();
            Messager.Content = str;
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate()
            {
                this.Hide();
            });
        } 
    }
}
