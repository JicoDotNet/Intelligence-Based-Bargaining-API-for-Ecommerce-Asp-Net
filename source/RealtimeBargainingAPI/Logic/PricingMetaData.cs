using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtimeBargainingAPI.Logic
{
	public class PricingMetaData
	{
		public static double UpperMaxPercentage { get { return 16; } }
		public static double OverheadValue { get { return 100; } }
		public static double MarginPercentage { get { return 15; } }
		public static int TotalBargainPossibility { get { return 6; } }
	}
}