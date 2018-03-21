using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Maddalena.ML.Model;

namespace Maddalena.ML.Adapters.Prestashop
{
    class Program
    {
        static async Task LoadCustomers()
        {
            string BaseUrl = "http://www.myweb.com/api";
            string Account = "ASDLKJOIQWEPROQWUPRPOQPPRQOW";
            string Password = "";

            var factory = new CustomerFactory(BaseUrl, Account, Password);

            foreach (customer c in factory.GetAll())
            {
                await  Person.CreateAsync(new Person
                {
                    ExternalId = c.id.Safe().ToString(),
                    FirstName = c.firstname.Safe(),
                    LastName = c.lastname.Safe(),
                    Organization = c.company.Safe(),
                    CreatedAt = c.date_add.SafeDate(),
                    UpdatedAt = c.date_upd.SafeDate(),
                    Birthday = c.birthday.SafeDate(),
                    Website = c.website.Safe(),
                    Notes = c.note.Safe(),
                });

                /*
                    int active { get; set; }
                    int show_public_prices { get; set; }
                    int is_guest { get; set; }
                    int newsletter { get; set; }
                    int optin { get; set; }
                    int deleted { get; set; }
                    long max_payment_days { get; set; }
                    decimal outstanding_allow_amount { get; set; }
                    string secure_key { get; set; }
                    string ip_registration_newsletter { get; set; }
                    string date_upd { get; set; }
                    string ape { get; set; }

                    string email { get; set; }
                    string passwd { get; set; }
                    long? id_gender { get; set; }
                    long? id_risk { get; set; }
                    long? id_shop_group { get; set; }
                    long? id_shop { get; set; }
                    long? id_lang { get; set; }
                    long? id_default_group { get; set; }
                    long? id { get; set; }
                    string siret { get; set; }
                    AssociationsCustomer associations { get; set; }
                 */
            }
        }
    }
}
