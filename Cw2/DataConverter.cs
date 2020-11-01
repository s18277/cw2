using System;
using System.IO;
using Cw2.Models;
using Cw2.Parsers;
using Cw2.Serializers;

namespace Cw2
{
    /// <summary>
    ///     <para>
    ///         Generyczna klasa zajmująca się zparsowaniem podanego pliku wykorzystując <see cref="IObjectParser{T}" />,
    ///         konwersją go do innego typu (w tym przypadku lista studentów jest zamieniana na obiekt
    ///         <see cref="UniversityData" />) oraz zapisem danych do pliku wykorzystując przekazany
    ///         <see cref="IObjectSerializer{T}" />.
    ///     </para>
    ///     <para>
    ///         Klasa jest napisana w sposób generyczny i nie zależy od sposobu zapisu danych wejściowych,
    ///         rodzaju tych danych, ani rodzaju zapisu danych wyjściowych.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">Typ danych odczytywany z pliku</typeparam>
    /// <typeparam name="TU">Typ danych zapisywany do pliku</typeparam>
    public class DataConverter<T, TU>
    {
        private readonly IObjectParser<T> _dataParser;
        private readonly IObjectSerializer<TU> _dataSerializer;
        private readonly TextReader _input;
        private readonly TextWriter _output;
        private readonly Func<T, TU> _parsedDataConverter;
        private TU _convertedData;
        private T _parsedData;

        public DataConverter(IObjectParser<T> dataParser, IObjectSerializer<TU> dataSerializer, TextReader input,
            TextWriter output, Func<T, TU> parsedDataConverter = null)
        {
            _dataParser = dataParser;
            _dataSerializer = dataSerializer;
            _input = input;
            _output = output;
            _parsedDataConverter = parsedDataConverter ?? (obj => (TU) (object) obj);
        }

        /// <summary>
        ///     Metoda zajmująca się konwersją danych. Dane są parsowane przy użyciu obiektu klasy implementującej
        ///     <see cref="IObjectParser{T}" />, konwertowane z kolekcji wpisów za pomocą metody przekazanej w konsktruktorze
        ///     <see cref="Func{TResult}" /> i zapisywane przy wykorzystaniu przekazanego obiektu
        ///     <see cref="IObjectSerializer{T}" />.
        /// </summary>
        public void Convert()
        {
            _parsedData = _dataParser.Parse(_input);
            _convertedData = _parsedDataConverter.Invoke(_parsedData);
            _dataSerializer.Serialize(_convertedData, _output);
        }
    }
}