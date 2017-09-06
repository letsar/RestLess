using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest
{

    public interface IUrlParameterFormatter
    {
        /// <summary>
        /// Transforms an <see cref="object"/> into a <see cref="string"/>.
        /// </summary>
        /// <param name="parameterValue">The <see cref="object"/> to format.</param>
        /// <returns></returns>
        string Format(object parameterValue);
    }
}
