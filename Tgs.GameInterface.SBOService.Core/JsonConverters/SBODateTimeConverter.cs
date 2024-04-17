using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SBOService.Core.JsonConverters;

internal class SBODateTimeConverter : JsonConverter<DateTimeOffset>
{
	private readonly TimeZoneInfo _timeZone;

	public SBODateTimeConverter()
		=> _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");

	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var dataString = reader.GetString();
		if (dataString == null)
		{
			return default;
		}

		var unspecifyDate = DateTime.Parse(dataString);

		return new DateTimeOffset(unspecifyDate, _timeZone.GetUtcOffset(unspecifyDate));
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) => throw new NotImplementedException();
}
