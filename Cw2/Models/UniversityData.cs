using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Cw2.Models
{
    /// <summary>
    ///     Klasa przechowująca dane o wszystkich studentach i liczbę studentów na poszczególnych kierunkach. Obiekt tej
    ///     klasy zostanie faktycznie serializowany do pliku.
    /// </summary>
    [Serializable]
    [XmlRoot("uczelnia")]
    public class UniversityData
    {
        public UniversityData()
        {
            CreationDate = DateTime.Now.ToString("dd.MM.yyyy");
        }

        public UniversityData(IEnumerable<Student> students, string author) : this()
        {
            Students = students.ToList();
            Studies = Students.GroupBy(student => student.Studies.NameOfStudies)
                .Select(studyGrouping => new StudiesData(studyGrouping.Key, studyGrouping.Count())).ToList();
            Author = author;
        }

        [XmlAttribute("createdAt")]
        [JsonPropertyName("createdAt")]
        public string CreationDate { get; set; }

        [XmlAttribute("author")]
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [XmlArray("studenci")]
        [XmlArrayItem("student")]
        [JsonPropertyName("studenci")]
        public List<Student> Students { get; set; }

        [XmlArray("activeStudies")]
        [XmlArrayItem("studies")]
        [JsonPropertyName("activeStudies")]
        public List<StudiesData> Studies { get; set; }
    }

    /// <summary>
    ///     Klasa przechowująca statystyki kierunków uczelni - liczbę studentów na poszczególnych kierunkach.
    /// </summary>
    [Serializable]
    [XmlRoot("studies")]
    public class StudiesData
    {
        public StudiesData() { }

        public StudiesData(string nameOfStudies, int numberOfStudents)
        {
            NameOfStudies = nameOfStudies;
            NumberOfStudents = numberOfStudents;
        }

        [XmlAttribute(AttributeName = "name")]
        [JsonPropertyName("name")]
        public string NameOfStudies { get; set; }

        [XmlAttribute(AttributeName = "numberOfStudents")]
        [JsonPropertyName("numberOfStudents")]
        public int NumberOfStudents { get; set; }
    }
}