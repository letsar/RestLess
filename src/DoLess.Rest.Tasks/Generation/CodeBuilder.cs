using System;
using System.IO;
using DoLess.Rest.Tasks.Helpers;
using Microsoft.CodeAnalysis;

namespace DoLess.Rest.Tasks
{
    public class CodeBuilder
    {
        public CodeBuilder(string originalFilePath)
        {
            this.OriginalFilePath = originalFilePath;
            this.GeneratedFilePath = GetGeneratedFilePath(originalFilePath);
        }

        public CodeBuilder(string originalFilePath, string outputDirectory)
        {
            this.OriginalFilePath = originalFilePath;
            this.GeneratedFilePath = Path.Combine(outputDirectory, GetGeneratedFileName(originalFilePath));
        }

        public string OriginalFilePath { get; }

        public string GeneratedFilePath { get; }

        public SyntaxNode RootNode { get; protected set; }

        public override string ToString()
        {
            return this.RootNode
                       .NormalizeWhitespace()
                       .Normalize()
                       .ToFullString();
        }

        protected static string GetGeneratedFilePath(string filePath)
        {            
            return Path.Combine(Path.GetDirectoryName(filePath), GetGeneratedFileName(filePath));
        }

        protected static string GetGeneratedFileName(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}{Constants.DoLessGeneratedFileSuffix}";
        }
    }
}
