using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AIBasedRealtimeBargaining.Models
{
	public class ResponseCommand
	{
		public RequestCommand _request { get; set; }
		public bool IsSuccess { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
		public NegotiatedValue _response { get; set; }
	}
}