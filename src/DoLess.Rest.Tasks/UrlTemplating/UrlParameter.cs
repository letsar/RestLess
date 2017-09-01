using System.Diagnostics;

namespace DoLess.Rest.Tasks.UrlTemplating
{
    [DebuggerDisplay("{Value}, IsMutable={IsMutable}")]
    internal class UrlParameter
    {
        private string value;

        public UrlParameter(string value, bool isMutable)
        {
            this.value = value;
            this.IsMutable = isMutable;
        }

        public string Value
        {
            get => this.value;
            set
            {
                if (this.IsMutable)
                {
                    this.value = value;
                }
            }
        }

        public bool IsMutable { get; }
    }
}
