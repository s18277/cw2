using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Cw2.Serializers
{
    /// <summary>
    ///     Implementacja interfejsu <see cref="IObjectSerializer{T}" /> do formatu XML.
    /// </summary>
    /// <typeparam name="T">Typ obiektu do zapisania</typeparam>
    public class XmlSerializer<T> : IObjectSerializer<T>
    {
        private readonly XmlSerializerNamespaces _xmlNamespaces;
        private readonly XmlWriterSettings _xmlSettings;

        public XmlSerializer(XmlWriterSettings xmlSettings = null, XmlSerializerNamespaces xmlNamespaces = null)
        {
            _xmlSettings = xmlSettings ?? new XmlWriterSettings {Indent = true, Encoding = Encoding.Default};
            _xmlNamespaces = xmlNamespaces;
        }

        public void Serialize(T objectToSerialize, TextWriter outputWriter)
        {
            var xmlWriter = XmlWriter.Create(outputWriter, _xmlSettings);
            var xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(xmlWriter, objectToSerialize, _xmlNamespaces);
            outputWriter.Flush();
        }
    }
}