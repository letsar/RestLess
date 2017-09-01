namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class ErrorDiagnostic : Diagnostic
    {
        public ErrorDiagnostic(string code) : base("error", code) { }
    }
}
