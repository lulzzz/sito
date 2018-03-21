using Mongolino;

namespace Maddalena.ML.Model
{
    public class Coupon : DBObject<Coupon>
    {
        public ObjectRef<Person> Customer { get; set; }

        public ObjectRef<Order> Order { get; set; }

        public string ExternalId { get; set; }

        public string Code { get; set; }

        public decimal Discount { get; set; }

        public decimal DiscountTax { get; set; }

        public string MetaData { get; set; }
    }
}