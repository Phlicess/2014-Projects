using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SCZY.Common;

namespace SCZY.Admin.ProductDialog
{
    //定义委托 刷新父窗口的ProductListView的数据
    public delegate void SaveProviderHander(); 
    /// <summary>
    /// EditProduct.xaml 的交互逻辑
    /// </summary>
    public partial class EditProduct : Window
    {
        private decimal productID;
        private Product product;
        public string type = "Add";
        public EditProduct(object selectProduct)
        {
            InitializeComponent();

            SetDataForComboBox();   //下拉列表绑定数据

            product = selectProduct as Product;

            Loaded += (sender, args) =>
            {
                SetTitle_Size();
                if (type == "Edit")
                {
                    productID = product.ID;
                    //如果是编辑订单窗口，就为窗口赋值
                    SetData(selectProduct as Product);
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
        /// <param name="selectedProduct"></param>
        public void SetData(Product selectedProduct)
        {
            product = selectedProduct;
            Brand.Text = selectedProduct.Brand;
            Texture.Text = selectedProduct.Texture;
            GramWeight.Text = selectedProduct.GramWeight.ToString();
            Reservation.Text = selectedProduct.Reservation.ToString();
            Level.Text = selectedProduct.Level;
            Length.Text = selectedProduct.Length.ToString();
            Wide.Text = selectedProduct.Width.ToString();
            Remark.Text = selectedProduct.Remark;
        }

        ///// <summary>
        ///// 给供应商报价列表赋值
        ///// </summary>
        //public void SetDataForListView()
        //{
        //    SCZYEntities sczy = new SCZYEntities();
        //    string sql =
        //        String.Format("select Provider.* from Product inner join ChanPin_GongYingShang on Product.ID = ChanPin_GongYingShang.Product_ID inner join Provider on Provider.ID = ChanPin_GongYingShang.Provider_ID where Product.ID = {0}", productID);
        //    List<Provider> providers = sczy.Providers.SqlQuery(sql).ToList();
        //    List<ViewProvider> viewProviders = new List<ViewProvider>();

        //    foreach (Provider provider in providers)
        //    {
        //        decimal providerID = provider.ID;
        //        ChanPin_GongYingShang chanPinGongYing = sczy.ChanPin_GongYingShang.FirstOrDefault(
        //            cf => cf.Provider_ID == providerID && cf.Product_ID == productID);
        //        if (chanPinGongYing == null)
        //        {
        //            continue;
        //        }
        //        ViewProvider viewProvider = new ViewProvider();
        //        viewProvider.ID = provider.ID;
        //        viewProvider.Name = provider.Name;
        //        viewProvider.Phone = provider.Phone;
        //        viewProvider.Tel = provider.Tel;
        //        viewProvider.Address = provider.Address;
        //        viewProvider.Company = provider.Company;
        //        viewProvider.Price = chanPinGongYing.Price;
        //    }

        //    ProviderListView.ItemsSource = viewProviders;
        //}

        /// <summary>
        /// 为下拉列表赋值
        /// </summary>
        public void SetDataForComboBox()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<Product> products = sczy.Products.ToList();
            //品牌
            Dictionary<string, string> brandDis = new Dictionary<string, string>();
            foreach (Product product in products)
            {
                if (!brandDis.Keys.Contains(product.Brand))
                {
                    brandDis.Add(product.Brand, product.Brand);
                }
            }
            Brand.ItemsSource = brandDis;
            Brand.DisplayMemberPath = "Value";
            Brand.SelectedValuePath = "Key";

            ////供应商
            //List<Provider> providers = sczy.Providers.ToList();
            //Provider.ItemsSource = providers;
            //Provider.DisplayMemberPath = "Name";
            //Provider.SelectedValuePath = "ID";

            //材质
            Dictionary<string, string> textureDis = new Dictionary<string, string>();
            foreach (Product product in products)
            {
                if (!textureDis.Keys.Contains(product.Texture))
                {
                    textureDis.Add(product.Texture, product.Texture);
                }
            }
            Texture.ItemsSource = textureDis;
            Texture.DisplayMemberPath = "Value";
            Texture.SelectedValuePath = "Key";
            //克重
            Dictionary<string, string> gramWeightDis = new Dictionary<string, string>();
            foreach (Product product in products)
            {
                if (!gramWeightDis.Keys.Contains(product.GramWeight.ToString()))
                {
                    gramWeightDis.Add(product.GramWeight.ToString(), product.GramWeight.ToString());
                }
            }
            GramWeight.ItemsSource = gramWeightDis;
            GramWeight.DisplayMemberPath = "Value";
            GramWeight.SelectedValuePath = "Key";
            //宽幅
            Dictionary<string, string> wideDis = new Dictionary<string, string>();
            foreach (Product product in products)
            {
                if (!wideDis.Keys.Contains(product.Width.ToString()))
                {
                    wideDis.Add(product.Width.ToString(), product.Width.ToString());
                }
            }
            Wide.ItemsSource = wideDis;
            Wide.DisplayMemberPath = "Value";
            Wide.SelectedValuePath = "Key";
            //长度
            Dictionary<string, string> lengthDis = new Dictionary<string, string>();
            foreach (Product product in products)
            {
                if (!lengthDis.Keys.Contains(product.Length.ToString()))
                {
                    lengthDis.Add(product.Length.ToString(), product.Length.ToString());
                }
            }
            Length.ItemsSource = lengthDis;
            Length.DisplayMemberPath = "Value";
            Length.SelectedValuePath = "Key";
            //等级
            Dictionary<string, string> levelDis = new Dictionary<string, string>();
            foreach (Product product in products)
            {
                if (!levelDis.Keys.Contains(product.Level))
                {
                    levelDis.Add(product.Level, product.Level);
                }
            }
            Level.ItemsSource = levelDis;
            Level.DisplayMemberPath = "Value";
            Level.SelectedValuePath = "Key";

        }

        //定义委托事件
        public event SaveProviderHander SaveProvider;

        /// <summary>
        /// 保存产品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SCZYEntities sczy = new SCZYEntities();
            if (type == "Edit")
            {
                Product oldProduct = sczy.Products.FirstOrDefault(o => o.ID == productID);
                oldProduct.Brand = Brand.Text;
                oldProduct.Texture = Texture.Text;
                oldProduct.GramWeight = float.Parse(GramWeight.Text);
                oldProduct.Reservation = double.Parse(Reservation.Text);
                oldProduct.Level = Level.Text;
                oldProduct.Length = double.Parse(Length.Text);
                oldProduct.Width = double.Parse(Wide.Text);
                oldProduct.Remark = Remark.Text;

                sczy.SaveChanges();
                Close();
            }
            else if (type == "Add")
            {
                Product newProduct = new Product();
                newProduct.Brand = Brand.Text;
                newProduct.Texture = Texture.Text;
                newProduct.GramWeight = float.Parse(GramWeight.Text);
                newProduct.Reservation = double.Parse(Reservation.Text);
                newProduct.Level = Level.Text;
                newProduct.Length = double.Parse(Length.Text);
                newProduct.Width = double.Parse(Wide.Text);
                newProduct.Remark = Remark.Text;

                sczy.Products.Add(newProduct);
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
