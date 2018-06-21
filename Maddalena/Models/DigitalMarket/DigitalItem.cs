﻿using Maddalena.Security;
using Mongolino;

namespace Maddalena.Models.DigitalMarket
{
    public class DigitalItem : DBObject<DigitalItem>
    {
        public ApplicationUser User { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string[] Tags { get; set; }

        public bool Visible { get; set; }

        public bool PayWhatYouWant { get; set; }

        public string Currency { get; set; }

        public decimal Price { get; set; }

        public bool HasPreview { get; set; }

        public bool VideoPreview { get; set; }

        public decimal Bought { get; set; }

        public decimal Views { get; set; }
    }
}