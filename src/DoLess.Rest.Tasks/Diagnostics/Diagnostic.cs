using System.Text;
using Microsoft.CodeAnalysis;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class Diagnostic
    {
        public Diagnostic(string type, string code)
        {
            this.Type = type;
            this.Code = code;
        }

        public string Type { get; }

        public string Code { get; }

        public string File { get; private set; }

        public int? Line { get; private set; }

        public int? Character { get; private set; }

        public string Message { get; protected set; }

        protected void SetLocation(Location location)
        {
            var fileLinePositionSpan = location.GetMappedLineSpan();
            var linePosition = fileLinePositionSpan.StartLinePosition;

            this.File = fileLinePositionSpan.Path;
            this.Line = linePosition.Line + 1;
            this.Character = linePosition.Character + 1;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.File))
            {
                builder.Append(this.File);

                if (this.Line.HasValue)
                {
                    builder.Append($"({this.Line.Value.ToString()}");

                    if (this.Character.HasValue)
                    {
                        builder.Append($",{this.Character.Value.ToString()}");
                    }

                    builder.Append(")");
                }

                builder.Append(": ");
            }

            builder.Append($"{this.Type} {this.Code}");

            if (string.IsNullOrWhiteSpace(this.Message))
            {
                builder.Append($": {this.Message}");
            }

            return builder.ToString();
        }

    }
}
