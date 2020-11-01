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
        private static TextWriter _logger = Console.Error;

        public static void SetErrorWriter(TextWriter logWriter)
        {
            _logger = logWriter;
        }

        public static void Log(object message)
        {
            _logger.WriteLine(message);
            _logger.Flush();
        }

        public static Exception LogException(Exception exception)
        {
            Log(exception);
            Log(string.Join(NewLine, new StackTrace().ToString().Split(NewLine).Skip(1)));
            return exception;
        }
    }
}