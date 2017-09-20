using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DoLess.Rest.Tasks
{
    public class BuildRestClientsTask : Task
    {
        [Required]
        public string BaseDirectory { get; set; }

        [Required]
        public ITaskItem[] SourceFiles { get; set; }

        [Output]
        public string[] OutputFiles { get; set; }

        public override bool Execute()
        {
            try
            {
                if (this.BaseDirectory.IsNullOrWhiteSpace())
                {
                    return this.LogError($"the {nameof(this.BaseDirectory)} is not set.");
                }

                var targetDir = new DirectoryInfo(this.BaseDirectory);
            }
            catch (Exception ex)
            {
                return this.LogErrorFromException(ex);
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
            this.Log.LogErrorFromException(ex);
            return false;
        }
    }
}
