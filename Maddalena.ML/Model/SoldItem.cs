using Mongolino;

namespace Maddalena.ML.Model
{
    public class SoldItem : DBObject<SoldItem>
    {
        public decimal TotalTax { get; set; }

        public decimal Total { get; set; }

        public decimal Subtotal { get; set; }

        public decimal SubtotalTax { get; set; }

        public int Quantity { get; set; }
        
        public string Name { get; set; }

        public string TaxClass { get; set; }

        public decimal Price { get; set; }

        public ObjectRef<Person> Person { get; set; }

        public ObjectRef<Model.Order> Order { get; set; }
    }
}
