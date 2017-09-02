using System.Diagnostics;

namespace DoLess.Rest.Tasks
{
    [DebuggerDisplay("{Value}, IsMutable={IsMutable}")]
    internal class Parameter
    {
        private string value;

        public Parameter(string value, bool isMutable)
        {
            this.value = value;
            this.IsMutable = isMutable;
            this.HasBeenSet = !isMutable;
        }

        public string Value
        {
            get => this.value;
            set
            {
                if (this.IsMutable)
                {
                    this.value = value;
                    this.HasBeenSet = true;
                }
            }
        }

        public bool IsMutable { get; }

        public bool HasBeenSet { get; private set; }
    }
}
