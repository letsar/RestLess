using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DoLess.Rest.Tasks.Helpers;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DoLess.Rest.Tasks
{
    public class BuildRestClientsTask : Task
    {
        [Required]
        public ITaskItem[] SourceFiles { get; set; }

        public override bool Execute()
        {
            try
            {
                var sourceFiles = (this.SourceFiles ?? Array.Empty<ITaskItem>()).Select(x => x.ItemSpec)
                                                                                .Distinct();
                var restClients = sourceFiles.Select(x => new { FilePath = x, FileName = Path.GetFileNameWithoutExtension(x) })
                                             .Where(x => !x.FileName.Contains(Constants.DoLessGeneratedFileSuffix))
                                             .Select(x => new RestClientBuilder(x.FilePath).Build())
                                             .Where(x => x.HasRestInterfaces)
                                             .ToArray();

                var restClientFactory = new RestClientFactoryBuilder(restClients).Build();

                // Save the files.                
                for (int i = 0; i < restClients.Length; i++)
                {
                    if (!this.Save(restClients[i]))
                    {
                        return false;
                    }
                }

                return this.Save(restClientFactory);
            }
            catch (Exception ex)
            {
                return this.LogErrorFromException(ex);
            }
        }

        private static Dictionary<string, string> GetMetadata(CodeBuilder codeBuilder)
        {
            return new Dictionary<string, string>
            {
                ["OriginalFilePath"] = codeBuilder.OriginalFilePath,
                ["GeneratedFilePath"] = codeBuilder.GeneratedFilePath,
            };
        }

        private bool Save(CodeBuilder codeBuilder)
        {
            var target = new FileInfo(codeBuilder.GeneratedFilePath);
            if (!target.Directory.Exists)
            {
                target.Directory.Create();
            }

            var newContent = codeBuilder.ToString();

            if (target.Exists)
            {
                var oldContent = File.ReadAllText(target.FullName, Encoding.UTF8);
                if (string.Equals(oldContent, newContent, StringComparison.Ordinal))
                {
                    // The contents are the same, nothing to do.
                    return true;
                }
                else if (target.IsReadOnly)
                {
                    // If the file is read-only, we cannot do anything.
                    return this.LogError($"The file '{target}' is read-only and cannot be overwritten.");
                }
            }

            // The content needs to be created/updated.

            // Parralel builds can cause more than one process to party on this file at the same time.
            // We have to retry if it fails.

            var written = false;
            for (int retry = 3; !written; retry--)
            {
                try
                {
                    File.WriteAllText(target.FullName, newContent, Encoding.UTF8);
                    written = true;
                }
                catch (Exception)
                {
                    if (retry < 0)
                    {
                        // Too many retries.
                        throw;
                    }

                    // We wait some time before retry.
                    Thread.Sleep(500);
                }
            }

            return true;
        }

        private bool LogError(string message)
        {
            this.Log.LogError(message);
            return false;
        }

        private bool LogErrorFromException(Exception ex)
        {
            this.Log.LogErrorFromException(ex, true);
            return false;
        }
    }
}
