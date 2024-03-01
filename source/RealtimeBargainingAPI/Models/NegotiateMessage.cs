using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBasedRealtimeBargaining.Models
{
	public class NegotiateMessage
	{
		public string Language { get; set; }
		public string MessageText { get; set; }
	}
}