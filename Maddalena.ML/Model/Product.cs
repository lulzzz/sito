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
        
        public bool ManageStock { get; set; }
        
        public string TaxClass { get; set; }
        
        public string TaxStatus { get; set; }
        
        public bool ShippingTaxable { get; set; }
        
        public string ShippingClass { get; set; }
        
        public string ShippingClassId { get; set; }
        
        public bool ReviewsAllowed { get; set; }
        
        public string GroupedProducts { get; set; }
        
        public string Variations { get; set; }
        
        //public List<ProductDefaultAttribute> DefaultAttributes { get; set; }
        
        //public List<ProductAttributeLine> Attributes { get; set; }
        
        public string Tags { get; set; }
        
        public string ExternalUrl { get; set; }
        
        //public List<ProductCategoryLine> Categories { get; set; }
        
        public int ParentId { get; set; }
        
        public string CrossSellIds { get; set; }
        
        public string UpsellIds { get; set; }
        
        public string RelatedIds { get; set; }
        
        public int RatingCount { get; set; }
        
        public string AverageRating { get; set; }
        
        public string PurchaseNote { get; set; }
        
        public int MenuOrder { get; set; }
        
        public int DownloadExpiry { get; set; }

        public string ShortDescription { get; set; }
        
        public string Description { get; set; }
        
        public string CatalogVisibility { get; set; }
        
        public bool Featured { get; set; }
        
        public string Status { get; set; }
        
        public string Type { get; set; }
        
        public string Sku { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public string Permalink { get; set; }
        
        public string Slug { get; set; }
        
        public string Name { get; set; }
        
        public DateTime DateModified { get; set; }
        
        public int DownloadLimit { get; set; }
        
        public string MetaData { get; set; }

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
       
    }
}
