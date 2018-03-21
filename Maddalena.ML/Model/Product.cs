using System;
using System.Collections.Generic;
using Mongolino;

namespace Maddalena.ML.Model
{
    public class Product : DBObject<Product>
    {
        public string ExternalId { get; set; }

        public bool ShippingRequired { get; set; }

        public string Length { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public decimal Weight { get; set; }

        public bool SoldIndividually { get; set; }

        public bool Backordered { get; set; }

        public bool BackordersAllowed { get; set; }

        public string Backorders { get; set; }

        public bool InStock { get; set; }

        public int StockQuantity { get; set; }

        public string TaxClass { get; set; }

        public string TaxStatus { get; set; }

        public bool ShippingTaxable { get; set; }

        public string ShippingClass { get; set; }

        public bool ReviewsAllowed { get; set; }

        public List<string> Attributes { get; set; }

        public List<string> Tags { get; set; }

        public string ExternalUrl { get; set; }

        public List<string> Categories { get; set; }

        public int RatingCount { get; set; }

        public string AverageRating { get; set; }

        public string Description { get; set; }

        public string CatalogVisibility { get; set; }

        public bool Featured { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public DateTime DateCreated { get; set; }

        public string Permalink { get; set; }

        public string Name { get; set; }

        public DateTime DateModified { get; set; }

        public int DownloadLimit { get; set; }

        public decimal RegularPrice { get; set; }

        public bool Downloadable { get; set; }

        public decimal SalePrice { get; set; }

        public bool Virtual { get; set; }

        public DateTime DateOnSaleFrom { get; set; }

        public DateTime DateOnSaleTo { get; set; }

        public decimal Price { get; set; }

        public bool OnSale { get; set; }

        public bool Purchasable { get; set; }

        public int TotalSales { get; set; }

        public bool HasRelated { get; set; }

        public bool HasCrossSell { get; set; }
        public bool HasUpSell { get; set; }
    }
}