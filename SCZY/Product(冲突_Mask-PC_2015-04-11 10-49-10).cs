//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCZY
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.ChanPin_GongYingShang = new HashSet<ChanPin_GongYingShang>();
            this.InStockDetails = new HashSet<InStockDetail>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.ProduceOutStocks = new HashSet<ProduceOutStock>();
            this.ProduceInStocks = new HashSet<ProduceInStock>();
        }
    
        public decimal ID { get; set; }
        public string Brand { get; set; }
        public string Texture { get; set; }
        public Nullable<float> GramWeight { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Length { get; set; }
        public Nullable<double> Reservation { get; set; }
        public string Remark { get; set; }
        public string Level { get; set; }
    
        public virtual ICollection<ChanPin_GongYingShang> ChanPin_GongYingShang { get; set; }
        public virtual ICollection<InStockDetail> InStockDetails { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProduceOutStock> ProduceOutStocks { get; set; }
        public virtual ICollection<ProduceInStock> ProduceInStocks { get; set; }
    }
}
