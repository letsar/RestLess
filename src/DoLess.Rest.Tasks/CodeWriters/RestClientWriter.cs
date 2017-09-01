using System.Collections.Generic;
using System.Linq;
using DoLess.Rest.Tasks.CodeParsers;

namespace DoLess.Rest.Tasks.CodeWriters
{
//    internal class RestClientWriter
//    {
//        private readonly static HashSet<string> DefaultUsings = new HashSet<string>
//        {
//            "using System;",
//            "using System.Net.Http;"
//        };

//        private readonly List<InterfaceParser> parsers;

//        public RestClientWriter()
//        {
//            this.parsers = new List<InterfaceParser>();
//        }

//        public void AddInterface(InterfaceParser parser)
//        {
//            this.parsers.Add(parser);
//        }

//        public string Build()
//        {
//            string initialization = this.BuildInitialization();
//            string usings = this.BuildUsings();

//            string code =
//$@"{usings}

//namespace DoLess.Rest
//{{
//    public static class RestClient
//    {{
//        private static readonly RestClientFactory RestClientFactory;

//        static RestClient()
//        {{
//            RestClientFactory = new RestClientFactory();
//            {initialization}
//        }}

//        /// <summary>
//        /// Creates a Rest Client from the specified interface.
//        /// </summary>
//        /// <typeparam name=""TRestClient"">The interface of the Rest Client.</typeparam>
//        /// <param name=""client"">The client.</param>
//        /// <param name=""settings"">The optional settings.</param>
//        /// <returns></returns>
//        public static TRestClient For<TRestClient>(HttpClient client, RestSettings settings = null)
//        {{
//            return (TRestClient)RestClientFactory.Create(typeof(TRestClient), client, settings);
//        }}
//    }}
//}}";

//            return code;
//        }

//        private string BuildInitialization()
//        {
//            return this.parsers
//                       .Select(x => this.CreateInitializer(x))
//                       .ToMultilineSting();
//        }

//        private string CreateInitializer(InterfaceParser interfaceParser)
//        {
//            return $"RestClientFactory.AddClient(typeof({interfaceParser.Namespace}.{interfaceParser.InterfaceName}), (HttpClient h, RestSettings s) => new {interfaceParser.RestClientName}(h, s));";
//        }

//        private string BuildUsings()
//        {
//            return this.parsers
//                       .SelectMany(x => x.Usings)
//                       .Union(DefaultUsings)
//                       .Distinct()
//                       .OrderBy(x => x)
//                       .ToMultilineSting();
//        }
//    }
}
