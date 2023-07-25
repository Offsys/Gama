using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace servicoAlunos.Models
{
    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _timeZoneId;

        public UtcDateTimeConverter(string timeZoneId)
        {
            _timeZoneId = timeZoneId;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetDateTime(out DateTime dateTime))
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }

            return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
            var dateTimeWithTimeZone = TimeZoneInfo.ConvertTimeFromUtc(value, timeZoneInfo);
            writer.WriteStringValue(dateTimeWithTimeZone);
        }
    }
}
