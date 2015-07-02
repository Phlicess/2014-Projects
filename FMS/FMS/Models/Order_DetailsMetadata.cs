using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


[MetadataType(typeof(Order_DetailsMD))]
	public partial class Order_Details
	{
	    public class Order_DetailsMD
	    {
	        [JsonIgnore()]  // 需引用 using Newtonsoft.Json;
	        public virtual Orders Orders { get; set; }
	    }
	}