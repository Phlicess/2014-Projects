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
    
    public partial class ProduceInStock
    {
        public ProduceInStock()
        {
            this.Products = new HashSet<Product>();
        }
    
        public decimal ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<double> Weight { get; set; }
        public string Remark { get; set; }
    
        public virtual ICollection<Product> Products { get; set; }
    }
}
