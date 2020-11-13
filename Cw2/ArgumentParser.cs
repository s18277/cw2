using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Cw2.Serializers;
using static Cw2.Logger;

namespace Cw2
{
    /// <summary>
    ///     <para>
    ///         Klasa pomocnicza weryfikująca poprawność przekazanych do programu argumentów.
    ///         Jest stworzona w sposób generyczny, więc nie trzeba jej modyfikować w przypadku zmiany danych do parsowania.
    ///         Będzie wymagała jedynie modyfikacji w momencie, gdy chcemy dodać nowy sposób zapisu danych
    ///         (metoda <see cref="DetermineCorrectSerializer" />).
    ///     </para>
    ///     <para>
    ///         Przechowuje obiekty <see cref="InputReader" /> i <see cref="OutputWriter" /> odpowiedzialne za odczyt i zapis
    ///         danych do pliku oraz obiekt <see cref="ObjectSerializer" /> odpowiedzialny za serializację odczytanych danych.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">Typ danych które zostaną zapisane do pliku.</typeparam>
    public class ArgumentParser<T> : IDisposable
    {
        private const int InputPathIndex = 0;
        private const int OutputPathIndex = 1;
        private const int SerializationFormatIndex = 2;
        private readonly string _inputFilename;
        private readonly string _outputFilename;
        private readonly XmlSerializerNamespaces _xmlSerializerNamespaces;
        private readonly XmlWriterSettings _xmlWriterSettings;

        public ArgumentParser(string[] arguments, XmlWriterSettings xmlWriterSettings = null,
            XmlSerializerNamespaces xmlSerializerNamespaces = null)
        {
            _xmlSerializerNamespaces = xmlSerializerNamespaces;
            _xmlWriterSettings = xmlWriterSettings;
            arguments = VerifyNumberOfArguments(arguments);
            _inputFilename = arguments[InputPathIndex];
            _outputFilename = arguments[OutputPathIndex];
            ObjectSerializer = DetermineCorrectSerializer(arguments[SerializationFormatIndex]);
        }

        public TextReader InputReader { get; private set; }
        public TextWriter OutputWriter { get; private set; }
        public IObjectSerializer<T> ObjectSerializer { get; }

        public void Dispose()
        {
            InputReader?.Dispose();
            OutputWriter?.Dispose();
        }

        private static string[] VerifyNumberOfArguments(string[] arguments)
        {
            if (arguments.Length == 3 && !arguments.Any(string.IsNullOrWhiteSpace)) return arguments;
            var numberOfTrimmedArguments = arguments.Count(argument => !string.IsNullOrWhiteSpace(argument));
            LogException(
                new ArgumentException(
                    $"Niepoprawna liczba argumentów! Wymagana 3, Podano [{numberOfTrimmedArguments}]!"));
            return new[] {"data.csv", "result.xml", "xml"};
        }

        /// <summary>
        ///     Metoda ustalająca sposób zapisu danych do pliku poprzez wybranie odpowiedniej implementacji interfesu
        ///     <see cref="IObjectSerializer{T}" />. W przypadku dodania nowego sposobu konstrukcja <c>switch</c>
        ///     powinna zostać rozszerzona o nowe opcje.
        /// </summary>
        /// <param name="serializerName">argument przekazany do programu określający rodzaj serializacji</param>
        /// <returns>obiekt odpowiedniej klasy serializującej dane</returns>
        private IObjectSerializer<T> DetermineCorrectSerializer(string serializerName)
        {
            return serializerName.ToLower() switch
            {
                "xml" => new XmlSerializer<T>(_xmlWriterSettings, _xmlSerializerNamespaces),
                "json" => new JsonSerializer<T>(),
                _ => throw LogException(
                    new ArgumentException($"Podano nieoczekiwany format wyjściowy: [{serializerName}]!"))
            };
        }

        public void OpenStreams()
        {
            InputReader = new StreamReader(TryToOpenPath(_inputFilename, File.OpenRead));
            OutputWriter = OpenAndClearOutput(_outputFilename);
        }

        private TextWriter OpenAndClearOutput(string outputPath)
        {
            var inputStream = TryToOpenPath(outputPath, File.OpenWrite);
            inputStream.SetLength(0);
            inputStream.Flush();
            return new StreamWriter(inputStream);
        }

        private static FileStream TryToOpenPath(string path, Func<string, FileStream> openFunction)
        {
            try
            {
                return openFunction(path);
            }
            catch (Exception ex) when (ex is PathTooLongException || ex is ArgumentException)
            {
                throw LogException(new ArgumentException($"Niepoprawna ścieżka: [{path}]!"));
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                throw LogException(new FileNotFoundException($"Plik pod ścieżką: [{path}] nie istnieje!"));
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is NotSupportedException ||
                                       ex is IOException)
            {
                throw LogException(new IOException($"Brak dostępu do pliku pod ścieżką: [{path}]!"));
            }
        }
    }
}