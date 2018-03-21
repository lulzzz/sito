using System;
using System.Collections.Generic;
using Mongolino;

namespace Maddalena.ML.Model
{
    public class Order : DBObject<Order>
    {
        public string ExternalId { get; set; }

        public ObjectRef<Person> Person { get; set; }

        public string Note { get; set; }

        public bool HasCoupon { get; set; }

        public bool HasShipping { get; set; }

        public bool HasFee { get; set; }

        public bool HasTax { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentMethodTitle { get; set; }

        public DateTime DatePaid { get; set; }

        public DateTime DateCompleted { get; set; }

        public bool PricesIncludeTax { get; set; }

        public decimal TotalTax { get; set; }

        public string CreatedVia { get; set; }

        public string Status { get; set; }

        public string Currency { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public decimal DiscountTotal { get; set; }

        public decimal DiscountTax { get; set; }

        public decimal ShippingTotal { get; set; }

        public decimal ShippingTax { get; set; }

        public decimal Total { get; set; }

        public bool Completed { get; set; }

        public bool Paid { get; set; }

        public bool HasRefund { get; set; }

        public int ItemCount { get; set; }

        public bool Modified { get; set; }

        public decimal TotalFee { get; set; }

        public decimal TotalCoupon { get; set; }

        public decimal TotalRefund { get; set; }
    }
}