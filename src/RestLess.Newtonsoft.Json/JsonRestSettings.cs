using Newtonsoft.Json;

namespace RestLess
{
    public class JsonRestSettings : RestSettings
    {
        public JsonRestSettings(JsonSerializerSettings jsonSerializerSettings = null)
        {
            this.JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
            this.FormFormatters.Default = new JsonFormFormatter();
            this.MediaTypeFormatters.Default = new JsonMediaTypeFormatter(JsonSerializer.Create(this.JsonSerializerSettings));
        }

        public JsonSerializerSettings JsonSerializerSettings { get; }
    }
}
