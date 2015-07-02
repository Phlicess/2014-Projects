using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace SCZY.Clerk.InStockDialog
{
    //定义委托 刷新父窗口的ProductListView的数据
    public delegate void SaveInStockHander(); 
    /// <summary>
    /// EditInStock.xaml 的交互逻辑
    /// </summary>
    public partial class EditInStock : Window
    {
        private decimal inStockID;
        private InStock inStock;
        public string type = "Add";
        public EditInStock(object selectedInStock)
        {
            InitializeComponent(); 
            inStock = selectedInStock as InStock;
            inStockID = inStock.ID;

            Loaded += (sender, args) =>
            {
                SetComboBoxValue();
                SetTitle_Size();
                if (type == "Edit")
                {
                    //如果是编辑订单窗口，就为窗口赋值
                    SetData(selectedInStock as InStock);
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
        /// 为下拉列表绑定数据
        /// </summary>
        public void SetComboBoxValue()
        {
            //客户下拉列表绑定
            SCZYEntities sczy = new SCZYEntities();
            List<Provider> providers = sczy.Providers.ToList();
            ProviderName.ItemsSource = providers;
            ProviderName.DisplayMemberPath = "Name";
            ProviderName.SelectedValuePath = "ID";
            //int i = int.Parse(inStock.Consumer_ID.ToString());
            if (type == "Edit")
            {
                ProviderName.SelectedValue = inStock.Provider_ID;
            }
            else if (type == "Add")
            {
                ProviderName.SelectedIndex = 0;
            }


        }

        /// <summary>
        /// 给弹出框赋值
        /// </summary>
        /// <param name="selectedInStock"></param>
        public void SetData(InStock selectedInStock)
        {
            inStock = selectedInStock;
            ProviderName.Text = selectedInStock.ProviderName;
            InStockDate.Text = selectedInStock.InStockDate.ToString();
        }


        //定义委托事件
        public event SaveInStockHander SaveInStock;
        /// <summary>
        /// 保存订单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            if (type == "Edit")
            {
                InStock oldInStock = sczy.InStocks.FirstOrDefault(o => o.ID == inStockID);
                oldInStock.InStockDate = InStockDate.DisplayDate;
                oldInStock.ProviderName = ProviderName.Text;

                sczy.SaveChanges();
                Close();
            }
            else if (type == "Add")
            {
                InStock newInStock = new InStock();
                newInStock.InStockDate = InStockDate.DisplayDate;
                newInStock.ProviderName = ProviderName.Text;
                newInStock.Provider_ID = Decimal.Parse(ProviderName.SelectedValue.ToString());
                newInStock.InStockState = "未划价";
                newInStock.ProviderName = ProviderName.Text;

                sczy.InStocks.Add(newInStock);
                sczy.SaveChanges();
                Close();

            }
            if (SaveInStock != null)
            {
                SaveInStock();
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
