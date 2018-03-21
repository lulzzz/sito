using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.ML.Model;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v2;

namespace Maddalena.ML.Adapters.WooCommerce
{
    class Program
    {
        public async Task LoadCustomers(WooCommerceCredential credential)
        {
            var rest = new RestAPI(credential.Url, credential.Key, credential.Secret);
            var wc = new WCObject(rest);

            var customers = await wc.Customer.GetAll();

            foreach (var p in customers)
            {
                var person = await Person.CreateAsync(new Person
                {
                    ExternalId = p.id.Safe().ToString(),
                    CreatedAt = p.date_created_gmt.Safe(),
                    Username = p.username,
                    FirstName = p.first_name.Safe(),
                    LastName = p.last_name.Safe(),
                    MiddleName = "",
                    Organization = "",
                    UpdateAt = p.date_modified_gmt.Safe()
                });

                if (!string.IsNullOrEmpty(p.email))
                {
                    await Contact.CreateAsync(new Contact
                    {
                        Person = person,
                        Type = ContactType.Email,
                        Value = p.email
                    });
                }

                await Address.CreateAsync(new Address
                {
                    Person = person,
                    Type = AddressType.Billing,
                    AddressLine1 = p.billing.address_1.Safe(),
                    AddressLine2 = p.billing.address_2.Safe(),
                    City = p.billing.city.Safe(),
                    Company = p.billing.company.Safe(),
                    PostalCode = p.billing.postcode.Safe(),
                    StateProvince = p.billing.state.Safe(),
                    Country = p.billing.country.Safe(),
                });

                if (!string.IsNullOrEmpty(p.billing.email) && p.billing.first_name != p.first_name && p.billing.last_name != p.last_name)
                {
                    await Contact.CreateAsync(new Contact
                    {
                        Person = person,
                        Type = ContactType.Email,
                        Value = p.billing.email
                    });
                }

                if (!string.IsNullOrEmpty(p.billing.phone) && p.billing.first_name != p.first_name && p.billing.last_name != p.last_name)
                {
                    await Contact.CreateAsync(new Contact
                    {
                        Person = person,
                        Type = ContactType.Telephone,
                        Value = p.billing.phone
                    });
                }

                await Address.CreateAsync(new Address
                {
                    Person = person,
                    Type = AddressType.Shipping,
                    AddressLine1 = p.shipping.address_1.Safe(),
                    AddressLine2 = p.shipping.address_2.Safe(),
                    City = p.shipping.city.Safe(),
                    Company = p.shipping.company.Safe(),
                    PostalCode = p.shipping.postcode.Safe(),
                    StateProvince = p.shipping.state.Safe(),
                    Country = p.shipping.country.Safe(),
                });
            }
        }

        public async Task LoadProducts(WooCommerceCredential credential)
        {
            var rest = new RestAPI(credential.Url, credential.Key, credential.Secret);
            var wc = new WCObject(rest);

            var products = await wc.Product.GetAll();

            foreach (var p in products)
            {
                await Model.Product.CreateAsync(new Model.Product()
                {
                    ExternalId = p.id.Safe().ToString(),
                    Type = p.type.Safe(),
                    Length = p.dimensions?.length.Safe(),
                    AverageRating = p.average_rating.Safe(),
                    Backordered = p.backordered.Safe(),
                    Backorders = p.backorders.Safe(),
                    BackordersAllowed = p.backorders_allowed.Safe(),
                    CatalogVisibility = p.catalog_visibility.Safe(),
                    HasRelated = p.related_ids.Safe().Any(),
                    HasCrossSell = p.cross_sell_ids.Safe().Any(),
                    HasUpSell = p.upsell_ids.Safe().Any(),
                    DateCreated = p.date_created_gmt.Safe(),
                    DateModified = p.date_modified_gmt.Safe(),
                    DateOnSaleFrom = p.date_on_sale_from.Safe(),
                    DateOnSaleTo = p.date_on_sale_to.Safe(),
                    Description = p.description.Safe(),
                    DownloadLimit = p.download_limit.Safe(),
                    Downloadable = p.downloadable.Safe(),
                    ExternalUrl = p.external_url.Safe(),
                    Featured = p.featured.Safe(),
                    Height = p.dimensions?.height.Safe(),
                    InStock = p.in_stock.Safe(),
                    Name = p.name.Safe(),
                    OnSale = p.on_sale.Safe(),
                    Permalink = p.permalink.Safe(),
                    Price = p.price.Safe(),
                    Purchasable = p.purchasable.Safe(),
                    ReviewsAllowed = p.reviews_allowed.Safe(),
                    Weight = p.weight.Safe(),
                    RatingCount = p.rating_count.Safe(),
                    RegularPrice = p.regular_price.Safe(),
                    SalePrice = p.sale_price.Safe(),
                    Width = p.dimensions?.width.Safe(),
                    StockQuantity = p.stock_quantity.Safe(),
                    ShippingClass = p.shipping_class.Safe(),
                    Virtual = p._virtual.Safe(),
                    ShippingRequired = p.shipping_required.Safe(),
                    ShippingTaxable = p.shipping_taxable.Safe(),
                    SoldIndividually = p.sold_individually.Safe(),
                    Tags = p.tags.Safe(x=>x.name),
                    Attributes = p.attributes.Safe(x=>x.name).Concat(p.default_attributes.Safe(x=>x.name)).ToList(),
                    Categories = p.categories.Safe(x=>x.name),
                    Status = p.status.Safe(),
                    TaxClass = p.tax_class.Safe(),
                    TaxStatus = p.tax_status.Safe(),
                    TotalSales = p.total_sales.Safe(),
                });
            }
        }

        public async Task LoadOrders(WooCommerceCredential credential)
        {
            var rest = new RestAPI(credential.Url, credential.Key, credential.Secret);
            var wc = new WCObject(rest);

            var orders = await wc.Order.GetAll();

            foreach (var o in orders)
            {
                var customerId = o.customer_id.Safe().ToString();
                var person = customerId != "0" ? Person.FirstOrDefault(x=>x.ExternalId == customerId) : null;

                var order = await Model.Order.CreateAsync(new Model.Order()
                {
                    Person = person,
                    ExternalId = o.id.Safe().ToString(),
                    Completed = o.date_completed_gmt != null,
                    CreatedVia = o.created_via.Safe(),
                    Currency = o.currency.Safe(),
                    DateCompleted = o.date_completed_gmt.Safe(),
                    DateCreated = o.date_created_gmt.Safe(),
                    DatePaid = o.date_paid_gmt.Safe(),
                    DiscountTotal = o.discount_total.Safe(),
                    Paid = o.date_paid != null,
                    DiscountTax = o.discount_tax.Safe(),
                    DateModified = o.date_modified_gmt.Safe(),
                    HasCoupon = o.coupon_lines?.Count != 0,
                    HasFee = o.fee_lines?.Count != 0,
                    HasRefund = o.refunds?.Count != 0,
                    HasShipping = o.shipping_lines?.Count != 0,
                    HasTax = o.tax_lines?.Count != 0,
                    ItemCount = o.line_items?.Count ?? 0,
                    Modified = o.date_modified_gmt != null,
                    Note = o.customer_note.Safe(),
                    PaymentMethod = o.payment_method.Safe(),
                    PaymentMethodTitle = o.payment_method_title.Safe(),
                    PricesIncludeTax = o.prices_include_tax.Safe(),
                    ShippingTax = o.shipping_tax.Safe(),
                    ShippingTotal = o.shipping_total.Safe(),
                    Status = o.status.Safe(),
                    Total = o.total.Safe(),
                    TotalTax = o.total_tax.Safe(),
                    TotalCoupon = o.coupon_lines.Safe(x=>x.discount.Safe()).Sum(),
                    TotalFee = o.fee_lines.Safe(x=>x.total.Safe()).Sum(),
                    TotalRefund = o.refunds.Safe(x=>x.total.Safe()).Sum()
                });

                foreach (var item in o.line_items.Safe())
                {
                    await SoldItem.CreateAsync(new SoldItem
                    {
                        Person = person,
                        Order = order,
                        Name = item.name.Safe(),
                        Price = item.price.Safe(),
                        Quantity = item.quantity.Safe(),
                        Subtotal = item.subtotal.Safe(),
                        SubtotalTax = item.subtotal_tax.Safe(),
                        TaxClass = item.tax_class.Safe(),
                        Total = item.total.Safe(),
                        TotalTax = item.total_tax.Safe()
                    });
                }

                var complains = o.refunds.Safe(x => new Complaint
                {
                    ExternalId = x.id.Safe().ToString(),
                    Customer = person,
                    Order = order,
                    Reason = x.reason.Safe(),
                    Total = x.total.Safe()
                });

                foreach (var comp in complains) await Complaint.CreateAsync(comp);

                if (!string.IsNullOrWhiteSpace(o.customer_ip_address))
                {
                    await WebVisit.CreateAsync(new WebVisit
                    {
                        IpAddress = o.customer_ip_address.Safe(),
                        UserAgent = o.customer_user_agent.Safe()
                    });
                }
            }
        }
    }
}

