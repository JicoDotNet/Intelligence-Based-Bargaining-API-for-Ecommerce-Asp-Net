using DataAccess.AzureStorage.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBasedRealtimeBargaining.Models
{
	public class NegotiatedCost : ExecuteTableEntity
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