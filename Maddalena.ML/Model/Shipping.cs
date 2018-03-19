using Mongolino;

namespace Maddalena.ML.Model
{
    public class Shipping : DBObject<Shipping>
    {
        public string ExternalId { get; set; }

        public ObjectRef<Customer> Customer { get; set; }

        public ObjectRef<Order> Order { get; set; }

        public string MethodTitle { get; set; }
        public string MethodId { get; set; }
        public decimal Total { get; set; }
        public decimal TotalTax { get; set; }
    }
}