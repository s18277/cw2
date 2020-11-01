using System;
using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Cw2.Models
{
    /// <summary>
    ///     Klasa przechowująca dane o jednym studencie.
    /// </summary>
    [Serializable]
    [XmlRoot("student")]
    public class Student : IEquatable<Student>
    {
        public Student() { }

        public Student(string firstName, string lastName, string fieldOfStudy, string modeOfStudy, int id,
            DateTime birthdate, MailAddress email, string mothersName, string fathersName)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = $"s{id}";
            Birthdate = birthdate.ToString("dd.MM.yyyy");
            Email = email.ToString();
            MothersName = mothersName;
            FathersName = fathersName;
            Studies = new Studies(fieldOfStudy, modeOfStudy);
        }

        [XmlAttribute(AttributeName = "indexNumber")]
        [JsonPropertyName("indexNumber")]
        public string Id { get; set; }

        [XmlElement(ElementName = "fname")]
        [JsonPropertyName("fname")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "lname")]
        [JsonPropertyName("lname")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "birthdate")]
        [JsonPropertyName("birthdate")]
        public string Birthdate { get; set; }

        [XmlElement(ElementName = "email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [XmlElement(ElementName = "mothersName")]
        [JsonPropertyName("mothersName")]
        public string MothersName { get; set; }

        [XmlElement(ElementName = "fathersName")]
        [JsonPropertyName("fathersName")]
        public string FathersName { get; set; }

        [XmlElement(ElementName = "studies")]
        [JsonPropertyName("studies")]
        public Studies Studies { get; set; }

        public bool Equals(Student other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && FirstName == other.FirstName && LastName == other.LastName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return GetType() == obj.GetType() && Equals(obj as Student);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(FirstName);
            hashCode.Add(LastName);
            return hashCode.ToHashCode();
        }

        public override string ToString()
        {
            return string.Join(',', FirstName, LastName, Studies, Id, Birthdate, Email, MothersName, FathersName);
        }
    }

    /// <summary>
    ///     Klasa przechowująca dane o kierunku i rodzaju studiów.
    /// </summary>
    [Serializable]
    [XmlRoot("studies")]
    public class Studies
    {
        public Studies() { }

        public Studies(string nameOfStudies, string modeOfStudies)
        {
            NameOfStudies = nameOfStudies;
            ModeOfStudies = modeOfStudies;
        }

        [XmlElement(ElementName = "name")]
        [JsonPropertyName("name")]
        public string NameOfStudies { get; set; }

        [XmlElement(ElementName = "mode")]
        [JsonPropertyName("mode")]
        public string ModeOfStudies { get; set; }

        public override string ToString()
        {
            return $"{NameOfStudies}, {ModeOfStudies}";
        }
    }
}