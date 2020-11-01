using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Cw2.Models;

namespace Cw2.Parsers.Fields
{
    /// <summary>
    ///     <para>
    ///         Klasa implementująca interfejs <see cref="IFieldsToObjectParser{TSerializable}" />. Parsuje podaną tablicę
    ///         pól do obiektu <see cref="Student" />, bądź zwraca wartość <c>null</c>.
    ///     </para>
    /// </summary>
    public class FieldsToStudentParser : IFieldsToObjectParser<Student>
    {
        private const int NumberOfFields = 9;
        private const int FirstNameIndex = 0;
        private const int LastNameIndex = 1;
        private const int FieldOfStudyIndex = 2;
        private const int ModeOfStudyIndex = 3;
        private const int StudentIdIndex = 4;
        private const int BirthdayIndex = 5;
        private const int EmailIndex = 6;
        private const int MothersNameIndex = 7;
        private const int FathersNameIndex = 8;

        public Student Parse(string[] fieldsToParse)
        {
            if (fieldsToParse.Length != NumberOfFields || AnyNullOrEmpty(fieldsToParse)) return null;
            try
            {
                var firstName = fieldsToParse[FirstNameIndex];
                var lastName = Regex.Replace(fieldsToParse[LastNameIndex], @"[\d]", string.Empty);
                var field = fieldsToParse[FieldOfStudyIndex];
                var mode = fieldsToParse[ModeOfStudyIndex];
                var id = int.Parse(fieldsToParse[StudentIdIndex]);
                var birthdate = DateTime.Parse(fieldsToParse[BirthdayIndex]);
                var email = new MailAddress(fieldsToParse[EmailIndex]);
                var mothersName = fieldsToParse[MothersNameIndex];
                var fathersName = fieldsToParse[FathersNameIndex];
                return new Student(firstName, lastName, field, mode, id, birthdate, email, mothersName, fathersName);
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException || ex is ArgumentException)
            {
                return null;
            }
        }

        private static bool AnyNullOrEmpty(params string[] strings)
        {
            return strings.Any(string.IsNullOrEmpty);
        }
    }
}