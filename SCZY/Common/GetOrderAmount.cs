using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    public class GetOrderAmount
    {
        Order order = new Order();
        private double? total = 0;
        public GetOrderAmount(Order dataOrder)
        {
            order = dataOrder;
        }

        public double? GetAmount()
        {
            SCZYEntities sczy = new SCZYEntities();
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            orderDetails = sczy.OrderDetails.Where(p => p.Order_ID == order.ID).ToList();
            foreach (var orderDetail in orderDetails)
            {
                List<Product> products = new List<Product>();
                products = sczy.Products.Where(p => p.ID == orderDetail.Product_ID).ToList();

                foreach (var item in products)
                {
                    if (orderDetail != null && orderDetail.Price != null)
                    {
                        Double? cash = 0;
                        switch (orderDetail.Unit)//根据出货的计数方式不同（"吨,张,令"），选择不同的计算方式
                        {
                            case "张":
                                cash = item.Length * item.Width * item.GramWeight * orderDetail.Count / 100 / 100 / 1000 / 1000 * orderDetail.Price;
                                break;
                            case "吨":
                                cash = orderDetail.Price * orderDetail.Count;
                                break;
                            case "令":
                                cash = item.Length * item.Width * item.GramWeight * orderDetail.Count * 500 / 100 / 100 / 1000 / 1000 * orderDetail.Price;
                                break;
                        }
                        //dr["货款"] = DataChange.CutDouble(cash);
                        total += cash;
                    }
                }
                
            }

            return total;
        }
    }
}
