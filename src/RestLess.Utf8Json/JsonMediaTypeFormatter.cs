//using System.IO;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//namespace RestLess
//{
//    public class JsonMediaTypeFormatter : IMediaTypeFormatter
//    {
//        private readonly JsonSerializer jsonSerializer;
//        public JsonMediaTypeFormatter(JsonSerializer jsonSerializer)
//        {
//            this.jsonSerializer = jsonSerializer;
//        }

//        public string MediaType => "application/json";

//        public Task<T> ReadAsync<T>(TextReader reader)
//        {
//            using (JsonReader jsonReader = new JsonTextReader(reader))
//            {
//                return Task.FromResult<T>(this.jsonSerializer.Deserialize<T>(jsonReader));
//            }
//        }

//        public Task WriteAsync<T>(T content, TextWriter writer)
//        {
//            this.jsonSerializer.Serialize(writer, content, typeof(T));
//            return Task.CompletedTask;
//        }
//    }
//}
