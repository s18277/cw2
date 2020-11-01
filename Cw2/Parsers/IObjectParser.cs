using System.IO;

namespace Cw2.Parsers
{
    /// <summary>
    ///     Interfejs zapewniający funkcjonalność odczytania obiektu z podanego <see cref="TextReader" />.
    /// </summary>
    /// <typeparam name="T">Typ obiektu do odczytania</typeparam>
    public interface IObjectParser<out T>
    {
        T Parse(TextReader inputReader);
    }
}