using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBasedRealtimeBargaining.Models
{
	/* Negotiate Status
     * Error = 0
     * Negotiate Possible and Done, NegotiationSuccess = 11
     * Previous Negotiated value can not same or smaller then new Negotiated Value, CanNotBeSame = 1
     * Cross Maximum Limit to Negotiate, MaxLimitExceeded = 2
     * if Negotiated reached ThresholdPrice, ReachedThresholdPrice = 3
     * if calculated negotiated price is smaller then offred price, OffredPriceAccepted = 4
     * On product Page load, Last offered price will retrive, LastPriceOffred = 5
     */
	public enum ENegotiatedMessage
	{
		Error = 0,

		CanNotBeSame = 1,

		MaxLimitExceeded = 2,

		ReachedThresholdPrice = 3,

		OffredPriceAccepted = 4,

		LastPriceOffred = 5,

		NegotiationSuccess = 11
	}
}