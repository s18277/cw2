using System.IO;

namespace Cw2.Serializers
{
    /// <summary>
    ///     Interfejs zapewniający funkcjonalność zapisu podanego obiektu do podanego <see cref="TextWriter" />.
    /// </summary>
    /// <typeparam name="T">Typ obiektu do zapisania</typeparam>
    public interface IObjectSerializer<in T>
    {
        void Serialize(T objectToSerialize, TextWriter outputWriter);
    }
}