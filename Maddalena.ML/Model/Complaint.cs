using Mongolino;

namespace Maddalena.ML.Model
{
    public class Complaint : DBObject<Complaint>
    {
        public ObjectRef<Person> Customer { get; set; }

        public ObjectRef<Order> Order { get; set; }

        public string ExternalId { get; set; }

        public string Reason { get; set; }

        public decimal Total { get; set; }
    }
}
