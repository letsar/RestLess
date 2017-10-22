using System;
using RestLess.Tasks.Diagnostics;

namespace RestLess.Tasks.Exceptions
{
    internal class ErrorDiagnosticException : Exception
    {
        public ErrorDiagnosticException(ErrorDiagnostic error) :
            base(error.ToString())
        {
            this.Error = error;
        }

        public ErrorDiagnosticException(ErrorDiagnostic error, Exception innerException) :
            base(error.ToString(), innerException)
        {
            this.Error = error;
        }

        public ErrorDiagnostic Error { get; }
    }
}
