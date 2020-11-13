using System.Collections.Generic;
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
            Logger.SetLogPath(ErrorLogPath);
            using var argumentParser = ParseArguments(arguments);
            // StreamReader plików wejściowych i StreamWriter plików wyjściowych są otwierane poza konstruktorem
            // aby uniknąć wyjątków I/O w samym konstruktorze.
            argumentParser.OpenStreams();
            var studentParser = new LineDelimitedParser<Student>(Delimiter, new FieldsToStudentParser());
            var universityDataConverter = new DataConverter<ICollection<Student>, UniversityData>(studentParser,
                argumentParser.ObjectSerializer,
                argumentParser.InputReader, argumentParser.OutputWriter,
                students => new UniversityData(students, Author));
            universityDataConverter.Convert();
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