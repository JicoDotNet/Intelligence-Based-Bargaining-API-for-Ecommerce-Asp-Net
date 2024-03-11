namespace System
{
	public static class Extension
	{
		public static long TimeStamp(this DateTime DTobj)
		{
			try
			{
				return ((DTobj.Ticks - 621355968000000000) / 10000);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}