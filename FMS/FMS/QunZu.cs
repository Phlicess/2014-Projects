//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class QunZu
    {
        public QunZu()
        {
            this.YongHus = new HashSet<YongHu>();
        }
    
        public int ID { get; set; }
        public Nullable<int> YongHu_ID { get; set; }
        public string GroupName { get; set; }
        public string GroupExplain { get; set; }
        public Nullable<bool> PublicGroup { get; set; }
    
        public virtual YongHu YongHu { get; set; }
        public virtual ICollection<YongHu> YongHus { get; set; }
    }
}
