using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Environment;

namespace Cw2
{
    /// <summary>
    ///     Klasa pomocnicza zajmująca się logowaniem wiadomości do pliku - błędów w parsowaniu i wyjątków.
    /// </summary>
    public static class Logger
    {
        private static string _logPath;

        public static void SetLogPath(string logPath)
        {
            File.WriteAllText(logPath, string.Empty);
            _logPath = logPath;
        }

        public static void Log(object message)
        {
            if (_logPath == null) return;
            File.AppendAllText(_logPath, message.ToString());
            File.AppendAllText(_logPath, NewLine);
        }

        public static Exception LogException(Exception exception)
        {
            Log(exception);
            Log(string.Join(NewLine, new StackTrace().ToString().Split(NewLine).Skip(1)));
            return exception;
        }
    }
}