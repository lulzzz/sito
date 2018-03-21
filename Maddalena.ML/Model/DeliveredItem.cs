using Mongolino;

namespace Maddalena.ML.Model
{
    internal class DeliveredItem : DBObject<DeliveredItem>
    {
        public string ExternalId { get; set; }

        public ObjectRef<Person> Customer { get; set; }

        public ObjectRef<Order> Order { get; set; }

        public decimal TotalTax { get; set; }

        public decimal Total { get; set; }

        public string Sku { get; set; }

        public decimal Subtotal { get; set; }

        public decimal SubtotalTax { get; set; }

        public int Quantity { get; set; }

        public string VariationId { get; set; }

        public string ProductId { get; set; }

        public string Name { get; set; }

        public string TaxClass { get; set; }

        public decimal Price { get; set; }
    }
}