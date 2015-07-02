using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCZY.OtherWindows
{
    /// <summary>
    /// DataGridPager.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridPager : UserControl
    {
        private string gridName;
        //对象
        private DataGrid grdList;       

        List<object> objects = new List<object>(); 
        //每页显示多少条
        private int pageNum = 10;
        //当前是第几页
        private int pIndex = 1;
       
        //最大页数
        private int MaxIndex = 1;
        //一共多少条
        private int allNum = 0; 
        public  DataGridPager()
        {
            InitializeComponent();
        }


        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="dtt"></param>
        /// <param name="Num"></param>
        public void ShowPages<T>(DataGrid grd, List<T> list, int Num)
        {
            if (list == null || list.Count == 0)
            {
                this.Visibility = Visibility.Hidden;
                return;
            }

            // this._dt = dt.Clone();
           
            this.grdList = grd;
            this.pageNum = Num;
            this.pIndex = 1;
            foreach (var r in list)
                this.objects.Add(r);
            SetMaxIndex();
            ReadDataTable();
        }
        #endregion

        #region 设置最多大页面
        /// <summary>
        /// 设置最多大页面
        /// </summary>
        private void SetMaxIndex()
        {
            //多少页
            int Pages = this.objects.Count / pageNum;
            if (this.objects.Count != (Pages * pageNum))
            {
                if (objects.Count < (Pages * pageNum))
                    Pages--;
                else
                    Pages++;
            }
            this.MaxIndex = Pages;
            this.allNum = this.objects.Count;
        }
        #endregion


        #region 画数据
        /// <summary>
        /// 画数据
        /// </summary>
        private void ReadDataTable()
        {
            try
            {
                IndexTextBlock.Text = this.pIndex.ToString();
                MaxTextBlock.Text = MaxIndex.ToString();

                List<object> nowPageList = new List<object>();

                int first = pageNum * (pIndex - 1);
                first = (first > 0) ? first : 0;
                //如何总数量大于每页显示数量
                if (objects.Count >= pageNum * pIndex)
                {
                    for (int i = first; i < pageNum * pIndex; i++)
                        nowPageList.Add(objects[i]);
                }
                else
                {
                    for (int i = first; i < this.objects.Count; i++)
                        nowPageList.Add(this.objects[i]);
                }
                grdList.ItemsSource = nowPageList;
            }
            catch
            {
                MessageBox.Show("错误");
            }
            finally
            {
                DisplayPagingInfo();
            }

        }

        #endregion

        #region 画每页显示等数据
        /// <summary>
        /// 画每页显示等数据
        /// </summary>
        private void DisplayPagingInfo()
        {
            if (this.pIndex == 1)
            {
                this.AheadTextBlock.IsEnabled = false;
                this.FirstTextBlock.IsEnabled = false;
            }
            else
            {
                this.AheadTextBlock.IsEnabled = true;
                this.FirstTextBlock.IsEnabled = true;
            }
            if (this.pIndex == this.MaxIndex)
            {
                this.NextTextBlock.IsEnabled = false;
                this.LastTextBlock.IsEnabled = false;

            }
            else
            {
                this.NextTextBlock.IsEnabled = true;
                this.LastTextBlock.IsEnabled = true;
            }
        }
        #endregion

        //跳转到首页
        private void GoFirstPage(object sender, MouseButtonEventArgs e)
        {
            this.pIndex = 1;
            ReadDataTable();
        }
        
        //跳转到上一页
        private void GoBeforePage(object sender, MouseButtonEventArgs e)
        {
            if (this.pIndex <= 1)
                return;
            this.pIndex--;
            ReadDataTable();
        }

        //跳转到指定页
        private void JumpThePage(object sender, MouseButtonEventArgs e)
        {
            if (JampTextBox.Text == "")
                return;
            if (Convert.ToInt32(JampTextBox.Text) <= 1)
                return;
            this.pIndex = Convert.ToInt32(JampTextBox.Text);
            ReadDataTable();
        }

        //跳转到下一页
        private void GoNextPage(object sender, MouseButtonEventArgs e)
        {
            if (this.pIndex >= this.MaxIndex)
                return;
            this.pIndex++;
            ReadDataTable();
        }

        //跳转到最后一页
        private void GoLastPage(object sender, MouseButtonEventArgs e)
        {
            this.pIndex = this.MaxIndex;
            ReadDataTable();
        }
    }
}
