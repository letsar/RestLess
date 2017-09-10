namespace DoLess.UriTemplates
{
    public sealed class OperatorNotSupportedException : UriTemplateException
    {
        public OperatorNotSupportedException(char op)
            : base($"The operator '{op}' is not yet supported")
        {
        }
    }
}
