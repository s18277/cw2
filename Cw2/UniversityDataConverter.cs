using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Cw2.Models;
using Cw2.Parsers;
using Cw2.Parsers.Fields;
using static System.Text.Encoding;

namespace Cw2
{
    /// <summary>
    ///     Główna klasa z funkcją <c>Main</c>.
    ///     Tworzy obiekty parsera danych <see cref="IObjectParser{T}" />, konwertera danych <see cref="DataConverter{T,TU}" />
    ///     do innego formatu i pomocnicze klasy <see cref="Logger" /> odpowiedzialne za logowanie błędów
    ///     i <see cref="ArgumentParser{T}" /> zajmujące się weryfikacją i parsowaniem przekazanych argumentów.
    /// </summary>
    internal static class UniversityDataConverter
    {
        private const string Author = "Paweł Rutkowski";
        private const string ErrorLogPath = "log.txt";
        private const string Delimiter = ",";

        private static void Main(string[] arguments)
        {
            ConfigureErrorLogging();
            PrepareUniversityDataConverter(ParseArguments(arguments)).Convert();
        }

        private static void ConfigureErrorLogging()
        {
            var logFile = File.OpenWrite(ErrorLogPath);
            logFile.SetLength(0);
            logFile.Flush();
            var errorWriter = new StreamWriter(logFile);
            Logger.SetErrorWriter(errorWriter);
        }

        private static DataConverter<ICollection<Student>, UniversityData> PrepareUniversityDataConverter(
            ArgumentParser<UniversityData> arguments)
        {
            var studentParser = new LineDelimitedParser<Student>(Delimiter, new FieldsToStudentParser());
            return new DataConverter<ICollection<Student>, UniversityData>(studentParser, arguments.ObjectSerializer,
                arguments.InputReader, arguments.OutputWriter, students => new UniversityData(students, Author));
        }

        private static ArgumentParser<UniversityData> ParseArguments(string[] arguments)
        {
            var xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");
            var xmlSettings = new XmlWriterSettings {Indent = true, OmitXmlDeclaration = true, Encoding = Default};
            return new ArgumentParser<UniversityData>(arguments, xmlSettings, xmlSerializerNamespaces);
        }
    }
}