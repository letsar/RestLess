﻿using System;
using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP HEAD method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HeadAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="HeadAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public HeadAttribute(string path) : base(HttpMethod.Head, path) { }
    }
}
