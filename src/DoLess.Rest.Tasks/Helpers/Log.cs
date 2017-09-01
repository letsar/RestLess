using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;

namespace DoLess.Rest.Tasks
{
    internal static class Log
    {
        private const string TaskName = "DoLess.Rest";
        private static TaskLoggingHelper Logger;

        public static void SetLogger(TaskLoggingHelper logger)
        {
            Logger = logger;
        }

        public static void Error(string errorCode, string message, params object[] messageArgs)
        {
            Logger?.LogError(TaskName, errorCode, null, null, 0, 0, 0, 0, message, messageArgs);
        }

        public static void Message(string message, params object[] messageArgs)
        {
            Logger?.LogMessage($"{TaskName}: {message}", messageArgs);
        }

        public static void Warning(string message, params object[] messageArgs)
        {
            Logger?.LogWarning($"{TaskName}: {message}", messageArgs);
        }
    }
}
