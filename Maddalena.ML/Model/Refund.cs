using Mongolino;

namespace Maddalena.ML.Model
{
    public class Refund : DBObject<Refund>
    {
        public ObjectRef<Customer> Customer { get; set; }

        public ObjectRef<Order> Order { get; set; }

        public string ExternalId { get; set; }

        public string Reason { get; set; }

        public decimal Total { get; set; }
    }
}