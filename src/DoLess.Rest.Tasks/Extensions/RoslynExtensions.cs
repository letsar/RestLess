using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using DoLess.Rest;

namespace DoLess.Rest
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoslynExtensions
    {
        /// <summary>
        /// Indicates wether the specified symbol inherits from the type parameter.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="self">The symbol.</param>
        /// <returns></returns>
        public static bool InheritsFrom<T>(this INamedTypeSymbol self)
        {
            if (self == null)
            {
                return false;
            }
            else if (self.ToString() == typeof(T).FullName)
            {
                return true;
            }
            else
            {
                return InheritsFrom<T>(self.BaseType);
            }
        }
    }
}
