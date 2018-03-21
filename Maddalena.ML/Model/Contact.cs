using Mongolino;

namespace Maddalena.ML.Model
{
    public class Contact : DBObject<Contact>
    {
        public ObjectRef<Person> Person { get; set; }

        public string Value { get; set; }

        public ContactType Type { get; set; }
    }
}