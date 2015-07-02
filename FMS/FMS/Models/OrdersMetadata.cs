using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


[MetadataType(typeof(OrderMD))]
	public partial class Order
	{
	    public class OrderMD
	    {
	        [JsonIgnore()] // 需引用 using Newtonsoft.Json;
	        public virtual ICollection<Order_details> Order_Details { get; set; }
	    }
	}