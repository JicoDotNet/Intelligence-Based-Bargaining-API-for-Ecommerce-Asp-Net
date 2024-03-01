using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBasedRealtimeBargaining.Models
{
	public class NegotiatedValue
	{
		public string Message { get; set; }
		public NegotiatedCost NegotiatedCost { get; set; }
		public ENegotiatedMessage Type { get; set; }
	}
}