using System;
using Mongolino;

namespace Maddalena.ML.Model
{
    public class Customer : DBObject<Customer>
    {
        public string ExternalId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public string MetaData { get; set; }

        public decimal TotalSpent { get; set; }

        public int OrdersCount { get; set; }
    }
}