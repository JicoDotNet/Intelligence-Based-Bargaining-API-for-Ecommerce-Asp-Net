using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBasedRealtimeBargaining.Models
{
	public class NegotiatedCost : TableEntity
	{
		public long? CustomerId { get; set; }
		public long ProductId { get; set; }
		public string Token { get; set; }
		public DateTime NegotiateTime { get; set; }
		public long NegotiateTimeStamp { get; set; }
		public double ProposedPrice { get; set; }
		public double OfferedPrice { get; set; }
		public string RowKeyNegotiatedMessage { get; set; }
		public int EnumNegotiatedMessage { get; set; }
	}
}