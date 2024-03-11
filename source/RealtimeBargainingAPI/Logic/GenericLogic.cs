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

		public static string AzureStorageConnectionString { get { return "DefaultEndpointsProtocol=https;AccountName=watersstorage;AccountKey=WJVUYwPCUYxr4OuTiRzNz5KgmNC8sMdO3c1dzchDjgjI1Sk6GlHZk2DuJ+FMbU3VqJTwpBN6gA2a+AStM0oH4w==;EndpointSuffix=core.windows.net"; } }
		public static string DefaultToken { get { return "bcd6b947-24c8-4151-956b-0e58bb504e84"; } }
	}
}
