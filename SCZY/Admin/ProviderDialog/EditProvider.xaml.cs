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

namespace SCZY.Admin.ProviderDialog
{
    //定义委托 刷新父窗口的ProductListView的数据
    public delegate void SaveProviderHander(); 
    /// <summary>
    /// EditProvider.xaml 的交互逻辑
    /// </summary>
    public partial class EditProvider : Window
    {
        private decimal providerID;
        private Provider provider;
        public string type = "Add";
        public EditProvider(object selectProvider)
        {
            InitializeComponent();
            provider = selectProvider as Provider;

            Loaded += (sender, args) =>
            {
                SetTitle_Size();
                if (type == "Edit")
                {
                    providerID = provider.ID;
                    //如果是编辑供应商窗口，就为窗口赋值
                    SetData(selectProvider as Provider);
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
        /// <param name="selectedProvider"></param>
        public void SetData(Provider selectedProvider)
        {
            provider = selectedProvider;
            ProviderName.Text = selectedProvider.Name;
            Phone.Text = selectedProvider.Phone;
            Tel.Text = selectedProvider.Tel;
            CompanyName.Text = selectedProvider.Company;
            Address.Text = selectedProvider.Address;
            Remark.Text = selectedProvider.Remark;
        }

        //定义委托事件
        public event SaveProviderHander SaveProvider;

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
                Provider oldProvider = sczy.Providers.FirstOrDefault(o => o.ID == providerID);
                oldProvider.Phone = Phone.Text;
                oldProvider.Name = ProviderName.Text;
                oldProvider.Address = Address.Text;
                oldProvider.Company = CompanyName.Text;
                oldProvider.Tel = Tel.Text;
                oldProvider.Remark = Remark.Text;

                sczy.SaveChanges();
                Close();
            }
            else if (type == "Add")
            {
                Provider newProvider = new Provider();
                newProvider.Phone = Phone.Text;
                newProvider.Name = ProviderName.Text;
                newProvider.Address = Address.Text;
                newProvider.Company = CompanyName.Text;
                newProvider.Tel = Tel.Text;
                newProvider.Remark = Remark.Text;

                sczy.Providers.Add(newProvider);
                sczy.SaveChanges();
                Close();

            }
            if (SaveProvider != null)
            {
                SaveProvider();
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
