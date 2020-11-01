namespace Cw2.Parsers.Fields
{
    /// <summary>
    ///     <para>Interfejs zapewniający tworzenie obiektu z przekazanej tablicy pól.</para>
    ///     <para>
    ///         Wydzielenie tego rodzaju funkcjonalności do osobnego interfejsu pozwala na implementację parsowania
    ///         obiektu na podstawie tablicy pól, niezależnie od sposobu zapisu i odczytu tych pól.
    ///     </para>
    /// </summary>
    /// <typeparam name="TSerializable">Typ obiektu do utworzenia</typeparam>
    public interface IFieldsToObjectParser<out TSerializable>
    {
        TSerializable Parse(string[] fieldsToParse);
    }
}