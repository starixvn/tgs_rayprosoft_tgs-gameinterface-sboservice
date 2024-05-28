namespace Tgs.GameInterface.SBOService.Core.Extensions;

public static class DateTimeExtensions
{
	private static TimeZoneInfo TimeZone => TimeZoneInfo.CreateCustomTimeZone(id: "GMT-4", baseUtcOffset: TimeSpan.FromHours(-4), "GMT-4", "GMT-4");

	public static string DatetimeConvertToString(this DateTimeOffset date)
	{
		return TimeZoneInfo.ConvertTime(date.ToUniversalTime(), TimeZone).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
	}

	public static DateTimeOffset StringConvertToDatetime(this string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return DateTime.UnixEpoch;
		}

		var date = DateTime.Parse(str);

		return new DateTimeOffset(date, TimeZone.GetUtcOffset(date));
	}
}
