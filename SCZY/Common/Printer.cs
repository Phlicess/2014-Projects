using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Reporting.WinForms;
using SCZY.Accountant;

namespace SCZY.Common
{
    public class Printer
    {
        private Order order = new Order();//保存要打印的数据
        private int m_currentPageIndex; //现在页的索引
        private IList<Stream> m_streams; //打印的数据流
        private double? total = 0;//订单的总款项
        private string type;//要打印单据的类型


        /// <summary>
        ///初始化函数 
        /// </summary>
        /// <param name="sytle">打印单据的类型</param>
        public Printer(string sytle)
        {
            if (sytle == null)
            {
                Console.Write("类型不能为空");
            }
            else
            {
                type = sytle;
            }
                            
        }


        public void GetOrderData(Order orderData)
        {
            if (orderData == null)
            {
                string msg = String.Format("没有要打印的数据");
                Console.WriteLine(msg);
                return;
            }
            order = orderData;
        }
        /// <summary>
        /// 打印选中的Order
        /// </summary>
        public void PrintOrders()
        {
            LocalReport report = new LocalReport();

            if (type == "Product")
            {
                report.ReportPath = "../../OutReport2.rdlc";
            }
            else if (type == "Order")
            {
                report.ReportPath = "../../Report1.rdlc";
            }
            
            report.DataSources.Add(new ReportDataSource("DataSet_Product", LoadProductData()));
            report.DataSources.Add(new ReportDataSource("DataSet_Order", LoadOrdersData()));

        //    report.DataSources.Add(new ReportDataSource("DataSet_OrderDetails", LoadOuttData()));

            Export(report);

            m_currentPageIndex = 0;
            PrintReport();
        }


        /// <summary>
        /// 设置报表的订单信息
        /// </summary>
        /// <returns>报表里面订单的信息</returns>
        private DataTable LoadOrdersData()
        {
            DataTable dt = new DataTable("or");
            dt.Columns.Add("订单号", typeof(String));
            dt.Columns.Add("实际发货日期", typeof(string));
            dt.Columns.Add("总计", typeof(double));

            DataRow dr = dt.NewRow();          
            dr["订单号"] = DataChange.IDToOrderID(order.ID);
            dr["实际发货日期"] = order.DeliveryDate.ToString();
            dr["总计"] = DataChange.DoubleToInt(total);
            // dr["ConsumerName"] = "我要这天再遮不住我的眼";
            dt.Rows.Add(dr);
            //DataRow dr1 = dt.NewRow();
            //dt.Rows.Add(dr1);
            return dt;
        }

        /// <summary>
        /// 设置报表的的产品信息（与订单相关连的）
        /// </summary>
        /// <returns>报表里面的产品的信息</returns>
        private DataTable LoadProductData()
        {
            DataTable dt = new DataTable("po");
            dt.Columns.Add("品牌", typeof(String));
            dt.Columns.Add("克重", typeof(float));
            dt.Columns.Add("宽幅", typeof(float));
            dt.Columns.Add("材质", typeof(String));
            dt.Columns.Add("张数", typeof(float));
            dt.Columns.Add("价格", typeof(float));
            dt.Columns.Add("货款", typeof(float));
            dt.Columns.Add("等级", typeof(String));
            dt.Columns.Add("长度", typeof(float));

            SCZYEntities sczy = new SCZYEntities();
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            orderDetails = sczy.OrderDetails.Where(p => p.Order_ID == order.ID).Distinct().ToList();

            if (orderDetails.Count == 0)
            {
                return null;
            }

            List<Product> listProducts = new List<Product>();

            foreach (var item in orderDetails)
            {
                listProducts.Add(sczy.Products.FirstOrDefault(p => p.ID == item.Product_ID));
            }
            foreach (var item in listProducts)
            {
                OrderDetail orderDetail =
                    sczy.OrderDetails.FirstOrDefault(p => p.Product_ID == item.ID && p.Order_ID == order.ID);

                DataRow dr = dt.NewRow();
                dr["品牌"] = item.Brand;
                dr["克重"] = item.GramWeight;
                dr["宽幅"] = item.Width;
                dr["材质"] = item.Texture;
                dr["长度"] = item.Length;
                dr["等级"] = item.Level;
                if (orderDetail != null)
                {
                    Double? cash = 0;
                    dr["价格"] = orderDetail.Price;
                    dr["张数"] = orderDetail.Count;
                    switch (orderDetail.Unit)//根据出货的计数方式不同（"吨,张,令"），选择不同的计算方式
                    {
                        case "张":
                             cash = item.Length * item.Width * item.GramWeight * orderDetail.Count / 100 / 100 / 1000 / 1000 * orderDetail.Price;
                             break;
                        case "吨":
                            cash = orderDetail.Price*orderDetail.Count;
                            break;
                        case "令":
                            cash = item.Length * item.Width * item.GramWeight * orderDetail.Count * 500 / 100 / 100 / 1000 / 1000 * orderDetail.Price;
                            break;
                    }
                    dr["货款"] = DataChange.CutDouble(cash);
                    total += cash;
                }
                
                // dr["ConsumerName"] = "我要这天再遮不住我的眼";
                dt.Rows.Add(dr);
                //DataRow dr1 = dt.NewRow();
            }
           
            for (int i = dt.Rows.Count; i < 10; i++)
            {
                DataRow tr = dt.NewRow();
                dt.Rows.Add(tr);
            }
            //dt.Rows.Add(dr1);
            return dt;
        }

        ///// <summary>
        ///// 设置报表的出库产品的重量信息（与订单相关连的）
        ///// </summary>
        ///// <returns></returns>
        //private DataTable LoadOuttData()
        //{
        //    DataTable dt = new DataTable("po");
        //    dt.Columns.Add("Price", typeof(String));
        //    dt.Columns.Add("Weight", typeof(String));
        //    SCZYEntities sczy = new SCZYEntities();
        //    List<OrderDetail> listOrderDetails = new List<OrderDetail>();
        //    listOrderDetails = sczy.OrderDetails.Where(p => p.ID == order.ID).Distinct().ToList();

        //    if (listOrderDetails.Count == 0)
        //    {
        //        return null;
        //    }

        //    foreach (var item in listOrderDetails)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Price"] = item.Price;
        //        dr["Weight"] = item.Weight;
        //        // dr["ConsumerName"] = "我要这天再遮不住我的眼";
        //        dt.Rows.Add(dr);
        //        //DataRow dr1 = dt.NewRow();
        //    }
        //    //dt.Rows.Add(dr1);
        //    return dt;
        //}
        /// <summary>
        /// 设置打印机的参数
        /// </summary>
        /// <param name="report">要打印的报表</param>
        private void Export(LocalReport report)
        {
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>EMF</OutputFormat>" +
              "  <PageWidth>12in</PageWidth>" +
              "  <PageHeight>6in</PageHeight>" +
              "  <MarginTop>0.2in</MarginTop>" +
              "  <MarginLeft>0.2in</MarginLeft>" +
              "  <MarginRight>0.25in</MarginRight>" +
              "  <MarginBottom>0.25in</MarginBottom>" +
              "</DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        /// <summary>
        /// 创建数据流
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileNameExtension"></param>
        /// <param name="encoding"></param>
        /// <param name="mimeType"></param>
        /// <param name="willSeek"></param>
        /// <returns></returns>
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new FileStream(name + "." + fileNameExtension, FileMode.Create);
            m_streams.Add(stream);
            return stream;
        }

       /// <summary>
       /// 打印
       /// </summary>
        private void PrintReport()
        {
            //const string printerName = "Microsoft Office Document Image Writer";

            if (m_streams == null || m_streams.Count == 0)
                return;

            PrintDocument printDoc = new PrintDocument();
            //    printDoc.PrinterSettings.PrinterName = printerName;
            //    printDoc.DefaultPageSettings.Landscape = true;
            // printDoc.PrintPage += new PrintPageEventHandler(this.Pri);




            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format("Can't find printer \"{0}\".", "默认打印机");
                Console.WriteLine(msg);
                return;
            }
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            printDoc.Print();
           // this.Close();
        }


        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
            ev.Graphics.DrawImage(pageImage, ev.PageBounds);

            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }
    }
}
