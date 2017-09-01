using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.CodeParsers;

namespace DoLess.Rest.Tasks.CodeWriters
{
//    internal class RestClientForInterfaceWriter
//    {
//        private readonly static HashSet<string> DefaultUsings = new HashSet<string>
//        {
//            "using System;",
//            "using System.Net.Http;",
//            "using System.Collections.Generic;",
//            "using System.Linq;",
//        };

//        private readonly InterfaceParser parser;

//        public RestClientForInterfaceWriter(InterfaceParser parser)
//        {
//            this.parser = parser;
//        }

//        public string Build()
//        {
//            string usings = this.BuildUsings();
//            string namespaceName = this.parser.Namespace;
//            string scope = this.parser.Scope;
//            string interfaceName = this.parser.InterfaceName;
//            string className = this.parser.RestClientName;
//            string genericParameters = this.parser.GenericParameters;
//            string constraintClauses = this.parser.ConstraintClauses;
//            string methods = this.BuildMethods();

//            string code =
//$@"{usings}
//namespace {namespaceName}
//{{
//    /// <summary>
//    /// Represents the implementation of the Rest interface <see cref=""{interfaceName}""/>.
//    /// </summary>
//    [Preserve]
//    {scope} sealed partial class {className}{genericParameters} : {interfaceName}{genericParameters}{constraintClauses}
//    {{
//        public {className}(HttpClient httpClient, RestSettings settings)
//        {{
//        }}
//        {methods}
//    }}
//}}";

//            return code;
//        }

//        private string BuildUsings()
//        {
//            // Adds the same usings as the interface + the default ones.
//            var usings = this.parser
//                             .Usings
//                             .Union(DefaultUsings)
//                             .OrderBy(x => x)
//                             .ToMultilineSting();

//            return usings;
//        }

//        private string BuildMethods()
//        {
//            return this.parser
//                       .RestMethods
//                       .Select(x => new RestMethodWriter(x).Build())
//                       .ToMultilineSting();
//        }
//    }
}
