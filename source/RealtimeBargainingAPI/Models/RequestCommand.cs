using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBasedRealtimeBargaining.Models
{
	public class RequestCommand
	{
		public string TokenKey { get; set; }
		public string Tenant { get; set; }
		public long CustomerId { get; set; }		
		public long ProductId { get; set; }
		public double ProposedCost { get; set; }
		public double ThresholdPrice { get; set; }
		public double OfferPrice { get; set; }
	}
}