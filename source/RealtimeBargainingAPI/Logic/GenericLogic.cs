namespace System
{
	public class GenericLogic
	{
		public static DateTime IstNow
		{
			get
			{
				return System.DateTime.UtcNow.AddHours(5.5);
			}
		}

		public static string AzureStorageConnectionString { get { return "--- Azure Table Storage Connection String ---"; } }
		public static string DefaultToken { get { return "bcd6b947-24c8-4151-956b-0e58bb504e84"; } }
	}
}
