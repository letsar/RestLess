namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class WarningDiagnostic : Diagnostic
    {
        public WarningDiagnostic(string code) : base("warning", code) { }
    }
}
