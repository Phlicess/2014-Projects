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
    
    public partial class YongHu_RenWu
    {
        public int YongHu_ID { get; set; }
        public int RenWu_ID { get; set; }
        public Nullable<bool> ReadOrNo { get; set; }
        public Nullable<bool> FinishOrNo { get; set; }
        public string GUID { get; set; }
    
        public virtual RenWu RenWu { get; set; }
        public virtual YongHu YongHu { get; set; }
    }
}
