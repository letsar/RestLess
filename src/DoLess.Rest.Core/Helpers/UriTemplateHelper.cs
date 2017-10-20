using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoLess.Rest.Helpers
{
    internal static class UriTemplateHelper
    {
        public static string AppendUriTemplateSuffix(string uriTemplate, string uriTemplateSuffix)
        {
            ExtractPathAndQuery(uriTemplate, out string uriTemplatePath, out IList<string> uriTemplateQueryParameters);
            ExtractPathAndQuery(uriTemplateSuffix, out string uriTemplateSuffixPath, out IList<string> uriTemplateSuffixQueryParameters);
            var allQueryParameters = uriTemplateQueryParameters.Concat(uriTemplateSuffixQueryParameters);
            string result = $"{uriTemplatePath}{uriTemplateSuffixPath}";
            if (uriTemplateQueryParameters.Count + uriTemplateSuffixQueryParameters.Count == 0)
            {
                return result;
            }
            else
            {
                string parameters = string.Join(",", allQueryParameters);
                return $"{result}{{?{parameters}}}";
            }
        }

        public static void ExtractPathAndQuery(string uriTemplate, out string path, out IList<string> queryParameters)
        {
            path = string.Empty;
            queryParameters = new List<string>();

            if (!uriTemplate.IsNullOrEmpty())
            {
                bool inParameter = false;
                bool inQuery = false;
                StringBuilder pathBuilder = new StringBuilder(uriTemplate.Length);
                StringBuilder varBuilder = new StringBuilder();
                for (int i = 0; i < uriTemplate.Length; i++)
                {
                    char c = uriTemplate[i];
                    if (inQuery)
                    {
                        if (c == ',')
                        {
                            queryParameters.Add(varBuilder.ToString());
                            varBuilder.Clear();
                        }
                        else if (c == '}')
                        {
                            inQuery = false;
                            inParameter = false;
                            queryParameters.Add(varBuilder.ToString());
                            varBuilder.Clear();
                        }
                        else
                        {
                            varBuilder.Append(c);
                        }
                    }
                    else if (inParameter && c == '?')
                    {
                        pathBuilder.Remove(pathBuilder.Length - 1, 1);
                        inQuery = true;
                    }
                    else if (c == '{')
                    {
                        inParameter = true;
                        pathBuilder.Append(c);
                    }
                    else if (c == '}')
                    {
                        inParameter = false;
                        pathBuilder.Append(c);
                    }
                    else
                    {
                        pathBuilder.Append(c);
                    }
                }
                if (varBuilder.Length > 0)
                {
                    queryParameters.Add(varBuilder.ToString());
                    varBuilder.Clear();
                }

                path = pathBuilder.ToString();
            }
        }
    }
}
