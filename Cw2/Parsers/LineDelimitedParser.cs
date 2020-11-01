using System.Collections.Generic;
using System.IO;
using Cw2.Parsers.Fields;
using static Cw2.Logger;

namespace Cw2.Parsers
{
    /// <summary>
    ///     <para>
    ///         Implementacja <see cref="IObjectParser{T}" /> odczytująca kolekcę obiektów z podanego <see cref="TextReader" />
    ///         linijka po linijce. Pola są oddzielone przekazanym delimiterem. Do stworzenia obiektu na podstawie odczytanych
    ///         pól
    ///         służy przekazany obiekt <see cref="IFieldsToObjectParser{TSerializable}" />.
    ///     </para>
    ///     <para>
    ///         Rozdzielenie dzielenia pliku na pola i parsowanie pól do obiektu pozwala na uniezależnienie tyc dwóch
    ///         operacji. Klasa <see cref="LineDelimitedParser{T}" /> może służyć do parsowania plików z różnymi delimiterami
    ///         różnych rodzajów obiektów.
    ///     </para>
    ///     <para>
    ///         Ta klasa rozszerza interfejs <see cref="IObjectParser{T}" /> z typem <see cref="List{T}" /> a nie bardziej
    ///         ogólnym <see cref="ICollection{T}" /> ze względu na to, że interfejs nie jest możliwy do serializacji do pliku
    ///         XML, podczas gdy faktyczny obiekt klasy <see cref="List{T}" /> już tak.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">Typ obiektu odczytywany z każdej linijki</typeparam>
    public class LineDelimitedParser<T> : IObjectParser<List<T>>
    {
        private readonly string _delimiter;
        private readonly IFieldsToObjectParser<T> _objectParser;
        private readonly List<T> _parsedObjects = new List<T>();
        private string _readLine;

        public LineDelimitedParser(string delimiter, IFieldsToObjectParser<T> objectParser)
        {
            _delimiter = delimiter;
            _objectParser = objectParser;
        }

        public List<T> Parse(TextReader inputReader)
        {
            _parsedObjects.Clear();
            while ((_readLine = inputReader.ReadLine()) != null)
            {
                var fields = _readLine.Split(_delimiter);
                var parsedObject = _objectParser.Parse(fields);
                if (FaultyParsedObject(parsedObject)) continue;
                _parsedObjects.Add(parsedObject);
            }

            return new List<T>(_parsedObjects);
        }

        private bool FaultyParsedObject(T parsedObject)
        {
            return NullParsedObject(parsedObject) || ParsedObjectExists(parsedObject);
        }

        private bool NullParsedObject(T parsedObject)
        {
            if (parsedObject != null) return false;
            Log($"Wpis [{_readLine}] zawiera błędy!");
            return true;
        }

        private bool ParsedObjectExists(T parsedObject)
        {
            if (!_parsedObjects.Contains(parsedObject)) return false;
            var alreadyParsedObject = _parsedObjects[_parsedObjects.IndexOf(parsedObject)];
            Log($"Odczytany obiekt [{_readLine}] już istnieje jako [{alreadyParsedObject}]!");
            return true;
        }
    }
}