using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Cw2.Serializers
{
    /// <summary>
    ///     Implementacja interfejsu <see cref="IObjectSerializer{T}" /> do formatu JSON.
    /// </summary>
    /// <typeparam name="T">Typ obiektu do zapisania</typeparam>
    public class JsonSerializer<T> : IObjectSerializer<T>
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public void Serialize(T objectToSerialize, TextWriter outputWriter)
        {
            var serializedJsonString = JsonSerializer.Serialize(objectToSerialize, SerializerOptions);
            outputWriter.Write(serializedJsonString);
            outputWriter.Flush();
        }
    }
}