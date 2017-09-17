using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DoLess.Rest.Json
{
    public class JsonRestSettings : RestSettings
    {
        public JsonRestSettings(JsonSerializerSettings jsonSerializerSettings = null)
        {
            this.JsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
            this.MediaTypeFormatter = new JsonMediaTypeFormatter(JsonSerializer.Create(this.JsonSerializerSettings));
            this.FormFormatter = new JsonFormFormatter();
        }

        public JsonSerializerSettings JsonSerializerSettings { get; }
    }
}
