namespace DoLess.Rest
{
    public class DefaultUrlParameterFormatter : IUrlParameterFormatter
    {
        public string Format(object parameterValue)
        {
            return parameterValue?.ToString();
        }
    }
}
