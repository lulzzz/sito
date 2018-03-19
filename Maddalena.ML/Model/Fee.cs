using Mongolino;

namespace Maddalena.ML.Model
{
    public class Fee : DBObject<Fee>
    {
        public string ExternalId { get; set; }

        public ObjectRef<Customer> Customer { get; set; }

        public ObjectRef<Order> Order { get; set; }

        public string Name { get; set; }

        public string TaxClass { get; set; }

        public string TaxStatus { get; set; }

        public decimal Total { get; set; }

        public decimal TotalTax { get; set; }

        public string MetaData { get; set; }

        protected object TotalValue { get; set; }
    }
}