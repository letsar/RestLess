using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal static class Codes
    {
        public const string MissingHttpAttributeErrorCode = "DLR001";
        public const string MultipleRestAttributesErrorCode = "DLR002";
        public const string MalformedUrlTemplateErrorCode = "DLR003";
        public const string UrlIdAlreadyExistsErrorCode = "DLR004";
        public const string UrlIdNotFoundErrorCode = "DLR005";
        public const string MultipleHttpAttributesErrorCode = "DLR006";        
        public const string ReturnTypeErrorCode = "DLR007";
    }
}
