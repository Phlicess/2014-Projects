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
    
    public partial class InStockDetail
    {
        public decimal ID { get; set; }
        public Nullable<decimal> Product_ID { get; set; }
        public decimal InStock_ID { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Weight { get; set; }
    
        public virtual InStock InStock { get; set; }
        public virtual Product Product { get; set; }
    }
}
