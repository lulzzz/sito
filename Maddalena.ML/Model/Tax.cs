using Mongolino;

namespace Maddalena.ML.Model
{
    public class Tax : DBObject<Tax>
    {
        public string ExternalId { get; set; }

        public ObjectRef<Customer> Customer { get; set; }

        public ObjectRef<Order> Order { get; set; }
        
        public string RateCode { get; set; }
        
        public string RateId { get; set; }
        
        public string Label { get; set; }
        
        public bool Compound { get; set; }
        
        public decimal TaxTotal { get; set; }
        
        public decimal ShippingTaxTotal { get; set; }
    }
}
