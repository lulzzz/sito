using System;
using Mongolino;

namespace Maddalena.ML.Model
{
    public class Person : DBObject<Person>
    {
        public string Organization { get; set; }

        public string ExternalId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public string Username { get; set; }
    }
}